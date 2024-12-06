using DevExpress.Drawing.Internal.Fonts.Interop;
using DevExpress.Mvvm;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Ribbon;
using Microsoft.VisualBasic;
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
        private DocumentPanel orderPanel;
        private DocumentPanel categoryPanel;
        private DocumentPanel productPanel;
        private DocumentPanel userPanel;
        private int orderPanelCount = 0;
        private int categoryPanelCount = 0;
        private int productPanelCount = 0;
        private int userPanelCount = 0;



        public MainView()
        {
            InitializeComponent();

            // Set the DataContext of the view to the ViewModel
            mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;

            orderPanel = new DocumentPanel();
            orderPanel.Content = mainViewModel.CurrentView;
            orderPanel.Caption = "Order";
            documentGroup.Add(orderPanel);
        }

        public User CurrentUser { get; }

        private void RibbonControl_SelectedPageChanged(object sender, DevExpress.Xpf.Ribbon.RibbonPropertyChangedEventArgs e)
        {
            var selectedPage = Ribbon.SelectedPage;

            if (selectedPage.Name == "OrderPage")
            {
                //DocumentPanel orderPanel = new DocumentPanel();
                //orderPanel.Content = mainViewModel.CurrentView;
                //documentGroup.Add(orderPanel);
                mainViewModel.SwitchToOrdersView();
                orderPanelCount++;
                AddPanel(ref orderPanel, orderPanelCount, "Order");
            }
            else if (selectedPage.Name == "CategoryPage")
            {
                mainViewModel.SwitchToCategoriesView();
                categoryPanelCount++;
                AddPanel(ref categoryPanel, categoryPanelCount, "Category");
            }
            else if (selectedPage.Name == "UserPage")
            {
                
                mainViewModel.SwitchToUsersView();
                userPanelCount++;
                AddPanel(ref userPanel, userPanelCount, "User");
            }
            else if (selectedPage.Name == "ProductPage")
            {
                
                mainViewModel.SwitchToProductsView();
                productPanelCount++;
                AddPanel(ref productPanel, productPanelCount, "Product");
            }
        }

        //private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{

        //}

        private void AddPanel(ref DocumentPanel docPanel, int panelCount, string panelCaption)
        {
            //docPanel = new DocumentPanel();
            //docPanel.Content = mainViewModel.CurrentView;
            //docPanel.Caption = panelCaption;
            //documentGroup.Add(docPanel);

            //if (docPanel != null) return;
            if (panelCount <= 3)
            {
                docPanel = new DocumentPanel();
                docPanel.Caption = panelCaption;
                docPanel.Content = mainViewModel.CurrentView;


                documentGroup.Add(docPanel);
            }
        }
    }
}
