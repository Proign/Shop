using System.Collections.Generic;
using System.Threading.Tasks;
using ShopApp.DAL.Models;

namespace ShopApp.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task<Product> GetProductByNameAsync(string name, string shopCode);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task UpdateProductStockAndPriceAsync(string name, string shopCode, int quantity, decimal price);
        Task<Product> FindCheapestProductAsync(string shopCode);
    }
}
