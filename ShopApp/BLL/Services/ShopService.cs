using System.Threading.Tasks;
using ShopApp.BLL.Interfaces;
using ShopApp.DAL.Interfaces;
using ShopApp.Models;

namespace ShopApp.BLL.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IProductRepository _productRepository;

        public ShopService(IShopRepository shopRepository, IProductRepository productRepository)
        {
            _shopRepository = shopRepository;
            _productRepository = productRepository;
        }

        public async Task CreateShopAsync(Shop shop)
        {
            await _shopRepository.AddShopAsync(shop);
        }

        public async Task<Shop> FindCheapestProductStoreAsync(string productName)
        {
            var cheapestProduct = await _productRepository.FindCheapestProductAsync(productName);

            if (cheapestProduct == null)
                return null;

            var shop = await _shopRepository.GetShopByCodeAsync(cheapestProduct.Shop.Code);

            return shop;
        }
    }
}
