﻿using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystem.UIComponents.Views;
using OrderManagementSystemServer.Repository;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<User> Users { get; private set; }

        public Action CloseWindow { get; set; }

        public string UserNameText { get; set; }
        public string UserPasswordText { get; set; }
        public string UserEmailText { get; set; }
        public string UserPhoneText { get; set; }
        public bool UserIsAdmin { get; set; }

        private User m_SelectedUser { get; set; }

        public User SelectedUser
        {
            get { return m_SelectedUser; }
            set
            {
                m_SelectedUser = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedUser)));
            }
        }
        public RelayCommand EditUserCommand { get; set; }

        public RelayCommand SubmitUserCommand { get; set; }
        public RelayCommand DeleteUserCommand { get; set; }

        public UserViewModel()
        {
            Users = GUIHandler.Instance.CacheManager.GetAllUsers();
            EditUserCommand = new RelayCommand(EditUser, CanEditUser);
            DeleteUserCommand = new RelayCommand(DeleteUser, CanDeleteUser);
            SubmitUserCommand = new RelayCommand(SubmitUser, CanSubmitUser);

        }


        private void SubmitUser(object obj)
        {
            int lastUserId = Users.Last().Id;

            // Create new Order object
            User user = new User
            {
                Id = lastUserId + 1,
                Name = UserNameText,
                Email = UserEmailText,
                Phone = UserPhoneText,
                Password = UserPasswordText,
                IsAdmin = UserIsAdmin
            };

            GUIHandler.Instance.CacheManager.AddUser(user);
            MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Add, user);
            CloseWindow.Invoke();
        }

        private bool CanSubmitUser(object obj)
        {
            return true;
        }
        private void DeleteUser(object obj)
        {
            MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Delete, SelectedUser);
        }

        private bool CanDeleteUser(object obj)
        {
            return SelectedUser != null;
        }


        private bool CanEditUser(object obj)
        {
            return SelectedUser != null;
        }

        private void EditUser(object obj)
        {
            EditUserView editUserView = new EditUserView();
            editUserView.LoadUser(SelectedUser);

            editUserView.ShowDialog();
        }
    }
}
