using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Drawing.Internal.Fonts.Interop;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Grid;
using OrderManagementSystem.UIComponents.ViewModels;

namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for DisplayOrdersView.xaml
    /// </summary>
    public partial class DisplayOrdersView : System.Windows.Controls.UserControl
    {

        private OrderViewModel m_OrderViewModel;
        public DisplayOrdersView()
        {
            InitializeComponent();

            // Set the data context of the view to the view model
            m_OrderViewModel = new OrderViewModel();
            this.DataContext = m_OrderViewModel;

            TableView tableView = OrderGrid.View as TableView;
            tableView.AllowEditing = false;


            tableView.FormatConditions.AddRange(new List<FormatConditionBase> {

                new FormatCondition() {
                    Expression = "[Status] == 'Pending'",
                    FieldName = "Status",
                    Format = new Format() {
                        Background = System.Windows.Media.Brushes.OrangeRed,
                    }
                },
                new FormatCondition() {
                    Expression = "[Status] == 'Delivered'",
                    FieldName = "Status",
                    Format = new Format() {
                        Background = System.Windows.Media.Brushes.LightGreen
                    }
                },
                new FormatCondition() {
                    Expression = "[Status] == 'Shipped'",
                    FieldName = "Status",
                    Format = new Format() {
                        Background = System.Windows.Media.Brushes.Yellow
                    }
                },



            });


        }

        private void OrderGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //var grid = sender as GridControl;
            TableView tableView = OrderGrid.View as TableView;
            var hitInfo = tableView.CalcHitInfo(e.OriginalSource as DependencyObject);

            if (hitInfo == null || hitInfo.RowHandle == GridControl.InvalidRowHandle)
            {
                Keyboard.ClearFocus();
                return;
            }
        }

    }
}
