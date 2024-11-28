using DevExpress.Xpf.Core;
using OrderManagementSystem.Cache.Models;
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


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for EditOrderView.xaml
    /// </summary>
    public partial class EditOrderView : ThemedWindow
    {
        public EditOrderView()
        {
            InitializeComponent();
            OrderStatusComboBox.ItemsSource = Enum.GetValues(typeof(Order.OrderStatus));


            //EditOrderViewModel editOrderViewModel = new EditOrderViewModel();
            //this.DataContext = editOrderViewModel;
        }
    }
}
