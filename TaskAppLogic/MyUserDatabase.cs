using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAppLogic
{
    public class MyUserDatabase : IUserDatabase
    {
        public IUser GetUser(string username, string password)
        {
            var user = new MyUser();
            user.UserName = username;
            return user;
        }
    }

    public class MyUser : IUser
    {
        public string UserName { get; internal set; }
    }

}
