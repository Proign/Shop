using ShopApp.BLL.Services;
using ShopApp.Models;
using ShopApp.DAL.Repositories;
using ShopApp.DAL.Interfaces;
using ShopApp.Configuration;
using ShopApp.Client.UI.ShopForm;
using ShopApp.Client.UI.ProductForm;
using System.Diagnostics;

namespace ShopApp.Client.UI.MainForm
{

    public partial class MainForm : Form
    {
        private readonly ProductService _productService;
        private readonly ShopService _shopService;
        private readonly IProductRepository _productRepository;
        private readonly IShopRepository _shopRepository;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly LoadService _loadService;
        private readonly SearchService _searchService;

        public MainForm()
        {
            InitializeComponent();

            Load += async (s, e) => await LoadShopsAsync();
            Load += async (s, e) => await LoadAvailableProductsToListBox();

            var appConfig = AppConfiguration.Load("Configuration/appsettings.json");

            if (appConfig.DataAccessMode == "Database")
            {
                var connectionString = appConfig.DatabaseSettings.ConnectionString;

                _productRepository = new SqlProductRepository(connectionString);
                _shopRepository = new SqlShopRepository(connectionString);
            }
            else
            {
                _productRepository = new FileProductRepository(appConfig.FileSettings.ProductFilePath);
                _shopRepository = new FileShopRepository(appConfig.FileSettings.ShopFilePath);
            }

            _shoppingCartService = new ShoppingCartService(_productRepository);
            _productService = new ProductService(_productRepository, _shopRepository);
            _shopService = new ShopService(_shopRepository, _productRepository);
            _searchService = new SearchService(_productRepository, _shopRepository);
            _loadService = new LoadService(_shopRepository, _productRepository, _productService);
        }

        private async Task LoadShopsAsync()
        {
            var shopNames = await _loadService.LoadShopsAsync();

            ShopStripComboBox.Items.Clear();
            ShopStripComboBox.Items.AddRange(shopNames.ToArray());

            ShopStripComboBox.SelectedIndex = 0;
        }

        private async Task LoadProducts(string selectedShop = null)
        {
            try
            {
                var products = await _loadService.LoadProductsAsync(selectedShop);
                UpdateProductsGridView(products);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке товаров: {ex.Message}");
            }
        }

        private void addShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var shopForm = new AddShopForm();
            shopForm.FormClosed += (s, args) => LoadShopsAsync(); 
            shopForm.ShowDialog();
        }

        private void addProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var productForm = new AddProductForm();
            var products = _productService.GetAllProductsWithShopDetailsAsync();

            var selectedShop = ShopStripComboBox.SelectedItem.ToString();

            productForm.FormClosed += (s, args) => LoadProducts(selectedShop);
            productForm.FormClosed += (s, args) => LoadAvailableProductsToListBox();

            productForm.ShowDialog();
        }

        private void UpdateShoppingCartListBox()
        {
            ShoppingCartListBox.Items.Clear();
            foreach (var item in _shoppingCartService.GetCartContents())
            {
                ShoppingCartListBox.Items.Add($"{item.Key.Name} - Количество: {item.Value}");
            }
        }

        private void ProductsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Получаем товар по выбранной строке
                var product = (Product)ProductsGridView.Rows[e.RowIndex].DataBoundItem;

                // Проверяем, есть ли товар в корзине и сколько его в наличии
                var currentQuantityInCart = _shoppingCartService.GetCartContents()
                                                                 .FirstOrDefault(p => p.Key.Name == product.Name)
                                                                 .Value;

                // Получаем доступное количество товара в магазине
                var availableQuantityInStore = product.Quantity;

                // Проверяем, можно ли добавить товар в корзину
                if (currentQuantityInCart >= availableQuantityInStore)
                {
                    MessageBox.Show("Невозможно добавить больше товаров, чем есть в наличии.");
                    return; // Не добавляем товар в корзину, если количество превышает доступное
                }

                // Добавляем товар в корзину
                _shoppingCartService.AddProduct(product);

                // Обновляем ListBox
                UpdateShoppingCartListBox();
            }
        }

        private void ShoppingCartListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Получаем выбранный товар из списка корзины
            var selectedItem = ShoppingCartListBox.SelectedItem?.ToString();

            if (selectedItem != null)
            {
                // Извлекаем имя товара
                var productName = selectedItem.Split('-')[0].Trim();

                // Находим товар в корзине
                var product = _shoppingCartService.GetCartContents()
                                                   .FirstOrDefault(p => p.Key.Name == productName).Key;

                // Если товар найден и количество больше 1, уменьшаем его
                if (product != null)
                {
                    int currentQuantity = _shoppingCartService.GetCartContents()[product];
                    if (currentQuantity > 1)
                    {
                        _shoppingCartService.UpdateProductQuantity(product, currentQuantity - 1);
                    }
                    else
                    {
                        _shoppingCartService.RemoveProduct(product);
                    }

                    // Обновляем ListBox
                    UpdateShoppingCartListBox();
                }
            }
        }

        private async void RedeemCartButton_Click(object sender, EventArgs e)
        {
            try
            {
                decimal maxPrice = 0;
                if (!string.IsNullOrWhiteSpace(MaxPriceTextBox.Text) && !decimal.TryParse(MaxPriceTextBox.Text, out maxPrice))
                {
                    MessageBox.Show("Введите корректную сумму.");
                    return;
                }

                var selectedShop = ShopStripComboBox.SelectedItem?.ToString();

                var (success, message, totalPurchaseCost) = await _shoppingCartService.RedeemCartAsync(maxPrice, selectedShop);

                MessageBox.Show(message);

                if (success)
                {
                    ShoppingCartListBox.Items.Clear();
                    UpdateShoppingCartListBox();

                    if (maxPrice > 0)
                    {
                        var remainingBudget = maxPrice - totalPurchaseCost;
                        MaxPriceTextBox.Text = remainingBudget.ToString("0.00");

                        if (selectedShop != "Все магазины" && remainingBudget > 0)
                        {
                            await PerformSearchByMaxPriceAsync(selectedShop, remainingBudget);
                        }
                        else
                        {
                            LoadProducts(selectedShop);
                        }
                    }
                    else
                    {
                        LoadProducts(selectedShop);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при оплате корзины: {ex.Message}");
            }
        }

        private async Task PerformSearchByMaxPriceAsync(string selectedShop, decimal maxPrice)
        {
            try
            {
                var affordableProducts = await _searchService.GetAffordableProductsByMaxPriceAsync(selectedShop, maxPrice);

                UpdateProductsGridView(affordableProducts);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске товаров: {ex.Message}");
            }
        }

        private async void ShopStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MaxPriceTextBox.Text = string.Empty;

            var selectedShop = ShopStripComboBox.SelectedItem.ToString();
            IEnumerable<Product> products;

            if (selectedShop == "Все магазины")
            {
                products = await _productService.GetAllProductsWithShopDetailsAsync();
            }
            else
            {
                var shops = await _shopRepository.GetAllShopsAsync();
                var selectedShopCode = shops.FirstOrDefault(shop => shop.Name == selectedShop)?.Code;

                products = selectedShopCode != null
                    ? (await _productService.GetAllProductsWithShopDetailsAsync())
                        .Where(p => p.Shop.Code == selectedShopCode && p.Quantity > 0) 
                    : Enumerable.Empty<Product>();
            }

            UpdateProductsGridView(products);
        }

        private void UpdateProductsGridView(IEnumerable<Product> products)
        {
            ProductsGridView.Columns.Clear();

            ProductsGridView.DataSource = products.ToList();

            ProductsGridView.Columns["Name"].HeaderText = "Название товара";
            ProductsGridView.Columns["Price"].HeaderText = "Цена";
            ProductsGridView.Columns["Quantity"].HeaderText = "Количество";

            ProductsGridView.Columns["Shop"].Visible = false;

            if (!ProductsGridView.Columns.Contains("ShopName"))
            {
                ProductsGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShopName", HeaderText = "Название магазина" });
                ProductsGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShopAddress", HeaderText = "Адрес магазина" });
                ProductsGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShopCode", HeaderText = "Код магазина" });
            }

            foreach (DataGridViewRow row in ProductsGridView.Rows)
            {
                var product = (Product)row.DataBoundItem;
                row.Cells["ShopName"].Value = product.Shop.Name;
                row.Cells["ShopAddress"].Value = product.Shop.Address;
                row.Cells["ShopCode"].Value = product.Shop.Code;
            }

            ProductsGridView.Columns["ShopName"].DisplayIndex = 3; 
            ProductsGridView.Columns["ShopAddress"].DisplayIndex = 4; 
            ProductsGridView.Columns["ShopCode"].DisplayIndex = 5;
            ProductsGridView.Columns["Name"].DisplayIndex = 0; 
            ProductsGridView.Columns["Quantity"].DisplayIndex = 1;
            ProductsGridView.Columns["Price"].DisplayIndex = 2;

            ProductsGridView.Refresh();
        }

        private async void InShopSearchButton_Click(object sender, EventArgs e)
        {
            var selectedShop = ShopStripComboBox.SelectedItem.ToString();
            if (decimal.TryParse(MaxPriceTextBox.Text, out var maxPrice))
            {
                await PerformSearchByMaxPriceAsync(selectedShop, maxPrice);
            }
            else
            {
                MessageBox.Show("Введите корректную сумму.");
            }
        }

        private async void PriceClearButton_Click(object sender, EventArgs e)
        {
            MaxPriceTextBox.Text = string.Empty;

            ShopStripComboBox.SelectedIndex = 0;

            try
            {
                // Получаем все товары для всех магазинов
                var products = await _productService.GetAllProductsWithShopDetailsAsync();

                // Отображаем все товары в GridView
                UpdateProductsGridView(products);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке товаров: {ex.Message}");
            }
        }

        private async void AllShopsSearchButton_Click(object sender, EventArgs e)
        {
            ShopStripComboBox.SelectedIndex = 0;
            var productName = CheapestProductTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show("Введите название товара для поиска.");
                return;
            }

            try
            {
                var matchingProducts = await _searchService.SearchProductByNameAsync(productName);

                if (matchingProducts.Any())
                {
                    var cheapestProduct = matchingProducts.First();

                    ProductsGridView.DataSource = new List<Product> { cheapestProduct };

                    ProductsGridView.Columns["Name"].HeaderText = "Название товара";
                    ProductsGridView.Columns["Quantity"].HeaderText = "Количество";
                    ProductsGridView.Columns["Price"].HeaderText = "Цена";

                    if (!ProductsGridView.Columns.Contains("ShopName"))
                    {
                        ProductsGridView.Columns.Add("ShopName", "Название магазина");
                        ProductsGridView.Columns.Add("ShopAddress", "Адрес магазина");
                        ProductsGridView.Columns.Add("ShopCode", "Код магазина");
                    }

                    foreach (DataGridViewRow row in ProductsGridView.Rows)
                    {
                        var product = (Product)row.DataBoundItem;

                        if (product.Shop != null)
                        {
                            row.Cells["ShopName"].Value = product.Shop.Name;
                            row.Cells["ShopAddress"].Value = product.Shop.Address;
                            row.Cells["ShopCode"].Value = product.Shop.Code;
                        }
                        else
                        {
                            Debug.WriteLine($"Product {product.Name} does not have a Shop associated with it.");
                        }
                    }

                    ProductsGridView.Columns["Name"].DisplayIndex = 0;
                    ProductsGridView.Columns["Quantity"].DisplayIndex = 1;
                    ProductsGridView.Columns["Price"].DisplayIndex = 2;
                    ProductsGridView.Columns["ShopName"].DisplayIndex = 3;
                    ProductsGridView.Columns["ShopAddress"].DisplayIndex = 4;
                    ProductsGridView.Columns["ShopCode"].DisplayIndex = 5;

                    ProductsGridView.Refresh();
                }
                else
                {
                    MessageBox.Show("Товар с указанным названием и доступным количеством не найден.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении поиска: {ex.Message}");
            }
        }


        private void ProductsGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (ProductsGridView.Columns[e.ColumnIndex].Name == "ShopName")
            {
                var product = ProductsGridView.Rows[e.RowIndex].DataBoundItem as Product;
                e.Value = product?.Shop?.Name;
            }
            else if (ProductsGridView.Columns[e.ColumnIndex].Name == "ShopAddress")
            {
                var product = ProductsGridView.Rows[e.RowIndex].DataBoundItem as Product;
                e.Value = product?.Shop?.Address;
            }
            else if (ProductsGridView.Columns[e.ColumnIndex].Name == "ShopCode")
            {
                var product = ProductsGridView.Rows[e.RowIndex].DataBoundItem as Product;
                e.Value = product?.Shop?.Code;
            }
        }

        private async void ClearAllShopsSearchButton_Click(object sender, EventArgs e)
        {
            CheapestProductTextBox.Text = string.Empty;

            for (int i = 0; i < ProductsListBox.Items.Count; i++)
            {
                ProductsListBox.SetItemChecked(i, false);
            }

            ShopStripComboBox.SelectedIndex = 0;

            try
            {
                var products = await _productService.GetAllProductsWithShopDetailsAsync();

                UpdateProductsGridView(products);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке товаров: {ex.Message}");
            }
        }

        private async Task LoadAvailableProductsToListBox()
        {
            try
            {
                ProductsListBox.Items.Clear();

                var allProducts = await _productRepository.GetAllProductsAsync();

                // Отбираем товары, количество которых больше 0, и убираем дубликаты по названию
                var uniqueProducts = allProducts
                    .Where(p => p.Quantity > 0)
                    .GroupBy(p => p.Name)
                    .Select(g => g.First()) // Берем первый товар с уникальным именем
                    .ToList();

                ProductsListBox.DisplayMember = "Name";

                foreach (var product in uniqueProducts)
                {
                    ProductsListBox.Items.Add(product);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке товаров: {ex.Message}");
            }
        }

        private async void CheapestBatchButton_Click(object sender, EventArgs e)
        {
            var selectedProducts = ProductsListBox.CheckedItems.Cast<Product>().ToList();

            if (!selectedProducts.Any())
            {
                MessageBox.Show("Пожалуйста, выберите хотя бы один товар.");
                return;
            }

            try
            {
                var productQuantities = new Dictionary<Product, int>();

                foreach (var selectedProduct in selectedProducts)
                {
                    string quantityInput = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Введите количество для товара {selectedProduct.Name}:",
                        "Введите количество",
                        "1");

                    if (!int.TryParse(quantityInput, out int desiredQuantity) || desiredQuantity <= 0)
                    {
                        MessageBox.Show($"Некорректное количество для товара {selectedProduct.Name}.");
                        return;
                    }

                    productQuantities[selectedProduct] = desiredQuantity;
                }

                var (cheapestShop, totalCost, productsAvailable) = await _searchService.FindCheapestShopForProductsAsync(productQuantities);

                if (!productsAvailable)
                {
                    MessageBox.Show("Не найдено магазина, в котором доступны все выбранные товары в нужном количестве.");
                    return;
                }

                DisplayCheapestShopProducts(cheapestShop, selectedProducts);
                MessageBox.Show($"Самый дешевый магазин для выбранного набора товаров: {cheapestShop.Name}.\nОбщая стоимость: {totalCost}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске самого дешевого магазина: {ex.Message}");
            }
        }

        private void DisplayCheapestShopProducts(Shop shop, List<Product> selectedProducts)
        {
            ShopStripComboBox.SelectedIndex = ShopStripComboBox.Items
                .Cast<string>()
                .ToList()
                .FindIndex(item => item == shop.Name || item == "Все магазины");

            bool isAllShopsSelected = ShopStripComboBox.SelectedItem?.ToString() == "Все магазины";

            var productsWithShopInfo = selectedProducts
                .Where(p => isAllShopsSelected || (p.Shop.Code == shop.Code && p.Quantity > 0))
                .Select(p => new Product
                {
                    Name = p.Name,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    Shop = new Shop
                    {
                        Code = p.Shop.Code,
                        Name = shop.Name,
                        Address = shop.Address
                    }
                })
                .ToList();

            if (!ProductsGridView.Columns.Contains("ShopName"))
            {
                ProductsGridView.Columns.Add("ShopName", "Название магазина");
                ProductsGridView.Columns.Add("ShopAddress", "Адрес магазина");
                ProductsGridView.Columns.Add("ShopCode", "Код магазина");
            }

            ProductsGridView.DataSource = productsWithShopInfo;

            ProductsGridView.Columns["Name"].HeaderText = "Название товара";
            ProductsGridView.Columns["Quantity"].HeaderText = "Количество";
            ProductsGridView.Columns["Price"].HeaderText = "Цена";
            ProductsGridView.Columns["ShopName"].HeaderText = "Название магазина";
            ProductsGridView.Columns["ShopAddress"].HeaderText = "Адрес магазина";
            ProductsGridView.Columns["ShopCode"].HeaderText = "Код магазина";

            ProductsGridView.CellFormatting += ProductsGridView_CellFormatting;

            ProductsGridView.Refresh();
        }

        private async void ProductsGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ProductsGridView.Columns["Quantity"].Index)
            {
                try
                {
                    var newQuantity = int.Parse(ProductsGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                    var product = ProductsGridView.Rows[e.RowIndex].DataBoundItem as Product;

                    if (product != null)
                    {
                        if (newQuantity <= 0 || newQuantity > product.Quantity)
                        {
                            MessageBox.Show("Некорректное количество товара.");
                            ProductsGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = product.Quantity;
                            return;
                        }

                        product.Quantity = newQuantity;

                        await _productRepository.UpdateProductStockAndPriceAsync(product.Name, product.Shop.Code, product.Quantity, product.Price);

                        ProductsGridView.Refresh();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при обновлении количества товара.");
                }
            }
        }
    }
}