using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Grid;
using OrderManagementSystem.UIComponents.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for DisplayOrdersView.xaml
    /// </summary>
    public partial class DisplayOrdersView : System.Windows.Controls.UserControl
    {

        private OrderViewModel m_objOrderViewModel;
        public DisplayOrdersView()
        {
            InitializeComponent();

            m_objOrderViewModel = new OrderViewModel();
            DataContext = m_objOrderViewModel;

            TableView tableView = OrderGrid.View as TableView;
            tableView.AllowEditing = false;


            tableView.FormatConditions.AddRange(new List<FormatConditionBase> {

                new FormatCondition() {
                    Expression = "[Status] == 'Pending'",
                    FieldName = "Status",
                    Format = new Format() {
                            Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, 255, 69, 0)) 
}
                },
                new FormatCondition() {
                    Expression = "[Status] == 'Delivered'",
                    FieldName = "Status",
                    Format = new Format() {
                        Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, 144, 238, 0))
}

                },
                new FormatCondition() {
                    Expression = "[Status] == 'Shipped'",
                    FieldName = "Status",
                    Format = new Format() {
 
                     
Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(64, 255, 255, 0))
}
                },
                    


            });


        }


    }
}
