using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystem.UIComponents.Views;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public ObservableCollection<Category> Categories { get; private set; }
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

        public ICommand SubmitCategoryCommand { get; set; }

        public ICommand EditCategoryCommand { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private Category m_objSelectedCategory { get; set; }

        public Category SelectedCategory
        {
            get { return m_objSelectedCategory; }
            set
            {
                m_objSelectedCategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategory)));
            }
        }

        Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        public bool HasErrors => Errors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (Errors.ContainsKey(propertyName))
            {
                return Errors[propertyName];
            }
            return null;
        }

        private User m_objCurrentUser { get; set; }

        public User CurrentUser
        {
            get { return m_objCurrentUser; }
            set
            {
                m_objCurrentUser = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentUser)));
            }
        }

        public CategoryViewModel()
        {
            Categories = GUIHandler.Instance.CacheManager.GetAllCategories();
            SubmitCategoryCommand = new RelayCommand(SubmitCategory);
            EditCategoryCommand = new RelayCommand(EditCategory);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory);
            CurrentUser = GUIHandler.Instance.CurrentUser;
        }

        public bool Validate(string propertyName, object propertyValue)
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
            return Errors.ContainsKey(propertyName);
        }
        private void SubmitCategory(object obj)
        {
            try
            {
                int? lastCategoryId = Categories.Last().Id;

                Category category = new Category
                {
                    Name = CategoryNameText,
                    Description = CategoryDescriptionText,
                };

                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    throw new ArgumentException("The category name cannot be empty.");
                }

                if (string.IsNullOrWhiteSpace(category.Description))
                {
                    throw new ArgumentException("The category description cannot be empty.");
                }

                

                if (Categories.Any(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"A category with the name '{category.Name}' already exists.");
                }

                MessageProcessor.SendMessage(
                    Enums.MessageType.Category,
                    Enums.MessageAction.Add,
                    category
                );


                CloseWindow?.Invoke();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
        }

      

        private void EditCategory(object obj)
        {
            EditCategoryView editCategoryView = new EditCategoryView();
            editCategoryView.LoadCategory(SelectedCategory);

            editCategoryView.ShowDialog();
        }

      

        private void DeleteCategory(object obj)
        {
            MessageProcessor.SendMessage(
                Enums.MessageType.Category,
                Enums.MessageAction.Delete,
                SelectedCategory.Id
            );
        }


    }
}
