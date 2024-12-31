using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystemServer.Repository;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for EditUserView.xaml
    /// </summary>
    public partial class EditUserView : ThemedWindow
    {
        EditUserViewModel editUserViewModel = null;
        public EditUserView()
        {
            InitializeComponent();

            editUserViewModel = new EditUserViewModel();
            DataContext = editUserViewModel;
            editUserViewModel.CloseWindow = this.Close;
        }

        public void LoadUser(User SelectedUser)
        {
            //if (SelectedUser == null) throw new ArgumentNullException(nameof(SelectedUser));

            editUserViewModel.Id = SelectedUser.Id;
            editUserViewModel.UserNameText = SelectedUser.Name;
            editUserViewModel.UserEmailText = SelectedUser.Email;
            editUserViewModel.UserPhoneText = SelectedUser.Phone;
            editUserViewModel.UserPasswordText = SelectedUser.Password;
            editUserViewModel.UserIsAdmin = SelectedUser.IsAdmin;
        }
    }
}
