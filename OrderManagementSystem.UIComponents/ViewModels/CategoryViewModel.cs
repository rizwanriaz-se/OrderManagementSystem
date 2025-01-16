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
    public class CategoryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Category> Categories { get; set; }
     
        public Action CloseWindow { get; set; }

       
        public string CategoryNameText { get; set; }
       
       
        public string CategoryDescriptionText { get; set; }
       

        public ICommand SubmitCategoryCommand { get; set; }

        public ICommand EditCategoryCommand { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

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

        

        private User m_objCurrentUser { get; set; }

        public User CurrentUser { get; set; }
       

        public CategoryViewModel()
        {
            Categories = GUIHandler.Instance.CacheManager.Categories;
            SubmitCategoryCommand = new RelayCommand(SubmitCategory);
            EditCategoryCommand = new RelayCommand(EditCategory);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory);
            CurrentUser = GUIHandler.Instance.CurrentUser;
        }

        private void SubmitCategory(object obj)
        {
            ValidateInputs();

            Category category = new Category
            {
                Name = CategoryNameText,
                Description = CategoryDescriptionText,
            };

            GUIHandler.Instance.ClientManager.SendMessage(
                    MessageType.Category,
                    MessageAction.Add,
                    category
                );


            CloseWindow?.Invoke();

        }

        private void ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(CategoryNameText))
            {
                DXMessageBox.Show("The category name cannot be empty.");
                return;
            }

            if (string.IsNullOrWhiteSpace(CategoryDescriptionText))
            {
                DXMessageBox.Show("The category description cannot be empty.");
                return;
            }

            if (Categories.Any(c => c.Name.Equals(CategoryNameText, StringComparison.OrdinalIgnoreCase)))
            {
                DXMessageBox.Show($"A category with the name '{CategoryNameText}' already exists.");
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
            GUIHandler.Instance.ClientManager.SendMessage(
                MessageType.Category,
                MessageAction.Delete,
                SelectedCategory.Id
            );
        }


    }
}
