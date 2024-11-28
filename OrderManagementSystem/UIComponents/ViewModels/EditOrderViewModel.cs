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

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditOrderViewModel : INotifyPropertyChanged
    {
        private Order _order;
        public EditOrderViewModel(Order order)
        {
            _order = order;
            //MessageBox.Show($"{ order}");
            // Initialize editable fields
            Id = order.Id;
            User = order.User.Name;
            OrderDate = order.OrderDate;
            ShippingAddress = order.ShippingAddress;
            Products = new ObservableCollection<ProductRow>(
                order.Products.Select(p => new ProductRow
                {
                    SelectedProduct = p.Key,
                    Quantity = p.Value
                })
            );
        }

        public int? Id { get; } // Non-editable
        public string User { get; } // Non-editable
        public DateTime? OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public ObservableCollection<ProductRow> Products { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ICommand SaveCommand => new RelayCommand(SaveOrder, CanSaveOrder);

        private bool CanSaveOrder(object obj)
        {
            return true;
        }

        private void SaveOrder(object obj)
        {
            // m_objOrder
            // m_stError
            // Logic to save the updated order
            _order.OrderDate = OrderDate;
            _order.ShippingAddress = ShippingAddress;
            _order.Products = Products.ToDictionary(
                row => row.SelectedProduct,
                row => row.Quantity
            );

            // Update the order in the database or collection
            OrderManager.UpdateOrder(_order);
        }
    }
}
