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
using DevExpress.Xpf.Core;
using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.UIComponents.ViewModels;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for AddOrderView.xaml
    /// </summary>
    public partial class AddOrderView : ThemedWindow
    {
        public AddOrderView()
        {
            InitializeComponent();

            AddOrderViewModel addOrderViewModel = new AddOrderViewModel();
            addOrderViewModel.CloseWindow = this.Close;
            //OrderStatusComboBox.ItemsSource = Enum.GetValues(typeof(Order.OrderStatus));
            this.DataContext = addOrderViewModel;
            
            //ProductListComboBox.ItemsSource = addOrderViewModel.AllProducts;


        }
    }
}
