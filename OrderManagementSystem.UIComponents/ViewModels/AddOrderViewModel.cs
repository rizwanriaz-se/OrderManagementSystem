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
    public class AddOrderViewModel : INotifyPropertyChanged
    {
        #region Declarations
        public Action CloseWindow { get; set; }

        private OrderStatus m_objSelectedStatus;
        private string m_stSelectedShippingAddress;
        private DateTime m_objSelectedShippingDate = DateTime.Now;
        private User m_objCurrentUser;

        public ICommand AddProductCommand { get; set; }
        public ICommand RemoveProductCommand { get; set; }
        public ICommand SubmitOrderCommand { get; set; }

        public ObservableCollection<OrderDetail> OrderDetails { get; set; } = new ObservableCollection<OrderDetail>();

        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<User> Users { get; set; }

        public User CurrentUser
        {
            get => m_objCurrentUser;
            set
            {
                if (m_objCurrentUser != value)
                {
                    m_objCurrentUser = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentUser)));
                }
            }
        }
        public ObservableCollection<OrderStatus> SelectableStatuses { get; }
      
        public event PropertyChangedEventHandler PropertyChanged;
        //public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        #endregion

        #region Data Bindings

        public OrderStatus SelectedStatus
        {
            get { return m_objSelectedStatus; }
            set
            {
                m_objSelectedStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStatus)));
            }
        }

        public string SelectedShippingAddress
        {
            get { return m_stSelectedShippingAddress; }
            set
            {
                m_stSelectedShippingAddress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedShippingAddress)));
            }
        }

        public DateTime SelectedShippingDate
        {
            get
            {
                return m_objSelectedShippingDate;
            }
            set
            {
                m_objSelectedShippingDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedShippingDate)));
            }
        }

        #endregion

        #region Constructor
        public AddOrderViewModel()
        {
            Products = GUIHandler.Instance.CacheManager.Products;
            Users = GUIHandler.Instance.CacheManager.Users;
            CurrentUser = GUIHandler.Instance.CurrentUser;

            AddProductCommand = new RelayCommand(AddOrderDetails);
            RemoveProductCommand = new RelayCommand(RemoveOrderDetails);
            SubmitOrderCommand = new RelayCommand(SubmitOrder);

            AddProductCommand = new RelayCommand(AddOrderDetails);
            RemoveProductCommand = new RelayCommand(RemoveOrderDetails);
            SubmitOrderCommand = new RelayCommand(SubmitOrder);
            
        }
        #endregion

        #region Command Actions
        private void SubmitOrder(object obj)
        {
            ValidateInputs();

            Order order = new Order
            {
                User = CurrentUser,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                OrderDetails = OrderDetails,
                ShippedDate = m_objSelectedShippingDate,
                ShippingAddress = m_stSelectedShippingAddress
            };

            ClientManager.Instance.SendMessage(
                MessageType.Order,
                MessageAction.Add,
                order
            );

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

        private void RemoveOrderDetails(object orderDetail)
        {
            OrderDetails.Remove(orderDetail as OrderDetail);
        }

        private void AddOrderDetails(object obj)
        {
            var newOrderDetail = new OrderDetail { Quantity = 1 };
            OrderDetails.Add(newOrderDetail);
        }
        #endregion
    }
}
