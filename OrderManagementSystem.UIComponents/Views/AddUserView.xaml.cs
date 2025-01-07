using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.ViewModels;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for AddUserView.xaml
    /// </summary>
    public partial class AddUserView : ThemedWindow
    {
        public AddUserView()
        {
            InitializeComponent();

            UserViewModel userViewModel = new UserViewModel();
            userViewModel.CloseWindow = this.Close;
            this.Owner = System.Windows.Application.Current.MainWindow;
            this.DataContext = userViewModel;
        }
    }
}
