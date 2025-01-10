using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystemServer.Repository;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for EditProductView.xaml
    /// </summary>
    public partial class EditProductView : ThemedWindow
    {
        private EditProductViewModel m_objEditProductViewModel = null;
        public EditProductView()
        {
            InitializeComponent();

            m_objEditProductViewModel = new EditProductViewModel();
            DataContext = m_objEditProductViewModel;
            this.Owner = System.Windows.Application.Current.MainWindow;
            m_objEditProductViewModel.CloseWindow = this.Close;

        }

        public void LoadProduct(Product SelectedProduct)
        {
            m_objEditProductViewModel.Id = SelectedProduct.Id;
            m_objEditProductViewModel.ProductNameText = SelectedProduct.Name;
            m_objEditProductViewModel.ProductDescriptionText = SelectedProduct.Description;
            m_objEditProductViewModel.ProductUnitPriceText = SelectedProduct.UnitPrice;
            m_objEditProductViewModel.SelectedCategory = m_objEditProductViewModel.Categories.FirstOrDefault(c => c.Id == SelectedProduct.Category.Id);
            //m_objEditProductViewModel.Picture = SelectedProduct.Picture;
            m_objEditProductViewModel.ProductUnitsInStockText = SelectedProduct.UnitsInStock;
        }
    }
}
