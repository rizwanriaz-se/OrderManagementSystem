using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystem.UIComponents.Views;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Data;
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

        private object m_objCategoryLock = new object();

        private User m_objCurrentUser { get; set; }

        public User CurrentUser { get; set; }
        public ICollectionView CategoryCollectionView { get; set; }

        public CategoryViewModel()
        {
            Categories = GUIHandler.Instance.CacheManager.Categories;
            CategoryCollectionView = CollectionViewSource.GetDefaultView(Categories);

            SubmitCategoryCommand = new RelayCommand(SubmitCategory);
            EditCategoryCommand = new RelayCommand(EditCategory);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory);
            CurrentUser = GUIHandler.Instance.CurrentUser;

            Categories.CollectionChanged += OnCollectionChanged;
            BindingOperations.EnableCollectionSynchronization(Categories, m_objCategoryLock);
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Category c in e.NewItems)
                {
                    if (c != null) c.OnPropertyChanged(nameof(c));
                }
            }
            if (e.OldItems != null)
            {
                foreach (Category c in e.OldItems)
                {
                    if (c != null) c.OnPropertyChanged(nameof(c));
                }
            }
        }


        private void SubmitCategory(object obj)
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

            if (Categories.Any(c => c.Name.Equals(CategoryNameText, StringComparison.OrdinalIgnoreCase)))
            {
                DXMessageBox.Show($"A category with the name '{CategoryNameText}' already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Category category = new Category
            {
                Name = CategoryNameText,
                Description = CategoryDescriptionText,
            };

            ClientManager.Instance.SendMessage(
                    MessageType.Category,
                    MessageAction.Add,
                    category
                );

            CloseWindow?.Invoke();
        }


        private void EditCategory(object obj)
        {
            EditCategoryView editCategoryView = new EditCategoryView();
            editCategoryView.LoadCategory(SelectedCategory);

            editCategoryView.ShowDialog();
        }

        private void DeleteCategory(object obj)
        {

            MessageBoxResult confirmationResult = DXMessageBox.Show($"Are you sure you want to delete selected category: {SelectedCategory.Name}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirmationResult == MessageBoxResult.Yes)
                ClientManager.Instance.SendMessage(
                    MessageType.Category,
                    MessageAction.Delete,
                    SelectedCategory.Id
                );

        }


    }
}
