using DevExpress.Xpf.Ribbon;
using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystem.UIComponents.Views;
using OrderManagementSystem.ViewModels;
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

namespace OrderManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for View1.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private MainViewModel mainViewModel;

        public MainView()
        {
            InitializeComponent();

            // Set the DataContext of the view to the ViewModel
            mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;
        }

        public User CurrentUser { get; }

        private void RibbonControl_SelectedPageChanged(object sender, DevExpress.Xpf.Ribbon.RibbonPropertyChangedEventArgs e)
        {
            var selectedPage = Ribbon.SelectedPage;
            if (selectedPage.Name == "OrderPage")
            {
                mainViewModel.SwitchToOrdersView();
            }
            else if (selectedPage.Name == "CategoryPage")
            {
                mainViewModel.SwitchToCategoriesView();
            }
            else if (selectedPage.Name == "UserPage")
            {
                mainViewModel.SwitchToUsersView();
            }
            else if (selectedPage.Name == "ProductPage")
            {
                mainViewModel.SwitchToProductsView();
            }
        }
    }
}
