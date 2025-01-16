using DevExpress.Xpf.Docking;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystemServer.Repository;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for View1.xaml
    /// </summary>
    public partial class MainView : System.Windows.Controls.UserControl
    {
        private MainViewModel m_objMainViewModel;
        private DocumentPanel? m_objOrderPanel;
        private DocumentPanel? m_objCategoryPanel;
        private DocumentPanel? m_objProductPanel;
        private DocumentPanel? m_objUserPanel;
        

        public MainView()
        {
            InitializeComponent();

            m_objMainViewModel = new MainViewModel();
            DataContext = m_objMainViewModel;

            AddPanel(ref m_objOrderPanel, "Order");


            if (activePanels.Count == 1)
            {
                var singlePanel = activePanels.Values.FirstOrDefault();
                if (singlePanel != null && singlePanel.Count == 1)
                {
                    singlePanel[0].AllowClose = false;
                    
                    //singlePanel[0].CloseCommand
                }
            }
        }

        public User? CurrentUser { get; }

        private void AddOrderBlotter_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddPanel(ref m_objOrderPanel, "Order");
        }
        private void AddCategoryBlotter_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddPanel(ref m_objCategoryPanel, "Category");
        }
        private void AddProductBlotter_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddPanel(ref m_objProductPanel, "Product");
        }
        private void AddUserBlotter_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddPanel(ref m_objUserPanel, "User");
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
                
                docPanel.CloseCommand = new RelayCommand(OnPanelCloseCommand);
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

        private void OnPanelCloseCommand(object param)
        {
            DocumentPanel documentPanel = param as DocumentPanel;
            string panelCaption = documentPanel.Caption as string;

            MyDockLayoutManager.DockController.Close(documentPanel);
            activePanels[panelCaption].Remove(documentPanel);
        }

        private object CreateViewForPanel(string panelCaption)
        {
            return panelCaption switch
            {
                "Order" => new DisplayOrdersView(),
                "Category" => new DisplayCategoriesView(),
                "Product" => new DisplayProductsView(),
                "User" => new DisplayUsersView(),
            };
        }
    }
}
