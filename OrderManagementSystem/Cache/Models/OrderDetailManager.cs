//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

//namespace OrderManagementSystem.Cache.Models
//{
//    new Order()
//    {
//        Id = 1, User = new User { Id = 1, Name = "John Doe", Email = "johndoe@example.com", Phone = "1234567890", Password = "Z1W(7MGkQ0", IsAdmin = true }, OrderDate = DateTime.Now, Status = "Pending", ShippedDate = DateTime.Now.AddDays(2), ShippingAddress = "1234 Main St, Springfield, IL 62701"
//            },
//            new Order()
//    {
//        Id = 2, User = new User { Id = 2, Name = "Jane David", Email = "janedavid@test.com", Phone = "0987654321", Password = "43GyyVMj_n ", IsAdmin = false }, OrderDate = DateTime.Now, Status = "Shipped", ShippedDate = DateTime.Now.AddDays(1), ShippingAddress = "5678 Elm St, Springfield, IL 62702"
//            },
//            new Order()
//    {
//        Id = 3, User = new User { Id = 3, Name = "John Smith", Email = "johnsmith@xyz.com", Phone = "1234567890", Password = "&qb42KSwtz", IsAdmin = false }, OrderDate = DateTime.Now, Status = "Delivered", ShippedDate = DateTime.Now.AddDays(3), ShippingAddress = "9101 Oak St, Springfield, IL 62703"

//            },
//            new Order()
//    {
//        Id = 4, User = new User { Id = 4, Name = "Taylor Travis", Email = "taylortravis@example.com", Phone = "0987654321", Password = "password", IsAdmin = false }, OrderDate = DateTime.Now, Status = "Pending", ShippedDate = DateTime.Now.AddDays(2), ShippingAddress = "1213 Pine St, Springfield, IL 62704"
//            },
//            new Order()
//    {
//        Id = 5, User = new User { Id = 5, Name = "Andrea Hernandez", Email = "andreahernandez@test.com", Phone = "3726139894", Password = "#THfdeD72", IsAdmin = false }, OrderDate = DateTime.Now, Status = "Shipped", ShippedDate = DateTime.Now.AddDays(1), ShippingAddress = "1415 Cedar St, Springfield, IL 62705"
//            }

//    public class OrderDetail
//    {

//        public Order Order { get; set; }
//        public List<Product> Products { get; set; }
//        public int UnitPrice { get; set; }
//        public int Quantity { get; set; }
//    }
//    public static ObservableCollection<Product> _AllProducts = new ObservableCollection<Product>()
//        {
//            new Product(){ Id = 1, Name = "Chai", Description = "10 boxes x 20 bags", Picture = null, UnitPrice = 18.00M, UnitsInStock = 39, Category = CategoryManager.GetCategoryById(1) },
//            new Product(){ Id = 2, Name = "Chang", Description = "24 - 12 oz bottles", Picture = null, UnitPrice = 19.00M, UnitsInStock = 17, Category = CategoryManager.GetCategoryById(1) },
//            new Product(){ Id = 3, Name = "Aniseed Syrup", Description = "12 - 550 ml bottles", Picture = null, UnitPrice = 10.00M, UnitsInStock = 13, Category = CategoryManager.GetCategoryById(2) },
//            new Product(){ Id = 4, Name = "Chef Anton's Cajun Seasoning", Description = "48 - 6 oz jars", Picture = null, UnitPrice = 22.00M, UnitsInStock = 53, Category = CategoryManager.GetCategoryById(2) },
//            new Product(){ Id = 5, Name = "Chef Anton's Gumbo Mix", Description = "36 boxes", Picture = null, UnitPrice = 21.35M, UnitsInStock = 0, Category = CategoryManager.GetCategoryById(2) },
//            new Product(){ Id = 6, Name = "Grandma's Boysenberry Spread", Description = "12 - 8 oz jars", Picture = null, UnitPrice = 25.00M, UnitsInStock = 120, Category = CategoryManager.GetCategoryById(2) },
//            new Product(){ Id = 7, Name = "Uncle Bob's Organic Dried Pears", Description = "12 - 1 lb pkgs.", Picture = null, UnitPrice = 30.00M, UnitsInStock = 15, Category = CategoryManager.GetCategoryById(7) },
//            new Product(){ Id = 8, Name = "Northwoods Cranberry Sauce", Description = "12 - 12 oz jars", Picture = null, UnitPrice = 40.00M, UnitsInStock = 6, Category = CategoryManager.GetCategoryById(2) },
//            new Product(){ Id = 9, Name = "Mishi Kobe Niku", Description = "18 - 500 g pkgs.", Picture = null, UnitPrice = 97.00M, UnitsInStock = 29, Category = CategoryManager.GetCategoryById(6) },
//            new Product(){ Id = 10, Name = "Ikura", Description = "12 - 200 ml jars", Picture = null, UnitPrice = 31.00M, UnitsInStock = 31, Category = CategoryManager.GetCategoryById(8) },
//            new Product(){ Id = 11, Name = "Queso Cabrales", Description = "1 kg pkg.", Picture = null, UnitPrice = 21.00M, UnitsInStock = 22, Category = CategoryManager.GetCategoryById(4) },
//            new Product(){ Id = 12, Name = "Queso Manchego La Pastora", Description = "10 - 500 g pkgs.", Picture = null, UnitPrice = 38.00M, UnitsInStock = 86, Category = CategoryManager.GetCategoryById(4) },
//            new Product(){ Id = 13, Name = "Konbu", Description = "2 kg box", Picture = null, UnitPrice = 6.00M, UnitsInStock = 24, Category = CategoryManager.GetCategoryById(8) },
//            new Product(){ Id = 14, Name = "Tofu", Description = "40 - 100 g pkgs.", Picture = null, UnitPrice = 23.25M, UnitsInStock = 35, Category = CategoryManager.GetCategoryById(7) },
//            new Product(){ Id = 15, Name = "Genen Shouyu", Description = "24 - 250 ml bottles", Picture = null, UnitPrice = 15.50M, UnitsInStock = 39, Category = CategoryManager.GetCategoryById(2) },

//        };
//    public class OrderDetailManager
//    {
//        new OrderDetail() {
//            Order = OrderManager.GetOrderById(1), Products = new List<Product> { ProductManager.GetProductById(1), ProductManager.GetProductById(2) }, UnitPrice = 18.00M, Quantity = 2},
//}
