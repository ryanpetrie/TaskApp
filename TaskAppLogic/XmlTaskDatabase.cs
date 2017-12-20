using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TaskAppLogic
{
    public class XmlTaskDatabase : ITaskDatabase
    {
        public XmlTaskDatabase(IUserDatabase userDatabase, string filename)
        {
            mUserDatabase = userDatabase;
            mFilename = filename;

            if (File.Exists(mFilename))
            {
                using (FileStream input = File.OpenRead(mFilename))
                {
                    mTasks = (List<XmlTask>)mSerializer.Deserialize(input);
                }

                // Fix up references to users.
                foreach (var task in mTasks)
                {
                    task.AssignedTo = mUserDatabase.GetUser(task.AssignedToUserName);
                }
            }
        }

        public IEnumerable<ITask> GetTasks(IUser user)
        {
            return mTasks.Cast<ITask>().Where(t => t.AssignedTo == user);
        }

        public ITask NewTask()
        {
            return new XmlTask();
        }

        public void SaveTask(ITask task)
        {
            if (!(task is XmlTask))
            {
                throw new ArgumentException("I can only handle tasks I created.");
            }

            var xmlTask = (XmlTask)task;

            if (!mTasks.Contains(xmlTask))
            {
                mTasks.Add(xmlTask);
            }

            Save();
        }

        private void Save()
        {
            // Make sure user names are saved.
            foreach (var task in mTasks)
            {
                task.AssignedToUserName = task.AssignedTo?.UserName;
            }

            using (FileStream output = File.Create(mFilename))
            {
                mSerializer.Serialize(output, mTasks);
            }
        }

        private readonly IUserDatabase mUserDatabase;
        private readonly string mFilename;
        private List<XmlTask> mTasks = new List<XmlTask>();
        private readonly XmlSerializer mSerializer = new XmlSerializer(typeof(List<XmlTask>));
    }

    public class XmlTask : ITask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [XmlIgnore] public IUser AssignedTo { get; set; }
        public DateTime Due { get; set; }
        public Priority Priority { get; set; }
        public bool Completed { get; set; }

        [XmlElement("AssignedTo")]
        public string AssignedToUserName;
    }
}
