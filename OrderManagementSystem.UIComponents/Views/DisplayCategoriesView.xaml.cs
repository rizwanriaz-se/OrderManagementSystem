using DevExpress.Xpf.Grid;
using OrderManagementSystem.UIComponents.Classes;
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
            DataContext = categoryViewModel;

            TableView tableView = CategoryGrid.View as TableView;
            tableView.AllowEditing = false;

            
            editRowItem.IsVisible = GUIHandler.Instance.CurrentUser.IsAdmin;
            deleteRowItem.IsVisible = GUIHandler.Instance.CurrentUser.IsAdmin;
        }
    }
}
