using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.DAL.Models;
using ShopApp.DAL.Interfaces;

namespace ShopApp.BLL.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IShopRepository _shopRepository;

        public ProductService(IProductRepository productRepository, IShopRepository shopRepository)
        {
            _productRepository = productRepository;
            _shopRepository = shopRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsWithShopDetailsAsync()
        {
            // Получаем все товары
            var products = await _productRepository.GetAllProductsAsync();

            if (products == null || !products.Any())
            {
                return new List<Product>();
            }

            // Получаем все магазины
            var shops = await _shopRepository.GetAllShopsAsync();

            if (shops == null || !shops.Any())
            {
                return products;
            }

            // Связываем каждый продукт с информацией о магазине
            foreach (var product in products)
            {
                // Находим соответствующий магазин для текущего продукта
                var shop = shops.FirstOrDefault(s => s.Code == product.Shop.Code);
                if (shop != null)
                {
                    product.Shop = shop;
                }
            }

            return products;
        }
    }
}