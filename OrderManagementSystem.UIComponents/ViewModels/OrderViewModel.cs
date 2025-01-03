﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.Xpf.Core;
//using OrderManagementSystem.Commands;
using OrderManagementSystem.UIComponents.Views;
//using OrderManagementSystem.Classes;
using OrderManagementSystem.UIComponents.Commands;
//using OrderManagementSystem.Repositories;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystemServer.Repository;

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
            Orders = GUIHandler.Instance.CacheManager.GetAllOrders();

            EditOrderCommand = new RelayCommand(ExecuteEditOrder, CanExecuteEditOrder);
            DeleteOrderCommand = new RelayCommand(ExecuteDeleteOrder, CanExecuteDeleteOrder);
        }

        private void ExecuteEditOrder(object obj)
        {

            if (SelectedOrder != null)
            {
                EditOrderView editOrderView = new EditOrderView();
                editOrderView.LoadOrder(SelectedOrder);
               
                editOrderView.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("Select a valid order to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanExecuteEditOrder(object obj)
        {
            return SelectedOrder != null;
        }

        private void ExecuteDeleteOrder(object obj)
        {
            MessageProcessor.SendMessage(Enums.MessageType.Order, Enums.MessageAction.Delete, SelectedOrder);
        }

        private bool CanExecuteDeleteOrder(object obj)
        {
            return SelectedOrder != null;
        }

    }
}
