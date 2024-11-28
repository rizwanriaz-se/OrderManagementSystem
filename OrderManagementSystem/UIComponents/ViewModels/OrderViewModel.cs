using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    
    public class OrderViewModel
    {
        public ICommand AddProductCommand { get; }
        public ICommand SubmitOrderCommand { get; }

        public ObservableCollection<Order> Orders { get; private set; }
        //public ObservableCollection<ProductViewModel> Products { get; set; }


        public OrderViewModel()
        {
            //Products = new ObservableCollection<ProductViewModel>();
            //AddProductCommand = new RelayCommand(AddProduct, CanAddProduct);
            //SubmitOrderCommand = new RelayCommand(SubmitOrder, CanSubmitOrder);
            Orders = OrderManager.GetAllOrders();
        }

        //private void AddProduct(object parameter)
        //{
        //    Products.Add(new ProductViewModel());
        //}

        //private bool CanAddProduct(object parameter)
        //{
        //    return true;
        //}

        //private void SubmitOrder(object parameter)
        //{
        //    Products.Add(new ProductViewModel());
        //}

        //private bool CanRemoveProduct(object parameter)
        //{
        //    return true;
        //}

        //private void SubmitOrder()
        //{
        //    // Logic for submitting the order
        //}

    }
}
