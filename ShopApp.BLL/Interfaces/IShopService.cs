using System.Threading.Tasks;
using ShopApp.DAL.Models;

namespace ShopApp.BLL.Interfaces
{
    public interface IShopService
    {
        Task CreateShopAsync(Shop shop);
        Task<Shop> FindCheapestProductStoreAsync(string productName);
    }
}
