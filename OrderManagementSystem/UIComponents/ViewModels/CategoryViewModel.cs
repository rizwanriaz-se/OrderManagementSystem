using DevExpress.Mvvm.POCO;
//using OrderManagementSystem.Cache.Models;
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
using OrderManagementSystem.Repositories.Repositories;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public ObservableCollection<Category> Categories { get; private set; }
        private string m_stCategoryNameText;
        private string m_stCategoryDescriptionText;
        public Action CloseWindow { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string CategoryNameText {
            get { return m_stCategoryNameText; }
            set
            {
                m_stCategoryNameText = value;
                Validate(nameof(CategoryNameText), m_stCategoryNameText);
            }
        }

        [Required(ErrorMessage = "Category description is required")]
        public string CategoryDescriptionText {
            get { return m_stCategoryDescriptionText; }
            set
            {
                m_stCategoryDescriptionText = value;
                Validate(nameof(CategoryDescriptionText), m_stCategoryDescriptionText);
            }
        }

        public RelayCommand SubmitCategoryCommand { get; set; }

        public RelayCommand EditCategoryCommand { get; set; }
        public RelayCommand DeleteCategoryCommand { get; set; }

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

            //GUIHandler.GetInstance().CacheManager.AddCategory(category);
            GUIHandler.GetInstance().MessageProcessor.SendMessage(
                Enums.MessageType.Category,
                Enums.MessageAction.Add,
                category
            );
            //GUIHandler.GetInstance().ClientManager.SendMessage(new Classes.Request
            //{
            //    MessageType = Enums.MessageType.Category,
            //    MessageAction = Enums.MessageAction.Add,
            //    Data = category
            //});
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
            return (SelectedCategory != null);
        }

        private void DeleteCategory(object obj)
        {
            //GUIHandler.GetInstance().CacheManager.DeleteCategory(SelectedCategory);
            GUIHandler.GetInstance().MessageProcessor.SendMessage(
                Enums.MessageType.Category,
                Enums.MessageAction.Delete,
                SelectedCategory
            );
            //Categories.Remove(SelectedCategory);
        }

        private bool CanDeleteCategory(object obj)
        {
            return (SelectedCategory != null);
        }


    }
}
