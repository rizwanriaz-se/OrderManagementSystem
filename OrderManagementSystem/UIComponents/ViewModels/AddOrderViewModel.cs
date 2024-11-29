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
    public class ProductRow : INotifyPropertyChanged
    {
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedProduct)));
            }
        }
        public int Quantity { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

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

        // Initialize required ViewModels to avoid null reference

        //public ProductViewModel productViewModel { get; set; } = new ProductViewModel();
        //public OrderViewModel orderViewModel { get; set; } = new OrderViewModel();
        //public UserViewModel userViewModel { get; set; } = new UserViewModel();

        // Declare Observable Collections for data bindings
        public ObservableCollection<ProductRow> ProductRows { get; set; } = new ObservableCollection<ProductRow>();
        public ObservableCollection<Product> AllProducts { get; set; }
        public ObservableCollection<User> AllUsers { get; set; }
        public ObservableCollection<OrderStatus> SelectableStatuses { get; }

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

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedUser)));
            }
        }
        #endregion

        #region Constructor
        public AddOrderViewModel()
        {
            AllProducts = ProductManager.GetAllProducts();
            AllUsers = UserManager.GetAllUsers();
            //SelectedStatus = OrderStatus.Pending;

            //SelectableStatuses = new ObservableCollection<OrderStatus>(
            //    Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().Where(
            //        status => status != OrderStatus.Pending)
            //    );

            //AllProducts = productViewModel.Products;
            //AllUsers = userViewModel.Users;
            AddProductCommand = new RelayCommand(AddProductRow, CanAddProductRow);
            RemoveProductCommand = new RelayCommand(RemoveProductRow, CanRemoveProductRow);
            SubmitOrderCommand = new RelayCommand(SubmitOrder, CanSubmitOrder);
        }
        #endregion

        #region Command Actions
        // Command Action to Submit Order
        private void SubmitOrder(object obj)
        {
            // Logic for submitting the order
            if (ProductRows == null || ProductRows.Count == 0)
            {
                MessageBox.Show("Please add at least one product.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Get the last order Id to generate new order Id
            int? lastOrderId = OrderManager.GetAllOrders().Last().Id;
            //int? lastOrderId = orderViewModel.Orders.Last().Id;

            // Create new Order object
            Order order = new Order
            {
                Id = lastOrderId + 1,
                User = _selectedUser,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                Products = ProductRows.ToDictionary(
                    row => ProductManager.GetProductByName(row), // Key: Product object
                    row => row.Quantity // Value: Quantity
                    ),
                ShippedDate = _selectedShippingDate,
                ShippingAddress = _selectedShippingAddress
            };

            // Add the new order to the OrderManager
            OrderManager.AddOrder(order);

            // Close the Add Order View window
            CloseWindow?.Invoke();
        }

        // Command Actions for adding and removing Product Rows
        private void RemoveProductRow(object productRow)
        {
            ProductRows.Remove(productRow as ProductRow);
        }

        private void AddProductRow(object obj)
        {
            ProductRows.Add(new ProductRow { Quantity = 1 });
        }
        #endregion

        #region Command Predicates
        // Command Predicates for Submit Order, Add Product Row and Remove Product Row
        private bool CanSubmitOrder(object obj)
        {
            return true;
        }
        private bool CanAddProductRow(object obj)
        {
            return true;
        }
        private bool CanRemoveProductRow(object obj)
        {
            return true;
        }
        #endregion
    }
}
