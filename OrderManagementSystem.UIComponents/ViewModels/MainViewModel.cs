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
        private object m_objCurrentView;

        public ICommand ViewOrderCommand { get; set; }
        public ICommand AddOrderCommand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }

        public ICommand ViewCategoryCommand { get; set; }
        public ICommand AddCategoryCommand { get; set; }

        public ICommand ViewProductCommand { get; set; }
        public ICommand AddProductCommand { get; set; }

        public ICommand ViewUserCommand { get; set; }
        public ICommand AddUserCommand { get; set; }




        private User m_objCurrentUser { get; set; }

        public object CurrentView
        {
            get { return m_objCurrentView; }
            set
            {
                m_objCurrentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

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
            // Set the default view
            CurrentView = new DisplayOrdersView();
            ViewOrderCommand = new RelayCommand(ExecuteViewOrder);
            AddOrderCommand = new RelayCommand(ExecuteAddOrder);
            ViewCategoryCommand = new RelayCommand(ExecuteViewCategory);
            AddCategoryCommand = new RelayCommand(ExecuteAddCategory);
            ViewProductCommand = new RelayCommand(ExecuteViewProduct);
            AddProductCommand = new RelayCommand(ExecuteAddProduct);
            ViewUserCommand = new RelayCommand(ExecuteViewUser);
            AddUserCommand = new RelayCommand(ExecuteAddUser);
            CurrentUser = GUIHandler.Instance.CurrentUser;
        }

        public void ExecuteViewUser(object obj)
        {
            CurrentView = new DisplayUsersView();
        }
       
        public void ExecuteAddUser(object obj)
        {
            AddUserView addUserView = new AddUserView();
            addUserView.Owner = System.Windows.Application.Current.MainWindow;

            addUserView.ShowDialog();
        }
    
        public void ExecuteViewProduct(object obj)
        {
            CurrentView = new DisplayProductsView();
        }
     
        public void ExecuteAddProduct(object obj)
        {
            AddProductView addProductView = new AddProductView();
            addProductView.Owner = System.Windows.Application.Current.MainWindow;

            addProductView.ShowDialog();
        }
    
        public void ExecuteViewOrder(object obj)
        {
            CurrentView = new DisplayOrdersView();
        }
        
        public void ExecuteAddOrder(object obj)
        {
            AddOrderView addOrderView = new AddOrderView();
            addOrderView.Owner = System.Windows.Application.Current.MainWindow;

            addOrderView.ShowDialog();
        }
       

        public void ExecuteViewCategory(object obj)
        {
            CurrentView = new DisplayCategoriesView();
        }
     
        public void ExecuteAddCategory(object obj)
        {
            AddCategoryView addCategoryView = new AddCategoryView();
            addCategoryView.Owner = System.Windows.Application.Current.MainWindow;

            addCategoryView.ShowDialog();
        }
    
        public event PropertyChangedEventHandler PropertyChanged;

        public void SwitchToOrdersView()
        {
            CurrentView = new DisplayOrdersView();

        }
        public void SwitchToCategoriesView()
        {
            CurrentView = new DisplayCategoriesView();
        }
        public void SwitchToProductsView()
        {
            CurrentView = new DisplayProductsView();
        }
        public void SwitchToUsersView()
        {
            CurrentView = new DisplayUsersView();
        }


    };
}
