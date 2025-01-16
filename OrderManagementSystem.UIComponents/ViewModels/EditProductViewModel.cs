﻿using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditProductViewModel
    {
        public Action CloseWindow { get; set; }

        public ObservableCollection<Category> Categories { get; private set; }

        public int Id { get; set; }

        public string ProductNameText { get; set; }
        public string ProductDescriptionText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public Category SelectedCategory { get; set; }

        public decimal ProductUnitPriceText { get; set; }

        public int ProductUnitsInStockText { get; set; }


        public ICommand SaveProductCommand { get; set; }


        public EditProductViewModel()
        {
            Categories = GUIHandler.Instance.CacheManager.Categories;
            SaveProductCommand = new RelayCommand(SaveProduct);
        }

        private void SaveProduct(object obj)
        {
            ValidateInputs();

            Product product = new Product
            {
                Id = Id,
                Name = ProductNameText,
                Description = ProductDescriptionText,
                Category = SelectedCategory,
                UnitPrice = Convert.ToDecimal(ProductUnitPriceText),
                UnitsInStock = Convert.ToInt32(ProductUnitsInStockText)
            };

            GUIHandler.Instance.ClientManager.SendMessage(MessageType.Product, MessageAction.Update, product);

            CloseWindow.Invoke();

        }

        private void ValidateInputs()
        {
            if (ProductNameText == null)
            {
                DXMessageBox.Show("The product name cannot be empty.");
                return;
            }
            if (ProductDescriptionText == null)
            {
                DXMessageBox.Show("The product description cannot be empty.");
                return;
            }
            if (SelectedCategory == null)
            {
                DXMessageBox.Show("Category must be selected.");
                return;
            }
            if (ProductUnitPriceText == null || ProductUnitPriceText < 1)
            {
                DXMessageBox.Show("The product unit price must be greater than 0.");
                return;
            }
            if (ProductUnitsInStockText == null || ProductUnitsInStockText < 1)
            {
                DXMessageBox.Show("The product units in stock value must be greater than 0.");
                return;
            }
        }
    }
}
