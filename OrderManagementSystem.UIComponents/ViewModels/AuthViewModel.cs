using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystem.UIComponents.Views;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using static OrderManagementSystemServer.Repository.User;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class AuthViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public class LoginInfo
        {
           
            [Required(ErrorMessage = "Role must be selected.")]
            public string SelectedLoginRole { get; set; }
          

            [Required(ErrorMessage = "Email is required"), RegularExpression(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$", ErrorMessage = "Please enter a valid email address.")]
            public string EmailLoginText { get; set; }
            //{
           

            [Required(ErrorMessage = "Password is required")]
            public string PasswordLoginText { get; set; }
           
        }

        public class RegisterInfo
        {
            //{
            [Required(ErrorMessage = "Name is required")]
            public string NameRegisterText { get; set; }

            [Required(ErrorMessage = "Email is required"), RegularExpression(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$", ErrorMessage = "Please enter a valid email address.")]
            public string EmailRegisterText { get; set; }
            

            [Required(ErrorMessage = "Phone number is required"), RegularExpression(@"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$", ErrorMessage = "Please enter a valid phone number.")]
            public string PhoneRegisterText { get; set; }
            

            [Required(ErrorMessage = "Password is required"), RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).{6,}$", ErrorMessage = "Password must consist of at least 6 alphanumeric characters.")]
            //[PasswordPropertyText(true)]
            public string PasswordRegisterText { get; set; }
            

        }

        public Action CloseWindow { get; set; }

        public string NameRegisterText {
            get { return m_objRegisterInfo.NameRegisterText; }
            set
            {
                m_objRegisterInfo.NameRegisterText = value;
                OnPropertyChanged(nameof(m_objRegisterInfo.NameRegisterText));
                ValidateRegister(nameof(m_objRegisterInfo.NameRegisterText), m_objRegisterInfo.NameRegisterText, m_objRegisterInfo);
            }
        }

        private int m_nSelectedTabIndex;

        private LoginInfo m_objLoginInfo = new LoginInfo();
        private RegisterInfo m_objRegisterInfo = new RegisterInfo();

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private bool m_bIsRegisterTabVisible;

        public List<string> Roles { get; } = new List<string> { "Admin", "Employee" };

        public bool IsRegisterTabVisible
        {
            get => m_bIsRegisterTabVisible;
            set
            {
                m_bIsRegisterTabVisible = value;
                OnPropertyChanged(nameof(IsRegisterTabVisible));
            }
        }

        [Required(ErrorMessage = "Role must be selected.")]
        public string SelectedLoginRole
        {
            get => m_objLoginInfo.SelectedLoginRole;
            set
            {
                m_objLoginInfo.SelectedLoginRole = value;
                OnPropertyChanged(nameof(m_objLoginInfo.SelectedLoginRole));
                IsRegisterTabVisible = m_objLoginInfo.SelectedLoginRole == "Employee";
                ValidateLogin(nameof(m_objLoginInfo.SelectedLoginRole), m_objLoginInfo.SelectedLoginRole, m_objLoginInfo);
            }
        }

        [Required(ErrorMessage = "Email is required")]
        public string EmailLoginText
        {
            get { return m_objLoginInfo.EmailLoginText; }
            set
            {
                m_objLoginInfo.EmailLoginText = value;
                OnPropertyChanged(nameof(m_objLoginInfo.EmailLoginText));
                ValidateLogin(nameof(m_objLoginInfo.EmailLoginText), m_objLoginInfo.EmailLoginText, m_objLoginInfo);
            }
        }

        [Required(ErrorMessage = "Password is required")]
        public string PasswordLoginText
        {
            get { return m_objLoginInfo.PasswordLoginText; }
            set
            {
                m_objLoginInfo.PasswordLoginText = value;
                OnPropertyChanged(nameof(m_objLoginInfo.PasswordLoginText));
                ValidateLogin(nameof(m_objLoginInfo.PasswordLoginText), m_objLoginInfo.PasswordLoginText, m_objLoginInfo);
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

       

        [Required(ErrorMessage = "Email is required")]
        public string EmailRegisterText
        {
            get { return m_objRegisterInfo.EmailRegisterText; }
            set
            {
                m_objRegisterInfo.EmailRegisterText = value;
                OnPropertyChanged(nameof(m_objRegisterInfo.EmailRegisterText));
                ValidateRegister(nameof(m_objRegisterInfo.EmailRegisterText), m_objRegisterInfo.EmailRegisterText, m_objRegisterInfo);
            }
        }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneRegisterText
        {
            get { return m_objRegisterInfo.PhoneRegisterText; }
            set
            {
                m_objRegisterInfo.PhoneRegisterText = value;
                OnPropertyChanged(nameof(m_objRegisterInfo.PhoneRegisterText));
                ValidateRegister(nameof(m_objRegisterInfo.PhoneRegisterText), m_objRegisterInfo.PhoneRegisterText, m_objRegisterInfo);
            }
        }

        [Required(ErrorMessage = "Password is required")]
        //[PasswordPropertyText(true)]
        public string PasswordRegisterText
        {
            get { return m_objRegisterInfo.PasswordRegisterText; }
            set
            {
                m_objRegisterInfo.PasswordRegisterText = value;
                OnPropertyChanged(nameof(m_objRegisterInfo.PasswordRegisterText));
                ValidateRegister(nameof(m_objRegisterInfo.PasswordRegisterText), m_objRegisterInfo.PasswordRegisterText, m_objRegisterInfo);
            }
        }

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

        public void ValidateRegister(string propertyName, object propertyValue, object validationObject)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(validationObject) { MemberName = propertyName };
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

        public void ValidateLogin(string propertyName, object propertyValue, object validationObject)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(validationObject) { MemberName = propertyName };
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

        public User CurrentUser
        {
            get { return m_User; }
            set { m_User = value; OnPropertyChanged(nameof(CurrentUser)); }
        }
        public bool RoleChecked { get; set; }
        public RelayCommand RegisterUserCommand { get; set; }
        public RelayCommand LoginUserCommand { get; set; }

        public AuthViewModel()
        {
            RegisterUserCommand = new RelayCommand(RegisterUser, CanRegisterUser);
            LoginUserCommand = new RelayCommand(LoginUser, CanLoginUser);
        }

        private bool CanLoginUser(object obj)
        {
            return Validator.TryValidateObject(m_objLoginInfo, new ValidationContext(m_objLoginInfo), null, true);
        }

        private void LoginUser(object obj)
        {



            User user = GUIHandler.Instance.CacheManager.GetAllUsers().FirstOrDefault(u =>
                u.Email == EmailLoginText &&
                CompareHashValues(u.Password, PasswordLoginText) &&
                u.IsAdmin == (SelectedLoginRole == "Admin")
                );
            if (user != null)
            {
                if (SelectedLoginRole == "Employee" && user.ApprovalStatus != ApprovalStates.Approved)
                {
                    DXMessageBox.Show($"Login failed. Your account is {user.ApprovalStatus}.", "Error");
                    return;
                }
                GUIHandler.Instance.CurrentUser = user;

                var mainWindow = new MainWindow();
                mainWindow.Show();

                CloseWindow.Invoke();
            }
            else
            {
                DXMessageBox.Show("No user found with these credentials.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private bool CanRegisterUser(object obj)
        {
            return Validator.TryValidateObject(m_objRegisterInfo, new ValidationContext(m_objRegisterInfo), null, true);
        }

        private void RegisterUser(object obj)
        {
            var user = new User
            {
                Id = GUIHandler.Instance.CacheManager.GetAllUsers().Last().Id + 1,
                Name = NameRegisterText,
                Email = EmailRegisterText,
                Phone = PhoneRegisterText,
                Password = PasswordRegisterText,
                IsAdmin = false,
                ApprovalStatus = ApprovalStates.Pending // Default to Pending

            };
           
            MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Add, user);
            

        }

        public bool CompareHashValues(string userStoredPasswordHash, string userInputPassword)
        {
            byte[] inputPasswordInBytes = Encoding.UTF8.GetBytes(userInputPassword);
            byte[] inputPasswordHashInBytes = SHA256.HashData(inputPasswordInBytes);

            byte[] storedPasswordHashInBytes = StringToByteArray(userStoredPasswordHash);

            bool bEqual = false;
            if (inputPasswordHashInBytes.Length == storedPasswordHashInBytes.Length)
            {
                int i = 0;
                while ((i < inputPasswordHashInBytes.Length) && (inputPasswordHashInBytes[i] == storedPasswordHashInBytes[i]))
                {
                    i += 1;
                }
                if (i == inputPasswordHashInBytes.Length)
                {
                    bEqual = true;
                }
            }
            return bEqual;
        }
        private static byte[] StringToByteArray(string hexInput)
        {
            if (hexInput.Length % 2 != 0)
            {
                throw new ArgumentException("Hexadecimal string must have an even length.");
            }

            byte[] byteArray = new byte[hexInput.Length / 2];

            for (int i = 0; i < hexInput.Length; i += 2)
            {
                string hexPair = hexInput.Substring(i, 2);
                byteArray[i / 2] = Convert.ToByte(hexPair, 16);
            }

            return byteArray;
        }


    }
}



