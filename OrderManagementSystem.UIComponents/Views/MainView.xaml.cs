using DevExpress.Mvvm;
using DevExpress.Xpf.Docking;
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
        private DocumentPanel? orderPanel;
        private DocumentPanel? categoryPanel;
        private DocumentPanel? productPanel;
        private DocumentPanel? userPanel;
        private Dictionary<string, int> panelCount = new Dictionary<string, int>()
        {
            { "Order", 0 },
            { "Category", 0 },
            { "Product", 0 },
            { "User", 0 },
        };

        public MainView()
        {
            InitializeComponent();

            mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;

            AddPanel(ref orderPanel, "Order");
        }

        public User? CurrentUser { get; }

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

        private readonly Dictionary<string, List<DocumentPanel>> activePanels = new();

        private void AddPanel(ref DocumentPanel? docPanel, string panelCaption)
        {
            if (!activePanels.ContainsKey(panelCaption))
            {
                activePanels[panelCaption] = new List<DocumentPanel>();
            }

            int panelLimit = panelCaption == "Order" ? 3 : 1;

            if (activePanels[panelCaption].Count < panelLimit)
            {
                docPanel = new DocumentPanel
                {
                    Caption = panelCaption,
                    Content = CreateViewForPanel(panelCaption)
                };
                documentGroup.Add(docPanel);
                activePanels[panelCaption].Add(docPanel);
            }
            else
            {
                var panelToSelect = documentGroup.Items
                    .OfType<DocumentPanel>()
                    .FirstOrDefault(panel => string.Equals(panel.Caption.ToString(), panelCaption, StringComparison.Ordinal));

                if (panelToSelect != null)
                {
                    documentGroup.SelectedTabIndex = documentGroup.Items.IndexOf(panelToSelect);
                }
            }
        }

        private object CreateViewForPanel(string panelCaption)
        {
            return panelCaption switch
            {
                "Order" => new DisplayOrdersView(),
                "Category" => new DisplayCategoriesView(),
                "Product" => new DisplayProductsView(),
                "User" => new DisplayUsersView(),
                _ => throw new ArgumentException("Invalid panel caption", nameof(panelCaption)),
            };
        }
    }
}
