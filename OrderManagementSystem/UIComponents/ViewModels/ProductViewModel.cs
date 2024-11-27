using OrderManagementSystem.Cache.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class ProductViewModel
    {
        public ObservableCollection<Product> Products { get; private set; }

        public ProductViewModel()
        {
            Products = ProductManager.GetAllProducts();
        }
    }
}
