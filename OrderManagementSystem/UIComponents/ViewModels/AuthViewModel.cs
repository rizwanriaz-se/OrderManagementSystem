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

namespace OrderManagementSystem.UIComponents.ViewModels
{
          
    public class AuthViewModel : INotifyPropertyChanged
    {
        public Action CloseWindow { get; set; }

        public string NameRegisterText { get; set; }

        private int m_nSelectedTabIndex;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public User CurrentUser { get; set; }
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

                CurrentUser = user;


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
                IsAdmin = RoleChecked
            };

            // Save the user to the database
            GUIHandler.GetInstance().CacheManager.AddUser(user);
            //CloseWindow.Invoke();

            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
            SelectedTabIndex = 0;

        }
    }
}
