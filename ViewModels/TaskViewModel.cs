using System;
using System.Collections.ObjectModel;
using LifeTasker.Models;
using LifeTasker.Services;

namespace LifeTasker.ViewModels
{
    public class TaskViewModel
    {
        public ObservableCollection<LifeTask> Tasks { get; }
        public Command AddTaskCommand { get; }
        public Command UpdateTaskCommand { get; }

        public TaskViewModel()
        {
            Tasks = new ObservableCollection<LifeTask>(Storage.LoadData());
            Tasks.CollectionChanged += (s, e) => SaveData();

            AddTaskCommand = new Command(AddTask);
            UpdateTaskCommand = new Command(UpdateTask);
        }

        private void AddTask(object parameter)
        {
            if (parameter is LifeTask task)
            {
                Tasks.Add(task);
            }
        }

        private void UpdateTask(object parameter)
        {
            if (parameter is Tuple<LifeTask, LifeTask> tasks)
            {
                var index = Tasks.IndexOf(tasks.Item1);
                if (index >= 0) Tasks[index] = tasks.Item2;
            }
        }

        private void SaveData()
        {
            Storage.SaveData(Tasks.ToList());
        }

        public void CheckDeadlines()
        {
            PriorityManager.CheckDeadlines(Tasks.ToList());
            SaveData();
        }
    }
}