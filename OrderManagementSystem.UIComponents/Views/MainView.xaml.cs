﻿using DevExpress.Xpf.Docking;
using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystemServer.Repository;

//using OrderManagementSystem.UIComponents.ViewModels;
//using OrderManagementSystem.UIComponents.Views;
//using OrderManagementSystem.ViewModels;

namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for View1.xaml
    /// </summary>
    public partial class MainView : System.Windows.Controls.UserControl
    {
        private MainViewModel mainViewModel;
        private DocumentPanel orderPanel;
        private DocumentPanel categoryPanel;
        private DocumentPanel productPanel;
        private DocumentPanel userPanel;
        //private int orderPanelCount = 0;
        //private int categoryPanelCount = 0;
        //private int productPanelCount = 0;
        //private int userPanelCount = 0;
        Dictionary<string, int> panelCount = new Dictionary<string, int>()
        {
            { "Order", 0 },
            { "Category", 0 },
            { "Product", 0 },
            { "User", 0 },
        };


        public MainView()
        {
            InitializeComponent();

            // Set the DataContext of the view to the ViewModel
            mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;

            //AddPanel(ref orderPanel, "Order");
            //orderPanel = new DocumentPanel();
            //orderPanel.Content = mainViewModel.CurrentView;
            //orderPanel.Caption = "Order";
            //documentGroup.Add(orderPanel);
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
                
                //orderPanelCount++;
                //AddPanel(ref orderPanel, orderPanelCount, "Order");
            }
            else if (selectedPage.Name == "CategoryPage")
            {
                mainViewModel.SwitchToCategoriesView();
                //categoryPanelCount++;
                //AddPanel(ref categoryPanel, categoryPanelCount, "Category");
            }
            else if (selectedPage.Name == "UserPage")
            {
                
                mainViewModel.SwitchToUsersView();
                //userPanelCount++;
                //AddPanel(ref userPanel, userPanelCount, "User");
            }
            else if (selectedPage.Name == "ProductPage")
            {
                
                mainViewModel.SwitchToProductsView();
                //productPanelCount++;
                //AddPanel(ref productPanel, productPanelCount, "Product");
            }
        }

        private void AddOrderBlotter_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddPanel(ref orderPanel, "Order");
        }
        private void AddCategoryBlotter_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddPanel(ref categoryPanel, "Category");
        }
        private void AddProductBlotter_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddPanel(ref productPanel, "Product");
        }
        private void AddUserBlotter_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddPanel(ref userPanel, "User");
        }

        //private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{

        //}

        private readonly Dictionary<string, List<DocumentPanel>> activePanels = new();

        private void AddPanel(ref DocumentPanel docPanel, string panelCaption)
        {
            if (!activePanels.ContainsKey(panelCaption))
            {
                activePanels[panelCaption] = new List<DocumentPanel>();
            }

            if (activePanels[panelCaption].Count < 3)
            {
                //docPanel = new DocumentPanel
                //{
                //    Caption = $"{panelCaption} {activePanels[panelCaption].Count + 1}",
                //    Content = mainViewModel.CurrentView
                //};
                docPanel = new DocumentPanel
                {
                    Caption = panelCaption,
                    Content = CreateViewForPanel(panelCaption)
                };
                documentGroup.Add(docPanel);
                activePanels[panelCaption].Add(docPanel);
            }
        }
        private object CreateViewForPanel(string panelCaption)
        {
            switch (panelCaption)
            {
                case "Order":
                    return new DisplayOrdersView();
                case "Category":
                    return new DisplayCategoriesView();
                case "Product":
                    return new DisplayProductsView();
                case "User":
                    return new DisplayUsersView();
                default:
                    return null; // Or throw an exception if this case shouldn't happen
            }
        }


    }
}
