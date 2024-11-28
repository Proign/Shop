using ShopApp.DAL.Models;
using ShopApp.DAL.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.DAL.Repositories
{
    public class FileShopRepository : IShopRepository
    {
        private readonly string _filePath;

        public FileShopRepository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task AddShopAsync(Shop shop)
        {
            var line = $"{shop.Code},{shop.Name},{shop.Address}";

            // Проверка, если файл уже содержит данные
            if (new FileInfo(_filePath).Length > 0)
            {
                // Добавляем новую строку перед записью, если файл уже содержит данные
                line = Environment.NewLine + line;
            }

            await File.AppendAllTextAsync(_filePath, line);
        }

        public async Task<IEnumerable<Shop>> GetAllShopsAsync()
        {
            var shops = new List<Shop>();

            // Чтение CSV файла
            try
            {
                var lines = await File.ReadAllLinesAsync(_filePath);

                foreach (var line in lines.Skip(1)) 
                {
                    if (string.IsNullOrWhiteSpace(line)) continue; 

                    var columns = line.Split(',');

                    if (columns.Length < 3) continue;

                    var shop = new Shop
                    {
                        Code = columns[0].Trim(),
                        Name = columns[1].Trim(),
                        Address = columns[2].Trim()
                    };

                    shops.Add(shop);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка при чтении файла", ex);
            }

            return shops;
        }

        public async Task<Shop> GetShopByCodeAsync(string code)
        {
            var shops = await GetAllShopsAsync();
            return shops.FirstOrDefault(s => s.Code == code);
        }
    }
}