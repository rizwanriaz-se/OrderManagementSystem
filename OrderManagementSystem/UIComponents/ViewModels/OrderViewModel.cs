using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;
using OrderManagementSystem.UIComponents.Views;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    
    public class OrderViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //public Dictionary<Product, int> Products { get; set; }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //public class OrderViewModel : INotifyPropertyChanged
        //
    
        //public ICommand AddProductCommand { get; }
        //public ICommand SubmitOrderCommand { get; }
        //public AddOrderView
        public ICommand EditOrderCommand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }

        //public ObservableCollection<ProductRow> ProductRows { get; }
          


        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        public ObservableCollection<Order> Orders { get; private set; }

        //public ObservableCollection<Product> Products { get; set; }

        public OrderViewModel()
        {
            //Products = new ObservableCollection<ProductViewModel>();
            //AddProductCommand = new RelayCommand(AddProduct, CanAddProduct);
            //SubmitOrderCommand = new RelayCommand(SubmitOrder, CanSubmitOrder);
            Orders = OrderManager.GetAllOrders();
            //ProductRows = new ObservableCollection<ProductRow>(Orders.Products.Select(p => new ProductRow { SelectedProduct = p.Key, Quantity = p.Value }));
            EditOrderCommand = new RelayCommand(ExecuteEditOrder, CanExecuteEditOrder);
            DeleteOrderCommand = new RelayCommand(ExecuteDeleteOrder, CanExecuteDeleteOrder);
        }

        private void ExecuteEditOrder(object obj)
        {
            EditOrderView editOrderView = new EditOrderView();
            //MessageBox.Show($"Hi: {SelectedOrder.Id}");


            EditOrderViewModel editOrderViewModel = new EditOrderViewModel(SelectedOrder);
            editOrderView.DataContext = editOrderViewModel;
            editOrderViewModel.CloseWindow = editOrderView.Close;
            editOrderView.ShowDialog();
        }
        private bool CanExecuteEditOrder(object obj)
        {
            return true;
        }

        private void ExecuteDeleteOrder(object obj) {
            OrderManager.DeleteOrder(SelectedOrder);
            //Orders.Remove(SelectedOrder);
        }

        private bool CanExecuteDeleteOrder(object obj)
        {
            return true;
        }


        //private void AddProduct(object parameter)
        //{
        //    Products.Add(new ProductViewModel());
        //}

        //private bool CanAddProduct(object parameter)
        //{
        //    return true;
        //}

        //private void SubmitOrder(object parameter)
        //{
        //    Products.Add(new ProductViewModel());
        //}

        //private bool CanRemoveProduct(object parameter)
        //{
        //    return true;
        //}

        //private void SubmitOrder()
        //{
        //    // Logic for submitting the order
        //}

    }
}
