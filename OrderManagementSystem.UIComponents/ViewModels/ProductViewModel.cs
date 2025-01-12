using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystem.UIComponents.Views;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        private string m_stProductNameText;
        private string m_stProductDescriptionText;
        private decimal m_decProductUnitPriceText;
        private int m_nProductUnitsInStockText;
        private Category m_objSelectedCategory;
        private Product m_objSelectedProduct;


        public Action CloseWindow { get; set; }

        public ICommand SubmitProductCommand { get; set; }
        public ICommand EditProductCommand { get; set; }

        public ICommand DeleteProductCommand { get; set; }


        public Product SelectedProduct
        {
            get { return m_objSelectedProduct; }
            set
            {
                m_objSelectedProduct = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedProduct)));
            }
        }

        public ObservableCollection<Product> Products { get; private set; }
        public ObservableCollection<Category> Categories { get; private set; }


        [Required(ErrorMessage = "Product Name is required")]
        public string ProductNameText
        {

            get { return m_stProductNameText; }
            set
            {
                m_stProductNameText = value;
            }

        }

        [Required(ErrorMessage = "Product Unit Price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Unit Price must be greater than zero.")]
        public decimal ProductUnitPriceText
        {


            get { return m_decProductUnitPriceText; }
            set
            {
                m_decProductUnitPriceText = Convert.ToDecimal(value);
            }

        }

        [Required(ErrorMessage = "Product Stock Units value is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Stock Units must be greater than zero.")]
        public int ProductUnitsInStockText
        {


            get { return m_nProductUnitsInStockText; }
            set
            {
                m_nProductUnitsInStockText = Convert.ToInt32(value);
            }

        }

        [Required(ErrorMessage = "Product Description is required")]
        public string ProductDescriptionText
        {


            get { return m_stProductDescriptionText; }
            set
            {
                m_stProductDescriptionText = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [Required(ErrorMessage = "Category must be selected.")]
        public Category SelectedCategory
        {
            get { return m_objSelectedCategory; }
            set
            {
                m_objSelectedCategory = value;
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

        public bool Validate(string propertyName, object propertyValue)
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

            return Errors.ContainsKey(propertyName);
        }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        public ProductViewModel()
        {
            Categories = GUIHandler.Instance.CacheManager.GetAllCategories();
            Products = GUIHandler.Instance.CacheManager.GetAllProducts();
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
            MessageProcessor.SendMessage(
                Enums.MessageType.Product,
                Enums.MessageAction.Delete,
                SelectedProduct.Id
            );
        }


        public void SubmitProduct(object obj)
        {
            try
            {
                int lastProductId = Products.Last().Id;

                if (Validate(nameof(ProductNameText), m_stProductNameText) || (Validate(nameof(ProductDescriptionText), m_stProductDescriptionText) || Validate(nameof(SelectedCategory), m_objSelectedCategory) || Validate(nameof(ProductUnitPriceText), m_decProductUnitPriceText) || Validate(nameof(ProductUnitsInStockText), m_nProductUnitsInStockText)))
                {
                    var errors = Errors.SelectMany(e => e.Value);
                    throw new Exception(string.Join('\n', errors));
                }

                if (Products.Any(p => p.Name.Equals(ProductNameText, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"A product with the name '{ProductNameText}' already exists.");
                }

                Product product = new Product
                {
                    Id = lastProductId + 1,
                    Name = ProductNameText,
                    UnitPrice = Convert.ToDecimal(ProductUnitPriceText),
                    UnitsInStock = Convert.ToInt32(ProductUnitsInStockText),
                    Description = ProductDescriptionText,
                    Category = SelectedCategory
                };

                MessageProcessor.SendMessage(
                    Enums.MessageType.Product,
                    Enums.MessageAction.Add,
                    product
                );
                CloseWindow?.Invoke();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
        }


    }
}
