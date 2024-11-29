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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public ICommand EditOrderCommand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }

          
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

        public OrderViewModel()
        {
          
            Orders = OrderManager.GetAllOrders();
           
            EditOrderCommand = new RelayCommand(ExecuteEditOrder, CanExecuteEditOrder);
            DeleteOrderCommand = new RelayCommand(ExecuteDeleteOrder, CanExecuteDeleteOrder);
        }

        private void ExecuteEditOrder(object obj)
        {
            EditOrderView editOrderView = new EditOrderView();

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

    }
}
