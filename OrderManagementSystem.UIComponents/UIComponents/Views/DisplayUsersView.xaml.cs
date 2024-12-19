using DevExpress.Xpf.Grid;
using OrderManagementSystem.Repositories.Repositories;
using OrderManagementSystem.UIComponents.UIComponents.ViewModels;
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

namespace OrderManagementSystem.UIComponents.UIComponents.Views
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
            //tableView.AllowEditing = false;

            ApprovalStatusComboBox.ItemsSource = Enum.GetValues(typeof(User.ApprovalStates));
        }
    }
}
