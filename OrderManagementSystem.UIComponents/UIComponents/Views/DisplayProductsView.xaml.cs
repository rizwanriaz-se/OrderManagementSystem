using DevExpress.Xpf.Grid;
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
    /// Interaction logic for DisplayProductsView.xaml
    /// </summary>
    public partial class DisplayProductsView : System.Windows.Controls.UserControl
    {
        public DisplayProductsView()
        {
            InitializeComponent();

            ProductViewModel productViewModel = new ProductViewModel();
            this.DataContext = productViewModel;

            TableView tableView = ProductGrid.View as TableView;
            tableView.AllowEditing = false;
        }
    }
}
