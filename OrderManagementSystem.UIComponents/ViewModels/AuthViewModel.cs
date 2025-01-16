using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
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
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using static OrderManagementSystemServer.Repository.User;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class AuthViewModel : INotifyPropertyChanged
    {

        public enum Roles
        {
            Admin,
            Employee
        }

        //private ISplashScreenManagerService m_objSplashScreenService;


        public ISplashScreenManagerService SplashScreenManagerService = new SplashScreenManagerService();
        

        public Action CloseWindow { get; set; }

        private bool m_bIsRegisterTabVisible;
        private int m_nSelectedTabIndex;
        private Roles m_enSelectedLoginRole;
        private string m_stEmailLoginText;

        public string NameRegisterText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsRegisterTabVisible
        {
            get => m_bIsRegisterTabVisible;
            set
            {
                m_bIsRegisterTabVisible = value;
                OnPropertyChanged(nameof(IsRegisterTabVisible));
            }
        }

        public Roles SelectedLoginRole
        {
            get { return m_enSelectedLoginRole; }
            set
            {
                m_enSelectedLoginRole = value;
                OnPropertyChanged(nameof(SelectedLoginRole));
                IsRegisterTabVisible = SelectedLoginRole == Roles.Employee;
            }
        }

        public IEnumerable<Roles> UserRoles
        {
            get
            {
                return (IEnumerable<Roles>)Enum.GetValues(typeof(Roles));
            }
        }

        public string EmailLoginText { get; set; }


        public string PasswordLoginText { get; set; }

        public int SelectedTabIndex
        {
            get { return m_nSelectedTabIndex; }
            set { m_nSelectedTabIndex = value; OnPropertyChanged(nameof(SelectedTabIndex)); }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string EmailRegisterText { get; set; }

        public string PhoneRegisterText { get; set; }

        public string PasswordRegisterText { get; set; }

        public User CurrentUser { get; set; }

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
            // list safe check
            // easily debuggable
            // can add safe checks and conditions
            ValidateLoginInputs();

            User user = null;
            if (GUIHandler.Instance.CacheManager.Users == null)
            {
                DXMessageBox.Show("No user currently exists. Please register first.", "Error");
                return;
            }

            foreach (User userItem in GUIHandler.Instance.CacheManager.Users)
            {
                if (userItem.Email == EmailLoginText && CompareHashValues(userItem.Password, PasswordLoginText))
                {
                    if (userItem.IsAdmin == (SelectedLoginRole == Roles.Admin))
                    {
                        if (userItem.IsArchived == false)
                        {
                            user = userItem;
                            break;
                        }
                    }
                }

            }

            if (user == null)
            {
                DXMessageBox.Show("No user found with these credentials", "Error");
                return;
            }


            if (SelectedLoginRole == Roles.Employee && user.UserApprovalStatus != UserApprovalStates.Approved)
            {
                DXMessageBox.Show($"Login failed. Your account approval status is:  {user.UserApprovalStatus}.");
                return;
            }


            GUIHandler.Instance.CurrentUser = user;

            if (!ClientManager.Instance.IsDataLoaded)
                ShowSplashScreen();

            //ClientManager.Instance.LoadData();
                
            
            //var mainWindow = new MainWindow();
            //mainWindow.Show();
            //CloseWindow.Invoke();

        }

        private void ShowSplashScreen()
        {

            SplashScreenManagerService.ViewModel = new DXSplashScreenViewModel();
            SplashScreenManagerService.ViewModel.Title = "Order Management System";
            SplashScreenManagerService.ViewModel.Subtitle = null;
            SplashScreenManagerService.ViewModel.Logo = null;
            SplashScreenManagerService.ViewModel.Copyright = null;
            SplashScreenManagerService.ViewModel.Status = "Loading data from server...";

            ClientManager.Instance.LoadData();

            while (!GUIHandler.Instance.ClientManager.IsDataLoaded)
            {
                SplashScreenManagerService.Show();
            }

            //Thread.Sleep(TimeSpan.FromSeconds(5));
            SplashScreenManagerService.Close();
        }

        private void ValidateLoginInputs()
        {
            if (EmailLoginText == null || !EmailLoginText.Contains('@') || EmailLoginText.Length <= 6)
            {
                DXMessageBox.Show("Email field must not be empty, should contain @ and have atleast 6 characters", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            if (PasswordLoginText == null || PasswordLoginText.Length < 6)
            {
                DXMessageBox.Show("Password field must not be empty, and should contain atleast 6 characters", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
        }

        private void RegisterUser(object obj)
        {
            ValidateRegisterInputs();

            var user = new User
            {
                Name = NameRegisterText,
                Email = EmailRegisterText,
                Phone = PhoneRegisterText,
                Password = PasswordRegisterText,
                IsAdmin = false,
                IsArchived = false,
                UserApprovalStatus = UserApprovalStates.Pending // Default to Pending
            };


            GUIHandler.Instance.ClientManager.SendMessage(MessageType.User, MessageAction.Add, user);

        }

        private void ValidateRegisterInputs()
        {
            if (NameRegisterText == null)
            {
                DXMessageBox.Show("Name field must not be empty.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            if (EmailRegisterText == null || !EmailRegisterText.Contains('@') || EmailRegisterText.Length <= 6)
            {
                DXMessageBox.Show("Email field must not be empty, should contain @ and have atleast 6 characters", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            if (PhoneRegisterText == null || PhoneRegisterText.Length != 12)
            {
                DXMessageBox.Show("Phone number field must not be empty, and should contain 11 digits", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            if (PasswordRegisterText == null || PasswordRegisterText.Length <= 6)
            {
                DXMessageBox.Show("Password field must not be empty, and should contain atleast 6 characters", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
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



