using DevExpress.Mvvm.CodeGenerators;
using OrderManagementSystem.Commands;
using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystem.UIComponents.Views;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace OrderManagementSystem.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentView;

        public ICommand ViewOrderCommand { get; set; }
        public ICommand AddOrderCommand { get; set; }
        public ICommand EditOrderCommand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainViewModel() {
            // Set the default view
            CurrentView = new DisplayOrdersView();
            ViewOrderCommand = new RelayCommand(ExecuteViewOrder, CanExecuteViewOrder);
            AddOrderCommand = new RelayCommand(ExecuteAddOrder, CanExecuteAddOrder);
            EditOrderCommand = new RelayCommand(ExecuteEditOrder, CanExecuteEditOrder);
            //DeleteOrderCommand = new RelayCommand(ExecuteDeleteOrder, CanExecuteDeleteOrder);
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
            addOrderView.Owner = Application.Current.MainWindow;

            addOrderView.ShowDialog();
        }
        public bool CanExecuteAddOrder(object arg)
        {
            return true;
        }
        public void ExecuteEditOrder(object obj)
        {
            EditOrderView editOrderView = new EditOrderView();
            editOrderView.Owner = Application.Current.MainWindow;

            editOrderView.ShowDialog();
        }
        public bool CanExecuteEditOrder(object arg)
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












































































        //private object _currentView;

        //public object CurrentView
        //{
        //    get { return _currentView; }
        //    set
        //    {
        //        _currentView = value;
        //        OnPropertyChanged(nameof(CurrentView));
        //    }
        //}

        //public ICommand ViewOrdersCommand { get; set; }
        //public ICommand ViewCategoriesCommand { get; set; }
        //public ICommand ViewUsersCommand { get; set; }
        //public ICommand ViewProductsCommand { get; set; }
        //public ICommand SwitchToOrdersViewCommand { get; }
        //public ICommand SwitchToCategoriesViewCommand { get; }



        //public MainViewModel()
        //{
        //    CurrentView = new OrderViewModel();
        //    ViewCategoriesCommand = new RelayCommand(ExecuteViewCategories, CanExecuteViewCategories);
        //    ViewOrdersCommand = new RelayCommand(ExecuteViewOrders, CanExecuteViewOrders);
        //    //ViewUsersCommand = new RelayCommand(ExecuteViewUsers, CanExecuteViewUsers);
        //    //ViewProductsCommand = new RelayCommand(ExecuteViewProducts, CanExecuteViewProducts);
        //    //SwitchToOrdersViewCommand = new RelayCommand(() => CurrentView = new OrdersView());
        //    //SwitchToCategoriesViewCommand = new RelayCommand(() => CurrentView = new CategoriesView());



        //}

        //private bool CanExecuteViewProducts(object arg)
        //{
        //    return true;
        //}
        //private bool CanExecuteViewUsers(object arg)
        //{
        //    return true;
        //}
        //private bool CanExecuteViewOrders(object arg)
        //{
        //    return true;
        //}
        //private bool CanExecuteViewCategories(object arg)
        //{
        //    return true;
        //}

        ////private void ExecuteViewProducts(object obj)
        ////{
        ////    //CurrentView = new ProductViewModel();
        ////    CurrentView = new ProductViewModel();
        ////}
        //private void ExecuteViewCategories(object obj)
        //{
        //    CurrentView = new DisplayCategoriesView();
        //}
        //private void ExecuteViewOrders(object obj)
        //{
        //    CurrentView = new DisplayOrdersView();
        //}



        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

    };
}
