using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Grid;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystemServer.Repository;
using System.Windows.Media;

namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for DisplayUsersView.xaml
    /// </summary>
    public partial class DisplayUsersView : System.Windows.Controls.UserControl
    {
        public DisplayUsersView()
        {
            InitializeComponent();

            UserViewModel userViewModel = new UserViewModel();
            DataContext = userViewModel;

            TableView tableView = UserGrid.View as TableView;
            bool isAdminUser = GUIHandler.Instance.CurrentUser.IsAdmin;
            
            tableView.AllowEditing = false;

        }
    }
}
