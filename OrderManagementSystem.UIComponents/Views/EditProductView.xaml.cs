using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystemServer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for EditProductView.xaml
    /// </summary>
    public partial class EditProductView : ThemedWindow
    {
        EditProductViewModel editProductViewModel = null;
        public EditProductView()
        {
            InitializeComponent();

            editProductViewModel = new EditProductViewModel();
            DataContext = editProductViewModel;
            editProductViewModel.CloseWindow = this.Close;

        }

        public void LoadProduct(Product SelectedProduct)
        {
            //if (SelectedProduct == null) throw new ArgumentNullException(nameof(SelectedProduct));

            editProductViewModel.Id = SelectedProduct.Id;
            editProductViewModel.ProductNameText = SelectedProduct.Name;
            editProductViewModel.ProductDescriptionText = SelectedProduct.Description;
            editProductViewModel.ProductUnitPriceText = SelectedProduct.UnitPrice;
            editProductViewModel.SelectedCategory = editProductViewModel.Categories.FirstOrDefault(c => c.Id == SelectedProduct.Category.Id);
            editProductViewModel.Picture = SelectedProduct.Picture;
            editProductViewModel.ProductUnitsInStockText = SelectedProduct.UnitsInStock;
        }
    }
}
