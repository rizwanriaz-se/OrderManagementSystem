using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystem.UIComponents.Views;
using OrderManagementSystemServer.Repository;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<User> Users { get; set; }
        public ICollectionView FilteredUsersView { get; set; }

        public Action CloseWindow { get; set; }

        public string UserNameText { get; set; }
        public string UserPasswordText { get; set; }
        public string UserEmailText { get; set; }
        public string UserPhoneText { get; set; }
        public bool UserIsAdmin { get; set; }

        public bool UserIsArchived { get; set; }

        public User SelectedUser { get; set; }

        public ICommand EditUserCommand { get; set; }

        public ICommand DeleteUserCommand { get; set; }

        public UserViewModel()
        {
            Users = GUIHandler.Instance.CacheManager.Users;
            Users.ForEach(u => u.PropertyChanged += OnUserPropertyChanged);

            FilteredUsersView = CollectionViewSource.GetDefaultView(Users);
            FilteredUsersView.Filter = FilterUsers;

            EditUserCommand = new RelayCommand(EditUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
        }

        private void OnUserPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            FilteredUsersView.Refresh();
        }



        private bool FilterUsers(object item)
        {
            if (item is not User user) return false;

            if (user.IsArchived) return false;
            return true;
        }



        private void DeleteUser(object obj)
        {
            MessageBoxResult confirmationResult = DXMessageBox.Show($"Are you sure you want to delete selected user: {SelectedUser.Name}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirmationResult == MessageBoxResult.Yes)
                ClientManager.Instance.SendMessage(MessageType.User, MessageAction.Delete, SelectedUser.Id);
        }


        private void EditUser(object obj)
        {
            EditUserView editUserView = new EditUserView();
            editUserView.LoadUser(SelectedUser);

            editUserView.ShowDialog();
        }
    }
}
