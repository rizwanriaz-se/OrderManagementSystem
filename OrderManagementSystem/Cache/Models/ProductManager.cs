using DevExpress.XtraExport.Implementation;
using OrderManagementSystem.UIComponents.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace OrderManagementSystem.Cache.Models
{
    public class ProductManager
    {

        private static ObservableCollection<Product> _AllProducts = new ObservableCollection<Product>
            {
            new Product(){ Id = 1, Name = "Chai", Description = "10 boxes x 20 bags", Picture = null, UnitPrice = 18.00M, UnitsInStock = 39, Category = CategoryManager.GetCategoryById(1) },
            new Product(){ Id = 2, Name = "Chang", Description = "24 - 12 oz bottles", Picture = null, UnitPrice = 19.00M, UnitsInStock = 17, Category = CategoryManager.GetCategoryById(1) },
            new Product(){ Id = 3, Name = "Aniseed Syrup", Description = "12 - 550 ml bottles", Picture = null, UnitPrice = 10.00M, UnitsInStock = 13, Category = CategoryManager.GetCategoryById(2) },
            new Product(){ Id = 4, Name = "Chef Anton's Cajun Seasoning", Description = "48 - 6 oz jars", Picture = null, UnitPrice = 22.00M, UnitsInStock = 53, Category = CategoryManager.GetCategoryById(2) },
            new Product(){ Id = 5, Name = "Chef Anton's Gumbo Mix", Description = "36 boxes", Picture = null, UnitPrice = 21.35M, UnitsInStock = 0, Category = CategoryManager.GetCategoryById(2) },
            new Product(){ Id = 6, Name = "Grandma's Boysenberry Spread", Description = "12 - 8 oz jars", Picture = null, UnitPrice = 25.00M, UnitsInStock = 120, Category = CategoryManager.GetCategoryById(2) },
            new Product(){ Id = 7, Name = "Uncle Bob's Organic Dried Pears", Description = "12 - 1 lb pkgs.", Picture = null, UnitPrice = 30.00M, UnitsInStock = 15, Category = CategoryManager.GetCategoryById(7) },
            new Product(){ Id = 8, Name = "Northwoods Cranberry Sauce", Description = "12 - 12 oz jars", Picture = null, UnitPrice = 40.00M, UnitsInStock = 6, Category = CategoryManager.GetCategoryById(2) },
            new Product(){ Id = 9, Name = "Mishi Kobe Niku", Description = "18 - 500 g pkgs.", Picture = null, UnitPrice = 97.00M, UnitsInStock = 29, Category = CategoryManager.GetCategoryById(6) },
            new Product(){ Id = 10, Name = "Ikura", Description = "12 - 200 ml jars", Picture = null, UnitPrice = 31.00M, UnitsInStock = 31, Category = CategoryManager.GetCategoryById(8) },
            new Product(){ Id = 11, Name = "Queso Cabrales", Description = "1 kg pkg.", Picture = null, UnitPrice = 21.00M, UnitsInStock = 22, Category = CategoryManager.GetCategoryById(4) },
            new Product(){ Id = 12, Name = "Queso Manchego La Pastora", Description = "10 - 500 g pkgs.", Picture = null, UnitPrice = 38.00M, UnitsInStock = 86, Category = CategoryManager.GetCategoryById(4) },
            new Product(){ Id = 13, Name = "Konbu", Description = "2 kg box", Picture = null, UnitPrice = 6.00M, UnitsInStock = 24, Category = CategoryManager.GetCategoryById(8) },
            new Product(){ Id = 14, Name = "Tofu", Description = "40 - 100 g pkgs.", Picture = null, UnitPrice = 23.25M, UnitsInStock = 35, Category = CategoryManager.GetCategoryById(7) },
            new Product(){ Id = 15, Name = "Genen Shouyu", Description = "24 - 250 ml bottles", Picture = null, UnitPrice = 15.50M, UnitsInStock = 39, Category = CategoryManager.GetCategoryById(2) },

            };


        public static ObservableCollection<Product> GetAllProducts()
        {
            return _AllProducts;
        }

        public static Product GetProductByID(int id)
        {
            //return  new Product{ Id = 15, Name = "Genen Shouyu", Description = "24 - 250 ml bottles", Picture = null, UnitPrice = 15.50M, UnitsInStock = 39, Category = CategoryManager.GetCategoryById(2) };
            return _AllProducts.FirstOrDefault(p => p.Id == id);
        }
        public static Product GetProductByName(ProductRow product) {
            //MessageBox.Show($"Hey: {product.SelectedProduct.Name} , fdsfs: {_AllProducts.FirstOrDefault(p => p.Name == product.SelectedProduct.Name)}");
            
            return _AllProducts.FirstOrDefault(p => p.Name == product.SelectedProduct.Name);
        }

        public static void AddProduct(Product product)
        {
            _AllProducts.Add(product);
        }

        public static void UpdateProduct(Product product)
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

                MessageBox.Show($"Product Updated: {existingProduct.Id}, {existingProduct.Name}, {existingProduct.Description}, {existingProduct.Category.Name}, {existingProduct.UnitPrice}, {existingProduct.UnitsInStock}");
            }
        }

        public static void DeleteProduct(Product product)
        {
            _AllProducts.Remove(product);
            //int productId = product.Id;

            //var productToDelete = _AllProducts.FirstOrDefault(p => p.Id == productId);
            //if (productToDelete != null)
            //{
            //    _AllProducts.Remove(productToDelete);
            //}
        }
    }
}
