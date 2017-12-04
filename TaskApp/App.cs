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

            // Create a temporary task.
            var task = mTaskDb.NewTask();
            task.AssignedTo = mLoggedInUser;
            task.Title = "Do stuff";
            task.Due = DateTime.Now + TimeSpan.FromDays(1);
            task.Priority = Priority.High;
            mTaskDb.SaveTask(task);

            MainMenu();
        }

        private IUser SignIn()
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
    (L)ist my tasks
    (Q)uit

What'll it be? ");

                switch (char.ToLower(Console.ReadKey().KeyChar))
                {
                    case 'l':
                        ListTasks(mLoggedInUser);
                        break;
                    case 'q':
                        Console.WriteLine($"\nSee ya later, {mLoggedInUser.UserName}!");
                        return;
                    default:
                        Console.WriteLine("\nI don't know that command.");
                        break;
                }
            }
        }

        private void ListTasks(IUser user)
        {
            Console.WriteLine($"\nOk, here are the tasks for {user.UserName}:");
            foreach (var task in mTaskDb.GetTasks(user))
            {
                Console.WriteLine($"\t{task.Title}, due {task.Due}, priority {task.Priority}");
            }
            Console.WriteLine();
        }

        private IUserDatabase mUserDb = new MyUserDatabase();
        private ITaskDatabase mTaskDb = new MockTaskDatabase();
        private IUser mLoggedInUser = null;
    }
}
