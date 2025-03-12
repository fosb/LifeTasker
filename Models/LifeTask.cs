using System;

namespace LifeTasker.Models
{
    public class LifeTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public Priority Priority { get; set; }
        public Category Category { get; set; }
        public double PriorityScore { get; set; }
    }

    public enum Priority { Extreme, Urgent, High, Medium, Low, Minimum }
    public enum Category { Adult, Health, Productive, FreeTime, Other }
}