using DevExpress.Xpf.Core;
using OrderManagementSystemServer.Repository;
using OrderManagementSystem.UIComponents.ViewModels;
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
using System.Windows.Shapes;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using static OrderManagementSystemServer.Repository.Order;
using System.Collections.ObjectModel;
using System.Printing;
using System.Diagnostics;
using DevExpress.Xpf.Editors;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for EditOrderView.xaml
    /// </summary>
    public partial class EditOrderView : ThemedWindow
    {
        EditOrderViewModel editOrderViewModel;
        public EditOrderView()
        {
            InitializeComponent();
            editOrderViewModel = new EditOrderViewModel();
            DataContext = editOrderViewModel;
            editOrderViewModel.CloseWindow = this.Close;
        
        }
       
        public void LoadOrder(Order SelectedOrder)
        {
            if (SelectedOrder == null) throw new ArgumentNullException(nameof(SelectedOrder));

            editOrderViewModel.Id = SelectedOrder.Id;
            editOrderViewModel.User = SelectedOrder.User ?? throw new ArgumentNullException(nameof(SelectedOrder.User));
            editOrderViewModel.OrderDate = SelectedOrder.OrderDate;
            editOrderViewModel.SelectedShippingDate = SelectedOrder.ShippedDate;
            editOrderViewModel.SelectedStatus = SelectedOrder.Status;
            editOrderViewModel.SelectedShippingAddress = SelectedOrder.ShippingAddress ?? throw new ArgumentNullException(nameof(SelectedOrder.ShippingAddress));
            editOrderViewModel.OrderDetails = new ObservableCollection<OrderDetail>(SelectedOrder.OrderDetails.Select(od =>
                new OrderDetail
                {
                    Product =  editOrderViewModel.AllProducts.FirstOrDefault(p => p.Id == od.Product.Id),
                    Quantity = od.Quantity
                }));

            

            editOrderViewModel.OrderDetails.CollectionChanged += (s, e) => editOrderViewModel.SaveOrderCommand.RaiseCanExecuteEventChanged();
        }
    }
}
