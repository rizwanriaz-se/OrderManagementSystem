using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystemServer.Repository;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for EditUserView.xaml
    /// </summary>
    public partial class EditUserView : ThemedWindow
    {
        private EditUserViewModel m_objEditUserViewModel = null;
        public EditUserView()
        {
            InitializeComponent();

            m_objEditUserViewModel = new EditUserViewModel();
            DataContext = m_objEditUserViewModel;
            this.Owner = System.Windows.Application.Current.MainWindow;
            m_objEditUserViewModel.CloseWindow = this.Close;
        }

        public void LoadUser(User SelectedUser)
        {

            m_objEditUserViewModel.Id = SelectedUser.Id;
            m_objEditUserViewModel.UserNameText = SelectedUser.Name;
            m_objEditUserViewModel.UserEmailText = SelectedUser.Email;
            m_objEditUserViewModel.UserPhoneText = SelectedUser.Phone;
            //m_objEditUserViewModel.UserPasswordText = SelectedUser.Password;
            m_objEditUserViewModel.UserIsAdmin = SelectedUser.IsAdmin;
        }
    }
}
