namespace ShopApp.Client.UI.MainForm
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            ShopMenuStrip = new MenuStrip();
            actionsToolStripMenuItem = new ToolStripMenuItem();
            addShopToolStripMenuItem = new ToolStripMenuItem();
            addProductToolStripMenuItem = new ToolStripMenuItem();
            ShopStripComboBox = new ToolStripComboBox();
            SearchGroupBox = new GroupBox();
            SearchTabControl = new TabControl();
            CurrentShopTabPage = new TabPage();
            PriceClearButton = new Button();
            InShopSearchButton = new Button();
            MaxPriceTextBox = new TextBox();
            ProductPriceLabel = new Label();
            AllShopsTabPage = new TabPage();
            CheapestBatchButton = new Button();
            ClearAllShopsSearchButton = new Button();
            AllShopsSearchButton = new Button();
            ProductsListBox = new CheckedListBox();
            BatchSearchLabel = new Label();
            CheapestProductTextBox = new TextBox();
            AllShopsProductLabel = new Label();
            CartGroupBox = new GroupBox();
            RedeemCartButton = new Button();
            ShoppingCartListBox = new ListBox();
            ProductsGridView = new DataGridView();
            ShopMenuStrip.SuspendLayout();
            SearchGroupBox.SuspendLayout();
            SearchTabControl.SuspendLayout();
            CurrentShopTabPage.SuspendLayout();
            AllShopsTabPage.SuspendLayout();
            CartGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ProductsGridView).BeginInit();
            SuspendLayout();
            // 
            // ShopMenuStrip
            // 
            ShopMenuStrip.BackColor = SystemColors.MenuBar;
            ShopMenuStrip.ImageScalingSize = new Size(20, 20);
            ShopMenuStrip.Items.AddRange(new ToolStripItem[] { actionsToolStripMenuItem, ShopStripComboBox });
            ShopMenuStrip.Location = new Point(0, 0);
            ShopMenuStrip.Name = "ShopMenuStrip";
            ShopMenuStrip.Size = new Size(1356, 32);
            ShopMenuStrip.TabIndex = 0;
            ShopMenuStrip.Text = "menuStrip1";
            // 
            // actionsToolStripMenuItem
            // 
            actionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addShopToolStripMenuItem, addProductToolStripMenuItem });
            actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            actionsToolStripMenuItem.Size = new Size(90, 28);
            actionsToolStripMenuItem.Text = "Добавить";
            // 
            // addShopToolStripMenuItem
            // 
            addShopToolStripMenuItem.Name = "addShopToolStripMenuItem";
            addShopToolStripMenuItem.Size = new Size(221, 26);
            addShopToolStripMenuItem.Text = "Добавить магазин";
            addShopToolStripMenuItem.Click += addShopToolStripMenuItem_Click;
            // 
            // addProductToolStripMenuItem
            // 
            addProductToolStripMenuItem.Name = "addProductToolStripMenuItem";
            addProductToolStripMenuItem.Size = new Size(221, 26);
            addProductToolStripMenuItem.Text = "Добавить товар";
            addProductToolStripMenuItem.Click += addProductToolStripMenuItem_Click;
            // 
            // ShopStripComboBox
            // 
            ShopStripComboBox.Margin = new Padding(10, 0, 10, 0);
            ShopStripComboBox.Name = "ShopStripComboBox";
            ShopStripComboBox.Size = new Size(200, 28);
            ShopStripComboBox.SelectedIndexChanged += ShopStripComboBox_SelectedIndexChanged;
            // 
            // SearchGroupBox
            // 
            SearchGroupBox.Controls.Add(SearchTabControl);
            SearchGroupBox.Location = new Point(1013, 31);
            SearchGroupBox.Name = "SearchGroupBox";
            SearchGroupBox.Size = new Size(331, 467);
            SearchGroupBox.TabIndex = 1;
            SearchGroupBox.TabStop = false;
            SearchGroupBox.Text = "Поиск";
            // 
            // SearchTabControl
            // 
            SearchTabControl.Controls.Add(CurrentShopTabPage);
            SearchTabControl.Controls.Add(AllShopsTabPage);
            SearchTabControl.ItemSize = new Size(50, 25);
            SearchTabControl.Location = new Point(6, 26);
            SearchTabControl.Name = "SearchTabControl";
            SearchTabControl.SelectedIndex = 0;
            SearchTabControl.Size = new Size(319, 435);
            SearchTabControl.TabIndex = 0;
            // 
            // CurrentShopTabPage
            // 
            CurrentShopTabPage.Controls.Add(PriceClearButton);
            CurrentShopTabPage.Controls.Add(InShopSearchButton);
            CurrentShopTabPage.Controls.Add(MaxPriceTextBox);
            CurrentShopTabPage.Controls.Add(ProductPriceLabel);
            CurrentShopTabPage.Location = new Point(4, 29);
            CurrentShopTabPage.Name = "CurrentShopTabPage";
            CurrentShopTabPage.Padding = new Padding(3);
            CurrentShopTabPage.Size = new Size(311, 402);
            CurrentShopTabPage.TabIndex = 0;
            CurrentShopTabPage.Text = "В текущем магазине";
            CurrentShopTabPage.UseVisualStyleBackColor = true;
            // 
            // PriceClearButton
            // 
            PriceClearButton.Location = new Point(6, 91);
            PriceClearButton.Name = "PriceClearButton";
            PriceClearButton.Size = new Size(94, 29);
            PriceClearButton.TabIndex = 5;
            PriceClearButton.Text = "Очистить";
            PriceClearButton.UseVisualStyleBackColor = true;
            PriceClearButton.Click += PriceClearButton_Click;
            // 
            // InShopSearchButton
            // 
            InShopSearchButton.Location = new Point(211, 91);
            InShopSearchButton.Name = "InShopSearchButton";
            InShopSearchButton.Size = new Size(94, 29);
            InShopSearchButton.TabIndex = 4;
            InShopSearchButton.Text = "Найти";
            InShopSearchButton.UseVisualStyleBackColor = true;
            InShopSearchButton.Click += InShopSearchButton_Click;
            // 
            // MaxPriceTextBox
            // 
            MaxPriceTextBox.Location = new Point(3, 42);
            MaxPriceTextBox.Name = "MaxPriceTextBox";
            MaxPriceTextBox.Size = new Size(302, 27);
            MaxPriceTextBox.TabIndex = 3;
            // 
            // ProductPriceLabel
            // 
            ProductPriceLabel.AutoSize = true;
            ProductPriceLabel.Location = new Point(6, 19);
            ProductPriceLabel.Name = "ProductPriceLabel";
            ProductPriceLabel.Size = new Size(214, 20);
            ProductPriceLabel.TabIndex = 2;
            ProductPriceLabel.Text = "По максимальной стоимости";
            // 
            // AllShopsTabPage
            // 
            AllShopsTabPage.Controls.Add(CheapestBatchButton);
            AllShopsTabPage.Controls.Add(ClearAllShopsSearchButton);
            AllShopsTabPage.Controls.Add(AllShopsSearchButton);
            AllShopsTabPage.Controls.Add(ProductsListBox);
            AllShopsTabPage.Controls.Add(BatchSearchLabel);
            AllShopsTabPage.Controls.Add(CheapestProductTextBox);
            AllShopsTabPage.Controls.Add(AllShopsProductLabel);
            AllShopsTabPage.Location = new Point(4, 29);
            AllShopsTabPage.Name = "AllShopsTabPage";
            AllShopsTabPage.Padding = new Padding(3);
            AllShopsTabPage.Size = new Size(311, 402);
            AllShopsTabPage.TabIndex = 1;
            AllShopsTabPage.Text = "Во всех магазинах";
            AllShopsTabPage.UseVisualStyleBackColor = true;
            // 
            // CheapestBatchButton
            // 
            CheapestBatchButton.Location = new Point(211, 313);
            CheapestBatchButton.Name = "CheapestBatchButton";
            CheapestBatchButton.Size = new Size(94, 29);
            CheapestBatchButton.TabIndex = 6;
            CheapestBatchButton.Text = "Найти";
            CheapestBatchButton.UseVisualStyleBackColor = true;
            CheapestBatchButton.Click += CheapestBatchButton_Click;
            // 
            // ClearAllShopsSearchButton
            // 
            ClearAllShopsSearchButton.Location = new Point(6, 313);
            ClearAllShopsSearchButton.Name = "ClearAllShopsSearchButton";
            ClearAllShopsSearchButton.Size = new Size(94, 29);
            ClearAllShopsSearchButton.TabIndex = 5;
            ClearAllShopsSearchButton.Text = "Очистить";
            ClearAllShopsSearchButton.UseVisualStyleBackColor = true;
            ClearAllShopsSearchButton.Click += ClearAllShopsSearchButton_Click;
            // 
            // AllShopsSearchButton
            // 
            AllShopsSearchButton.Location = new Point(211, 97);
            AllShopsSearchButton.Name = "AllShopsSearchButton";
            AllShopsSearchButton.Size = new Size(94, 29);
            AllShopsSearchButton.TabIndex = 4;
            AllShopsSearchButton.Text = "Найти";
            AllShopsSearchButton.UseVisualStyleBackColor = true;
            AllShopsSearchButton.Click += AllShopsSearchButton_Click;
            // 
            // ProductsListBox
            // 
            ProductsListBox.FormattingEnabled = true;
            ProductsListBox.Location = new Point(6, 171);
            ProductsListBox.Name = "ProductsListBox";
            ProductsListBox.Size = new Size(299, 136);
            ProductsListBox.TabIndex = 3;
            // 
            // BatchSearchLabel
            // 
            BatchSearchLabel.AutoSize = true;
            BatchSearchLabel.Location = new Point(6, 128);
            BatchSearchLabel.Name = "BatchSearchLabel";
            BatchSearchLabel.Size = new Size(199, 40);
            BatchSearchLabel.TabIndex = 2;
            BatchSearchLabel.Text = "Партия товаров \r\nс наименьшей стоимостью";
            // 
            // CheapestProductTextBox
            // 
            CheapestProductTextBox.Location = new Point(6, 64);
            CheapestProductTextBox.Name = "CheapestProductTextBox";
            CheapestProductTextBox.Size = new Size(299, 27);
            CheapestProductTextBox.TabIndex = 1;
            // 
            // AllShopsProductLabel
            // 
            AllShopsProductLabel.AutoSize = true;
            AllShopsProductLabel.Location = new Point(6, 21);
            AllShopsProductLabel.Name = "AllShopsProductLabel";
            AllShopsProductLabel.Size = new Size(249, 40);
            AllShopsProductLabel.TabIndex = 0;
            AllShopsProductLabel.Text = "Товар с наименьшей стоимостью \r\nпо названию";
            // 
            // CartGroupBox
            // 
            CartGroupBox.Controls.Add(RedeemCartButton);
            CartGroupBox.Controls.Add(ShoppingCartListBox);
            CartGroupBox.Location = new Point(766, 31);
            CartGroupBox.Name = "CartGroupBox";
            CartGroupBox.Size = new Size(241, 467);
            CartGroupBox.TabIndex = 2;
            CartGroupBox.TabStop = false;
            CartGroupBox.Text = "Корзина";
            // 
            // RedeemCartButton
            // 
            RedeemCartButton.Location = new Point(6, 416);
            RedeemCartButton.Name = "RedeemCartButton";
            RedeemCartButton.Size = new Size(94, 29);
            RedeemCartButton.TabIndex = 1;
            RedeemCartButton.Text = "Купить";
            RedeemCartButton.UseVisualStyleBackColor = true;
            RedeemCartButton.Click += RedeemCartButton_Click;
            // 
            // ShoppingCartListBox
            // 
            ShoppingCartListBox.FormattingEnabled = true;
            ShoppingCartListBox.Location = new Point(6, 26);
            ShoppingCartListBox.Name = "ShoppingCartListBox";
            ShoppingCartListBox.Size = new Size(229, 384);
            ShoppingCartListBox.TabIndex = 0;
            ShoppingCartListBox.MouseDoubleClick += ShoppingCartListBox_MouseDoubleClick;
            // 
            // ProductsGridView
            // 
            ProductsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ProductsGridView.BackgroundColor = SystemColors.Window;
            ProductsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ProductsGridView.Location = new Point(12, 41);
            ProductsGridView.Name = "ProductsGridView";
            ProductsGridView.RowHeadersWidth = 51;
            ProductsGridView.Size = new Size(748, 457);
            ProductsGridView.TabIndex = 3;
            ProductsGridView.CellContentClick += ProductsGridView_CellContentClick;
            ProductsGridView.CellEndEdit += ProductsGridView_CellEndEdit;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1356, 510);
            Controls.Add(ProductsGridView);
            Controls.Add(CartGroupBox);
            Controls.Add(SearchGroupBox);
            Controls.Add(ShopMenuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Магазин";
            ShopMenuStrip.ResumeLayout(false);
            ShopMenuStrip.PerformLayout();
            SearchGroupBox.ResumeLayout(false);
            SearchTabControl.ResumeLayout(false);
            CurrentShopTabPage.ResumeLayout(false);
            CurrentShopTabPage.PerformLayout();
            AllShopsTabPage.ResumeLayout(false);
            AllShopsTabPage.PerformLayout();
            CartGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ProductsGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip ShopMenuStrip;
        private ToolStripMenuItem actionsToolStripMenuItem;
        private ToolStripMenuItem addShopToolStripMenuItem;
        private ToolStripMenuItem addProductToolStripMenuItem;
        private GroupBox SearchGroupBox;
        private TabControl SearchTabControl;
        private TabPage CurrentShopTabPage;
        private TabPage AllShopsTabPage;
        private Label ProductPriceLabel;
        private TextBox ProductTitleTextBox;
        private Label ProductTitleLabel;
        private Button InShopSearchButton;
        private TextBox MaxPriceTextBox;
        private Label AllShopsProductLabel;
        private Label BatchSearchLabel;
        private TextBox CheapestProductTextBox;
        private CheckedListBox ProductsListBox;
        private Button AllShopsSearchButton;
        private GroupBox CartGroupBox;
        private Button RedeemCartButton;
        private ListBox ShoppingCartListBox;
        private DataGridView ProductsGridView;
        private ToolStripComboBox ShopStripComboBox;
        private Button PriceClearButton;
        private Button ClearAllShopsSearchButton;
        private Button CheapestBatchButton;
    }
}