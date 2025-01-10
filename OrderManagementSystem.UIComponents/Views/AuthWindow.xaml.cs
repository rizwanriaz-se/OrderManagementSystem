using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;

using OrderManagementSystem.UIComponents.ViewModels;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : ThemedWindow
    {
        public Action CloseWindow { get; set; }
     
        public AuthWindow()
        {
            InitializeComponent();
         

            // Set the DataContext of the window to the ViewModel
            AuthViewModel authViewModel = new AuthViewModel();
            authViewModel.CloseWindow = this.Close;
            this.DataContext = authViewModel;
        }

        
    }
}
