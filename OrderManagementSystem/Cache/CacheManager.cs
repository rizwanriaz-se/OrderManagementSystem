using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.UIComponents.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Cache
{
    public class CacheManager
    {
        public ObservableCollection<Category> _AllCategories { get; private set; }
        public ObservableCollection<Order> _AllOrders { get; private set; }
        public ObservableCollection<Product> _AllProducts { get; private set; }
        public ObservableCollection<User> _AllUsers { get; private set; }
        public CacheManager()
        {
            _AllUsers = new ObservableCollection<User>()
        {
            new User(){ Id = 1, Name = "John Doe", Email = "johndoe@example.com", Phone = "1234567890", Password = "Z1W(7MGkQ0", IsAdmin = true },
            new User(){ Id = 2, Name = "Jane David", Email = "janedavid@test.com", Phone = "0987654321", Password = "43GyyVMj_n ", IsAdmin = false },
            new User(){ Id = 3, Name = "John Smith", Email = "johnsmith@xyz.com", Phone = "1234567890", Password = "&qb42KSwtz", IsAdmin = false },
            new User(){ Id = 4, Name = "Taylor Travis", Email = "taylortravis@example.com", Phone = "0987654321", Password = "password", IsAdmin = false },
            new User(){ Id = 5, Name="Andrea Hernandez", Email="andreahernandez@test.com", Phone="3726139894", Password="#THfdeD72", IsAdmin=false },
        };

            _AllCategories = new ObservableCollection<Category>
            {
                new Category() { Id = 1, Name = "Beverages", Description = "Soft drinks, coffees, teas, beers, and ales", Picture = null },
            new Category() { Id = 2, Name = "Condiments", Description = "Sweet and savory sauces, relishes, spreads, and seasonings", Picture = null },
            new Category() { Id = 3, Name = "Confections", Description = "Desserts, candies, and sweet breads", Picture = null },
            new Category() { Id = 4, Name = "Dairy Products", Description = "Cheeses", Picture = null },
            new Category() { Id = 5, Name = "Grains/Cereals", Description = "Breads, crackers, pasta, and cereal", Picture = null },
            new Category() { Id = 6, Name = "Meat/Poultry", Description = "Prepared meats", Picture = null },
            new Category() { Id = 7, Name = "Produce", Description = "Dried fruit and bean curd", Picture = null },
            new Category() { Id = 8, Name = "Seafood", Description = "Seaweed and fish", Picture = null }
            };

            _AllProducts = new ObservableCollection<Product>
            {
            new Product(){ Id = 1, Name = "Chai", Description = "10 boxes x 20 bags", Picture = null, UnitPrice = 18.00M, UnitsInStock = 39, Category = GetCategoryById(1) },
            new Product(){ Id = 2, Name = "Chang", Description = "24 - 12 oz bottles", Picture = null, UnitPrice = 19.00M, UnitsInStock = 17, Category = GetCategoryById(1) },
            new Product(){ Id = 3, Name = "Aniseed Syrup", Description = "12 - 550 ml bottles", Picture = null, UnitPrice = 10.00M, UnitsInStock = 13, Category = GetCategoryById(2) },
            new Product(){ Id = 4, Name = "Chef Anton's Cajun Seasoning", Description = "48 - 6 oz jars", Picture = null, UnitPrice = 22.00M, UnitsInStock = 53, Category = GetCategoryById(2) },
            new Product(){ Id = 5, Name = "Chef Anton's Gumbo Mix", Description = "36 boxes", Picture = null, UnitPrice = 21.35M, UnitsInStock = 0, Category = GetCategoryById(2) },
            new Product(){ Id = 6, Name = "Grandma's Boysenberry Spread", Description = "12 - 8 oz jars", Picture = null, UnitPrice = 25.00M, UnitsInStock = 120, Category = GetCategoryById(2) },
            new Product(){ Id = 7, Name = "Uncle Bob's Organic Dried Pears", Description = "12 - 1 lb pkgs.", Picture = null, UnitPrice = 30.00M, UnitsInStock = 15, Category = GetCategoryById(7) },
            new Product(){ Id = 8, Name = "Northwoods Cranberry Sauce", Description = "12 - 12 oz jars", Picture = null, UnitPrice = 40.00M, UnitsInStock = 6, Category = GetCategoryById(2) },
            new Product(){ Id = 9, Name = "Mishi Kobe Niku", Description = "18 - 500 g pkgs.", Picture = null, UnitPrice = 97.00M, UnitsInStock = 29, Category = GetCategoryById(6) },
            new Product(){ Id = 10, Name = "Ikura", Description = "12 - 200 ml jars", Picture = null, UnitPrice = 31.00M, UnitsInStock = 31, Category = GetCategoryById(8) },
            new Product(){ Id = 11, Name = "Queso Cabrales", Description = "1 kg pkg.", Picture = null, UnitPrice = 21.00M, UnitsInStock = 22, Category = GetCategoryById(4) },
            new Product(){ Id = 12, Name = "Queso Manchego La Pastora", Description = "10 - 500 g pkgs.", Picture = null, UnitPrice = 38.00M, UnitsInStock = 86, Category = GetCategoryById(4) },
            new Product(){ Id = 13, Name = "Konbu", Description = "2 kg box", Picture = null, UnitPrice = 6.00M, UnitsInStock = 24, Category = GetCategoryById(8) },
            new Product(){ Id = 14, Name = "Tofu", Description = "40 - 100 g pkgs.", Picture = null, UnitPrice = 23.25M, UnitsInStock = 35, Category = GetCategoryById(7) },
            new Product(){ Id = 15, Name = "Genen Shouyu", Description = "24 - 250 ml bottles", Picture = null, UnitPrice = 15.50M, UnitsInStock = 39, Category = GetCategoryById(2) },

            };

            _AllOrders = new ObservableCollection<Order>
        {
            new Order()
            {
                Id = 1, User = GetUserByID(1), OrderDate = DateTime.Now, Status = Order.OrderStatus.Pending , ShippedDate = DateTime.Now.AddDays(2), ShippingAddress = "1234 Main St, Springfield, IL 62701", Products = new Dictionary<Product, int>()
                {

                { GetProductByID(1), 1 },
                    { GetProductByID(2), 3 }
                }
            },
            new Order()
            {
                Id = 2, User = GetUserByID(2), OrderDate = DateTime.Now, Status = Order.OrderStatus.Shipped, ShippedDate = DateTime.Now.AddDays(1), ShippingAddress = "5678 Elm St, Springfield, IL 62702" , Products = new Dictionary<Product, int>()
                {
                    { GetProductByID(3), 1 },
                    { GetProductByID(4), 2 },
                    { GetProductByID(5), 1 }
                }
            },
            new Order()
            {
                Id = 3, User = GetUserByID(3), OrderDate = DateTime.Now, Status = Order.OrderStatus.Delivered, ShippedDate = DateTime.Now.AddDays(3), ShippingAddress = "9101 Oak St, Springfield, IL 62703", Products = new Dictionary<Product, int>()
                {
                    { GetProductByID(6), 1 },
                    { GetProductByID(7), 1 },
                    { GetProductByID(8), 1 }
                }

            },
            new Order()
            {
                Id = 4, User = GetUserByID(4), OrderDate = DateTime.Now, Status = Order.OrderStatus.Pending, ShippedDate = DateTime.Now.AddDays(2), ShippingAddress = "1213 Pine St, Springfield, IL 62704", Products = new Dictionary<Product, int>()
                {
                    { GetProductByID(9), 2 },
                    { GetProductByID(10), 1 },
                    { GetProductByID(11), 3 }
                }
            },
            new Order()
            {
                Id = 5, User = GetUserByID(5), OrderDate = DateTime.Now, Status = Order.OrderStatus.Shipped, ShippedDate = DateTime.Now.AddDays(1), ShippingAddress = "1415 Cedar St, Springfield, IL 62705", Products = new Dictionary<Product, int>()
                {
                    { GetProductByID(12), 1 },
                    { GetProductByID(13), 2 },
                    { GetProductByID(14), 1 }
                }
            }
        };
           
        }


        public Category GetCategoryById(int id)
        {
            return _AllCategories.FirstOrDefault(c => c.Id == id);
        }
        public ObservableCollection<Category> GetAllCategories()
        {
            return _AllCategories;
        }
        public void AddCategory(Category category)
        {
            _AllCategories.Add(category);
        }
        public void DeleteCategory(Category category)
        {
            _AllCategories.Remove(category);
        }

        public void UpdateCategory(Category category)
        {
            var categoryToUpdate = _AllCategories.FirstOrDefault(c => c.Id == category.Id);
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.Picture = category.Picture;

            //MessageBox.Show("Category updated successfully");
        }

        public ObservableCollection<Order> GetAllOrders()
        {
            return _AllOrders;
        }
        public Order GetOrderById(int id)
        {
            return _AllOrders.FirstOrDefault(o => o.Id == id);
        }
        public Order AddOrder(Order order)
        {
            //order.Id = _AllOrders.Max(o => o.Id) + 1;
            _AllOrders.Add(order);
            return order;
        }

        public void UpdateOrder(Order updatedOrder)
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

                //MessageBox.Show($"Order Updated: {existingOrder.Id}, {existingOrder.User.Name}, {existingOrder.OrderDate}, " +
                //$"{existingOrder.Products.Count} products, {existingOrder.ShippedDate}, {existingOrder.ShippingAddress}");
            }
            else
            {
                //MessageBox.Show("Order not found.");
            }
        }

        public void DeleteOrder(Order order)
        {
            _AllOrders.Remove(order);
        }

        public ObservableCollection<Product> GetAllProducts()
        {
            return _AllProducts;
        }

        public Product GetProductByID(int id)
        {

            return _AllProducts.FirstOrDefault(p => p.Id == id);
        }
        public Product GetProductByName(ProductRow product)
        {


            return _AllProducts.FirstOrDefault(p => p.Name == product.SelectedProduct.Name);
        }

        public void AddProduct(Product product)
        {
            _AllProducts.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = _AllProducts.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Category = product.Category;
                existingProduct.Picture = product.Picture;
                existingProduct.UnitPrice = product.UnitPrice;
                existingProduct.UnitsInStock = product.UnitsInStock;

                //MessageBox.Show($"Product Updated: {existingProduct.Id}, {existingProduct.Name}, {existingProduct.Description}, {existingProduct.Category.Name}, {existingProduct.UnitPrice}, {existingProduct.UnitsInStock}");
            }
        }

        public void DeleteProduct(Product product)
        {
            _AllProducts.Remove(product);
            //int productId = product.Id;

            //var productToDelete = _AllProducts.FirstOrDefault(p => p.Id == productId);
            //if (productToDelete != null)
            //{
            //    _AllProducts.Remove(productToDelete);
            //}
        }

        public ObservableCollection<User> GetAllUsers()
        {
            return _AllUsers;
        }

        public User GetUserByID(int id)
        {
            return _AllUsers.FirstOrDefault(u => u.Id == id);
        }
    }
}
