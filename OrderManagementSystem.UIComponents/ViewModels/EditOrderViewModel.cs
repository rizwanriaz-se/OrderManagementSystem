using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using static OrderManagementSystemServer.Repository.Order;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditOrderViewModel : INotifyPropertyChanged
    {
        public Action CloseWindow { get; set; }
        public ObservableCollection<Product> AllProducts { get; private set; }

        public ICommand AddProductCommand { get; set; }
        public ICommand RemoveProductCommand { get; set; }
        public ICommand SaveOrderCommand { get; set; }

        public EditOrderViewModel()
        {
            AllProducts = GUIHandler.Instance.CacheManager.Products;
            AddProductCommand = new RelayCommand(AddOrderDetails);
            RemoveProductCommand = new RelayCommand(RemoveOrderDetails);
            SaveOrderCommand = new RelayCommand(SaveOrder);
            SelectableStatuses = new ObservableCollection<OrderStatus>(
                 Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                 .Where(status => status != OrderStatus.Pending)
             );
        }


        private DateTime? m_objOrderDate;
        private DateTime? m_objSelectedShippingDate;
        private OrderStatus? m_objSelectedStatus;
        private string m_stSelectedShippingAddress;


        public int? Id { get; set; }
        public User User { get; set; }


        public DateTime? OrderDate { get; set; }

        public DateTime? SelectedShippingDate { get; set; }


        public OrderStatus? SelectedStatus { get; set; }

        public ObservableCollection<OrderDetail> OrderDetails { get; set; }

        public string SelectedShippingAddress { get; set; }


        public ObservableCollection<OrderStatus> SelectableStatuses { get; }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        private void RemoveOrderDetails(object orderDetails)
        {
            OrderDetails.Remove(orderDetails as OrderDetail);
        }

        private void AddOrderDetails(object obj)
        {
            var newOrderDetail = new OrderDetail { Quantity = 1 };
            OrderDetails.Add(newOrderDetail);
        }

        private void SaveOrder(object obj)
        {
            ValidateInputs();

            Order order = new Order
            {
                Id = Id,
                User = User,
                OrderDate = OrderDate,
                Status = SelectedStatus,
                ShippedDate = SelectedShippingDate,
                ShippingAddress = SelectedShippingAddress,
                OrderDetails = new ObservableCollection<OrderDetail>(OrderDetails),
            };


            GUIHandler.Instance.ClientManager.SendMessage(MessageType.Order, MessageAction.Update, order);

            CloseWindow?.Invoke();

        }

        private void ValidateInputs()
        {
            if (OrderDetails.Count <= 0)
            {
                DXMessageBox.Show("Please add some products before proceeding.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!OrderDetails.All(order => order.Product != null && order.Quantity > 0))
            {
                DXMessageBox.Show("Products and their respective quantity should not be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SelectedShippingAddress == null)
            {
                DXMessageBox.Show("Shipping address field can not be empty.");
                return;
            }
        }
    }
}
