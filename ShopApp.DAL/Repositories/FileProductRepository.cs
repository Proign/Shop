using ShopApp.DAL.Models;
using ShopApp.DAL.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Formats.Asn1;
using System.Globalization;

namespace ShopApp.DAL.Repositories
{
    public class FileProductRepository : IProductRepository
    {
        private readonly string _filePath;

        public FileProductRepository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                // Проверка существования файла и корректного пути
                if (string.IsNullOrEmpty(_filePath))
                {
                    throw new InvalidOperationException("Путь к файлу не задан.");
                }

                // Создаем файл с заголовком, если он отсутствует
                if (!File.Exists(_filePath))
                {
                    await File.WriteAllTextAsync(_filePath, "ProductName,Price,Quantity,ShopCode" + Environment.NewLine);
                }

                // Чтение текущего содержимого файла
                var existingContent = await File.ReadAllTextAsync(_filePath);

                if (!existingContent.EndsWith(Environment.NewLine))
                {
                    await File.AppendAllTextAsync(_filePath, Environment.NewLine);
                }

                var productLine = $"{product.Name.Trim()},{product.Price.ToString(CultureInfo.InvariantCulture)},{product.Quantity},{product.Shop.Code.Trim()}";

                await File.AppendAllTextAsync(_filePath, productLine + Environment.NewLine);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Не удалось добавить товар в файл.", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();

            // Чтение CSV файла
            try
            {
                var lines = await File.ReadAllLinesAsync(_filePath);

                foreach (var line in lines.Skip(1)) // Пропускаем первую строку
                {
                    if (string.IsNullOrWhiteSpace(line)) continue; // Пропускаем пустые строки

                    var columns = line.Split(',');

                    if (columns.Length < 4) continue;

                    // Получаем свойства продукта и магазина из CSV-данных
                    var name = columns[0].Trim();
                    var price = decimal.TryParse(columns[1].Trim(), out var parsedPrice) ? parsedPrice : 0;
                    var quantity = int.TryParse(columns[2].Trim(), out var parsedQuantity) ? parsedQuantity : 0;
                    var shopCode = columns[3].Trim();

                    var shop = new Shop
                    {
                        Code = shopCode
                    };

                    var product = new Product
                    {
                        Name = name,
                        Price = price,
                        Quantity = quantity,
                        Shop = shop 
                    };

                    products.Add(product);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка при чтении файла", ex);
            }

            return products;
        }
        public async Task UpdateProductStockAndPriceAsync(string name, string shopCode, int quantity, decimal price)
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException("Файл данных товаров не найден.");
            }

            var lines = await File.ReadAllLinesAsync(_filePath);
            var updatedLines = new List<string>();
            bool productFound = false;

            if (lines.Length == 0)
            {
                throw new InvalidOperationException("Файл данных пуст.");
            }

            foreach (var line in lines.Skip(1))  
            {
                var data = line.Split(',');

                if (data.Length == 4)
                {
                    string itemName = data[0].Trim();
                    string itemShopCode = data[3].Trim();

                    // Отладочные выводы
                    Console.WriteLine($"Считываем строку из файла: {line}");
                    Console.WriteLine($"Проверяем: Имя товара = '{itemName}' и код магазина = '{itemShopCode}'");

                    // Проверяем полное совпадение с товаром
                    if (string.Equals(itemName, name.Trim(), StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(itemShopCode, shopCode.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        // Если товар найден, обновляем его данные
                        Console.WriteLine("Товар найден, обновляем его данные.");
                        updatedLines.Add($"{itemName},{price},{quantity},{itemShopCode}");
                        productFound = true;
                    }
                    else
                    {
                        updatedLines.Add(line); 
                    }
                }
                else
                {
                    Console.WriteLine($"Ошибка в формате строки: {line}");
                }
            }

            if (!productFound)
            {
                Console.WriteLine($"Товар с названием '{name}' и кодом магазина '{shopCode}' не найден.");
                throw new KeyNotFoundException($"Товар с названием '{name}' и кодом магазина '{shopCode}' не найден.");
            }

            updatedLines.Insert(0, lines[0]); 
            await File.WriteAllLinesAsync(_filePath, updatedLines);
        }

        public async Task<Product> GetProductByNameAsync(string name, string shopCode)
        {
            if (!File.Exists(_filePath))
            {
                return null;
            }

            var lines = await File.ReadAllLinesAsync(_filePath);

            foreach (var line in lines)
            {
                var data = line.Split(',');

                if (data.Length >= 4 && data[0] == name && data[3] == shopCode)
                {
                    if (decimal.TryParse(data[1], out var price) && int.TryParse(data[2], out var quantity))
                    {
                        var shop = new Shop
                        {
                            Code = shopCode
                        };

                        return new Product
                        {
                            Name = name,
                            Price = price,
                            Quantity = quantity,
                            Shop = shop 
                        };
                    }
                }
            }

            return null;
        }

        public async Task<Product> FindCheapestProductAsync(string shopCode)
        {
            if (!File.Exists(_filePath))
            {
                return null;
            }

            var lines = await File.ReadAllLinesAsync(_filePath);
            Product cheapestProduct = null;

            foreach (var line in lines)
            {
                var data = line.Split(',');

                if (data.Length >= 4 && data[3] == shopCode && decimal.TryParse(data[1], out var price) && int.TryParse(data[2], out var quantity))
                {
                    var shop = new Shop
                    {
                        Code = shopCode
                    };

                    var currentProduct = new Product
                    {
                        Name = data[0],
                        Price = price,
                        Quantity = quantity,
                        Shop = shop 
                    };

                    if (cheapestProduct == null || currentProduct.Price < cheapestProduct.Price)
                    {
                        cheapestProduct = currentProduct;
                    }
                }
            }
            return cheapestProduct;
        }
    }
}