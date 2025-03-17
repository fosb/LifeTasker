using Terminal.Gui;
using LifeTasker.Services;
using LifeTasker.Utilities;
using System.Data;
using LifeTasker.Models;
using System.Threading.Tasks;
using LifeTasker.Views;

namespace LifePlanner
{
    class Program
    {
        private static List<LifeTask> _tasks;

        static void Main()
        {
            // Initialize data
            _tasks = Storage.LoadData();
            PriorityManager.CheckDeadlines(_tasks);
            Storage.SaveData(_tasks);

            // GUI Setup
            Application.Init();
            var top = Application.Top;

            Window mainWindow = new MainWindow(_tasks);

            Application.Top.Add(mainWindow);
            Application.Run();
        }
    }
}