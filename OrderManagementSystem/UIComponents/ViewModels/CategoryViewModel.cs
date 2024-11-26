using OrderManagementSystem.Cache.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class CategoryViewModel
    {
        public ObservableCollection<Category> Categories { get; private set; }

        public CategoryViewModel()
        {
            Categories = CategoryManager.GetAllCategories();
        }
    }
}
