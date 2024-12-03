using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditUserViewModel
    {
        private User _User;

        public Action CloseWindow { get; set; }

        public int? Id { get; set; }
        public string UserNameText { get; set; }
        public string UserEmailText { get; set; }

        public string UserPhoneText { get; set; }
        public string UserPasswordText { get; set; }
        public bool UserIsAdmin { get; set; }

        public ICommand SaveUserCommand { get; set; }

        public EditUserViewModel(User user)
        {
            SaveUserCommand = new RelayCommand(SaveUser, CanSaveUser);

            _User = user;

            Id = user.Id;
            UserNameText = user.Name;
            UserEmailText = user.Email;
            UserPhoneText = user.Phone;
            UserPasswordText = user.Password;
            UserIsAdmin = user.IsAdmin;
        }

        private bool CanSaveUser(object obj)
        {
            return true;
        }

        private void SaveUser(object obj)
        {
            _User.Name = UserNameText;
            _User.Email = UserEmailText;
            _User.Phone = UserPhoneText;
            _User.Password = UserPasswordText;
            _User.IsAdmin = UserIsAdmin;


            GUIHandler.GetInstance().CacheManager.UpdateUser(_User);

            CloseWindow.Invoke();

        }
    }
}
