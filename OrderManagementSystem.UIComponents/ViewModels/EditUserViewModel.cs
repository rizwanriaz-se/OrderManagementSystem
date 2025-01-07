using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditUserViewModel : INotifyDataErrorInfo
    {
        private User m_objUser;

        private string m_stUserNameText;
        private string m_stUserEmailText;
        private string m_stUserPhoneText;
        private string m_stUserPasswordText;

        public Action CloseWindow { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string UserNameText
        {

            get { return m_stUserNameText; }
            set
            {
                m_stUserNameText = value;
                Validate(nameof(UserNameText), m_stUserNameText);
            }
        }

        [Required(ErrorMessage = "Email is required")]
        public string UserEmailText
        {
            get { return m_stUserEmailText; }
            set
            {
                m_stUserEmailText = value;
                Validate(nameof(UserEmailText), m_stUserEmailText);
            }
        }

        [Required(ErrorMessage = "Phone Number is required")]
        public string UserPhoneText
        {

            get { return m_stUserPhoneText; }
            set
            {
                m_stUserPhoneText = value;
                Validate(nameof(UserPhoneText), m_stUserPhoneText);
            }
        }

        [Required(ErrorMessage = "Password is required")]
        public string UserPasswordText
        {

            get { return m_stUserPasswordText; }
            set
            {
                m_stUserPasswordText = value;
                Validate(nameof(UserPasswordText), m_stUserPasswordText);
            }
        }

        public void Validate(string propertyName, object propertyValue)
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
            SaveUserCommand.RaiseCanExecuteEventChanged();
        }

        public bool UserIsAdmin { get; set; }

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

        public RelayCommand SaveUserCommand { get; set; }

        public EditUserViewModel()
        {
            SaveUserCommand = new RelayCommand(SaveUser, CanSaveUser);
        }

        private bool CanSaveUser(object obj)
        {
            return Validator.TryValidateObject(this, new ValidationContext(this), null, true);
        }

        private void SaveUser(object obj)
        {
            m_objUser = new User();
            m_objUser.Id = Id;
            m_objUser.Name = UserNameText;
            m_objUser.Email = UserEmailText;
            m_objUser.Phone = UserPhoneText;
            m_objUser.Password = UserPasswordText;
            m_objUser.IsAdmin = UserIsAdmin;

            MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Update, m_objUser);

            CloseWindow.Invoke();

        }
    }
}
