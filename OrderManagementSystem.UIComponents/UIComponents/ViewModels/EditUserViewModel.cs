using DevExpress.Data;
using DevExpress.DirectX.Common.DirectWrite;
using DevExpress.XtraRichEdit.Fields.Expression;
//using OrderManagementSystem.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OrderManagementSystem.Repositories.Repositories;
//using OrderManagementSystem.Classes;
using OrderManagementSystem.UIComponents.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Commands;

namespace OrderManagementSystem.UIComponents.UIComponents.ViewModels
{
    public class EditUserViewModel: INotifyDataErrorInfo
    {
        private User _User;

        private string _UserNameText;
        private string _UserEmailText;
        private string _UserPhoneText;
        private string _UserPasswordText;

        public Action CloseWindow { get; set; }

        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string UserNameText
        {

            get { return _UserNameText; }
            set
            {
                _UserNameText = value;
                Validate(nameof(UserNameText), _UserNameText);
            }
        }

        [Required(ErrorMessage = "Email is required")]
        public string UserEmailText
        {
            get { return _UserEmailText; }
            set
            {
                _UserEmailText = value;
                Validate(nameof(UserEmailText), _UserEmailText);
            }
        }

        [Required(ErrorMessage = "Phone Number is required")]
        public string UserPhoneText
        {

            get { return _UserPhoneText; }
            set
            {
                _UserPhoneText = value;
                Validate(nameof(UserPhoneText), _UserPhoneText);
            }
        }

        [Required(ErrorMessage = "Password is required")]
        public string UserPasswordText
        {

            get { return _UserPasswordText; }
            set
            {
                _UserPasswordText = value;
                Validate(nameof(UserPasswordText), _UserPasswordText);
            }
        }

        public void Validate(string propertyName, object propertyValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyName };
            Validator.TryValidateProperty(propertyValue, context, results);

            if (results.Any())
            {
                Errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                Errors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            SaveUserCommand.RaiseCanExecuteEventChanged();
        }

        public bool UserIsAdmin { get; set; }

        public bool HasErrors => Errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        public IEnumerable GetErrors(string propertyName)
        {
            if (Errors.ContainsKey(propertyName))
            {
                return Errors[propertyName];
            }
            return null;
        }

        public RelayCommand SaveUserCommand { get; set; }

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
            return Validator.TryValidateObject(this, new ValidationContext(this), null, true);
        }

        private void SaveUser(object obj)
        {
            _User.Name = UserNameText;
            _User.Email = UserEmailText;
            _User.Phone = UserPhoneText;
            _User.Password = UserPasswordText;
            _User.IsAdmin = UserIsAdmin;


            //GUIHandler.GetInstance().CacheManager.UpdateUser(_User);
            
            GUIHandler.GetInstance().MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Update, _User);

            CloseWindow.Invoke();

        }
    }
}
