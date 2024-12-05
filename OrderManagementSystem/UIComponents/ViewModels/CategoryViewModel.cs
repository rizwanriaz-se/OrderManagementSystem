﻿using DevExpress.Mvvm.POCO;
using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;
using OrderManagementSystem.UIComponents.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public ObservableCollection<Category> Categories { get; private set; }
        public Action CloseWindow { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string CategoryNameText { get; set; }

        [Required(ErrorMessage = "Category description is required")]
        public string CategoryDescriptionText { get; set; }

        public RelayCommand SubmitCategoryCommand { get; set; }

        public ICommand EditCategoryCommand { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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

        Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();
        //public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => Errors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (Errors.ContainsKey(propertyName))
            {
                return Errors[propertyName];
            }
            return null;
        }

        



        public CategoryViewModel()
        {
            Categories = GUIHandler.GetInstance().CacheManager.GetAllCategories();
            SubmitCategoryCommand = new RelayCommand(SubmitCategory, CanSubmitCategory);
            EditCategoryCommand = new RelayCommand(EditCategory, CanEditCategory);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory, CanDeleteCategory);

        }
        public void Validate(string propertyName, object propertyValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyName };
            Validator.TryValidateProperty(propertyValue, context, results);

            if (results.Any())
            {
                Errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                Errors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            SubmitCategoryCommand.RaiseCanExecuteEventChanged();
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

            GUIHandler.GetInstance().CacheManager.AddCategory(category);
            CloseWindow?.Invoke();
        }

        private bool CanSubmitCategory(object obj)
        {
            return Validator.TryValidateObject(this, new ValidationContext(this), null, true);
            //return true;
        }

        private void EditCategory(object obj)
        {
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

        private void DeleteCategory(object obj)
        {
            GUIHandler.GetInstance().CacheManager.DeleteCategory(SelectedCategory);
            //Categories.Remove(SelectedCategory);
        }

        private bool CanDeleteCategory(object obj)
        {
            return true;
        }

        
    }
}
