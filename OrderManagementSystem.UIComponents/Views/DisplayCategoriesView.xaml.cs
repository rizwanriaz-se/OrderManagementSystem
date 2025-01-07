using DevExpress.Xpf.Grid;
using OrderManagementSystem.UIComponents.ViewModels;

namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for DisplayCategoriesView.xaml
    /// </summary>
    public partial class DisplayCategoriesView : System.Windows.Controls.UserControl
    {
        public DisplayCategoriesView()
        {
            InitializeComponent();

            CategoryViewModel categoryViewModel = new CategoryViewModel();
            this.DataContext = categoryViewModel;

            TableView tableView = CategoryGrid.View as TableView;
            tableView.AllowEditing = false;
        }
    }
}
