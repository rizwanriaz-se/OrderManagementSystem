using DevExpress.XtraExport.Implementation;
using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditProductViewModel
    {
        private Product _Product;

        public Action CloseWindow {  get; set; }

        public ObservableCollection<Category> Categories { get; private set; }

        public int Id { get; set; }
        public string ProductNameText { get; set; }
        public string ProductDescriptionText { get; set; }
        public Category SelectedCategory { get; set; }
        public byte[] Picture { get; set; }
        public decimal ProductUnitPriceText { get; set; }
        public int ProductUnitsInStockText { get; set; }

        public ICommand SaveProductCommand { get; set; }

        public EditProductViewModel(Product product)
        {
            Categories = CategoryManager.GetAllCategories();
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
            return true;
        }

        private void SaveProduct(object obj)
        {
            _Product.Name = ProductNameText;
            _Product.Description = ProductDescriptionText;
            _Product.Category = SelectedCategory;
            _Product.Picture = Picture;
            _Product.UnitPrice = ProductUnitPriceText;
            _Product.UnitsInStock = ProductUnitsInStockText;

            ProductManager.UpdateProduct(_Product);

            CloseWindow.Invoke();

        }
    }
}
