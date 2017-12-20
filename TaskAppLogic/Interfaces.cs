using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAppLogic
{
    public enum Priority
    {
        Low,
        Medium,
        High
    }

    public enum GroupRole
    {
        Member,
        Owner
    }

    public interface IUser
    {
        string UserName { get; }
    }

    public interface ILoggedInUser
    {
        IUser User { get; }
    }

    public interface ITask
    {
        string Title { get; set; }
        string Description { get; set; }
        IUser AssignedTo { get; set; }
        DateTime Due { get; set; }
        Priority Priority { get; set; }
        bool Completed { get; set; }
    }

    public interface IGroup
    {
        string Name { get; set; }

        IEnumerable<IUser> GetUsers();
        bool IsUserInGroup(IUser user);
        void AddUser(ILoggedInUser loggedInUser, IUser userToAdd);
        void RemoveUser(ILoggedInUser loggedInUser, IUser userToRemove);
        GroupRole GetRoleForUser(IUser user);
        void SetRoleForUser(ILoggedInUser loggedInUser, IUser user, GroupRole role);
    }

    public interface IUserDatabase
    {
        ILoggedInUser Login(string username, string password);
        IUser GetUser(string username);
    }

    public interface ITaskDatabase
    {
        IEnumerable<ITask> GetTasks(IUser user);
        ITask NewTask();
        void SaveTask(ITask task);
    }

    public interface IGroupDatabase
    {
        IEnumerable<IGroup> GetGroups(IUser user);
        IGroup NewGroup();
        void SaveGroup(IGroup Group);
    }
}
