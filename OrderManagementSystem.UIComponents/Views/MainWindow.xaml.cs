using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.ViewModels;

namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {

        public MainWindow()
        {
            InitializeComponent();

            AuthViewModel authViewModel = new AuthViewModel();
            this.DataContext = authViewModel;

        }

    }
}
