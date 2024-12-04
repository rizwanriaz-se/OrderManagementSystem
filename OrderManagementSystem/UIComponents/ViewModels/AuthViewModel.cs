using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.XtraRichEdit.Model.History;
using OrderManagementSystem.Cache;
using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static OrderManagementSystem.Cache.Models.User;

namespace OrderManagementSystem.UIComponents.ViewModels
{
          
    public class AuthViewModel : INotifyPropertyChanged
    {
        public Action CloseWindow { get; set; }

        public string NameRegisterText { get; set; }

        private int m_nSelectedTabIndex;

        public event PropertyChangedEventHandler PropertyChanged;

        private string m_stSelectedRole;
        private bool m_bIsRegisterTabVisible;

        public List<string> Roles { get; } = new List<string> { "Admin", "Employee" };

        public string SelectedRole
        {
            get => m_stSelectedRole;
            set
            {
                m_stSelectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
                IsRegisterTabVisible = m_stSelectedRole == "Employee";
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

        public string EmailLoginText { get; set; }
        public string PasswordLoginText { get; set; }
        public string EmailRegisterText { get; set; }
        public string PhoneRegisterText { get; set; }
        public string PasswordRegisterText { get; set; }

        private User m_User;

        public User CurrentUser {
            get { return m_User; }
            set { m_User = value; OnPropertyChanged(nameof(CurrentUser)); }
        }
        public bool RoleChecked { get; set; }
        public ICommand RegisterUserCommand { get; set; }
        public ICommand LoginUserCommand { get; set; }
        public AuthViewModel() {
            RegisterUserCommand = new RelayCommand(RegisterUser, CanRegisterUser);
            LoginUserCommand = new RelayCommand(LoginUser, CanLoginUser);
        }

        private bool CanLoginUser(object obj)
        {
            return true;
        }

        private void LoginUser(object obj) {

            var user = GUIHandler.GetInstance().CacheManager.GetAllUsers().FirstOrDefault(u => u.Email == EmailLoginText && u.Password == PasswordLoginText);

            
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

                DXMessageBox.Show($"Logged in as: {GUIHandler.GetInstance().CurrentUser.Name}");
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
            return true;
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
            GUIHandler.GetInstance().CacheManager.AddUser(user);
            //CloseWindow.Invoke();

            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
            //SelectedTabIndex = 0;
            DXMessageBox.Show("Registration successful. Your account is pending approval.", "Info");


        }
    }
}
