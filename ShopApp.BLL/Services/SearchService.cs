using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.DAL.Repositories;
using ShopApp.DAL.Interfaces;
using ShopApp.DAL.Models;

namespace ShopApp.BLL.Services
{
    public class SearchService
    {
        private readonly IProductRepository _productRepository;
        private readonly IShopRepository _shopRepository;
        private readonly ProductService _productService;

        public SearchService(IProductRepository productRepository, IShopRepository shopRepository)
        {
            _productRepository = productRepository;
            _shopRepository = shopRepository;
            _productService = new ProductService(_productRepository, _shopRepository);
        }

        public async Task<List<Product>> GetAffordableProductsByMaxPriceAsync(string selectedShop, decimal maxPrice)
        {
            // Получаем код выбранного магазина
            var shops = await _shopRepository.GetAllShopsAsync();
            var selectedShopCode = shops.FirstOrDefault(shop => shop.Name == selectedShop)?.Code;

            if (selectedShopCode == null)
            {
                throw new InvalidOperationException("Магазин не выбран.");
            }

            // Получаем все товары для выбранного магазина и отфильтровываем те, у которых количество > 0
            var products = (await _productService.GetAllProductsWithShopDetailsAsync())
                             .Where(p => p.Shop != null && p.Shop.Code == selectedShopCode && p.Quantity > 0)
                             .OrderBy(p => p.Price)  // Сортируем товары по цене
                             .ToList();

            List<Product> affordableProducts = new List<Product>();
            decimal currentTotal = 0;

            foreach (var product in products)
            {
                int maxUnitsAffordable = (int)Math.Floor((maxPrice - currentTotal) / product.Price);

                int unitsToBuy = Math.Min(maxUnitsAffordable, product.Quantity);

                if (unitsToBuy > 0)
                {
                    affordableProducts.Add(new Product
                    {
                        Name = product.Name,
                        Shop = new Shop
                        {
                            Code = product.Shop.Code,
                            Name = product.Shop.Name,
                            Address = product.Shop.Address
                        },
                        Quantity = unitsToBuy,
                        Price = product.Price
                    });

                    currentTotal += unitsToBuy * product.Price;

                    if (currentTotal >= maxPrice)
                    {
                        break;
                    }
                }
            }

            return affordableProducts;
        }

        public async Task<(Shop cheapestShop, decimal totalCost, bool productsAvailable)> FindCheapestShopForProductsAsync(Dictionary<Product, int> productQuantities)
        {
            var allShops = await _shopRepository.GetAllShopsAsync();
            var allProducts = await _productRepository.GetAllProductsAsync();

            var shopTotalCosts = new Dictionary<Shop, decimal>();

            foreach (var shop in allShops)
            {
                decimal totalCost = 0;
                bool allProductsAvailable = true;

                foreach (var (selectedProduct, desiredQuantity) in productQuantities)
                {
                    var productInShop = allProducts
                        .FirstOrDefault(p => p.Shop.Code == shop.Code && p.Name == selectedProduct.Name && p.Quantity >= desiredQuantity);

                    if (productInShop == null)
                    {
                        allProductsAvailable = false;
                        break;
                    }

                    totalCost += productInShop.Price * desiredQuantity;
                }

                if (allProductsAvailable)
                {
                    shopTotalCosts[shop] = totalCost;
                }
            }

            if (!shopTotalCosts.Any())
            {
                return (null, 0, false);
            }

            var cheapestShop = shopTotalCosts.OrderBy(kvp => kvp.Value).First();
            return (cheapestShop.Key, cheapestShop.Value, true);
        }

        public async Task<List<Product>> SearchProductByNameAsync(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Product name cannot be empty.", nameof(productName));
            }

            var allProducts = await _productRepository.GetAllProductsAsync();

            foreach (var product in allProducts)
            {
                product.Shop = await _shopRepository.GetShopByCodeAsync(product.Shop.Code); 
            }

            var matchingProducts = allProducts
                .Where(p => p.Name.Equals(productName, StringComparison.OrdinalIgnoreCase) && p.Quantity > 0)
                .OrderBy(p => p.Price)
                .ToList();

            return matchingProducts;
        }
    }
}