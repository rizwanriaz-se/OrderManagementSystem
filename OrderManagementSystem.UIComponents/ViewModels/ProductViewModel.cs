using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystem.UIComponents.Views;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public Action CloseWindow { get; set; }

        public ICommand SubmitProductCommand { get; set; }
        public ICommand EditProductCommand { get; set; }

        public ICommand DeleteProductCommand { get; set; }


        public Product SelectedProduct { get; set; }


        public ObservableCollection<Product> Products { get;  set; }
        public ObservableCollection<Category> Categories { get;  set; }


        public string ProductNameText { get; set; }


        public decimal ProductUnitPriceText { get; set; }


        public int ProductUnitsInStockText { get; set; }


        public string ProductDescriptionText { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public Category SelectedCategory { get; set; }

        public ProductViewModel()
        {
            Categories = GUIHandler.Instance.CacheManager.Categories;
            Products = GUIHandler.Instance.CacheManager.Products;
            SubmitProductCommand = new RelayCommand(SubmitProduct);
            EditProductCommand = new RelayCommand(EditProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
        }

        private void EditProduct(object obj)
        {
            EditProductView editProductView = new EditProductView();
            editProductView.LoadProduct(SelectedProduct);

            editProductView.ShowDialog();
        }


        private void DeleteProduct(object obj)
        {
            MessageBoxResult confirmationResult = DXMessageBox.Show($"Are you sure you want to delete selected product: {SelectedProduct.Name}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirmationResult == MessageBoxResult.Yes)
                ClientManager.Instance.SendMessage(
                MessageType.Product,
                MessageAction.Delete,
                SelectedProduct.Id
            );
        }


        public void SubmitProduct(object obj)
        {
            if (ProductNameText == null)
            {
                DXMessageBox.Show("The product name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ProductDescriptionText == null)
            {
                DXMessageBox.Show("The product description cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SelectedCategory == null)
            {
                DXMessageBox.Show("Category must be selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ProductUnitPriceText == null || ProductUnitPriceText < 1)
            {
                DXMessageBox.Show("The product unit price must be greater than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ProductUnitsInStockText == null || ProductUnitsInStockText < 1)
            {
                DXMessageBox.Show("The product units in stock value must be greater than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Products.Any(p => p.Name.Equals(ProductNameText, StringComparison.OrdinalIgnoreCase)))
            {
                DXMessageBox.Show($"A product with the name '{ProductNameText}' already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Product product = new Product
            {
                Name = ProductNameText,
                UnitPrice = Convert.ToDecimal(ProductUnitPriceText),
                UnitsInStock = Convert.ToInt32(ProductUnitsInStockText),
                Description = ProductDescriptionText,
                Category = SelectedCategory
            };

            ClientManager.Instance.SendMessage(
                MessageType.Product,
                MessageAction.Add,
                product
            );
            CloseWindow?.Invoke();
        }

    }
}
