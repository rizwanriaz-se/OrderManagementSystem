using DevExpress.Xpf.Core;
using DevExpress.XtraRichEdit.Fields.Expression;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystem.UIComponents.Views;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
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
            
            [Required(ErrorMessage = "Name is required")]
            public string NameRegisterText { get; set; }

            [Required(ErrorMessage = "Email is required"), RegularExpression(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$", ErrorMessage = "Please enter a valid email address.")]
            public string EmailRegisterText { get; set; }
            

            [Required(ErrorMessage = "Phone number is required"), RegularExpression(@"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$", ErrorMessage = "Please enter a valid phone number.")]
            public string PhoneRegisterText { get; set; }
            

            [Required(ErrorMessage = "Password is required"), RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).{6,}$", ErrorMessage = "Password must consist of at least 6 alphanumeric characters.")]
            public string PasswordRegisterText { get; set; }
            

        }

        public Action CloseWindow { get; set; }

        public string NameRegisterText {
            get { return m_objRegisterInfo.NameRegisterText; }
            set
            {
                m_objRegisterInfo.NameRegisterText = value;
                OnPropertyChanged(nameof(m_objRegisterInfo.NameRegisterText));
            }
        }

        private int m_nSelectedTabIndex;
        private bool m_bIsRegisterTabVisible;
        private LoginInfo m_objLoginInfo = new LoginInfo();
        private RegisterInfo m_objRegisterInfo = new RegisterInfo();
        private User m_User;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


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

        public string SelectedLoginRole
        {
            get => m_objLoginInfo.SelectedLoginRole;
            set
            {
                m_objLoginInfo.SelectedLoginRole = value;
                OnPropertyChanged(nameof(m_objLoginInfo.SelectedLoginRole));
                IsRegisterTabVisible = m_objLoginInfo.SelectedLoginRole == "Employee";
                
            }
        }

        public string EmailLoginText
        {
            get { return m_objLoginInfo.EmailLoginText; }
            set
            {
                m_objLoginInfo.EmailLoginText = value;
                OnPropertyChanged(nameof(m_objLoginInfo.EmailLoginText));
              
            }
        }

        public string PasswordLoginText
        {
            get { return m_objLoginInfo.PasswordLoginText; }
            set
            {
                m_objLoginInfo.PasswordLoginText = value;
                OnPropertyChanged(nameof(m_objLoginInfo.PasswordLoginText));
               
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

        public bool ValidateRegister(string propertyName, object propertyValue, object validationObject)
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
            //RegisterUserCommand.RaiseCanExecuteEventChanged();

            return RegisterErrors.ContainsKey(propertyName);
        }

        public bool ValidateLogin(string propertyName, object propertyValue, object validationObject)
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
            //LoginUserCommand.RaiseCanExecuteEventChanged();

            return LoginErrors.ContainsKey(propertyName);
        }

        public User CurrentUser
        {
            get { return m_User; }
            set { m_User = value; OnPropertyChanged(nameof(CurrentUser)); }
        }
        public bool RoleChecked { get; set; }
        public ICommand RegisterUserCommand { get; set; }
        public ICommand LoginUserCommand { get; set; }

        public ICommand NavigateToLoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public AuthViewModel()
        {
            SelectedTabIndex = 0;

            RegisterUserCommand = new RelayCommand(RegisterUser);
            LoginUserCommand = new RelayCommand(LoginUser);

            NavigateToLoginCommand = new RelayCommand(NavigateToLogin);
            NavigateToRegisterCommand = new RelayCommand(NavigateToRegister);
        }

       

        private void NavigateToRegister(object obj)
        {
            SelectedTabIndex = 1;
        }

      

        private void NavigateToLogin(object obj)
        {
            SelectedTabIndex = 0;
        }

       
        private void LoginUser(object obj)
        {
            try
            {
                // list safe check
                // easily debuggable
                // can add safe checks and conditions


                User? user = GUIHandler.Instance.CacheManager.GetAllUsers().FirstOrDefault(u =>
                        u.Email == EmailLoginText &&
                        u.IsArchived == false &&
                        CompareHashValues(u.Password, PasswordLoginText) &&
                        u.IsAdmin == (SelectedLoginRole == "Admin")
                        );
                
                if (ValidateLogin(nameof(m_objLoginInfo.EmailLoginText), m_objLoginInfo.EmailLoginText, m_objLoginInfo) || ValidateLogin(nameof(m_objLoginInfo.PasswordLoginText), m_objLoginInfo.PasswordLoginText, m_objLoginInfo) || ValidateLogin(nameof(m_objLoginInfo.SelectedLoginRole), m_objLoginInfo.SelectedLoginRole, m_objLoginInfo))
                {
                    var errors = LoginErrors.SelectMany(o => o.Value);

                    throw new Exception(string.Join('\n', errors));

                }
                if (user != null)
                {
                    if (SelectedLoginRole == "Employee" && user.ApprovalStatus != ApprovalStates.Approved)
                    {
                        throw new Exception($"Login failed. Your account approval status is:  {user.ApprovalStatus}.");
                    }

                   

                    GUIHandler.Instance.CurrentUser = user;

                    var mainWindow = new MainWindow();
                    mainWindow.Show();

                    CloseWindow.Invoke();
                }
                else
                {
                    throw new Exception("No user found with these credentials.");
                   
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
        }

     
        private void RegisterUser(object obj)
        {
            try { 
                if (ValidateRegister(nameof(m_objRegisterInfo.EmailRegisterText), m_objRegisterInfo.EmailRegisterText, m_objRegisterInfo) || ValidateRegister(nameof(m_objRegisterInfo.PhoneRegisterText), m_objRegisterInfo.PhoneRegisterText, m_objRegisterInfo) || ValidateRegister(nameof(m_objRegisterInfo.PasswordRegisterText), m_objRegisterInfo.PasswordRegisterText, m_objRegisterInfo) || ValidateRegister(nameof(m_objRegisterInfo.NameRegisterText), m_objRegisterInfo.NameRegisterText, m_objRegisterInfo)){
                    var errors = RegisterErrors.SelectMany(e => e.Value);
                    throw new Exception(string.Join('\n', errors));
                }


            var user = new User
            {
                Id = GUIHandler.Instance.CacheManager.GetAllUsers().Last().Id + 1,
                Name = NameRegisterText,
                Email = EmailRegisterText,
                Phone = PhoneRegisterText,
                Password = PasswordRegisterText,
                IsAdmin = false,
                IsArchived = false,
                ApprovalStatus = ApprovalStates.Pending // Default to Pending
            };


            MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Add, user);
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

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



