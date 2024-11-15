using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShopApp.Models;
using ShopApp.DAL.Repositories;
using ShopApp.DAL.Interfaces;

namespace ShopApp.BLL.Services
{
    public class ShoppingCartService
    {
        private readonly Dictionary<Product, int> _cart;
        private readonly IProductRepository _productRepository;

        public ShoppingCartService(IProductRepository productRepository)
        {
            _cart = new Dictionary<Product, int>();
            _productRepository = productRepository;
        }

        public void AddProduct(Product product, int quantity = 1)
        {
            if (_cart.ContainsKey(product))
            {
                _cart[product] += quantity;
            }
            else
            {
                _cart[product] = quantity;
            }
        }

        public void UpdateProductQuantity(Product product, int quantity)
        {
            if (_cart.ContainsKey(product))
            {
                _cart[product] = quantity;
            }
        }

        public void RemoveProduct(Product product)
        {
            if (_cart.ContainsKey(product))
            {
                _cart.Remove(product);
            }
        }

        public void ClearCart()
        {
            _cart.Clear();
        }

        public Dictionary<Product, int> GetCartContents()
        {
            return new Dictionary<Product, int>(_cart);
        }

        public async Task<(bool Success, string Message, decimal TotalCost)> RedeemCartAsync(decimal maxPrice, string selectedShop)
        {
            if (_cart.Count == 0)
                return (false, "Корзина пуста.", 0);

            decimal totalPurchaseCost = 0;

            foreach (var item in _cart)
            {
                var product = item.Key;
                var quantityInCart = item.Value;

                var allProducts = await _productRepository.GetAllProductsAsync();

                var productToUpdate = allProducts.FirstOrDefault(p => p.Name == product.Name && p.Shop.Code == product.Shop.Code);

                if (productToUpdate != null)
                {
                    if (productToUpdate.Quantity >= quantityInCart)
                    {
                        productToUpdate.Quantity -= quantityInCart;
                        totalPurchaseCost += quantityInCart * productToUpdate.Price;
                    }
                    else
                    {
                        return (false, $"Недостаточно товара {product.Name} для выполнения операции.", totalPurchaseCost);
                    }

                    await _productRepository.UpdateProductStockAndPriceAsync(product.Name, product.Shop.Code, productToUpdate.Quantity, productToUpdate.Price);
                }
            }

            ClearCart();

            return (true, $"Корзина успешно оплачена, товары списаны.\nОбщая стоимость покупки: {totalPurchaseCost.ToString("0.00")}", totalPurchaseCost);
        }
    }
}