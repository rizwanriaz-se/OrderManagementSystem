using DevExpress.XtraExport.Implementation;
using DevExpress.XtraRichEdit.Fields.Expression;
using DevExpress.XtraRichEdit.Model.History;
//using OrderManagementSystem.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using OrderManagementSystem.Classes;
using OrderManagementSystem.UIComponents.Commands;
//using OrderManagementSystem.Repositories;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystemServer.Repository;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditProductViewModel : INotifyDataErrorInfo
    {
        private Product _Product;

        public Action CloseWindow { get; set; }

        public ObservableCollection<Category> Categories { get; private set; }

        private string _ProductNameText;
        private string _ProductDescriptionText;
        //private byte[] _Picture;
        private decimal _ProductUnitPriceText;
        private int _ProductUnitsInStockText;
        private Category _SelectedCategory;

        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        public string ProductNameText
        {

            get { return _ProductNameText; }
            set
            {
                _ProductNameText = value;
                Validate(nameof(ProductNameText), _ProductNameText);
            }
        }

        [Required(ErrorMessage = "Product Description is required")]
        public string ProductDescriptionText
        {

            get { return _ProductDescriptionText; }
            set
            {
                _ProductDescriptionText = value;
                Validate(nameof(ProductDescriptionText), _ProductDescriptionText);
            }

        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        [Required(ErrorMessage = "Category must be selected.")]
        public Category SelectedCategory {
            get { return _SelectedCategory; }
            set
            {
                _SelectedCategory = value;
                Validate(nameof(SelectedCategory), _SelectedCategory);
            }
        }
        public byte[] Picture { get; set; }

        [Required(ErrorMessage = "Product Unit Price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Unit Price must be greater than zero.")]
        public decimal ProductUnitPriceText
        {

            get { return _ProductUnitPriceText; }
            set
            {
                _ProductUnitPriceText = value;
                Validate(nameof(ProductUnitPriceText), _ProductUnitPriceText);
            }
        }

        [Required(ErrorMessage = "Product Stock Units value is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Stock Units must be greater than zero.")]
        public int ProductUnitsInStockText
        {

            get { return _ProductUnitsInStockText; }
            set
            {
                _ProductUnitsInStockText = value;
                Validate(nameof(ProductUnitsInStockText), _ProductUnitsInStockText);
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

        public EditProductViewModel(Product product)
        {
            Categories = GUIHandler.Instance.CacheManager.GetAllCategories();
            SaveProductCommand = new RelayCommand(SaveProduct, CanSaveProduct);

            _Product = product;


            Id = product.Id;
            ProductNameText = product.Name;
            ProductDescriptionText = product.Description;
            SelectedCategory = product.Category;
            Picture = product.Picture;
            ProductUnitPriceText = product.UnitPrice;
            ProductUnitsInStockText = product.UnitsInStock;

        }

        private bool CanSaveProduct(object obj)
        {
            return !HasErrors && Validator.TryValidateObject(this, new ValidationContext(this), null, true);
        }

        private void SaveProduct(object obj)
        {
            _Product.Name = ProductNameText;
            _Product.Description = ProductDescriptionText;
            _Product.Category = SelectedCategory;
            _Product.Picture = Picture;
            _Product.UnitPrice = ProductUnitPriceText;
            _Product.UnitsInStock = ProductUnitsInStockText;

            //GUIHandler.Instance.CacheManager.UpdateProduct(_Product);
            GUIHandler.Instance.MessageProcessor.SendMessage(Enums.MessageType.Product, Enums.MessageAction.Update, _Product);

            CloseWindow.Invoke();

        }
    }
}
