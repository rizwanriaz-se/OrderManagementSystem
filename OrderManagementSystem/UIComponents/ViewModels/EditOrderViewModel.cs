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
    public class EditOrderViewModel : INotifyPropertyChanged
    {
        private Order _order;

        public Action CloseWindow { get; set; }
        public ObservableCollection<Product> AllProducts { get; private set; }


        public ICommand AddProductCommand { get; set; }
        public ICommand RemoveProductCommand { get; set; }
        public ICommand SaveOrderCommand { get; set; }
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

        public int? Id { get; } // Non-editable
        public User User { get; } // Non-editable
        public DateTime? OrderDate { get; set; }
        public DateTime? SelectedShippingDate { get; set; }
        public OrderStatus? SelectedStatus { get; set; }

        public ObservableCollection<OrderDetail> OrderDetails { get; set; }

        public string SelectedShippingAddress { get; set; }
        //public ObservableCollection<ProductRow> ProductRows { get; set; }

        //public ObservableCollection<OrderDetail> OrderDetails { get; set; }
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
            OrderDetails.Add(new OrderDetail { Quantity = 1 });
        }

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
            GUIHandler.GetInstance().CacheManager.UpdateOrder(_order);

            // Close the window
            CloseWindow?.Invoke();

        }
    }
}

//Todo:
// Look for situations when order date would need to be updated