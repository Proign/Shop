using System.Collections.Generic;
using System.Threading.Tasks;
using ShopApp.DAL.Models;

namespace ShopApp.DAL.Interfaces
{
    public interface IShopRepository
    {
        Task AddShopAsync(Shop shop);
        Task<Shop> GetShopByCodeAsync(string code);
        Task<IEnumerable<Shop>> GetAllShopsAsync();
    }
}
