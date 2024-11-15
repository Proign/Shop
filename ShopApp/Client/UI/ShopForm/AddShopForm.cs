using Microsoft.Extensions.Configuration;
using ShopApp.DAL.Interfaces;
using ShopApp.DAL.Repositories;
using ShopApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopApp.Client.UI.ShopForm
{
    public partial class AddShopForm : Form
    {
        private readonly IShopRepository _shopRepository;

        public AddShopForm()
        {
            InitializeComponent();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("Configuration/appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var dataAccessMode = configuration.GetValue<string>("DataAccessMode");

            if (dataAccessMode == "Database")
            {
                var connectionString = configuration.GetSection("DatabaseSettings")["ConnectionString"];
                _shopRepository = new SqlShopRepository(connectionString);
            }
            else if (dataAccessMode == "File")
            {
                var shopFilePath = configuration.GetSection("FileSettings")["ShopFilePath"];
                _shopRepository = new FileShopRepository(shopFilePath);
            }
            else
            {
                throw new Exception("Неизвестный режим работы.");
            }

            AddShopButton.Click += async (sender, e) => await AddShopButton_Click(sender, e);
        }

        private async Task AddShopButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CodeTextBox.Text) ||
                string.IsNullOrEmpty(NameTextBox.Text) ||
                string.IsNullOrEmpty(AddressTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            try
            {
                var existingShop = await _shopRepository.GetShopByCodeAsync(CodeTextBox.Text);
                if (existingShop != null)
                {
                    MessageBox.Show("Магазин с таким кодом уже существует. Пожалуйста, введите уникальный код.");
                    return;
                }

                var newShop = new Shop
                {
                    Code = CodeTextBox.Text,
                    Name = NameTextBox.Text,
                    Address = AddressTextBox.Text
                };

                await _shopRepository.AddShopAsync(newShop);

                MessageBox.Show("Магазин успешно добавлен!");

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении магазина: {ex.Message}");
            }
        }
    }
}