using DevExpress.Xpf.Controls;
//using OrderManagementSystem.Commands;
using OrderManagementSystem.UIComponents.UIComponents.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OrderManagementSystem.Repositories.Repositories;
//using OrderManagementSystem.Classes;
using OrderManagementSystem.UIComponents.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;

namespace OrderManagementSystem.UIComponents.UIComponents.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public Action CloseWindow { get; set; }

        public ICommand SubmitProductCommand { get; set; }
        public ICommand EditProductCommand { get; set; }

        public ICommand DeleteProductCommand { get; set; }

        private Product m_SelectedProduct;

        public Product SelectedProduct
        {
            get { return m_SelectedProduct; }
            set
            {
                m_SelectedProduct = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedProduct)));
            }
        }

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
            Categories = GUIHandler.GetInstance().CacheManager.GetAllCategories();
            Products = GUIHandler.GetInstance().CacheManager.GetAllProducts();
            SubmitProductCommand = new RelayCommand(SubmitProduct, CanSubmitProduct);
            EditProductCommand = new RelayCommand(EditProduct, CanEditProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanDeleteProduct);
        }

        private void EditProduct(object obj)
        {
            EditProductView editProductView = new EditProductView();
            EditProductViewModel editProductViewModel = new EditProductViewModel(SelectedProduct);
            
            editProductView.DataContext = editProductViewModel;
            editProductViewModel.CloseWindow = editProductView.Close;
            editProductView.ShowDialog();
        }

        private bool CanEditProduct(object obj)
        {
            return (SelectedProduct != null);
        }

        private void DeleteProduct(object obj) {
            //GUIHandler.GetInstance().CacheManager.DeleteProduct(SelectedProduct);
            GUIHandler.GetInstance().MessageProcessor.SendMessage(
                Enums.MessageType.Product,
                Enums.MessageAction.Delete,
                SelectedProduct
            );
            //Products.Remove(SelectedProduct);
        }

        private bool CanDeleteProduct(object obj)
        {
            return (SelectedProduct != null);
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

            //GUIHandler.GetInstance().CacheManager.AddProduct(product);
            GUIHandler.GetInstance().MessageProcessor.SendMessage(
                Enums.MessageType.Product,
                Enums.MessageAction.Add,
                product
            );
            CloseWindow?.Invoke();
        }

        private bool CanSubmitProduct(object obj)
        {
            return true;
        }

    }
}
