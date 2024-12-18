using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.XtraRichEdit.Model.History;
using OrderManagementSystem.Cache;
using OrderManagementSystem.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Controls;
using System.Windows.Input;
using static OrderManagementSystem.Repositories.Repositories.User;
using OrderManagementSystem.Repositories.Repositories;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class Request
    {
        public string Type { get; set; }
        public string Data { get; set; }
    }
    public class AuthViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public Action CloseWindow { get; set; }

        public string NameRegisterText { get; set; }

        private int m_nSelectedTabIndex;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private string m_stSelectedRole;
        private bool m_bIsRegisterTabVisible;

        public List<string> Roles { get; } = new List<string> { "Admin", "Employee" };

        //[Required(ErrorMessage = "Role must be selected.")]
        public string SelectedRole
        {
            get => m_stSelectedRole;
            set
            {
                m_stSelectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
                IsRegisterTabVisible = m_stSelectedRole == "Employee";
                //ValidateLogin(nameof(SelectedRole), m_stSelectedRole);
            }
        }

        public bool IsRegisterTabVisible
        {
            get => m_bIsRegisterTabVisible;
            set
            {
                m_bIsRegisterTabVisible = value;
                OnPropertyChanged(nameof(IsRegisterTabVisible));
            }
        }

        public int SelectedTabIndex
        {
            get { return m_nSelectedTabIndex; }
            set { m_nSelectedTabIndex = value; OnPropertyChanged(nameof(SelectedTabIndex)); }
        }


        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string m_stEmailLoginText;
        private string m_stPasswordLoginText;
        private string m_stEmailRegisterText;
        private string m_stPhoneRegisterText;
        private string m_stPasswordRegisterText;

        [Required(ErrorMessage = "Email is required")]
        public string EmailLoginText
        {
            get { return m_stEmailLoginText; }
            set
            {
                m_stEmailLoginText = value;
                OnPropertyChanged(nameof(EmailLoginText));
                ValidateLogin(nameof(EmailLoginText), m_stEmailLoginText);
                CommandManager.InvalidateRequerySuggested(); // Updates command state

            }
        }

        [Required(ErrorMessage = "Password is required")]
        public string PasswordLoginText
        {
            get { return m_stPasswordLoginText; }
            set
            {
                m_stPasswordLoginText = value;
                OnPropertyChanged(nameof(PasswordLoginText));
                ValidateLogin(nameof(PasswordLoginText), m_stPasswordLoginText);
                CommandManager.InvalidateRequerySuggested(); // Updates command state

            }
        }

        [Required(ErrorMessage = "Email is required")]
        public string EmailRegisterText
        {
            get { return m_stEmailRegisterText; }
            set
            {
                m_stEmailRegisterText = value;
                OnPropertyChanged(nameof(EmailRegisterText));
                ValidateRegister(nameof(EmailRegisterText), m_stEmailRegisterText);
            }
        }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneRegisterText
        {
            get { return m_stPhoneRegisterText; }
            set
            {
                m_stPhoneRegisterText = value;
                OnPropertyChanged(nameof(PhoneRegisterText));
                ValidateRegister(nameof(PhoneRegisterText), m_stPhoneRegisterText);
            }
        }

        [Required(ErrorMessage = "Password is required")]
        public string PasswordRegisterText
        {
            get { return m_stPasswordRegisterText; }
            set
            {
                m_stPasswordRegisterText = value;
                OnPropertyChanged(nameof(PasswordRegisterText));
                ValidateRegister(nameof(PasswordRegisterText), m_stPasswordRegisterText);
            }
        }

        //public string EmailRegisterText { get; set; }
        //public string PhoneRegisterText { get; set; }
        //public string PasswordRegisterText { get; set; }

        private User m_User;

        Dictionary<string, List<string>> LoginErrors = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> RegisterErrors = new Dictionary<string, List<string>>();

        public bool HasErrors => LoginErrors.Count > 0 || RegisterErrors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (SelectedTabIndex == 0 && LoginErrors.ContainsKey(propertyName))
            {
                return LoginErrors[propertyName];
            }
            else if (SelectedTabIndex == 1 && RegisterErrors.ContainsKey(propertyName))
            {
                return RegisterErrors[propertyName];
            }
            return null;
        }

        public void ValidateRegister(string propertyName, object propertyValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyName };
            Validator.TryValidateProperty(propertyValue, context, results);

            if (results.Any())
            {
                RegisterErrors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                RegisterErrors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RegisterUserCommand.RaiseCanExecuteEventChanged();
        }

        public void ValidateLogin(string propertyName, object propertyValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyName };
            Validator.TryValidateProperty(propertyValue, context, results);

            if (results.Any())
            {
                LoginErrors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                LoginErrors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            LoginUserCommand.RaiseCanExecuteEventChanged();
        }

        public User CurrentUser {
            get { return m_User; }
            set { m_User = value; OnPropertyChanged(nameof(CurrentUser)); }
        }
        public bool RoleChecked { get; set; }
        public RelayCommand RegisterUserCommand { get; set; }
        public RelayCommand LoginUserCommand { get; set; }

        //public bool HasErrors => throw new NotImplementedException();

        public AuthViewModel() {
            RegisterUserCommand = new RelayCommand(RegisterUser, CanRegisterUser);
            LoginUserCommand = new RelayCommand(LoginUser, CanLoginUser);
            //GUIHandler.GetInstance().MessageProcessor.SendMessage(Enums.MessageType.All, Enums.MessageAction.Load, null);

            // Trigger validation for initial state
            //ValidateLogin(nameof(EmailLoginText), EmailLoginText);
            //ValidateLogin(nameof(PasswordLoginText), PasswordLoginText);

            //ValidateRegister(nameof(EmailRegisterText), EmailRegisterText);
            //ValidateRegister(nameof(PhoneRegisterText), PhoneRegisterText);
            //ValidateRegister(nameof(PasswordRegisterText), PasswordRegisterText);
        }

        private bool CanLoginUser(object obj)
        {
            
            //return !LoginErrors.Any();
            return !HasErrors;
        }

        private void LoginUser(object obj) {

            

            var user = GUIHandler.GetInstance().CacheManager.GetAllUsers().FirstOrDefault(u => u.Email == EmailLoginText && u.Password == PasswordLoginText);
             //var user =  GUIHandler.GetInstance().ClientManager(EmailLoginText, PasswordLoginText);
            //GUIHandler.GetInstance().ClientManager.SendRequest(
            //    new Request
            //    {
            //        Type = "Login",
            //        Data = { Email = EmailLoginText, Password = PasswordLoginText }
            //    });
            //var user = ClientManager.Instance().SendLoginRequest(EmailLoginText, PasswordLoginText);
          
            if (user != null)
            {
                if (SelectedRole == "Employee" && user.ApprovalStatus != ApprovalStates.Approved)
                {
                    DXMessageBox.Show($"Login failed. Your account is {user.ApprovalStatus}.", "Error");
                    return;
                }
                //CurrentUser = user;

                //GUIHandler.GetInstance().CacheManager.SetCurrentUser(user);

                //GUIHandler.GetInstance().CacheManager.CurrentUser = user;
                GUIHandler.GetInstance().CurrentUser = user;

                //GUIHandler.GetInstance().MessageProcessor.SendMessage(Enums.MessageType.All, Enums.MessageAction.Load, null);

                //DXMessageBox.Show($"Logged in as: {GUIHandler.GetInstance().CurrentUser.Name}");
                var mainWindow = new MainWindow();
                mainWindow.Show();

                CloseWindow.Invoke();

            }
            else
            {
                DXMessageBox.Show("Invalid email or password", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private bool CanRegisterUser(object obj)
        {
            //return !RegisterErrors.Any();
            return !HasErrors;

        }

        private void RegisterUser(object obj)
        {
            var user = new User
            {
                Id = GUIHandler.GetInstance().CacheManager.GetAllUsers().Last().Id + 1,
                Name = NameRegisterText,
                Email = EmailRegisterText,
                Phone = PhoneRegisterText,
                Password = PasswordRegisterText,
                //IsAdmin = RoleChecked,
                IsAdmin = false,
                ApprovalStatus = ApprovalStates.Pending // Default to Pending

            };

            // Save the user to the database
            //GUIHandler.GetInstance().CacheManager.AddUser(user);

            GUIHandler.GetInstance().MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Add, user);
            //CloseWindow.Invoke();

            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
            //SelectedTabIndex = 0;
            DXMessageBox.Show("Registration successful. Your account is pending approval.", "Info");

        }

        //public IEnumerable GetErrors(string propertyName)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
