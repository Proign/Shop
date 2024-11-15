namespace ShopApp.Client.UI.ProductForm
{
    partial class AddProductForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddProductForm));
            ProductNameLabel = new Label();
            ShopCodeLabel = new Label();
            ProductPriceLabel = new Label();
            QuantityLabel = new Label();
            textBox1 = new TextBox();
            ShopCodeComboBox = new ComboBox();
            ProductPriceTextBox = new TextBox();
            QuantityTextBox = new TextBox();
            AddProductButton = new Button();
            SuspendLayout();
            // 
            // ProductNameLabel
            // 
            ProductNameLabel.AutoSize = true;
            ProductNameLabel.Location = new Point(12, 21);
            ProductNameLabel.Name = "ProductNameLabel";
            ProductNameLabel.Size = new Size(77, 20);
            ProductNameLabel.TabIndex = 0;
            ProductNameLabel.Text = "Название";
            // 
            // ShopCodeLabel
            // 
            ShopCodeLabel.AutoSize = true;
            ShopCodeLabel.Location = new Point(12, 66);
            ShopCodeLabel.Name = "ShopCodeLabel";
            ShopCodeLabel.Size = new Size(105, 20);
            ShopCodeLabel.TabIndex = 1;
            ShopCodeLabel.Text = "Код магазина";
            // 
            // ProductPriceLabel
            // 
            ProductPriceLabel.AutoSize = true;
            ProductPriceLabel.Location = new Point(12, 116);
            ProductPriceLabel.Name = "ProductPriceLabel";
            ProductPriceLabel.Size = new Size(45, 20);
            ProductPriceLabel.TabIndex = 2;
            ProductPriceLabel.Text = "Цена";
            // 
            // QuantityLabel
            // 
            QuantityLabel.AutoSize = true;
            QuantityLabel.Location = new Point(12, 165);
            QuantityLabel.Name = "QuantityLabel";
            QuantityLabel.Size = new Size(90, 20);
            QuantityLabel.TabIndex = 3;
            QuantityLabel.Text = "Количество";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(95, 18);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(318, 27);
            textBox1.TabIndex = 4;
            // 
            // ShopCodeComboBox
            // 
            ShopCodeComboBox.FormattingEnabled = true;
            ShopCodeComboBox.Location = new Point(120, 63);
            ShopCodeComboBox.Name = "ShopCodeComboBox";
            ShopCodeComboBox.Size = new Size(293, 28);
            ShopCodeComboBox.TabIndex = 5;
            // 
            // ProductPriceTextBox
            // 
            ProductPriceTextBox.Location = new Point(63, 113);
            ProductPriceTextBox.Name = "ProductPriceTextBox";
            ProductPriceTextBox.Size = new Size(350, 27);
            ProductPriceTextBox.TabIndex = 6;
            // 
            // QuantityTextBox
            // 
            QuantityTextBox.Location = new Point(108, 162);
            QuantityTextBox.Name = "QuantityTextBox";
            QuantityTextBox.Size = new Size(305, 27);
            QuantityTextBox.TabIndex = 7;
            // 
            // AddProductButton
            // 
            AddProductButton.Location = new Point(319, 232);
            AddProductButton.Name = "AddProductButton";
            AddProductButton.Size = new Size(94, 29);
            AddProductButton.TabIndex = 8;
            AddProductButton.Text = "Добавить";
            AddProductButton.UseVisualStyleBackColor = true;
            // 
            // AddProductForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(425, 273);
            Controls.Add(AddProductButton);
            Controls.Add(QuantityTextBox);
            Controls.Add(ProductPriceTextBox);
            Controls.Add(ShopCodeComboBox);
            Controls.Add(textBox1);
            Controls.Add(QuantityLabel);
            Controls.Add(ProductPriceLabel);
            Controls.Add(ShopCodeLabel);
            Controls.Add(ProductNameLabel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AddProductForm";
            Text = "Добавить товар";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label ProductNameLabel;
        private Label ShopCodeLabel;
        private Label ProductPriceLabel;
        private Label QuantityLabel;
        private TextBox textBox1;
        private ComboBox ShopCodeComboBox;
        private TextBox ProductPriceTextBox;
        private TextBox QuantityTextBox;
        private Button AddProductButton;
    }
}