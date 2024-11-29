using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditCategoryViewModel
    {
        private Category _Category;

        public Action CloseWindow { get; set; }

        public int? Id { get; set; }
        public string CategoryNameText { get; set; }
        public string CategoryDescriptionText { get; set; }
        public byte[] Picture { get; set; }

        public ICommand SaveCategoryCommand { get; set; }

        public EditCategoryViewModel(Category category)
        {
            SaveCategoryCommand = new RelayCommand(SaveCategory, CanSaveCategory);

            _Category = category;

            Id = category.Id;
            CategoryNameText = category.Name;
            CategoryDescriptionText = category.Description;
            Picture = category.Picture;
        }

        private bool CanSaveCategory(object obj)
        {
            return true;
        }

        private void SaveCategory(object obj)
        {
            _Category.Name = CategoryNameText;
            _Category.Description = CategoryDescriptionText;
            _Category.Picture = Picture;
            
            CategoryManager.UpdateCategory(_Category);

            CloseWindow.Invoke();

        }
    }
}
