using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystem.UIComponents.Views;
using OrderManagementSystemServer.Repository;
using System.ComponentModel;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public ICommand AddOrderCommand { get; set; }
        public ICommand AddCategoryCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand AddUserCommand { get; set; }
        private User m_objCurrentUser { get; set; }

        
        public User CurrentUser
        {
            get { return m_objCurrentUser; }
            set
            {
                m_objCurrentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainViewModel()
        {
           AddOrderCommand = new RelayCommand(ExecuteAddOrder);
            AddCategoryCommand = new RelayCommand(ExecuteAddCategory);
            AddProductCommand = new RelayCommand(ExecuteAddProduct); 
            AddUserCommand = new RelayCommand(ExecuteAddUser);
            CurrentUser = GUIHandler.Instance.CurrentUser;
        }

        public void ExecuteAddUser(object obj)
        {
            AddUserView addUserView = new AddUserView();
            addUserView.ShowDialog();
        }
     
        public void ExecuteAddProduct(object obj)
        {
            AddProductView addProductView = new AddProductView();
            addProductView.ShowDialog();
        }
        
        public void ExecuteAddOrder(object obj)
        {
            
            AddOrderView addOrderView = new AddOrderView();
            addOrderView.ShowDialog();
        }
     
        public void ExecuteAddCategory(object obj)
        {
            AddCategoryView addCategoryView = new AddCategoryView();
            addCategoryView.ShowDialog();
        }
    
        public event PropertyChangedEventHandler PropertyChanged;

    };
}
