using Terminal.Gui;
using LifeTasker.Models;
using LifeTasker.Services;

namespace LifeTasker.Views
{
    public class TaskDialog : Dialog
    {
        private readonly LifeTask _task;
        private readonly bool _isNew;
        private TextField _titleField;
        private ComboBox _priorityDropdown;
        private ComboBox _categoryDropdown;

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
            _titleField = new TextField(_task.Title ?? "")
            {
                X = 1,
                Y = 2,
                Width = Dim.Fill() - 2
            };

            // Priority Dropdown
            var priorityLabel = new Label("Priority:") { X = 1, Y = 4 };
            _priorityDropdown = new ComboBox()
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
            _categoryDropdown = new ComboBox()
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
            btnSave.Clicked += () => SaveTask();

            // Add all controls to the dialog
            Add(titleLabel, _titleField,
                priorityLabel, _priorityDropdown,
                categoryLabel, _categoryDropdown,
                btnSave);
        }

        private void SaveTask()
        {
            if (_titleField == null || _priorityDropdown == null || _categoryDropdown == null)
            {
                MessageBox.ErrorQuery("Error", "Dialog controls are not properly initialized.", "OK");
                return;
            }

            Result = new LifeTask
            {
                Title = _titleField.Text.ToString(),
                Priority = (Priority)_priorityDropdown.SelectedItem,
                Category = (Category)_categoryDropdown.SelectedItem
            };
            Application.RequestStop();
        }
    }
}