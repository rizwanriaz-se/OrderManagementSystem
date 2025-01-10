using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystem.UIComponents.Views;
using OrderManagementSystemServer.Repository;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

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


        private Order m_objSelectedOrder;
        public Order SelectedOrder
        {
            get { return m_objSelectedOrder; }
            set
            {
                m_objSelectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        public ObservableCollection<Order> Orders { get; private set; }

        public OrderViewModel()
        {
            Orders = GUIHandler.Instance.CacheManager.GetAllOrders();

            EditOrderCommand = new RelayCommand(ExecuteEditOrder);
            DeleteOrderCommand = new RelayCommand(ExecuteDeleteOrder);
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
    
        private void ExecuteDeleteOrder(object obj)
        {
            MessageProcessor.SendMessage(Enums.MessageType.Order, Enums.MessageAction.Delete, SelectedOrder);
        }

   

    }
}
