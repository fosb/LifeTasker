using Terminal.Gui;
using LifeTasker.Models;
using LifeTasker.Services;

namespace LifeTasker.Views
{
    public class TaskDialog : Dialog
    {
        private readonly LifeTask _task;
        private readonly bool _isNew;

        public LifeTask Result { get; private set; }

        public TaskDialog(LifeTask existingTask = null)
            : base(existingTask == null ? "New Task" : "Edit Task", 50, 20)
        {
            _isNew = existingTask == null;
            _task = existingTask ?? new LifeTask();

            X = Pos.Center();
            Y = Pos.Center();

            SetupControls();
        }

        private void SetupControls()
        {
            // Title Field
            var titleLabel = new Label("Title:") { X = 1, Y = 1 };
            var titleField = new TextField(_task.Title ?? "")
            {
                X = 1,
                Y = 2,
                Width = Dim.Fill() - 2
            };

            // Priority Dropdown
            var priorityLabel = new Label("Priority:") { X = 1, Y = 4 };
            var priorityDropdown = new ComboBox()
            {
                X = 1,
                Y = 5,
                Width = 20,
                Height = 5,
                Source = new ListWrapper(Enum.GetNames(typeof(Priority))),
                SelectedItem = (int)_task.Priority
            };

            // Category Dropdown
            var categoryLabel = new Label("Category:") { X = 1, Y = 7 };
            var categoryDropdown = new ComboBox()
            {
                X = 1,
                Y = 8,
                Width = 20,
                Height = 5,
                Source = new ListWrapper(Enum.GetNames(typeof(Category))),
                SelectedItem = (int)_task.Category
            };

            // Save Button
            var btnSave = new Button("Save") { X = 1, Y = 11 };
            btnSave.Clicked += () => SaveTask(titleField, priorityDropdown, categoryDropdown);

            Add(titleLabel, titleField,
                priorityLabel, priorityDropdown,
                categoryLabel, categoryDropdown,
                btnSave);
        }

        private void SaveTask(TextField titleField, ComboBox priorityDropdown, ComboBox categoryDropdown)
        {
            Result = new LifeTask
            {
                Title = titleField.Text.ToString(),
                Priority = (Priority)priorityDropdown.SelectedItem,
                Category = (Category)categoryDropdown.SelectedItem,
                Deadline = DateTime.Now.AddDays(PriorityManager.GetDaysForPriority((Priority)priorityDropdown.SelectedItem))
            };

            if (_isNew)
            {
                Application.RequestStop();
                return;
            }

            // Preserve existing task ID if editing
            Result.Id = _task.Id;
            Application.RequestStop();
        }
    }
}