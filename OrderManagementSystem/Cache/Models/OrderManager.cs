using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;
using System.Windows;
using System.ComponentModel;

namespace OrderManagementSystem.Cache.Models
{
    public class OrderManager
    {

        private static ObservableCollection<Order> _AllOrders = new ObservableCollection<Order>
        {
            new Order()
            {
                Id = 1, User = UserManager.GetUserByID(1), OrderDate = DateTime.Now, Status = Order.OrderStatus.Pending , ShippedDate = DateTime.Now.AddDays(2), ShippingAddress = "1234 Main St, Springfield, IL 62701", Products = new Dictionary<Product, int>()
                {

                { ProductManager.GetProductByID(1), 1 },
                { ProductManager.GetProductByID(2), 3 }
                }
            },
            new Order()
            {
                Id = 2, User = UserManager.GetUserByID(2), OrderDate = DateTime.Now, Status = Order.OrderStatus.Shipped, ShippedDate = DateTime.Now.AddDays(1), ShippingAddress = "5678 Elm St, Springfield, IL 62702" , Products = new Dictionary<Product, int>()
                {
                    { ProductManager.GetProductByID(3), 1 },
                    { ProductManager.GetProductByID(4), 2 },
                    { ProductManager.GetProductByID(5), 1 }
                }
            },
            new Order()
            {
                Id = 3, User = UserManager.GetUserByID(3), OrderDate = DateTime.Now, Status = Order.OrderStatus.Delivered, ShippedDate = DateTime.Now.AddDays(3), ShippingAddress = "9101 Oak St, Springfield, IL 62703", Products = new Dictionary<Product, int>()
                {
                    { ProductManager.GetProductByID(6), 1 },
                    { ProductManager.GetProductByID(7), 1 },
                    { ProductManager.GetProductByID(8), 1 }
                }

            },
            new Order()
            {
                Id = 4, User = UserManager.GetUserByID(4), OrderDate = DateTime.Now, Status = Order.OrderStatus.Pending, ShippedDate = DateTime.Now.AddDays(2), ShippingAddress = "1213 Pine St, Springfield, IL 62704", Products = new Dictionary<Product, int>()
                {
                    { ProductManager.GetProductByID(9), 2 },
                    { ProductManager.GetProductByID(10), 1 },
                    { ProductManager.GetProductByID(11), 3 }
                }
            },
            new Order()
            {
                Id = 5, User = UserManager.GetUserByID(5), OrderDate = DateTime.Now, Status = Order.OrderStatus.Shipped, ShippedDate = DateTime.Now.AddDays(1), ShippingAddress = "1415 Cedar St, Springfield, IL 62705", Products = new Dictionary<Product, int>()
                {
                    { ProductManager.GetProductByID(12), 1 },
                    { ProductManager.GetProductByID(13), 2 },
                    { ProductManager.GetProductByID(14), 1 }
                }
            }
        };

        public static ObservableCollection<Order> GetAllOrders()
        {
            return _AllOrders;
        }
        public static Order GetOrderById(int id)
        {
            return _AllOrders.FirstOrDefault(o => o.Id == id);
        }
        public static Order AddOrder(Order order)
        {
            //order.Id = _AllOrders.Max(o => o.Id) + 1;
            _AllOrders.Add(order);
            return order;
        }

        public static void UpdateOrder(Order updatedOrder)
        {
            // Retrieve the existing order with the same ID
            var existingOrder = _AllOrders.FirstOrDefault(o => o.Id == updatedOrder.Id);

            if (existingOrder != null)
            {
                // Update the existing order's properties
                existingOrder.User = updatedOrder.User;
                existingOrder.OrderDate = updatedOrder.OrderDate;
                existingOrder.Status = updatedOrder.Status;
                existingOrder.ShippedDate = updatedOrder.ShippedDate;
                existingOrder.ShippingAddress = updatedOrder.ShippingAddress;
                existingOrder.Products = updatedOrder.Products;

                MessageBox.Show($"Order Updated: {existingOrder.Id}, {existingOrder.User.Name}, {existingOrder.OrderDate}, " +
                    $"{existingOrder.Products.Count} products, {existingOrder.ShippedDate}, {existingOrder.ShippingAddress}");
            }
            else
            {
                MessageBox.Show("Order not found.");
            }
        }

    }
}
