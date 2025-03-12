using LifeTasker.Models;

namespace LifeTasker.Services
{
    public static class PriorityManager
    {
        public static void CheckDeadlines(List<LifeTask> tasks)
        {
            foreach (var task in tasks.Where(t => !t.IsCompleted))
            {
                while (DateTime.Now > task.Deadline && task.Priority > Priority.Extreme)
                {
                    task.Priority--;
                    task.Deadline = DateTime.Now.AddDays(GetDaysForPriority(task.Priority));
                }
            }
        }

        public static int GetDaysForPriority(Priority priority) => priority switch
        {
            Priority.Extreme => 1,
            Priority.Urgent => 2,
            Priority.High => 3,
            Priority.Medium => 5,
            Priority.Low => 7,
            Priority.Minimum => 14,
            _ => 14
        };

        public static List<LifeTask> GetPriorityTasks(List<LifeTask> tasks)
        {
            const int baseWeight = 100;
            var scoredTasks = tasks
                .Where(t => !t.IsCompleted)
                .Select(t => new
                {
                    Task = t,
                    Score = (Priority.Extreme - t.Priority + 1) * baseWeight *
                           (1 / (Math.Max((t.Deadline - DateTime.Now).TotalDays, 0) + 1))
                })
                .OrderByDescending(t => t.Score)
                .Take(5)
                .ToList();

            if (!scoredTasks.Any()) return new List<LifeTask>();

            double totalScore = scoredTasks.Sum(t => t.Score);
            foreach (var st in scoredTasks)
            {
                st.Task.PriorityScore = (st.Score / totalScore) * 100;
            }

            return scoredTasks.Select(st => st.Task).ToList();
        }
    }
}