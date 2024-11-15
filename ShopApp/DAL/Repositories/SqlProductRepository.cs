using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using ShopApp.Models;
using ShopApp.DAL.Interfaces;
using System.Data;

namespace ShopApp.DAL.Repositories
{
    public class SqlProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public SqlProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddProductAsync(Product product)
        {
            var query = "INSERT INTO Products (Name, ShopCode, Quantity, Price) VALUES (@Name, @ShopCode, @Quantity, @Price)";

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@ShopCode", product.Shop.Code);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@Price", product.Price);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<Product> GetProductByNameAsync(string name, string shopCode)
        {
            var query = "SELECT * FROM Products WHERE Name = @Name AND ShopCode = @ShopCode";

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@ShopCode", shopCode);

                var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    var shop = new Shop
                    {
                        Code = reader.GetString(reader.GetOrdinal("ShopCode"))
                    };

                    return new Product
                    {
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                        Shop = shop
                    };
                }
            }

            return null;
        }

        public async Task UpdateProductStockAndPriceAsync(string name, string shopCode, int quantity, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(shopCode))
            {
                throw new ArgumentException("Имя товара и код магазина должны быть заполнены.");
            }

            const string query = @"
            UPDATE Products
            SET Price = @Price, Quantity = @Quantity
            WHERE Name = @Name AND ShopCode = @ShopCode";

            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name.Trim());
                        command.Parameters.AddWithValue("@ShopCode", shopCode.Trim());
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Quantity", quantity);

                        var rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected == 0)
                        {
                            Console.WriteLine($"Товар с названием '{name}' и кодом магазина '{shopCode}' не найден.");
                            throw new KeyNotFoundException($"Товар с названием '{name}' и кодом магазина '{shopCode}' не найден.");
                        }

                        Console.WriteLine($"Товар '{name}' успешно обновлен: цена = {price}, количество = {quantity}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении товара: {ex.Message}");
                throw new InvalidOperationException("Ошибка при обновлении товара в базе данных SQLite", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqliteCommand("SELECT Name, ShopCode, Quantity, Price FROM Products", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var shop = new Shop
                        {
                            Code = reader.GetString(1) 
                        };

                        products.Add(new Product
                        {
                            Name = reader.GetString(0),
                            Shop = shop,               
                            Quantity = reader.GetInt32(2),
                            Price = reader.GetDecimal(3)
                        });
                    }
                }
            }

            return products;
        }

        public async Task<Product> FindCheapestProductAsync(string shopCode)
        {
            var query = "SELECT Name, ShopCode, Quantity, Price FROM Products WHERE ShopCode = @ShopCode ORDER BY Price ASC LIMIT 1";

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@ShopCode", shopCode);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var shop = new Shop
                        {
                            Code = reader.GetString(reader.GetOrdinal("ShopCode"))
                        };

                        return new Product
                        {
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Shop = shop, 
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                        };
                    }
                }
            }

            return null;
        }
    }
}