using Terminal.Gui;
using LifeTasker.Services;
using LifeTasker.Utilities;
using System.Data;
using LifeTasker.Models;
using System.Threading.Tasks;
using LifeTasker.Views;
using LifeTasker.ViewModels;

namespace LifePlanner
{
    class Program
    {
        static void Main()
        {
            Application.Init();

            var viewModel = new TaskViewModel();
            var mainWindow = new MainWindow(viewModel);

            Application.Top.Add(mainWindow);
            Application.Run();
        }
    }
}