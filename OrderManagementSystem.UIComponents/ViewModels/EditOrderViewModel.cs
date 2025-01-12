using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using static OrderManagementSystemServer.Repository.Order;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditOrderViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public Action CloseWindow { get; set; }
        public ObservableCollection<Product> AllProducts { get; private set; }

        public ICommand AddProductCommand { get; set; }
        public ICommand RemoveProductCommand { get; set; }
        public ICommand SaveOrderCommand { get; set; }

        public EditOrderViewModel()
        {
            AllProducts = GUIHandler.Instance.CacheManager.GetAllProducts();
            AddProductCommand = new RelayCommand(AddOrderDetails);
            RemoveProductCommand = new RelayCommand(RemoveOrderDetails);
            SaveOrderCommand = new RelayCommand(SaveOrder);
            SelectableStatuses = new ObservableCollection<OrderStatus>(
                 Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                 .Where(status => status != OrderStatus.Pending)
             );
        }
     
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private DateTime? m_objOrderDate;
        private DateTime? m_objSelectedShippingDate;
        private OrderStatus? m_objSelectedStatus;
        private string m_stSelectedShippingAddress;


        public int? Id { get; set; }
        public User User { get; set; }
        

        [Required(ErrorMessage = "Order Date is required.")]
        public DateTime? OrderDate
        {
            get { return m_objOrderDate; }
            set
            {
                m_objOrderDate = value;
                OnPropertyChanged(nameof(OrderDate));
                Validate(nameof(OrderDate), m_objOrderDate);
            }
        }

        //[Required(ErrorMessage = "Shipping Date must be selected.")]
        public DateTime? SelectedShippingDate
        {

            get { return m_objSelectedShippingDate; }
            set
            {
                m_objSelectedShippingDate = value;
                OnPropertyChanged(nameof(SelectedShippingDate));
            }

        }

        [Required(ErrorMessage = "Status must be selected.")]
        public OrderStatus? SelectedStatus
        {

            get { return m_objSelectedStatus; }
            set
            {
                m_objSelectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
                Validate(nameof(SelectedStatus), m_objSelectedStatus);
            }
        }

        public ObservableCollection<OrderDetail> OrderDetails { get; set; }

        [Required(ErrorMessage = "Shipping Address must be selected.")]
        public string SelectedShippingAddress
        {

            get { return m_stSelectedShippingAddress; }
            set
            {
                m_stSelectedShippingAddress = value;
                OnPropertyChanged(nameof(SelectedShippingAddress));
                Validate(nameof(SelectedShippingAddress), m_stSelectedShippingAddress);
            }
        }

        public ObservableCollection<OrderStatus> SelectableStatuses { get; }

        Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool HasErrors => Errors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (Errors.ContainsKey(propertyName))
            {
                return Errors[propertyName];
            }
            return null;
        }

        public bool Validate(string propertyName, object propertyValue)
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

            return Errors.ContainsKey(propertyName);
        }

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

            try
            {
                if (OrderDetails.Count <= 0)
                {
                    throw new Exception("Please add some products before proceeding.");
                  
                }

                if (!OrderDetails.All(order => order.Product != null && order.Quantity > 0))
                {
                    throw new Exception("Products and their respective quantity should not be empty.");
                    
                }

                if (Validate(nameof(SelectedShippingAddress), m_stSelectedShippingAddress) || Validate(nameof(SelectedStatus), m_objSelectedStatus) || Validate(nameof(SelectedShippingDate), m_objSelectedShippingDate))
                {

                    var errors = Errors.SelectMany(o => o.Value);

                    throw new Exception(string.Join('\n', errors));

                }

                Order _order = new Order();
                _order.Id = Id;
                _order.User = User;
                _order.OrderDate = OrderDate;
                _order.Status = SelectedStatus;
                _order.ShippedDate = SelectedShippingDate;
                _order.ShippingAddress = SelectedShippingAddress;
                _order.OrderDetails = new ObservableCollection<OrderDetail>(OrderDetails);

                // Update the order in the database or collection
                MessageProcessor.SendMessage(Enums.MessageType.Order, Enums.MessageAction.Update, _order);

                // Close the window
                CloseWindow?.Invoke();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;            }
        }
    }
}
