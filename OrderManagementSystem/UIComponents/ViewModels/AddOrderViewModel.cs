using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class AddOrderViewModel : INotifyPropertyChanged
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
        public ICommand AddProductCommand { get; set; }
        public ICommand RemoveProductCommand { get; set; }
        public ICommand SubmitOrderCommand { get; set; }

        // Declare Observable Collections for data bindings
        //public ObservableCollection<ProductRow> ProductRows { get; set; } = new ObservableCollection<ProductRow>();
        public ObservableCollection<OrderDetail> OrderDetails { get; set; } = new ObservableCollection<OrderDetail>();

        public ObservableCollection<Product> AllProducts { get; set; }
        public ObservableCollection<User> AllUsers { get; set; }
        private User _currentUser;

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
        #endregion

        #region Data Bindings

        // Data Bindings for Selected Properties
        public OrderStatus SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStatus)));
            }
        }

        public string SelectedShippingAddress
        {
            get { return _selectedShippingAddress; }
            set
            {
                _selectedShippingAddress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedShippingAddress)));
            }
        }
        
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
            }
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
            return true;
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
