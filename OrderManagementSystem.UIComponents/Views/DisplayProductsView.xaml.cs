using DevExpress.Xpf.Grid;
using OrderManagementSystem.UIComponents.ViewModels;

namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for DisplayProductsView.xaml
    /// </summary>
    public partial class DisplayProductsView : System.Windows.Controls.UserControl
    {
        public DisplayProductsView()
        {
            InitializeComponent();

            ProductViewModel productViewModel = new ProductViewModel();
            DataContext = productViewModel;

            TableView tableView = ProductGrid.View as TableView;
            tableView.AllowEditing = false;
        }
    }
}
