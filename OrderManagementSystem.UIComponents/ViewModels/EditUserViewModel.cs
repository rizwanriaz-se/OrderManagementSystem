using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditUserViewModel : INotifyDataErrorInfo
    {
        private User m_objUser;

        private string m_stUserNameText;
        private string m_stUserEmailText;
        private string m_stUserPhoneText;
        private string m_stUserPasswordText;
        private User.ApprovalStates m_objSelectedStatus;

        public Action CloseWindow { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string UserNameText
        {

            get { return m_stUserNameText; }
            set
            {
                m_stUserNameText = value;
            }
        }

        [Required(ErrorMessage = "Email is required")]
        public string UserEmailText
        {
            get { return m_stUserEmailText; }
            set
            {
                m_stUserEmailText = value;
            }
        }

        [Required(ErrorMessage = "User Appoval Status value is required")]
        public User.ApprovalStates UserApprovalStatus
        {
            get { return m_objSelectedStatus; }
            set
            {
                m_objSelectedStatus = value;
            }
        }

        [Required(ErrorMessage = "Phone Number is required")]
        public string UserPhoneText
        {

            get { return m_stUserPhoneText; }
            set
            {
                m_stUserPhoneText = value;
            }
        }

        [Required(ErrorMessage = "Password is required")]
        public string UserPasswordText
        {

            get { return m_stUserPasswordText; }
            set
            {
                m_stUserPasswordText = value;
            }
        }

        public bool Validate(string propertyName, object propertyValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyName };
            Validator.TryValidateProperty(propertyValue, context, results);

            if (results.Any())
            {
                Errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                Errors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

            return Errors.ContainsKey(propertyName);
        }

        public bool UserIsAdmin { get; set; }
        public bool UserIsArchived { get; set; }

        public bool HasErrors => Errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        public IEnumerable GetErrors(string propertyName)
        {
            if (Errors.ContainsKey(propertyName))
            {
                return Errors[propertyName];
            }
            return null;
        }

        public ICommand SaveUserCommand { get; set; }

        public EditUserViewModel()
        {
            SaveUserCommand = new RelayCommand(SaveUser);
        }


        private void SaveUser(object obj)
        {

            try
            {
                if (Validate(nameof(UserNameText), m_stUserNameText) || Validate(nameof(UserEmailText), m_stUserEmailText) || Validate(nameof(UserApprovalStatus), m_objSelectedStatus) || Validate(nameof(UserPhoneText), m_stUserPhoneText) || Validate(nameof(UserPasswordText), m_stUserPasswordText))
                {
                    var errors = Errors.SelectMany(e => e.Value);
                    throw new Exception(string.Join('\n', errors));
                }

                m_objUser = new User();
                m_objUser.Id = Id;
                m_objUser.Name = UserNameText;
                m_objUser.Email = UserEmailText;
                m_objUser.Phone = UserPhoneText;
                m_objUser.IsArchived = UserIsArchived;
                m_objUser.Password = UserPasswordText;
                m_objUser.IsAdmin = UserIsAdmin;
                m_objUser.ApprovalStatus = UserApprovalStatus;

                MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Update, m_objUser);

                CloseWindow.Invoke();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

        }
    }
}
