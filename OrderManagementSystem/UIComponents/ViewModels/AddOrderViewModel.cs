using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static OrderManagementSystem.Cache.Models.Order;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    // Data model used to store dynamically created Product Rows
    //public class ProductRow : INotifyPropertyChanged
    //{
    //    private Product _selectedProduct;
    //    public Product SelectedProduct
    //    {
    //        get { return _selectedProduct; }
    //        set
    //        {
    //            _selectedProduct = value;
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedProduct)));
    //        }
    //    }
    //    public int Quantity { get; set; }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //}

    // View Model for Add Order View
    public class AddOrderViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Declarations
        // Declare Action to close Add Order View window when invoked
        public Action CloseWindow { get; set; }

        // Declare properties for data bindings
        private OrderStatus _selectedStatus;
        private string _selectedShippingAddress;
        private DateTime _selectedShippingDate = DateTime.Now;
        private User _selectedUser;


        // Declare Commands
        public RelayCommand AddProductCommand { get; set; }
        public RelayCommand RemoveProductCommand { get; set; }
        public RelayCommand SubmitOrderCommand { get; set; }

        // Declare Observable Collections for data bindings
        //public ObservableCollection<ProductRow> ProductRows { get; set; } = new ObservableCollection<ProductRow>();
        public ObservableCollection<OrderDetail> OrderDetails { get; set; } = new ObservableCollection<OrderDetail>();

        public ObservableCollection<Product> AllProducts { get; set; }
        public ObservableCollection<User> AllUsers { get; set; }
        private User _currentUser;

        //[Required(ErrorMessage = "Current name is required")]
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentUser)));
                }
            }
        }
        public ObservableCollection<OrderStatus> SelectableStatuses { get; }
        //public User CurrentUser { get; set; } = GUIHandler.GetInstance().CacheManager.CurrentUser;



        // Declare PropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        #endregion

        #region Data Bindings

        // Data Bindings for Selected Properties
        [Required(ErrorMessage = "Status must be selected.")]
        public OrderStatus SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStatus)));
                Validate(nameof(SelectedStatus), _selectedStatus);

            }
        }

        [Required(ErrorMessage = "Shipping Address is required")]
        public string SelectedShippingAddress
        {
            get { return _selectedShippingAddress; }
            set
            {
                _selectedShippingAddress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedShippingAddress)));
                Validate(nameof(SelectedShippingAddress), _selectedShippingAddress);

            }
        }

        [Required(ErrorMessage = "Shipping Date is required")]
        public DateTime SelectedShippingDate
        {
            get
            {
                return _selectedShippingDate;
            }
            set
            {
                _selectedShippingDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedShippingDate)));
                Validate(nameof(SelectedShippingDate), _selectedShippingDate);

            }
        }

        Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();
        //public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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

        //public User SelectedUser
        //{
        //    get { return _selectedUser; }
        //    set
        //    {
        //        _selectedUser = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedUser)));
        //    }
        //}
        #endregion

        #region Constructor
        public AddOrderViewModel()
        {
            AllProducts = GUIHandler.GetInstance().CacheManager.GetAllProducts();
            AllUsers = GUIHandler.GetInstance().CacheManager.GetAllUsers();
            CurrentUser = GUIHandler.GetInstance().CurrentUser;

            if (CurrentUser == null)
            {
                // Handle the case where CurrentUser is null
                MessageBox.Show("Current user is not set.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            AddProductCommand = new RelayCommand(AddOrderDetails, CanAddOrderDetails);
            RemoveProductCommand = new RelayCommand(RemoveOrderDetails, CanRemoveOrderDetails);
            SubmitOrderCommand = new RelayCommand(SubmitOrder, CanSubmitOrder);
        }
        #endregion

        #region Command Actions
        // Command Action to Submit Order
        private void SubmitOrder(object obj)
        {
            // Logic for submitting the order
            //if (ProductRows == null || ProductRows.Count == 0)
            //{
            //    MessageBox.Show("Please add at least one product.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            //if (OrderDetail == null || ProductRows.Count == 0)
            //{
            //    MessageBox.Show("Please add at least one product.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            // Get the last order Id to generate new order Id
            int? lastOrderId = GUIHandler.GetInstance().CacheManager.GetAllOrders().Last().Id;

            // Create new Order object
            Order order = new Order
            {
                Id = lastOrderId + 1,
                User = CurrentUser,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                OrderDetails = OrderDetails,
                //OrderDetails = ProductRows.Select(row => new OrderDetail
                //{
                //    Product = GUIHandler.GetInstance().CacheManager.GetProductByName(row),
                //    Quantity = row.Quantity
                //}).ToList(),

                ShippedDate = _selectedShippingDate,
                ShippingAddress = _selectedShippingAddress
            };

            // Add the new order to the OrderManager
            GUIHandler.GetInstance().CacheManager.AddOrder(order);

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
            OrderDetails.Add(new OrderDetail { Quantity = 1 });
        }
        #endregion

        #region Command Predicates
        // Command Predicates for Submit Order, Add Product Row and Remove Product Row
        private bool CanSubmitOrder(object obj)
        {
            return Validator.TryValidateObject(this, new ValidationContext(this), null, true) && OrderDetails.Count > 0 && OrderDetails.All(order=>
                {
                    return order.Product != null && order.Quantity > 0;
                });
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
