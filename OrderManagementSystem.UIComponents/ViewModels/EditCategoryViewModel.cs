using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditCategoryViewModel
    {
        public Action CloseWindow { get; set; }


        public string CategoryNameText { get; set; }

        public string CategoryDescriptionText { get; set; }

        public int? Id { get; set; }

        public ICommand SaveCategoryCommand { get; set; }


        public EditCategoryViewModel()
        {
            SaveCategoryCommand = new RelayCommand(SaveCategory);
        }



        private void SaveCategory(object obj)
        {
            if (string.IsNullOrWhiteSpace(CategoryNameText))
            {
                DXMessageBox.Show("The category name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(CategoryDescriptionText))
            {
                DXMessageBox.Show("The category description cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (GUIHandler.Instance.CacheManager.Categories.Any(c => c.Name.Equals(CategoryNameText, StringComparison.OrdinalIgnoreCase)))
            {
                DXMessageBox.Show($"A category with the name '{CategoryNameText}' already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Category category = new Category
            {
                Id = Id,
                Name = CategoryNameText,
                Description = CategoryDescriptionText
            };



            ClientManager.Instance.SendMessage(MessageType.Category, MessageAction.Update, category);

            CloseWindow.Invoke();

        }

    }
}
