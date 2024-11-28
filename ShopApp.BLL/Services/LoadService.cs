using System.Collections.Generic;
using System.Threading.Tasks;
using ShopApp.DAL.Models;
using ShopApp.DAL.Repositories;
using ShopApp.DAL.Interfaces;

namespace ShopApp.BLL.Services
{
    public class LoadService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IProductRepository _productRepository;
        private readonly ProductService _productService;

        public LoadService(IShopRepository shopRepository, IProductRepository productRepository, ProductService productService)
        {
            _shopRepository = shopRepository;
            _productRepository = productRepository;
            _productService = productService;
        }

        public async Task<List<string>> LoadShopsAsync()
        {
            var shops = await _shopRepository.GetAllShopsAsync();
            var shopNames = new List<string> { "Все магазины" };

            shopNames.AddRange(shops.Select(shop => shop.Name));

            return shopNames;
        }

        public async Task<IEnumerable<Product>> LoadProductsAsync(string selectedShop = null)
        {
            var products = await _productService.GetAllProductsWithShopDetailsAsync();

            if (!string.IsNullOrEmpty(selectedShop) && selectedShop != "Все магазины")
            {
                var shops = await _shopRepository.GetAllShopsAsync();
                var selectedShopCode = shops.FirstOrDefault(shop => shop.Name == selectedShop)?.Code;

                if (selectedShopCode != null)
                {
                    products = products.Where(p => p.Shop.Code == selectedShopCode);
                }
            }

            return products;
        }
    }
}