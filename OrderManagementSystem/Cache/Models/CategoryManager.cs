using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrderManagementSystem.Cache.Models
{
    public class CategoryManager
    {

        public static ObservableCollection<Category> _AllCategories = new ObservableCollection<Category>()
        {
            new Category(){Id = 1, Name = "Beverages", Description = "Soft drinks, coffees, teas, beers, and ales", Picture = null},
            new Category(){Id = 2, Name = "Condiments", Description = "Sweet and savory sauces, relishes, spreads, and seasonings", Picture = null},
            new Category(){Id = 3, Name = "Confections", Description = "Desserts, candies, and sweet breads", Picture = null},
            new Category(){Id = 4, Name = "Dairy Products", Description = "Cheeses", Picture = null},
            new Category(){Id = 5, Name = "Grains/Cereals", Description = "Breads, crackers, pasta, and cereal", Picture = null},
            new Category(){Id = 6, Name = "Meat/Poultry", Description = "Prepared meats", Picture = null},
            new Category(){Id = 7, Name = "Produce", Description = "Dried fruit and bean curd", Picture = null},
            new Category(){Id = 8, Name = "Seafood", Description = "Seaweed and fish", Picture = null}
        };

     
    public static Category GetCategoryById(int id)
        {
            return _AllCategories.FirstOrDefault(c => c.Id == id);
        }
        public static ObservableCollection<Category> GetAllCategories()
        {
            return _AllCategories;
        }
        public static void AddCategory(Category category)
        {
            _AllCategories.Add(category);
        }
        public static void DeleteCategory(Category category)
        {
            _AllCategories.Remove(category);
        }

        public static void UpdateCategory(Category category)
        {
            var categoryToUpdate = _AllCategories.FirstOrDefault(c => c.Id == category.Id);
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.Picture = category.Picture;

            MessageBox.Show("Category updated successfully");
        }
    }
}
