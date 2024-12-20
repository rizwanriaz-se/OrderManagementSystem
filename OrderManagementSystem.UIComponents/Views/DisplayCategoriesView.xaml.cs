using DevExpress.Xpf.Grid;
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
