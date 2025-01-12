using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Grid;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystem.UIComponents.Views;
using OrderManagementSystemServer.Repository;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<User> Users { get; private set; }
        public ICollectionView FilteredUsersView { get; private set; }

        private bool m_bUserIsArchived;
        public Action CloseWindow { get; set; }

        public string UserNameText { get; set; }
        public string UserPasswordText { get; set; }
        public string UserEmailText { get; set; }
        public string UserPhoneText { get; set; }
        public bool UserIsAdmin { get; set; }

        public bool UserIsArchived {
            get { return m_bUserIsArchived; }
            set {
                m_bUserIsArchived = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserIsArchived)));
                FilteredUsersView.Refresh();
            } 
        }

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
        //public ICommand<RowFilterArgs> UserFilterCommand { get; set; }

        public ICommand DeleteUserCommand { get; set; }

        public UserViewModel()
        {
            Users = GUIHandler.Instance.CacheManager.GetAllUsers();
            //Users.foreach ((var) item in collection)
            //{

            //}
            
            FilteredUsersView = CollectionViewSource.GetDefaultView(Users);
            FilteredUsersView.Filter = FilterUsers;

            EditUserCommand = new RelayCommand(EditUser);
            //UserFilterCommand = new DelegateCommand<RowFilterArgs>(FilterUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);

        }

        private bool FilterUsers(object item)
        {
            if (item is not User user) return false;

            if (user.IsArchived) return false;
            return true;
        }

        //private void FilterUser(RowFilterArgs args)
        //{
        //    //RowFilterArgs rowFilterArgs = dataRows as RowFilterArgs;
        //    User user = args.Item as User;
        //    if (user.IsArchived)
        //    {
        //        args.Visible = false;
        //    }
        //    else
        //    {
        //        args.Visible = true;
        //    }
        //}


        private void DeleteUser(object obj)
        {            
            MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Delete, SelectedUser.Id);

        }


        private void EditUser(object obj)
        {
            EditUserView editUserView = new EditUserView();
            editUserView.LoadUser(SelectedUser);

            editUserView.ShowDialog();
        }
    }
}
