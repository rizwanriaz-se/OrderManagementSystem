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

        public class AddOrderViewModel
    {
        public ObservableCollection<ProductRow> ProductRows { get; set; } = new ObservableCollection<ProductRow>();
        public ICommand AddProductCommand { get; set; }
        public ICommand RemoveProductCommand { get; set; }
        public ICommand SubmitOrderCommand { get; set; }

        //public OrderViewModel orderViewModel { get; set; }
        public ProductViewModel productViewModel { get; set; } = new ProductViewModel();
        public OrderViewModel orderViewModel { get; set; } = new OrderViewModel();
        //public OrderViewModel Order { get; set; }

        public ObservableCollection<Product> AllProducts { get; set; }
        public ObservableCollection<ProductViewModel> UserProducts { get; set; }

        //public ICommand AddProductCommand { get; set; }
        //public ICommand RemoveProductCommand { get; set; }

        public AddOrderViewModel()
        {
            // Initialize the ProductViewModel to avoid null reference

            AllProducts = productViewModel.Products;
            AddProductCommand = new RelayCommand(AddProductRow, CanAddProductRow);
            RemoveProductCommand = new RelayCommand(RemoveProductRow, CanRemoveProductRow);
            SubmitOrderCommand = new RelayCommand(SubmitOrder, CanSubmitOrder);
            //UserProducts = new ObservableCollection<ProductViewModel>();
            //orderViewModel = new OrderViewModel();
            //AddProductCommand = new RelayCommand(AddProduct, CanAddProduct);
            //RemoveProductCommand = new RelayCommand(RemoveProduct, CanRemoveProduct);
            //Order = new OrderViewModel();
        }

        private void SubmitOrder(object obj)
        {
            // Logic for submitting the order
            if (ProductRows == null || ProductRows.Count == 0)
            {
                MessageBox.Show("Please add at least one product.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

  
        int? lastOrderId = orderViewModel.Orders.Last().Id;
            Order order = new Order
            {
                Id = lastOrderId + 1,
                User = new User { Id = 1, Name = "John Doe" },
                OrderDate = DateTime.Now,
                Status = Order.OrderStatus.Pending,
                Products = ProductRows.ToDictionary(
                    row => ProductManager.GetProductByName(row), // Key: Product object
                    row => row.Quantity // Value: Quantity
                    ),
                ShippedDate = DateTime.Now,
                ShippingAddress = "1234 Main St, Springfield, IL 62701"
            };

            OrderManager.AddOrder(order);

        }
        private bool CanSubmitOrder(object obj)
        {
            return true;
        }

        private bool CanRemoveProductRow(object obj)
        {
            return true;       
        }

        private void RemoveProductRow(object productRow)
        {
            ProductRows.Remove(productRow as ProductRow);
        }

        private bool CanAddProductRow(object obj)
        {
            return true;        }

        private void AddProductRow(object obj)
        {
            ProductRows.Add(new ProductRow { Quantity = 1 });
        }

        //private bool CanAddProduct(object obj)
        //{
        //    return true;
        //}


        //private void AddProduct(object obj)
        //{
        //    UserProducts.Add(new ProductViewModel());
        //}
        //private void RemoveProduct(object obj) {

        //}

        private bool CanAddProduct(object obj)
        {
            return true;
        }
        private bool CanRemoveProduct(object obj)
        {
            return true;
        }



    }
}
