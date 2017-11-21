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

    public interface IUser
    {
        string UserName { get; }
    }

    public interface ITask
    {
        string Title { get; set; }
        string Description { get; set; }
        string AssignedTo { get; set; }
        DateTime Due { get; set; }
        Priority Priority { get; set; }
    }

    public interface IUserDatabase
    {
        IUser GetUser(string username, string password);
    }

    public interface ITaskDatabase
    {
        IEnumerable<ITask> GetTasks(string userName);
        ITask NewTask();
        void SaveTask(ITask task);
    }
}
