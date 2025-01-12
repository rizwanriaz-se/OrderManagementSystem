using DevExpress.Xpf.Core;
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
            DataContext = addOrderViewModel;

        }
    }
}
