using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.ViewModels;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for AddProductView.xaml
    /// </summary>
    public partial class AddProductView : ThemedWindow
    {
        public AddProductView()
        {
            InitializeComponent();
            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.CloseWindow = this.Close;
            DataContext = productViewModel;
        }
    }
}
