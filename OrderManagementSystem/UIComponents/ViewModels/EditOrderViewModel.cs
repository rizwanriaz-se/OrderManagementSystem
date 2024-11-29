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
            AllProducts = ProductManager.GetAllProducts();
            AddProductCommand = new RelayCommand(AddProductRow, CanAddProductRow);
            RemoveProductCommand = new RelayCommand(RemoveProductRow, CanRemoveProductRow);
            SaveOrderCommand = new RelayCommand(SaveOrder, CanSubmitOrder);
            //SelectedStatus = OrderStatus.Pending;

            _order = order;
            //MessageBox.Show($"{ order.Id} {order.OrderDate} {order.User} {order.Products}");
            // Initialize editable fields
            Id = order.Id;
            User = order.User;
            OrderDate = order.OrderDate;
            SelectedShippingDate = order.ShippedDate;
            SelectedStatus = order.Status;
            SelectedShippingAddress = order.ShippingAddress;
            ProductRows = new ObservableCollection<ProductRow>(
                order.Products.Select(p => new ProductRow
                {
                    SelectedProduct = p.Key,
                    Quantity = p.Value
                })
            );
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

        public string SelectedShippingAddress { get; set; }
        public ObservableCollection<ProductRow> ProductRows { get; set; }
        public ObservableCollection<OrderStatus> SelectableStatuses { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        

        private void RemoveProductRow(object productRow)
        {
            ProductRows.Remove(productRow as ProductRow);
        }

        private void AddProductRow(object obj)
        {
            ProductRows.Add(new ProductRow { Quantity = 1 });
        }

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

        private bool CanSaveOrder(object obj)
        {
            return true;
        }

        private void SaveOrder(object obj)
        {
            // m_Order
            // m_stError
            // Logic to save the updated order
            _order.Id = Id;
            _order.User = User;
            _order.OrderDate = OrderDate;
            _order.Status = SelectedStatus;
            _order.ShippedDate = SelectedShippingDate;
            _order.ShippingAddress = SelectedShippingAddress;
            _order.Products = ProductRows.ToDictionary(
                row => row.SelectedProduct,
                row => row.Quantity
            );

            // Update the order in the database or collection
            OrderManager.UpdateOrder(_order);

            // Close the window
            CloseWindow?.Invoke();

        }
    }
}

//Todo:
// Look for situations when order date would need to be updated