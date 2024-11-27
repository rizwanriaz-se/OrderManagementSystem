using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{

    public class ProductRow
    {
        public string SelectedProduct { get; set; }
        public int Quantity { get; set; }
    }

        public class AddOrderViewModel
    {
        public ObservableCollection<ProductRow> ProductRows { get; set; } = new ObservableCollection<ProductRow>();
        public ICommand AddProductCommand { get; set; }
        public ICommand RemoveProductCommand { get; set; }

        //public OrderViewModel orderViewModel { get; set; }
        public ProductViewModel productViewModel { get; set; } = new ProductViewModel();
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
            //UserProducts = new ObservableCollection<ProductViewModel>();
            //orderViewModel = new OrderViewModel();
            //AddProductCommand = new RelayCommand(AddProduct, CanAddProduct);
            //RemoveProductCommand = new RelayCommand(RemoveProduct, CanRemoveProduct);
            //Order = new OrderViewModel();
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
            throw new NotImplementedException();
        }

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
