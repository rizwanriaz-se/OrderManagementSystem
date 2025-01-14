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
            m_objEditUserViewModel.CloseWindow = this.Close;

            ApprovalStatusComboBox.ItemsSource = Enum.GetValues(typeof(User.ApprovalStates));

        }

        public void LoadUser(User SelectedUser)
        {
            m_objEditUserViewModel.Id = SelectedUser.Id;
            m_objEditUserViewModel.UserNameText = SelectedUser.Name;
            m_objEditUserViewModel.UserEmailText = SelectedUser.Email;
            m_objEditUserViewModel.UserPhoneText = SelectedUser.Phone;
            m_objEditUserViewModel.UserIsArchived = SelectedUser.IsArchived;
            m_objEditUserViewModel.UserIsAdmin = SelectedUser.IsAdmin;
            m_objEditUserViewModel.UserApprovalStatus = SelectedUser.ApprovalStatus;
        }
    }
}
