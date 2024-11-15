namespace ShopApp.Client.UI.ShopForm
{
    partial class AddShopForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddShopForm));
            CodeLabel = new Label();
            NameLabel = new Label();
            AddressLabel = new Label();
            AddShopButton = new Button();
            CodeTextBox = new TextBox();
            NameTextBox = new TextBox();
            AddressTextBox = new TextBox();
            SuspendLayout();
            // 
            // CodeLabel
            // 
            CodeLabel.AutoSize = true;
            CodeLabel.Location = new Point(12, 19);
            CodeLabel.Name = "CodeLabel";
            CodeLabel.Size = new Size(124, 20);
            CodeLabel.TabIndex = 0;
            CodeLabel.Text = "Уникальный код";
            // 
            // NameLabel
            // 
            NameLabel.AutoSize = true;
            NameLabel.Location = new Point(12, 67);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new Size(77, 20);
            NameLabel.TabIndex = 1;
            NameLabel.Text = "Название";
            // 
            // AddressLabel
            // 
            AddressLabel.AutoSize = true;
            AddressLabel.Location = new Point(15, 116);
            AddressLabel.Name = "AddressLabel";
            AddressLabel.Size = new Size(51, 20);
            AddressLabel.TabIndex = 2;
            AddressLabel.Text = "Адрес";
            // 
            // AddShopButton
            // 
            AddShopButton.Location = new Point(272, 192);
            AddShopButton.Name = "AddShopButton";
            AddShopButton.Size = new Size(94, 29);
            AddShopButton.TabIndex = 3;
            AddShopButton.Text = "Добавить";
            AddShopButton.UseVisualStyleBackColor = true;
            // 
            // CodeTextBox
            // 
            CodeTextBox.Location = new Point(142, 16);
            CodeTextBox.Name = "CodeTextBox";
            CodeTextBox.Size = new Size(224, 27);
            CodeTextBox.TabIndex = 4;
            // 
            // NameTextBox
            // 
            NameTextBox.Location = new Point(98, 64);
            NameTextBox.Name = "NameTextBox";
            NameTextBox.Size = new Size(268, 27);
            NameTextBox.TabIndex = 5;
            // 
            // AddressTextBox
            // 
            AddressTextBox.Location = new Point(72, 113);
            AddressTextBox.Name = "AddressTextBox";
            AddressTextBox.Size = new Size(294, 27);
            AddressTextBox.TabIndex = 6;
            // 
            // AddShopForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(378, 233);
            Controls.Add(AddressTextBox);
            Controls.Add(NameTextBox);
            Controls.Add(CodeTextBox);
            Controls.Add(AddShopButton);
            Controls.Add(AddressLabel);
            Controls.Add(NameLabel);
            Controls.Add(CodeLabel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AddShopForm";
            Text = "Добавить магазин";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label CodeLabel;
        private Label NameLabel;
        private Label AddressLabel;
        private Button AddShopButton;
        private TextBox CodeTextBox;
        private TextBox NameTextBox;
        private TextBox AddressTextBox;
    }
}