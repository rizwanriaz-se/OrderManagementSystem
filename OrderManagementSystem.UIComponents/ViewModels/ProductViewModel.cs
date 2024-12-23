using DevExpress.Xpf.Controls;
//using OrderManagementSystem.Commands;
using OrderManagementSystem.UIComponents.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using OrderManagementSystem.Classes;
using OrderManagementSystem.UIComponents.Commands;
//using OrderManagementSystem.Repositories;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystemServer.Repository;
using System.ComponentModel.DataAnnotations;
using DevExpress.XtraRichEdit.Fields.Expression;
using System.Collections;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        private string m_stProductNameText;
        private string m_stProductDescriptionText;
        private decimal m_decProductUnitPriceText;
        private int m_nProductUnitsInStockText;
        private Category m_objSelectedCategory;


        public Action CloseWindow { get; set; }

        public RelayCommand SubmitProductCommand { get; set; }
        public RelayCommand EditProductCommand { get; set; }

        public RelayCommand DeleteProductCommand { get; set; }

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


        [Required(ErrorMessage = "Product Name is required")]
        public string ProductNameText {

            get { return m_stProductNameText; }
            set
            {
                m_stProductNameText = value;
                Validate(nameof(ProductNameText), m_stProductNameText);
            }

        }

        [Required(ErrorMessage = "Product Unit Price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Unit Price must be greater than zero.")]
        public decimal ProductUnitPriceText {


            get { return m_decProductUnitPriceText; }
            set
            {
                m_decProductUnitPriceText = Convert.ToDecimal(value);
                Validate(nameof(ProductUnitPriceText), m_decProductUnitPriceText);
            }

        }

        [Required(ErrorMessage = "Product Stock Units value is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Stock Units must be greater than zero.")]
        public int ProductUnitsInStockText {


            get { return m_nProductUnitsInStockText; }
            set
            {
                m_nProductUnitsInStockText = Convert.ToInt32(value);
                Validate(nameof(ProductUnitsInStockText), m_nProductUnitsInStockText);
            }

        }

        [Required(ErrorMessage = "Product Description is required")]
        public string ProductDescriptionText {


            get { return m_stProductDescriptionText; }
            set
            {
                m_stProductDescriptionText = value;
                Validate(nameof(ProductDescriptionText), m_stProductDescriptionText);
            }
        }

        [Required(ErrorMessage = "Category must be selected.")]
        private Category _selectedCategory {

            get { return m_objSelectedCategory; }
            set
            {
                m_objSelectedCategory = value;
                Validate(nameof(SelectedCategory), m_objSelectedCategory);
            }
        }
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

        public bool HasErrors => Errors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (Errors.ContainsKey(propertyName))
            {
                return Errors[propertyName];
            }
            return null;
        }

        public void Validate(string propertyName, object propertyValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyName };
            Validator.TryValidateProperty(propertyValue, context, results);

            if (results.Any())
            {
                Errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                Errors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            SubmitProductCommand.RaiseCanExecuteEventChanged();
        }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        public ProductViewModel()
        {
            Categories = GUIHandler.Instance.CacheManager.GetAllCategories();
            Products = GUIHandler.Instance.CacheManager.GetAllProducts();
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
            return SelectedProduct != null;
        }

        private void DeleteProduct(object obj)
        {
            //GUIHandler.Instance.CacheManager.DeleteProduct(SelectedProduct);
            GUIHandler.Instance.MessageProcessor.SendMessage(
                Enums.MessageType.Product,
                Enums.MessageAction.Delete,
                SelectedProduct
            );
            //Products.Remove(SelectedProduct);
        }

        private bool CanDeleteProduct(object obj)
        {
            return SelectedProduct != null;
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

            //GUIHandler.Instance.CacheManager.AddProduct(product);
            GUIHandler.Instance.MessageProcessor.SendMessage(
                Enums.MessageType.Product,
                Enums.MessageAction.Add,
                product
            );
            CloseWindow?.Invoke();
        }

        private bool CanSubmitProduct(object obj)
        {
            return Validator.TryValidateObject(this, new ValidationContext(this), null, true);
            return true;
        }

    }
}
