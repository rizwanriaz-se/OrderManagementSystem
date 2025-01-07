using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using static OrderManagementSystemServer.Repository.Order;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class AddOrderViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Declarations
        // Declare Action to close Add Order View window when invoked
        public Action CloseWindow { get; set; }

        // Declare properties for data bindings
        private OrderStatus m_objSelectedStatus;
        private string m_objSelectedShippingAddress;
        private DateTime m_objSelectedShippingDate = DateTime.Now;
        private User m_objSelectedUser;


        // Declare Commands
        public RelayCommand AddProductCommand { get; set; }
        public RelayCommand RemoveProductCommand { get; set; }
        public RelayCommand SubmitOrderCommand { get; set; }

        // Declare Observable Collections for data bindings
        public ObservableCollection<OrderDetail> OrderDetails { get; set; } = new ObservableCollection<OrderDetail>();

        public ObservableCollection<Product> AllProducts { get; set; }
        public ObservableCollection<User> AllUsers { get; set; }
        private User m_objCurrentUser;

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
      



        // Declare PropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        #endregion

        #region Data Bindings

        // Data Bindings for Selected Properties
        [Required(ErrorMessage = "Status must be selected.")]
        public OrderStatus SelectedStatus
        {
            get { return m_objSelectedStatus; }
            set
            {
                m_objSelectedStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStatus)));
                Validate(nameof(SelectedStatus), m_objSelectedStatus);

            }
        }

        [Required(ErrorMessage = "Shipping Address is required")]
        public string SelectedShippingAddress
        {
            get { return m_objSelectedShippingAddress; }
            set
            {
                m_objSelectedShippingAddress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedShippingAddress)));
                Validate(nameof(SelectedShippingAddress), m_objSelectedShippingAddress);

            }
        }

        [Required(ErrorMessage = "Shipping Date is required")]
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
                Validate(nameof(SelectedShippingDate), m_objSelectedShippingDate);

            }
        }

        Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        public bool HasErrors => Errors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (Errors.ContainsKey(propertyName))
            {
                return Errors[propertyName];
            }
            return null;
        }

        public void Validate(string propertyName, object propertyValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyName };
            Validator.TryValidateProperty(propertyValue, context, results);

            if (results.Any())
            {
                Errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                Errors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            SubmitOrderCommand.RaiseCanExecuteEventChanged();
        }

       
        #endregion

        #region Constructor
        public AddOrderViewModel()
        {
            AllProducts = GUIHandler.Instance.CacheManager.GetAllProducts();
            AllUsers = GUIHandler.Instance.CacheManager.GetAllUsers();
            CurrentUser = GUIHandler.Instance.CurrentUser;

            if (CurrentUser == null)
            {
                // Handle the case where CurrentUser is null
                System.Windows.MessageBox.Show("Current user is not set.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AddProductCommand = new RelayCommand(AddOrderDetails, CanAddOrderDetails);
            RemoveProductCommand = new RelayCommand(RemoveOrderDetails, CanRemoveOrderDetails);
            SubmitOrderCommand = new RelayCommand(SubmitOrder, CanSubmitOrder);
            
            // Subscribe to OrderDetails changes
            OrderDetails.CollectionChanged += (s, e) => SubmitOrderCommand.RaiseCanExecuteEventChanged();
        }
        #endregion

        #region Command Actions
        // Command Action to Submit Order
        private void SubmitOrder(object obj)
        {
            // Get the last order Id to generate new order Id
            int? lastOrderId = GUIHandler.Instance.CacheManager.GetAllOrders().Last().Id;

            // Create new Order object
            Order order = new Order
            {
                Id = lastOrderId + 1,
                User = CurrentUser,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                OrderDetails = OrderDetails,
                ShippedDate = m_objSelectedShippingDate,
                ShippingAddress = m_objSelectedShippingAddress
            };

            // Add the new order to the OrderManager
            MessageProcessor.SendMessage(
                Enums.MessageType.Order,
                Enums.MessageAction.Add,
                order
            );
            // Close the Add Order View window
            CloseWindow?.Invoke();
        }

        // Command Actions for adding and removing Product Rows
        private void RemoveOrderDetails(object orderDetail)
        {
            OrderDetails.Remove(orderDetail as OrderDetail);
        }

        private void AddOrderDetails(object obj)
        {
            var newOrderDetail = new OrderDetail { Quantity = 1 };
            newOrderDetail.PropertyChanged += (s, e) => SubmitOrderCommand.RaiseCanExecuteEventChanged();
            OrderDetails.Add(newOrderDetail);
        }

        // Command Predicates for Submit Order, Add Product Row and Remove Product Row
        private bool CanSubmitOrder(object obj)
        { 
            bool isValid = Validator.TryValidateObject(this, new ValidationContext(this), null, true);

            bool hasValidOrderDetails = OrderDetails.Count > 0 && OrderDetails.All(order => order.Product != null && order.Quantity > 0);

            return isValid && hasValidOrderDetails;
        }
        private bool CanAddOrderDetails(object obj)
        {
            return true;
        }
        private bool CanRemoveOrderDetails(object obj)
        {
            return true;
        }
        #endregion
    }
}
