using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditCategoryViewModel : INotifyDataErrorInfo
    {
        private Category m_objCategory;
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

        public ICommand SaveCategoryCommand { get; set; }

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
            //SaveCategoryCommand.RaiseCanExecuteEventChanged();
        }

        public EditCategoryViewModel()
        {
            SaveCategoryCommand = new RelayCommand(SaveCategory);
        }

     

        private void SaveCategory(object obj)
        {
            try
            {
                m_objCategory = new Category();
                m_objCategory.Id = Id;
                m_objCategory.Name = CategoryNameText;
                m_objCategory.Description = CategoryDescriptionText;

                if (string.IsNullOrWhiteSpace(m_objCategory.Name))
                {
                    throw new ArgumentException("The category name cannot be empty.");
                }

                if (string.IsNullOrWhiteSpace(m_objCategory.Description))
                {
                    throw new ArgumentException("The category description cannot be empty.");
                }

                if (!Validator.TryValidateObject(this, new ValidationContext(this), null, true))
                {
                    var errorMessages = Errors.SelectMany(e => e.Value).ToList();
                    throw new Exception(string.Join(Environment.NewLine, errorMessages));
                }


                if (GUIHandler.Instance.CacheManager.GetAllCategories().Any(c => c.Name.Equals(m_objCategory.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"A category with the name '{m_objCategory.Name}' already exists.");
                }


                MessageProcessor.SendMessage(Enums.MessageType.Category, Enums.MessageAction.Update, m_objCategory);

                CloseWindow.Invoke();
            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
        }
    }
}
