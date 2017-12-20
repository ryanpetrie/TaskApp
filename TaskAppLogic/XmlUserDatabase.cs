using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography;

namespace TaskAppLogic
{
    public class XmlUserDatabase : IUserDatabase
    {
        public XmlUserDatabase(string filename)
        {
            mFilename = filename;
            if (File.Exists(filename))
            {
                using (var file = File.OpenRead(filename))
                {
                    mUsers = (List<XmlUser>)mSerializer.Deserialize(file);
                }
            }
            else
            {
                mUsers = new List<XmlUser>();
            }
        }

        public IUser GetUser(string username)
        {
            return mUsers.Find(u => u.UserName == username);
        }

        public ILoggedInUser Login(string username, string password)
        {
            password = HashPassword(password);
            var user = mUsers.Find(u => u.UserName == username && u.PasswordHash == password);
            return (user != null) ? new LoggedInUser { User = user } : null;
        }

        public bool AddUser(string username, string password)
        {
            if (GetUser(username) != null) return false;

            var user = new XmlUser { UserName = username, PasswordHash = HashPassword(password) };
            mUsers.Add(user);
            Save();
            return true;
        }

        internal void Save()
        {
            using (var file = File.Create(mFilename))
            {
                mSerializer.Serialize(file, mUsers);
            }
        }

        internal static string HashPassword(string password)
        {
            byte[] hash = sHasher.ComputeHash(Encoding.UTF8.GetBytes(password + sSalt));
            return Convert.ToBase64String(hash);
        }

        private static HashAlgorithm sHasher = SHA256.Create();
        private static string sSalt = "Please select a task from the list.";

        private List<XmlUser> mUsers;
        private readonly string mFilename;
        private readonly XmlSerializer mSerializer = new XmlSerializer(typeof(List<XmlUser>));
    }


    public class XmlUser : IUser
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }

    class LoggedInUser : ILoggedInUser
    {
        public IUser User { get; internal set; }
    }
}
