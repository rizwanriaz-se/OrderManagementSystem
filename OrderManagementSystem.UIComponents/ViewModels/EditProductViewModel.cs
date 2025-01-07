using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditProductViewModel : INotifyDataErrorInfo
    {
        private Product _Product;

        public Action CloseWindow { get; set; }

        public ObservableCollection<Category> Categories { get; private set; }

        private string m_stProductNameText;
        private string m_stProductDescriptionText;
        private decimal m_decProductUnitPriceText;
        private int m_nProductUnitsInStockText;
        private Category m_objSelectedCategory;

        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        public string ProductNameText
        {

            get { return m_stProductNameText; }
            set
            {
                m_stProductNameText = value;
                Validate(nameof(ProductNameText), m_stProductNameText);
            }
        }

        [Required(ErrorMessage = "Product Description is required")]
        public string ProductDescriptionText
        {

            get { return m_stProductDescriptionText; }
            set
            {
                m_stProductDescriptionText = value;
                Validate(nameof(ProductDescriptionText), m_stProductDescriptionText);
            }

        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        [Required(ErrorMessage = "Category must be selected.")]
        public Category SelectedCategory {
            get { return m_objSelectedCategory; }
            set
            {
                m_objSelectedCategory = value;
                Validate(nameof(SelectedCategory), m_objSelectedCategory);
            }
        }
        public byte[] Picture { get; set; }

        [Required(ErrorMessage = "Product Unit Price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Unit Price must be greater than zero.")]
        public decimal ProductUnitPriceText
        {

            get { return m_decProductUnitPriceText; }
            set
            {
                m_decProductUnitPriceText = Convert.ToDecimal(value); ;
                Validate(nameof(ProductUnitPriceText), m_decProductUnitPriceText);
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
                Validate(nameof(ProductUnitsInStockText), m_nProductUnitsInStockText);
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
            SaveProductCommand.RaiseCanExecuteEventChanged();
        }

        public RelayCommand SaveProductCommand { get; set; }


        public EditProductViewModel()
        {
            Categories = GUIHandler.Instance.CacheManager.GetAllCategories();
            SaveProductCommand = new RelayCommand(SaveProduct, CanSaveProduct);
        }

        private bool CanSaveProduct(object obj)
        {
            return !HasErrors && Validator.TryValidateObject(this, new ValidationContext(this), null, true);
        }

        private void SaveProduct(object obj)
        {
            _Product = new Product();
            _Product.Id = Id;
            _Product.Name = ProductNameText;
            _Product.Description = ProductDescriptionText;
            _Product.Category = SelectedCategory;
            _Product.Picture = Picture;
            _Product.UnitPrice = ProductUnitPriceText;
            _Product.UnitsInStock = ProductUnitsInStockText;

            MessageProcessor.SendMessage(Enums.MessageType.Product, Enums.MessageAction.Update, _Product);

            CloseWindow.Invoke();

        }
    }
}
