﻿using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.XtraRichEdit.Model.History;
//using OrderManagementSystem.Cache;
//using OrderManagementSystem.Commands;
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
//using static OrderManagementSystem.Repositories.User;
//using OrderManagementSystem.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystem.UIComponents.Views;
//using OrderManagementSystem.Repositories;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystemServer.Repository;
using static OrderManagementSystemServer.Repository.User;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    //public class Request
    //{
    //    public string Type { get; set; }
    //    public string Data { get; set; }
    //}
    //public class AuthViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    //{
    //    public Action CloseWindow { get; set; }

    //    public string NameRegisterText { get; set; }

    //    private int m_nSelectedTabIndex;

    //    private string m_stEmailLoginText;
    //    private string m_stPasswordLoginText;
    //    private string m_stEmailRegisterText;
    //    private string m_stPhoneRegisterText;
    //    private string m_stPasswordRegisterText;


    //    public event PropertyChangedEventHandler PropertyChanged;
    //    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    //    private string? m_stSelectedLoginRole;
    //    private bool m_bIsRegisterTabVisible;

    //    public List<string> Roles { get; } = new List<string> { "Admin", "Employee" };

    //    [Required(ErrorMessage = "Role must be selected.")]
    //    public string? SelectedLoginRole
    //    {
    //        get => m_stSelectedLoginRole;
    //        set
    //        {
    //            m_stSelectedLoginRole = value;
    //            OnPropertyChanged(nameof(SelectedLoginRole));
    //            IsRegisterTabVisible = m_stSelectedLoginRole == "Employee";
    //            ValidateLogin(nameof(SelectedLoginRole), m_stSelectedLoginRole);
    //        }
    //    }

    //    public bool IsRegisterTabVisible
    //    {
    //        get => m_bIsRegisterTabVisible;
    //        set
    //        {
    //            m_bIsRegisterTabVisible = value;
    //            OnPropertyChanged(nameof(IsRegisterTabVisible));
    //        }
    //    }

    //    public int SelectedTabIndex
    //    {
    //        get { return m_nSelectedTabIndex; }
    //        set { m_nSelectedTabIndex = value; OnPropertyChanged(nameof(SelectedTabIndex)); }
    //    }


    //    private void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }

    //    [Required(ErrorMessage = "Email is required")]
    //    public string EmailLoginText
    //    {
    //        get { return m_stEmailLoginText; }
    //        set
    //        {
    //            m_stEmailLoginText = value;
    //            OnPropertyChanged(nameof(EmailLoginText));
    //            ValidateLogin(nameof(EmailLoginText), m_stEmailLoginText);
    //            //CommandManager.InvalidateRequerySuggested(); // Updates command state

    //        }
    //    }

    //    [Required(ErrorMessage = "Password is required")]
    //    public string PasswordLoginText
    //    {
    //        get { return m_stPasswordLoginText; }
    //        set
    //        {
    //            m_stPasswordLoginText = value;
    //            OnPropertyChanged(nameof(PasswordLoginText));
    //            ValidateLogin(nameof(PasswordLoginText), m_stPasswordLoginText);
    //            //CommandManager.InvalidateRequerySuggested(); // Updates command state

    //        }
    //    }

    //    [Required(ErrorMessage = "Email is required")]
    //    public string EmailRegisterText
    //    {
    //        get { return m_stEmailRegisterText; }
    //        set
    //        {
    //            m_stEmailRegisterText = value;
    //            OnPropertyChanged(nameof(EmailRegisterText));
    //            ValidateRegister(nameof(EmailRegisterText), m_stEmailRegisterText);
    //        }
    //    }

    //    [Required(ErrorMessage = "Phone number is required")]
    //    //[Phone(ErrorMessage = "Not a valid phone number")]
    //    public string PhoneRegisterText
    //    {
    //        get { return m_stPhoneRegisterText; }
    //        set
    //        {
    //            m_stPhoneRegisterText = value;
    //            OnPropertyChanged(nameof(PhoneRegisterText));
    //            ValidateRegister(nameof(PhoneRegisterText), m_stPhoneRegisterText);
    //        }
    //    }

    //    [Required(ErrorMessage = "Password is required")]
    //    //[PasswordPropertyText(true)]
    //    public string PasswordRegisterText
    //    {
    //        get { return m_stPasswordRegisterText; }
    //        set
    //        {
    //            m_stPasswordRegisterText = value;
    //            OnPropertyChanged(nameof(PasswordRegisterText));
    //            ValidateRegister(nameof(PasswordRegisterText), m_stPasswordRegisterText);
    //        }
    //    }

    //    //public string EmailRegisterText { get; set; }
    //    //public string PhoneRegisterText { get; set; }
    //    //public string PasswordRegisterText { get; set; }

    //    private User m_User;

    //    Dictionary<string, List<string>> LoginErrors = new Dictionary<string, List<string>>();
    //    Dictionary<string, List<string>> RegisterErrors = new Dictionary<string, List<string>>();

    //    public bool HasErrors => LoginErrors.Count > 0 || RegisterErrors.Count > 0;

    //    public IEnumerable GetErrors(string propertyName)
    //    {
    //        if (SelectedTabIndex == 0 && LoginErrors.ContainsKey(propertyName))
    //        {
    //            return LoginErrors[propertyName];
    //        }
    //        else if (SelectedTabIndex == 1 && RegisterErrors.ContainsKey(propertyName))
    //        {
    //            return RegisterErrors[propertyName];
    //        }
    //        return null;
    //    }

    //    public void ValidateRegister(string propertyName, object propertyValue)
    //    {
    //        var results = new List<ValidationResult>();
    //        var context = new ValidationContext(this) { MemberName = propertyName };
    //        Validator.TryValidateProperty(propertyValue, context, results);

    //        if (results.Any())
    //        {
    //            RegisterErrors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
    //        }
    //        else
    //        {
    //            RegisterErrors.Remove(propertyName);
    //        }

    //        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    //        RegisterUserCommand.RaiseCanExecuteEventChanged();
    //    }

    //    public void ValidateLogin(string propertyName, object propertyValue)
    //    {
    //        var results = new List<ValidationResult>();
    //        var context = new ValidationContext(this) { MemberName = propertyName };
    //        Validator.TryValidateProperty(propertyValue, context, results);

    //        if (results.Any())
    //        {
    //            LoginErrors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
    //        }
    //        else
    //        {
    //            LoginErrors.Remove(propertyName);
    //        }

    //        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    //        LoginUserCommand.RaiseCanExecuteEventChanged();
    //    }

    //    public User CurrentUser
    //    {
    //        get { return m_User; }
    //        set { m_User = value; OnPropertyChanged(nameof(CurrentUser)); }
    //    }
    //    public bool RoleChecked { get; set; }
    //    public RelayCommand RegisterUserCommand { get; set; }
    //    public RelayCommand LoginUserCommand { get; set; }

    //    //public bool HasErrors => throw new NotImplementedException();

    //    public AuthViewModel()
    //    {
    //        RegisterUserCommand = new RelayCommand(RegisterUser, CanRegisterUser);
    //        LoginUserCommand = new RelayCommand(LoginUser, CanLoginUser);
    //        //MessageProcessor.SendMessage(Enums.MessageType.All, Enums.MessageAction.Load, null);

    //        // Trigger validation for initial state
    //        ValidateLogin(nameof(EmailLoginText), EmailLoginText);
    //        ValidateLogin(nameof(PasswordLoginText), PasswordLoginText);

    //        ValidateRegister(nameof(EmailRegisterText), EmailRegisterText);
    //        ValidateRegister(nameof(PhoneRegisterText), PhoneRegisterText);
    //        ValidateRegister(nameof(PasswordRegisterText), PasswordRegisterText);
    //    }

    //    private bool CanLoginUser(object obj)
    //    {

    //        //return !LoginErrors.Any();
    //        return !(LoginErrors.Count > 0);
    //        //return !HasErrors;
    //    }

    //    private void LoginUser(object obj)
    //    {

    //        User user = GUIHandler.Instance.CacheManager.GetAllUsers().FirstOrDefault(u => 
    //            u.Email == EmailLoginText && 
    //            u.Password == PasswordLoginText && 
    //            u.IsAdmin == (SelectedLoginRole=="Admin")
    //            );
    //        //var user =  GUIHandler.Instance.ClientManager(EmailLoginText, PasswordLoginText);
    //        //GUIHandler.Instance.ClientManager.SendRequest(
    //        //    new Request
    //        //    {
    //        //        Type = "Login",
    //        //        Data = { Email = EmailLoginText, Password = PasswordLoginText }
    //        //    });
    //        //var user = ClientManager.Instance().SendLoginRequest(EmailLoginText, PasswordLoginText);

    //        if (user != null)
    //        {
    //            if (SelectedLoginRole == "Employee" && user.ApprovalStatus != ApprovalStates.Approved)
    //            {
    //                DXMessageBox.Show($"Login failed. Your account is {user.ApprovalStatus}.", "Error");
    //                return;
    //            }
    //            //CurrentUser = user;

    //            //GUIHandler.Instance.CacheManager.SetCurrentUser(user);

    //            //GUIHandler.Instance.CacheManager.CurrentUser = user;
    //            GUIHandler.Instance.CurrentUser = user;

    //            //MessageProcessor.SendMessage(Enums.MessageType.All, Enums.MessageAction.Load, null);

    //            //DXMessageBox.Show($"Logged in as: {GUIHandler.Instance.CurrentUser.Name}");
    //            var mainWindow = new MainWindow();
    //            mainWindow.Show();

    //            CloseWindow.Invoke();
    //        }
    //        else
    //        {
    //            DXMessageBox.Show("No user found with these credentials.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
    //        }
    //    }

    //    private bool CanRegisterUser(object obj)
    //    {
    //        //return !RegisterErrors.Any();
    //        return !(RegisterErrors.Count > 0);
    //        //return !HasErrors;

    //    }

    //    private void RegisterUser(object obj)
    //    {
    //        var user = new User
    //        {
    //            Id = GUIHandler.Instance.CacheManager.GetAllUsers().Last().Id + 1,
    //            Name = NameRegisterText,
    //            Email = EmailRegisterText,
    //            Phone = PhoneRegisterText,
    //            Password = PasswordRegisterText,
    //            //IsAdmin = RoleChecked,
    //            IsAdmin = false,
    //            ApprovalStatus = ApprovalStates.Pending // Default to Pending

    //        };

    //        // Save the user to the database
    //        //GUIHandler.Instance.CacheManager.AddUser(user);

    //        MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Add, user);
    //        //CloseWindow.Invoke();

    //        //MainWindow mainWindow = new MainWindow();
    //        //mainWindow.Show();
    //        //SelectedTabIndex = 0;
    //        DXMessageBox.Show("Registration successful. Your account is pending approval.", "Info");

    //    }

    //    //public IEnumerable GetErrors(string propertyName)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}
    //}



    public class AuthViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public class LoginInfo
        {
           
            [Required(ErrorMessage = "Role must be selected.")]
            public string SelectedLoginRole { get; set; }
          

            [Required(ErrorMessage = "Email is required")]
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

            //public string PasswordRegisterText { get; set; }
            [Required(ErrorMessage = "Email is required")]
            public string EmailRegisterText { get; set; }
            

            [Required(ErrorMessage = "Phone number is required")]
            //[Phone(ErrorMessage = "Not a valid phone number")]
            public string PhoneRegisterText { get; set; }
            

            [Required(ErrorMessage = "Password is required")]
            //[PasswordPropertyText(true)]
            public string PasswordRegisterText { get; set; }
            

        }

        public Action CloseWindow { get; set; }

        public string NameRegisterText {
            get { return registerInfo.NameRegisterText; }
            set
            {
                registerInfo.NameRegisterText = value;
                OnPropertyChanged(nameof(registerInfo.NameRegisterText));
                ValidateRegister(nameof(registerInfo.NameRegisterText), registerInfo.NameRegisterText, registerInfo);
            }
        }

        private int m_nSelectedTabIndex;

        
        //private string m_stEmailRegisterText;
        //private string m_stPhoneRegisterText;
        //private string m_stPasswordRegisterText;

        private LoginInfo loginInfo = new LoginInfo();
        private RegisterInfo registerInfo = new RegisterInfo();

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private bool m_bIsRegisterTabVisible;

        public List<string> Roles { get; } = new List<string> { "Admin", "Employee" };


        //private string m_stEmailLoginText;
        //private string m_stPasswordLoginText;
        //private string m_stSelectedLoginRole;
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
            get => loginInfo.SelectedLoginRole;
            set
            {
                loginInfo.SelectedLoginRole = value;
                OnPropertyChanged(nameof(loginInfo.SelectedLoginRole));
                IsRegisterTabVisible = loginInfo.SelectedLoginRole == "Employee";
                ValidateLogin(nameof(loginInfo.SelectedLoginRole), loginInfo.SelectedLoginRole, loginInfo);
            }
        }

        [Required(ErrorMessage = "Email is required")]
        public string EmailLoginText
        {
            get { return loginInfo.EmailLoginText; }
            set
            {
                loginInfo.EmailLoginText = value;
                OnPropertyChanged(nameof(loginInfo.EmailLoginText));
                ValidateLogin(nameof(loginInfo.EmailLoginText), loginInfo.EmailLoginText, loginInfo);
                //CommandManager.InvalidateRequerySuggested(); // Updates command state

            }
        }

        [Required(ErrorMessage = "Password is required")]
        public string PasswordLoginText
        {
            get { return loginInfo.PasswordLoginText; }
            set
            {
                loginInfo.PasswordLoginText = value;
                OnPropertyChanged(nameof(loginInfo.PasswordLoginText));
                ValidateLogin(nameof(loginInfo.PasswordLoginText), loginInfo.PasswordLoginText, loginInfo);
                //CommandManager.InvalidateRequerySuggested(); // Updates command state

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
            get { return registerInfo.EmailRegisterText; }
            set
            {
                registerInfo.EmailRegisterText = value;
                OnPropertyChanged(nameof(registerInfo.EmailRegisterText));
                ValidateRegister(nameof(registerInfo.EmailRegisterText), registerInfo.EmailRegisterText, registerInfo);
            }
        }

        [Required(ErrorMessage = "Phone number is required")]
        //[Phone(ErrorMessage = "Not a valid phone number")]
        public string PhoneRegisterText
        {
            get { return registerInfo.PhoneRegisterText; }
            set
            {
                registerInfo.PhoneRegisterText = value;
                OnPropertyChanged(nameof(registerInfo.PhoneRegisterText));
                ValidateRegister(nameof(registerInfo.PhoneRegisterText), registerInfo.PhoneRegisterText, registerInfo);
            }
        }

        [Required(ErrorMessage = "Password is required")]
        //[PasswordPropertyText(true)]
        public string PasswordRegisterText
        {
            get { return registerInfo.PasswordRegisterText; }
            set
            {
                registerInfo.PasswordRegisterText = value;
                OnPropertyChanged(nameof(registerInfo.PasswordRegisterText));
                ValidateRegister(nameof(registerInfo.PasswordRegisterText), registerInfo.PasswordRegisterText, registerInfo);
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

        //public bool HasErrors => throw new NotImplementedException();

        public AuthViewModel()
        {
            RegisterUserCommand = new RelayCommand(RegisterUser, CanRegisterUser);
            LoginUserCommand = new RelayCommand(LoginUser, CanLoginUser);
            //MessageProcessor.SendMessage(Enums.MessageType.All, Enums.MessageAction.Load, null);

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
            //return !(LoginErrors.Count > 0);
            return Validator.TryValidateObject(loginInfo, new ValidationContext(loginInfo), null, true);
            //return !HasErrors;
        }

        private void LoginUser(object obj)
        {

            User user = GUIHandler.Instance.CacheManager.GetAllUsers().FirstOrDefault(u =>
                u.Email == EmailLoginText &&
                u.Password == PasswordLoginText &&
                u.IsAdmin == (SelectedLoginRole == "Admin")
                );
            //var user =  GUIHandler.Instance.ClientManager(EmailLoginText, PasswordLoginText);
            //GUIHandler.Instance.ClientManager.SendRequest(
            //    new Request
            //    {
            //        Type = "Login",
            //        Data = { Email = EmailLoginText, Password = PasswordLoginText }
            //    });
            //var user = ClientManager.Instance().SendLoginRequest(EmailLoginText, PasswordLoginText);

            if (user != null)
            {
                if (SelectedLoginRole == "Employee" && user.ApprovalStatus != ApprovalStates.Approved)
                {
                    DXMessageBox.Show($"Login failed. Your account is {user.ApprovalStatus}.", "Error");
                    return;
                }
                //CurrentUser = user;

                //GUIHandler.Instance.CacheManager.SetCurrentUser(user);

                //GUIHandler.Instance.CacheManager.CurrentUser = user;
                GUIHandler.Instance.CurrentUser = user;

                //MessageProcessor.SendMessage(Enums.MessageType.All, Enums.MessageAction.Load, null);

                //DXMessageBox.Show($"Logged in as: {GUIHandler.Instance.CurrentUser.Name}");
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
            //return !RegisterErrors.Any();
            return Validator.TryValidateObject(registerInfo, new ValidationContext(registerInfo), null, true);
            //return !HasErrors;

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
                //IsAdmin = RoleChecked,
                IsAdmin = false,
                ApprovalStatus = ApprovalStates.Pending // Default to Pending

            };

            // Save the user to the database
            //GUIHandler.Instance.CacheManager.AddUser(user);

            MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Add, user);
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



