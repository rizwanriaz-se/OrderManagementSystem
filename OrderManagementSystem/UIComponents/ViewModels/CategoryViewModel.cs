using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Category> Categories { get; private set; }
        public Action CloseWindow { get; set; }

        public string CategoryNameText { get; set; }

        public string CategoryDescriptionText { get; set; }

        public ICommand SubmitCategoryCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        //public string CategoryNameText
        //{
        //    get { return _categoryNameText; }
        //    set { _categoryNameText = value; }
        //}





        public CategoryViewModel()
        {
            Categories = CategoryManager.GetAllCategories();
            SubmitCategoryCommand = new RelayCommand(SubmitCategory, CanSubmitCategory);
        }

        private void SubmitCategory(object obj)
        {
            int? lastCategoryId = Categories.Last().Id;

            // Create new Order object
            Category category = new Category
            {
                Id = lastCategoryId + 1,
                Name = CategoryNameText,
                Description = CategoryDescriptionText,
                Picture = null
            };

            CategoryManager.AddCategory(category);
            CloseWindow?.Invoke();

        }

        private bool CanSubmitCategory(object obj)
        {
            return true;
        }
    }
}
