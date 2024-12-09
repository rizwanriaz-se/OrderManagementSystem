using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
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
    public partial class DisplayOrdersView : UserControl
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
                        Background = Brushes.OrangeRed,
                    }
                },
                new FormatCondition() {
                    Expression = "[Status] == 'Delivered'",
                    FieldName = "Status",
                    Format = new Format() {
                        Background = Brushes.LightGreen
                    }
                },
                new FormatCondition() {
                    Expression = "[Status] == 'Shipped'",
                    FieldName = "Status",
                    Format = new Format() {
                        Background = Brushes.Yellow
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


        //        public void GeneratePanel()
        //{
        //    //LayoutPanel panel = new LayoutPanel();
        //    //panel.Caption = "Panel 1";
        //    //panel.AllowDock = true;

        //    //DockPanel dockPanel = new DockPanel();
        //    //GridControl gridControl = new GridControl();
        //    //gridControl.ItemsSource = Orders;
        //    //gridControl.Height = 300;
        //    //gridControl.SelectedItem = SelectedOrder;
        //    //gridControl.View = new TableView { DetailHeaderContent = "Products", ShowGroupPanel = false };
        //    //gridControl.ContextMenu = new ContextMenu();
        //    //MenuItem editOrderMenuItem = new MenuItem { Header = "Edit Order" };
        //    //editOrderMenuItem.Command = EditOrderCommand;
        //    //MenuItem deleteOrderMenuItem = new MenuItem { Header = "Delete Order" };
        //    //deleteOrderMenuItem.Command = DeleteOrderCommand;
        //    //deleteOrderMenuItem.CommandParameter = gridControl.SelectedItem;
        //    //gridControl.ContextMenu.Items.Add(editOrderMenuItem);
        //    //gridControl.ContextMenu.Items.Add(deleteOrderMenuItem);
        //    //gridControl.DetailDescriptor = new DataControlDetailDescriptor { ItemsSourceBinding = new Binding("OrderDetails") };

        //    //GridControl gridControl2 = new GridControl();
        //    //gridControl2.View = new TableView { AutoWidth = true, ShowGroupPanel = false };
        //    //gridControl2.Columns.Add(new GridColumn { FieldName = "Product.Name", Header = "Product Name" });
        //    //gridControl2.Columns.Add(new GridColumn { FieldName = "Quantity", Header = "Quantity" });

        //    //gridControl.DetailDescriptor = gridControl2;
        //    var layoutPanel = new LayoutPanel
        //    {
        //        Caption = "Panel 1",
        //        AllowDock = true
        //    };

        //    // Create the DockPanel
        //    var dockPanel = new DockPanel();

        //    // Create the GridControl
        //    var gridControl = new GridControl
        //    {
        //        Height = 300,
        //        ItemsSource = new Binding("Orders"),
        //        SelectedItem = new Binding("SelectedOrder")
        //    };

        //    // Create the TableView
        //    var tableView = new TableView
        //    {
        //        DetailHeaderContent = "Products",
        //        ShowGroupPanel = false
        //    };
        //    gridControl.View = tableView;

        //    // Create ContextMenu
        //    var contextMenu = new ContextMenu();

        //    var editMenuItem = new MenuItem
        //    {
        //        Header = "Edit Order",
        //        Command = m_OrderViewModel.EditOrderCommand
        //    };

        //    var deleteMenuItem = new MenuItem
        //    {
        //        Header = "Delete Order",
        //        Command = m_OrderViewModel.DeleteOrderCommand,
        //        CommandParameter = new Binding("SelectedItem")
        //        {
        //            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(GridControl), 1)
        //        }
        //    };

        //    contextMenu.Items.Add(editMenuItem);
        //    contextMenu.Items.Add(deleteMenuItem);
        //    gridControl.ContextMenu = contextMenu;

        //    // Create DetailDescriptor
        //    var detailGridControl = new GridControl();
        //    var detailTableView = new TableView
        //    {
        //        AutoWidth = true,
        //        ShowGroupPanel = false
        //    };
        //    detailGridControl.View = detailTableView;

        //    detailGridControl.Columns.Add(new GridColumn { FieldName = "Product.Name", Header = "Product Name" });
        //    detailGridControl.Columns.Add(new GridColumn { FieldName = "Quantity", Header = "Quantity" });

        //    var detailDescriptor = new DataControlDetailDescriptor
        //    {
        //        ItemsSourceBinding = new Binding("OrderDetails"),
        //        DataControl = detailGridControl
        //    };
        //    gridControl.DetailDescriptor = detailDescriptor;

        //    // Add Columns to the GridControl
        //    gridControl.Columns.Add(new GridColumn { FieldName = "OrderID", Header = "ID", Width = 30, AllowResizing = DevExpress.Utils.DefaultBoolean.False });
        //    gridControl.Columns.Add(new GridColumn { FieldName = "Employee", Binding = new Binding("User.Name"), Header = "Employee", Width = 130, AllowResizing = DevExpress.Utils.DefaultBoolean.False });
        //    gridControl.Columns.Add(new GridColumn { FieldName = "OrderDate", Binding = new Binding("OrderDate"), Header = "Order Date", Width = new GridColumnWidth(3.0, GridColumnUnitType.Star) });
        //    gridControl.Columns.Add(new GridColumn { FieldName = "Status", Binding = new Binding("Status"), Header = "Status", Width = new GridColumnWidth(2.0, GridColumnUnitType.Star), HorizontalHeaderContentAlignment = HorizontalAlignment.Center });
        //    gridControl.Columns.Add(new GridColumn { FieldName = "ShippingDate", Binding = new Binding("ShippedDate"), Header = "Shipped Date", Width = new GridColumnWidth(3.0, GridColumnUnitType.Star) });
        //    gridControl.Columns.Add(new GridColumn { FieldName = "ShippingAddress", Binding = new Binding("ShippingAddress"), Header = "Shipping Address", Width = new GridColumnWidth(5.0, GridColumnUnitType.Star) });

        //    // Add GridControl to DockPanel
        //    dockPanel.Children.Add(gridControl);

        //    // Set DockPanel as the content of LayoutPanel
        //    layoutPanel.Content = dockPanel;

        //    // Add LayoutPanel to your LayoutGroup or DockLayoutManager
        //    var dockLayoutManager = new DockLayoutManager();
        //    var layoutGroup = new LayoutGroup { Caption = "LayoutRoot" };
        //    layoutGroup.Add(layoutPanel);
        //    dockLayoutManager.LayoutRoot = layoutGroup;

        //    // Add DockLayoutManager to the Window
        //    this.Content = dockLayoutManager;
        //}


    }
}
