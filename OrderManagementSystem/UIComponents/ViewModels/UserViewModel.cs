using OrderManagementSystem.Cache.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.UIComponents.ViewModels
{
    public class UserViewModel
    {
        public ObservableCollection<User> Users { get; private set; }
        public UserViewModel()
        {
            Users = UserManager.GetAllUsers();
        }
    }
}
