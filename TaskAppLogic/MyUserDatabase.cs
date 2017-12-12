using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAppLogic
{
    public class MyUserDatabase : IUserDatabase
    {
        public ILoggedInUser Login(string username, string password)
        {
            var user = new MyLoggedInUser();
            user.UserName = username;
            mUsers.Add(user);
            return user;
        }

        public IUser GetUser(string username)
        {
            return mUsers.Find(u => u.UserName == username);
        }

        private readonly List<MyUser> mUsers = new List<MyUser>();
    }

    public class MyUser : IUser
    {
        public string UserName { get; internal set; }
    }

    class MyLoggedInUser : MyUser, ILoggedInUser
    {
    }

}
