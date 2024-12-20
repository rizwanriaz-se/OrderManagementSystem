﻿//using OrderManagementSystem.Commands;
using OrderManagementSystem.UIComponents.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
//using OrderManagementSystem.Classes;
using OrderManagementSystem.UIComponents.Commands;
//using OrderManagementSystem.Repositories;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystemServer.Repository;

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
        public ICommand EditUserCommand { get; set; }

        public ICommand SubmitUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }

        public UserViewModel()
        {
            Users = GUIHandler.GetInstance().CacheManager.GetAllUsers();
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

            GUIHandler.GetInstance().CacheManager.AddUser(user);
            GUIHandler.GetInstance().MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Add, user);
            CloseWindow.Invoke();
            //Users.Add(user);
        }

        private bool CanSubmitUser(object obj)
        {
            return true;
        }
        private void DeleteUser(object obj)
        {
            GUIHandler.GetInstance().CacheManager.DeleteUser(SelectedUser);
            GUIHandler.GetInstance().MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Delete, SelectedUser);
            //Users.Remove(SelectedUser);
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

            EditUserViewModel editUserViewModel = new EditUserViewModel(SelectedUser);
            editUserView.DataContext = editUserViewModel;
            editUserViewModel.CloseWindow = editUserView.Close;
            editUserView.ShowDialog();
        }
    }
}
