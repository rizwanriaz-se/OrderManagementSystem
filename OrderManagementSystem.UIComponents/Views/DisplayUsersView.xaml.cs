using DevExpress.Xpf.Grid;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystemServer.Repository;

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
            this.DataContext = userViewModel;

            TableView tableView = UserGrid.View as TableView;
            bool isAdminUser = GUIHandler.Instance.CurrentUser.IsAdmin;
            //if (!isAdminUser)
            //{
            //    tableView.AllowEditing = false;
            //}
            tableView.AllowEditing = false;

            //ApprovalStatusComboBox.ItemsSource = Enum.GetValues(typeof(User.ApprovalStates));
        }
    }
}
