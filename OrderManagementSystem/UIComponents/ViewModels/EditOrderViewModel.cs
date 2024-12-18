using DevExpress.XtraRichEdit.Fields.Expression;
using OrderManagementSystem.Repositories;
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
using static OrderManagementSystem.Repositories.Order;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditOrderViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private Order _order;

        public Action CloseWindow { get; set; }
        public ObservableCollection<Product> AllProducts { get; private set; }


        public RelayCommand AddProductCommand { get; set; }
        public RelayCommand RemoveProductCommand { get; set; }
        public RelayCommand SaveOrderCommand { get; set; }
        public EditOrderViewModel(Order order)
        {
            AllProducts = GUIHandler.GetInstance().CacheManager.GetAllProducts();
            AddProductCommand = new RelayCommand(AddOrderDetails, CanAddOrderDetails);
            RemoveProductCommand = new RelayCommand(RemoveOrderDetails, CanRemoveOrderDetails);
            SaveOrderCommand = new RelayCommand(SaveOrder, CanSubmitOrder);

            _order = order;

            Id = order.Id;
            User = order.User;
            OrderDate = order.OrderDate;
            SelectedShippingDate = order.ShippedDate;
            SelectedStatus = order.Status;
            SelectedShippingAddress = order.ShippingAddress;
            OrderDetails = new ObservableCollection<OrderDetail>(order.OrderDetails);
            //ProductRows = new ObservableCollection<ProductRow>(
            //    order.Products.Select(p => new ProductRow
            //    {
            //        SelectedProduct = p.Key,
            //        Quantity = p.Value
            //    })
            //);
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
        

        public int? Id { get; } // Non-editable
        public User User { get; } // Non-editable

        [Required(ErrorMessage = "Order Date is required.")]
        public DateTime? OrderDate {
            
            get { return _orderDate; }
            set
            {
                _orderDate = value;
                OnPropertyChanged(nameof(OrderDate));
                Validate(nameof(OrderDate), _orderDate);
            }
        }

        //[Required(ErrorMessage = "Shipping Date must be selected.")]
        public DateTime? SelectedShippingDate {

            get { return _selectedShippingDate; }
            set
            {
                _selectedShippingDate = value;
                OnPropertyChanged(nameof(SelectedShippingDate));
                //Validate(nameof(SelectedShippingDate), _selectedShippingDate);
            }

        }

        [Required(ErrorMessage = "Status must be selected.")]
        public OrderStatus? SelectedStatus {
        
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
        public string SelectedShippingAddress {
        
            get { return _selectedShippingAddress; }
            set
            {
                _selectedShippingAddress = value;
                OnPropertyChanged(nameof(SelectedShippingAddress));
                Validate(nameof(SelectedShippingAddress), _selectedShippingAddress);
            }
        }



        //public ObservableCollection<ProductRow> ProductRows { get; set; }

        //public ObservableCollection<OrderDetail> OrderDetails { get; set; }
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
            OrderDetails.Add(new OrderDetail { Quantity = 1 });
        }

        private bool CanSubmitOrder(object obj)
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

        private bool CanSaveOrder(object obj)
        {
            return true;
        }

        private void SaveOrder(object obj)
        {

            // Logic to save the updated order
            _order.Id = Id;
            _order.User = User;
            _order.OrderDate = OrderDate;
            _order.Status = SelectedStatus;
            _order.ShippedDate = SelectedShippingDate;
            _order.ShippingAddress = SelectedShippingAddress;
            _order.OrderDetails = OrderDetails;


           

            // Update the order in the database or collection
            //GUIHandler.GetInstance().CacheManager.UpdateOrder(_order);
            GUIHandler.GetInstance().MessageProcessor.SendMessage(Enums.MessageType.Order, Enums.MessageAction.Update, _order);

            // Close the window
            CloseWindow?.Invoke();

        }
    }
}

//Todo:
// Look for situations when order date would need to be updated