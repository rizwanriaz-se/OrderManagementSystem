using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystem.Cache.Models;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    
    public class OrderViewModel
    {
    
        public ObservableCollection<Order> Orders { get; private set; }

        public OrderViewModel()
        {
            Orders = OrderManager.GetAllOrders();
        }
    }
}
