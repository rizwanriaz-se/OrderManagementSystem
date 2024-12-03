using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Commands;
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

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<User> Users { get; private set; }

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

        public ICommand DeleteUserCommand { get; set; }

        public UserViewModel()
        {
            Users = GUIHandler.GetInstance().CacheManager.GetAllUsers();
            EditUserCommand = new RelayCommand(EditUser, CanEditUser);
            DeleteUserCommand = new RelayCommand(DeleteUser, CanDeleteUser);

        }

        private void DeleteUser(object obj)
        {
            GUIHandler.GetInstance().CacheManager.DeleteUser(SelectedUser);
            //Users.Remove(SelectedUser);
        }

        private bool CanDeleteUser(object obj)
        {
            return true;
        }


        private bool CanEditUser(object obj)
        {
            return true;
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
