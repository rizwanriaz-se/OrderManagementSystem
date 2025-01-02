using DevExpress.Mvvm.CodeGenerators;
//using OrderManagementSystem.Commands;
//using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystem.UIComponents.Views;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
//using OrderManagementSystem.Classes;
using OrderManagementSystem.UIComponents.Commands;
//using OrderManagementSystem.Repositories;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystemServer.Repository;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentView;

        public RelayCommand ViewOrderCommand { get; set; }
        public RelayCommand AddOrderCommand { get; set; }
        public RelayCommand DeleteOrderCommand { get; set; }

        public RelayCommand ViewCategoryCommand { get; set; }
        public ICommand AddCategoryCommand { get; set; }

        //public RelayCommand AddProductBlotterCommand { get; set; }
        //public RelayCommand AddUserBlotterCommand { get; set; }

        public RelayCommand ViewProductCommand { get; set; }
        public RelayCommand AddProductCommand { get; set; }

        //public RelayCommand AddOrderBlotterCommand { get; set; }

        public RelayCommand ViewUserCommand { get; set; }
        public RelayCommand AddUserCommand { get; set; }




        private User m_CurrentUser { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public User CurrentUser
        {
            get { return m_CurrentUser; }
            set
            {
                m_CurrentUser = value;
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
            ViewOrderCommand = new RelayCommand(ExecuteViewOrder, CanExecuteViewOrder);
            AddOrderCommand = new RelayCommand(ExecuteAddOrder, CanExecuteAddOrder);
            ViewCategoryCommand = new RelayCommand(ExecuteViewCategory, CanExecuteViewCategory);
            AddCategoryCommand = new RelayCommand(ExecuteAddCategory, CanExecuteAddCategory);
            ViewProductCommand = new RelayCommand(ExecuteViewProduct, CanExecuteViewProduct);
            AddProductCommand = new RelayCommand(ExecuteAddProduct, CanExecuteAddProduct);
            ViewUserCommand = new RelayCommand(ExecuteViewUser, CanExecuteViewUser);
            AddUserCommand = new RelayCommand(ExecuteAddUser, CanExecuteAddUser);
            CurrentUser = GUIHandler.Instance.CurrentUser;
            //AddOrderBlotterCommand = new RelayCommand(ExecuteAddOrderBlotter, CanExecuteAddOrderBlotter);
            //AddProductBlotterCommand = new RelayCommand(ExecuteAddProductBlotter, CanExecuteAddProductBlotter);
            //AddUserBlotterCommand = new RelayCommand(ExecuteAddUserBlotter, CanExecuteAddUserBlotter);

        }

        //private bool CanExecuteAddOrderBlotter(object obj)
        //{
        //    return true;
        //}

        //private void ExecuteAddOrderBlotter(object obj)
        //{
        //    //DisplayOrdersView displayOrdersView = new DisplayOrdersView();
        //    //displayOrdersView.GeneratePanel();

        //    //OrderBlotter orderBlotter = new OrderBlotter();
        //    //orderBlotter.Show();
        //}

        public void ExecuteViewUser(object obj)
        {
            CurrentView = new DisplayUsersView();
        }
        public bool CanExecuteViewUser(object arg)
        {
            return true;
        }
        public void ExecuteAddUser(object obj)
        {
            AddUserView addUserView = new AddUserView();
            addUserView.Owner = System.Windows.Application.Current.MainWindow;

            addUserView.ShowDialog();
        }
        public bool CanExecuteAddUser(object arg)
        {
            return true;
        }
        public void ExecuteViewProduct(object obj)
        {
            CurrentView = new DisplayProductsView();
        }
        public bool CanExecuteViewProduct(object arg)
        {
            return true;
        }
        public void ExecuteAddProduct(object obj)
        {
            AddProductView addProductView = new AddProductView();
            addProductView.Owner = System.Windows.Application.Current.MainWindow;

            addProductView.ShowDialog();
        }
        public bool CanExecuteAddProduct(object arg)
        {
            return true;
        }

        public void ExecuteViewOrder(object obj)
        {
            CurrentView = new DisplayOrdersView();
        }
        public bool CanExecuteViewOrder(object arg)
        {
            return true;
        }
        public void ExecuteAddOrder(object obj)
        {
            AddOrderView addOrderView = new AddOrderView();
            addOrderView.Owner = System.Windows.Application.Current.MainWindow;

            addOrderView.ShowDialog();
        }
        public bool CanExecuteAddOrder(object arg)
        {
            return true;
        }

        public void ExecuteViewCategory(object obj)
        {
            CurrentView = new DisplayCategoriesView();
        }
        public bool CanExecuteViewCategory(object arg)
        {
            return true;
        }
        public void ExecuteAddCategory(object obj)
        {
            AddCategoryView addCategoryView = new AddCategoryView();
            addCategoryView.Owner = System.Windows.Application.Current.MainWindow;

            addCategoryView.ShowDialog();
        }
        public bool CanExecuteAddCategory(object arg)
        {
            return true;
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
