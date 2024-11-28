using DevExpress.Xpf.Controls;
using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public Action CloseWindow { get; set; }

        public ICommand SubmitProductCommand { get; set; }

        public ObservableCollection<Product> Products { get; private set; }
        public ObservableCollection<Category> Categories { get; private set; }

        public string ProductNameText {  get; set; }

        public string ProductUnitPriceText { get; set; }

        public string ProductUnitsInStockText { get; set; }

        public string ProductDescriptionText { get; set; }

        private Category _selectedCategory { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategory)));
            }
        }

        public ProductViewModel()
        {
            Categories = CategoryManager.GetAllCategories();
            Products = ProductManager.GetAllProducts();
            SubmitProductCommand = new RelayCommand(SubmitProduct, CanSubmitProduct);
        }

        public void SubmitProduct(object obj)
        {
            int lastProductId = Products.Last().Id;

            // Create new Product object
            Product product = new Product
            {
                Id = lastProductId + 1,
                Name = ProductNameText,
                UnitPrice = Convert.ToDecimal(ProductUnitPriceText),
                UnitsInStock = Convert.ToInt32(ProductUnitsInStockText),
                Description = ProductDescriptionText,
                Category = SelectedCategory
            };

            ProductManager.AddProduct(product);
            CloseWindow?.Invoke();
        }

        private bool CanSubmitProduct(object obj)
        {
            return true;
        }

    }
}
