using DevExpress.Xpf.Grid;
using OrderManagementSystemServer.Repository;
using OrderManagementSystem.UIComponents.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OrderManagementSystem.UIComponents.Classes;

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
            if (!isAdminUser)
            {
                tableView.AllowEditing = false;
            }
            //tableView.AllowEditing = false;

            ApprovalStatusComboBox.ItemsSource = Enum.GetValues(typeof(User.ApprovalStates));
        }
    }
}
