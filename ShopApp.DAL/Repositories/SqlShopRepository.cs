using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using ShopApp.DAL.Interfaces;
using ShopApp.DAL.Models;

namespace ShopApp.DAL.Repositories
{
    public class SqlShopRepository : IShopRepository
    {
        private readonly string _connectionString;

        public SqlShopRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddShopAsync(Shop shop)
        {
            var query = "INSERT INTO Shops (Code, Name, Address) VALUES (@Code, @Name, @Address)";

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Code", shop.Code);
                command.Parameters.AddWithValue("@Name", shop.Name);
                command.Parameters.AddWithValue("@Address", shop.Address);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<Shop> GetShopByCodeAsync(string code)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqliteCommand("SELECT * FROM Shops WHERE Code = @code", connection);
            command.Parameters.AddWithValue("@code", code);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Shop
                {
                    Code = reader.GetString(0),
                    Name = reader.GetString(1),
                    Address = reader.GetString(2)
                };
            }
            return null;
        }

        public async Task<IEnumerable<Shop>> GetAllShopsAsync()
        {
            var shops = new List<Shop>();
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqliteCommand("SELECT * FROM Shops", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                shops.Add(new Shop
                {
                    Code = reader.GetString(0),
                    Name = reader.GetString(1),
                    Address = reader.GetString(2)
                });
            }
            return shops;
        }
    }
}