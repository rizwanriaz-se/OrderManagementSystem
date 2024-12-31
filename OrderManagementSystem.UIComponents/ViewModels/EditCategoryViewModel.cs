//using OrderManagementSystem.Commands;
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
//using OrderManagementSystem.Classes;
using OrderManagementSystem.UIComponents.Commands;
//using OrderManagementSystem.Repositories;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystemServer.Repository;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditCategoryViewModel : INotifyDataErrorInfo
    {
        private Category _Category;
        private string m_stCategoryNameText;
        private string m_stCategoryDescriptionText;
        public Action CloseWindow { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string CategoryNameText
        {
            get { return m_stCategoryNameText; }
            set
            {
                m_stCategoryNameText = value;
                Validate(nameof(CategoryNameText), m_stCategoryNameText);
            }
        }

        [Required(ErrorMessage = "Category description is required")]
        public string CategoryDescriptionText
        {
            get { return m_stCategoryDescriptionText; }
            set
            {
                m_stCategoryDescriptionText = value;
                Validate(nameof(CategoryDescriptionText), m_stCategoryDescriptionText);
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        public int? Id { get; set; }
        //public string CategoryNameText { get; set; }
        //public string CategoryDescriptionText { get; set; }
        public byte[] Picture { get; set; }

        public RelayCommand SaveCategoryCommand { get; set; }

        public bool HasErrors => Errors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (Errors.ContainsKey(propertyName))
            {
                return Errors[propertyName];
            }
            return null;
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
            SaveCategoryCommand.RaiseCanExecuteEventChanged();
        }

        public EditCategoryViewModel()
        {
            SaveCategoryCommand = new RelayCommand(SaveCategory, CanSaveCategory);

            //_Category = category;

            //Id = category.Id;
            //CategoryNameText = category.Name;
            //CategoryDescriptionText = category.Description;
            //Picture = category.Picture;
        }

        //public EditCategoryViewModel(Category category)
        //{
        //    SaveCategoryCommand = new RelayCommand(SaveCategory, CanSaveCategory);

        //    _Category = category;

        //    Id = category.Id;
        //    CategoryNameText = category.Name;
        //    CategoryDescriptionText = category.Description;
        //    Picture = category.Picture;
        //}

        private bool CanSaveCategory(object obj)
        {
            return Validator.TryValidateObject(this, new ValidationContext(this), null, true);
        }

        private void SaveCategory(object obj)
        {
            _Category = new Category();
            _Category.Id = Id;
            _Category.Name = CategoryNameText;
            _Category.Description = CategoryDescriptionText;
            _Category.Picture = Picture;


            //GUIHandler.Instance.CacheManager.UpdateCategory(_Category);
            MessageProcessor.SendMessage(Enums.MessageType.Category, Enums.MessageAction.Update, _Category);

            CloseWindow.Invoke();

        }
    }
}
