using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAppLogic
{
    public class MockUserDatabase : IUserDatabase
    {
        public ILoggedInUser Login(string username, string password)
        {
            var user = mUsers.Find(u => u.UserName == username);
            if (user == null)
            {
                user = new MockUser { UserName = username };
                mUsers.Add(user);
            }
            return new MockLoggedInUser { User = user };
        }

        public IUser GetUser(string username)
        {
            return mUsers.Find(u => u.UserName == username);
        }

        private readonly List<MockUser> mUsers = new List<MockUser>();
    }

    public class MockUser : IUser
    {
        public string UserName { get; internal set; }
    }

    class MockLoggedInUser : ILoggedInUser
    {
        public IUser User { get; internal set; }
    }

}
