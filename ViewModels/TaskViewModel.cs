using LifeTasker.Models;
using LifeTasker.Services;
using System.Collections.Generic;
using System;

namespace LifeTasker.ViewModels
{
    public class TaskViewModel
    {
        public List<LifeTask> Tasks { get; private set; }

        public event Action DataUpdated;

        public TaskViewModel()
        {
            LoadData();
        }

        public void LoadData()
        {
            Tasks = Storage.LoadData();
            PriorityManager.CheckDeadlines(Tasks);
            Storage.SaveData(Tasks);
        }

        public void SaveData()
        {
            Storage.SaveData(Tasks);
            DataUpdated?.Invoke();
        }

        public void AddTask(LifeTask task)
        {
            Tasks.Add(task);
            SaveData();
        }

        public void UpdateTask(LifeTask existingTask, LifeTask newTask)
        {
            var index = Tasks.IndexOf(existingTask);
            if (index >= 0)
            {
                Tasks[index] = newTask;
                SaveData();
            }
        }

        public void CheckDeadlines()
        {
            PriorityManager.CheckDeadlines(Tasks);
            SaveData();
        }
    }
}