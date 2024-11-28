using Microsoft.Extensions.Configuration;
using ShopApp.DAL.Interfaces;
using ShopApp.DAL.Repositories;
using ShopApp.DAL.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopApp.Client.UI.ProductForm
{
    public partial class AddProductForm : Form
    {
        private readonly IProductRepository _productRepository;
        private readonly IShopRepository _shopRepository;

        public event EventHandler ProductAdded;

        public AddProductForm()
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
                _productRepository = new SqlProductRepository(connectionString);
                _shopRepository = new SqlShopRepository(connectionString);
            }
            else if (dataAccessMode == "File")
            {
                var productFilePath = configuration.GetSection("FileSettings")["ProductFilePath"];
                var shopFilePath = configuration.GetSection("FileSettings")["ShopFilePath"];
                _productRepository = new FileProductRepository(productFilePath);
                _shopRepository = new FileShopRepository(shopFilePath);
            }
            else
            {
                throw new Exception("Неизвестный режим работы.");
            }

            LoadShopsToComboBox();

            AddProductButton.Click += async (sender, e) => await AddProductButton_Click(sender, e);
        }

        private async Task AddProductButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) ||
                string.IsNullOrEmpty(ProductPriceTextBox.Text) ||
                string.IsNullOrEmpty(QuantityTextBox.Text) ||
                ShopCodeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            try
            {
                var selectedShopCode = ShopCodeComboBox.SelectedItem.ToString().Split('-')[0].Trim();

                var shop = await _shopRepository.GetShopByCodeAsync(selectedShopCode);

                if (shop == null)
                {
                    MessageBox.Show("Магазин не найден.");
                    return;
                }

                var newProduct = new Product
                {
                    Name = textBox1.Text,
                    Shop = shop, 
                    Price = decimal.Parse(ProductPriceTextBox.Text),
                    Quantity = int.Parse(QuantityTextBox.Text)
                };

                await _productRepository.AddProductAsync(newProduct);

                MessageBox.Show("Товар успешно добавлен!");
                ProductAdded?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении товара: {ex.Message}");
            }
        }

        private async Task LoadShopsToComboBox()
        {
            try
            {
                var shops = await _shopRepository.GetAllShopsAsync();

                var shopDisplayItems = shops.Select(shop => $"{shop.Code} - {shop.Name}").ToList();

                ShopCodeComboBox.DataSource = shopDisplayItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке магазинов: {ex.Message}");
            }
        }
    }
}