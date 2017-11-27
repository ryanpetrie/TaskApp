using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAppLogic
{
    public class MyTaskDatabase : ITaskDatabase
    {
        public IEnumerable<ITask> GetTasks(IUser user)
        {
            foreach (MyTask task in mTasks)
            {
                if (task.AssignedTo == user)
                {
                    yield return task;
                }
            }
        }

        public ITask NewTask()
        {
            return new MyTask();
        }

        public void SaveTask(ITask task)
        {
            if (task is MyTask)
            {
                mTasks.Add((MyTask)task);
            }
            else
            {
                throw new ArgumentException("I want MyTask!");
            }
        }

        private readonly List<MyTask> mTasks = new List<MyTask>();
    }

    internal class MyTask : ITask
    {
        public string Title { get; set; }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IUser AssignedTo { get; set; }
        public DateTime Due { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Priority Priority { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
