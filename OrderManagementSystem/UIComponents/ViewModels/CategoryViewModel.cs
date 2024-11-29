using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;
using OrderManagementSystem.UIComponents.Views;
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

        public ICommand EditCategoryCommand { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private Category m_SelectedCategory { get; set; }

        public Category SelectedCategory
        {
            get { return m_SelectedCategory; }
            set
            {
                m_SelectedCategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategory)));
            }
        }



        public CategoryViewModel()
        {
            Categories = CategoryManager.GetAllCategories();
            SubmitCategoryCommand = new RelayCommand(SubmitCategory, CanSubmitCategory);
            EditCategoryCommand = new RelayCommand(EditCategory, CanEditCategory);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory, CanDeleteCategory);

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

        private void EditCategory(object obj) {
            EditCategoryView editCategoryView = new EditCategoryView();

            EditCategoryViewModel editCategoryViewModel = new EditCategoryViewModel(SelectedCategory);
            editCategoryView.DataContext = editCategoryViewModel;
            editCategoryViewModel.CloseWindow = editCategoryView.Close;
            editCategoryView.ShowDialog();
        }

        private bool CanEditCategory(object obj)
        {
            return true;
        }

        private void DeleteCategory(object obj) {
            CategoryManager.DeleteCategory(SelectedCategory);
            //Categories.Remove(SelectedCategory);
        }

        private bool CanDeleteCategory(object obj)
        {
            return true;
        }
    }
}
