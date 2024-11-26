using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Cache.Models
{
    public class UserManager
    {
        public static ObservableCollection<User> _AllUsers = new ObservableCollection<User>()
        {
            new User(){ Id = 1, Name = "John Doe", Email = "johndoe@example.com", Phone = "1234567890", Password = "Z1W(7MGkQ0", IsAdmin = true },
            new User(){ Id = 2, Name = "Jane David", Email = "janedavid@test.com", Phone = "0987654321", Password = "43GyyVMj_n ", IsAdmin = false },
            new User(){ Id = 3, Name = "John Smith", Email = "johnsmith@xyz.com", Phone = "1234567890", Password = "&qb42KSwtz", IsAdmin = false },
            new User(){ Id = 4, Name = "Taylor Travis", Email = "taylortravis@example.com", Phone = "0987654321", Password = "password", IsAdmin = false },
            new User(){ Id = 5, Name="Andrea Hernandez", Email="andreahernandez@test.com", Phone="3726139894", Password="#THfdeD72", IsAdmin=false },
        };

        public static ObservableCollection<User> GetAllUsers()
        {
            return _AllUsers;
        }

        public static User GetUserByID(int id)
        {
            return _AllUsers.FirstOrDefault(u => u.Id == id);
        }

    }
}
