using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAppLogic;

namespace TaskApp
{
    class App
    {
        public App()
        {
            mUserDb = new XmlUserDatabase("users.xml");
            mTaskDb = new XmlTaskDatabase(mUserDb, "tasks.xml");
        }

        public void Run()
        {
            for (int attempts = 0; attempts < 3; ++attempts)
            {
                mLoggedInUser = SignIn();
                if (mLoggedInUser != null) break;
                Console.WriteLine("Error: login attempt failed.");
            }

            // If we couldn't log in after a few tries, exit the program.
            if (mLoggedInUser == null) return;

            MainMenu();
        }

        private ILoggedInUser SignIn()
        {
            Console.Write("Enter your user name: ");
            string username = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = ConsoleHelpers.ReadPasswordLine();

            return mUserDb.Login(username, password);
        }

        private void MainMenu()
        {
            while (true)
            {
                Console.Write(@"
Main menu:
    (T)o-do list
    (A)dd a task
    (C)omplete a task
    (L)ist all my tasks
    (Q)uit

What'll it be? ");

                switch (char.ToLower(Console.ReadKey().KeyChar))
                {
                    case 't':
                        Console.WriteLine($"\nOk, here is the to-do list for {mLoggedInUser.User.UserName}:");
                        var tasks = mTaskDb.GetTasks(mLoggedInUser.User).Where(t => !t.Completed).OrderByDescending(t => t.Priority);
                        ListTasks(tasks);
                        break;
                    case 'l':
                        Console.WriteLine($"\nOk, here are all the tasks for {mLoggedInUser.User.UserName}:");
                        ListTasks(mTaskDb.GetTasks(mLoggedInUser.User));
                        break;
                    case 'a':
                        CreateTask(mLoggedInUser.User);
                        break;
                    case 'q':
                        Console.WriteLine($"\nSee ya later, {mLoggedInUser.User.UserName}!");
                        return;
                    case 'c':
                        Console.WriteLine();
                        CompleteTask(mLoggedInUser.User);
                        break;
                    default:
                        Console.WriteLine("\nI don't know that command.");
                        break;
                }
            }
        }

        private void CompleteTask(IUser user)
        {
            ITask[] tasks = mTaskDb.GetTasks(user).Where(t => !t.Completed).ToArray();
            if (tasks.Length == 0)
            {
                Console.WriteLine("No tasks to complete!");
                return;
            }

            ITask task;
            for (int i=0; i < tasks.Length; ++i)
            {
                task = tasks[i];
                Console.WriteLine($"\t({i}) {task.Title}, due {task.Due}, priority {task.Priority}");
            }

            Console.Write("Which task do you want to complete (Enter to abort)? ");
            string s = Console.ReadLine();
            if (!int.TryParse(s, out int index) || index < 0 || index >= tasks.Length)
            {
                Console.WriteLine("No task changed.");
                return;
            }

            task = tasks[index];
            task.Completed = true;
            mTaskDb.SaveTask(task);
            Console.WriteLine($"Task '{task.Title}' completed.");
        }

        private void CreateTask(IUser user)
        {
            Console.Write("\nNew task title: ");
            string title = Console.ReadLine();
            Console.Write("Description: ");
            string desc = Console.ReadLine();
            DateTime due = InputDateTime("Due date and time: ");
            var priority = InputEnum<Priority>("Priority: ");

            Console.Write("Do you want to create this task? (Y)es or (N)o: ");
            char key = char.ToLower(Console.ReadKey().KeyChar);
            if (key != 'y')
            {
                Console.WriteLine("\nNo task created.");
            }
            else
            {
                ITask task = mTaskDb.NewTask();
                task.AssignedTo = user;
                task.Completed = false;
                task.Description = desc;
                task.Due = due;
                task.Priority = priority;
                task.Title = title;
                mTaskDb.SaveTask(task);

                Console.WriteLine("\nTask created!");
            }
        }

        private static DateTime InputDateTime(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine();
                if (DateTime.TryParse(s, out DateTime dt))
                {
                    return dt;
                }
                if (TimeSpan.TryParse(s, out TimeSpan ts))
                {
                    return DateTime.Now + ts;
                }

                Console.WriteLine("Sorry, that is not in a format I understand.");
            }
        }

        private static T InputEnum<T>(string prompt) where T : struct
        {
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine();
                if (Enum.TryParse<T>(s, true, out T result))
                {
                    return result;
                }

                var enumNames = string.Join(",", Enum.GetNames(typeof(T)));
                Console.WriteLine($"Sorry, that's not a valid {typeof(T).Name}.\n  Valid values: ({enumNames}).");
            }
        }

        private void ListTasks(IEnumerable<ITask> tasks)
        {
            foreach (var task in tasks)
            {
                Console.WriteLine($"\t{task.Title}, due {task.Due}, priority {task.Priority}");
            }
            Console.WriteLine();
        }

        private readonly IUserDatabase mUserDb;
        private readonly ITaskDatabase mTaskDb;
        private ILoggedInUser mLoggedInUser = null;
    }
}
