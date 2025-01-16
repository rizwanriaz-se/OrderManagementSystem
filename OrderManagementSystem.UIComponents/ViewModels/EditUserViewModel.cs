using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;
using OrderManagementSystemServer.Repository;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class EditUserViewModel : INotifyPropertyChanged
    {

        public Action CloseWindow { get; set; }

        public int Id { get; set; }

        public string UserNameText { get; set; }


        public string UserEmailText { get; set; }

        public User.UserApprovalStates UserApprovalStatus { get; set; }


        public string UserPhoneText { get; set; }

        public string UserPasswordText { get; set; }

        public bool UserIsArchived { get; set; }

        public bool UserIsAdmin { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;


        public ICommand SaveUserCommand { get; set; }

        public EditUserViewModel()
        {
            SaveUserCommand = new RelayCommand(SaveUser);
        }


        private void SaveUser(object obj)
        {
            
            ValidateInputs();

            User user = new User
            {
                Id = Id,
                Name = UserNameText,
                Email = UserEmailText,
                Phone = UserPhoneText,
                IsArchived = UserIsArchived,
                Password = UserPasswordText,
                IsAdmin = UserIsAdmin,
                UserApprovalStatus = UserApprovalStatus,
            };



            GUIHandler.Instance.ClientManager.SendMessage(MessageType.User, MessageAction.Update, user);

            CloseWindow.Invoke();


        }

        private void ValidateInputs()
        {
            if (UserNameText == null)
            {
                DXMessageBox.Show("Name field must not be empty.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            if (UserEmailText == null || !UserEmailText.Contains('@') || UserEmailText.Length <= 6)
            {
                DXMessageBox.Show("Email field must not be empty, should contain @ and have atleast 6 characters", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            if (UserApprovalStatus == null)
            {
                DXMessageBox.Show("User Approval Status must be selected.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            if (UserPhoneText == null || UserPhoneText.Length != 12)
            {
                DXMessageBox.Show("Phone number field must not be empty, and should contain 11 digits", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            if (UserPasswordText == null || UserPasswordText.Length <= 6)
            {
                DXMessageBox.Show("Password field must not be empty, and should contain atleast 6 characters", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
        }
    }
}
