﻿using DevExpress.XtraRichEdit.Fields.Expression;
//using OrderManagementSystem.Commands;
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
//using static OrderManagementSystem.Repositories.Order;
//using OrderManagementSystem.Classes;
using OrderManagementSystem.UIComponents.Commands;
//using OrderManagementSystem.Repositories;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystemServer.Repository;
using static OrderManagementSystemServer.Repository.Order;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditOrderViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public Action CloseWindow { get; set; }
        public ObservableCollection<Product> AllProducts { get; private set; }

        public RelayCommand AddProductCommand { get; set; }
        public RelayCommand RemoveProductCommand { get; set; }
        public RelayCommand SaveOrderCommand { get; set; }

        public EditOrderViewModel()
        {
            AllProducts = GUIHandler.Instance.CacheManager.GetAllProducts();
            AddProductCommand = new RelayCommand(AddOrderDetails, CanAddOrderDetails);
            RemoveProductCommand = new RelayCommand(RemoveOrderDetails, CanRemoveOrderDetails);
            SaveOrderCommand = new RelayCommand(SaveOrder, CanSaveOrder);
            SelectableStatuses = new ObservableCollection<OrderStatus>(
                 Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                 .Where(status => status != OrderStatus.Pending)
             );
        }
     
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private DateTime? _orderDate;
        private DateTime? _selectedShippingDate;
        private OrderStatus? _selectedStatus;
        private string _selectedShippingAddress;


        public int? Id { get; set; } // editable
        public User User { get; set; } // editable
        //public int? Id { get; } // Non-editable
        //public User User { get; } // Non-editable

        [Required(ErrorMessage = "Order Date is required.")]
        public DateTime? OrderDate
        {
            get { return _orderDate; }
            set
            {
                _orderDate = value;
                OnPropertyChanged(nameof(OrderDate));
                Validate(nameof(OrderDate), _orderDate);
            }
        }

        //[Required(ErrorMessage = "Shipping Date must be selected.")]
        public DateTime? SelectedShippingDate
        {

            get { return _selectedShippingDate; }
            set
            {
                _selectedShippingDate = value;
                OnPropertyChanged(nameof(SelectedShippingDate));
                //Validate(nameof(SelectedShippingDate), _selectedShippingDate);
            }

        }

        [Required(ErrorMessage = "Status must be selected.")]
        public OrderStatus? SelectedStatus
        {

            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
                Validate(nameof(SelectedStatus), _selectedStatus);
            }
        }

        public ObservableCollection<OrderDetail> OrderDetails { get; set; }

        [Required(ErrorMessage = "Shipping Address must be selected.")]
        public string SelectedShippingAddress
        {

            get { return _selectedShippingAddress; }
            set
            {
                _selectedShippingAddress = value;
                OnPropertyChanged(nameof(SelectedShippingAddress));
                Validate(nameof(SelectedShippingAddress), _selectedShippingAddress);
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
            SaveOrderCommand.RaiseCanExecuteEventChanged();
        }

        private void RemoveOrderDetails(object orderDetails)
        {
            OrderDetails.Remove(orderDetails as OrderDetail);
        }

        private void AddOrderDetails(object obj)
        {
            var newOrderDetail = new OrderDetail { Quantity = 1 };
            newOrderDetail.PropertyChanged += (s, e) => SaveOrderCommand.RaiseCanExecuteEventChanged();
            OrderDetails.Add(newOrderDetail);
        }

        private bool CanSaveOrder(object obj)
        {
            return Validator.TryValidateObject(this, new ValidationContext(this), null, true) && OrderDetails.Count > 0 && OrderDetails.All(order =>
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

        private void SaveOrder(object obj)
        {

            // Logic to save the updated order
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
    }
}

//Todo:
// Look for situations when order date would need to be updated