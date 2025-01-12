using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.ViewModels;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for AddCategoryView.xaml
    /// </summary>
    public partial class AddCategoryView : ThemedWindow
    {
        public AddCategoryView()
        {
            InitializeComponent();

            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.CloseWindow = this.Close;
            this.DataContext = categoryViewModel;

        }

    }
}
