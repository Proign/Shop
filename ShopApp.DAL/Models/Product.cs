﻿namespace ShopApp.DAL.Models
{
    public class Product
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Shop Shop { get; set; }
    }
}
