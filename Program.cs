using Terminal.Gui;
using LifeTasker.Services;
using LifeTasker.Utilities;
using System.Data;
using LifeTasker.Models;
using System.Threading.Tasks;

namespace LifePlanner
{
    class Program
    {
        private static List<LifeTask> _tasks;
        private static TableView _taskTableView;
        private static TableView _priorityTableView;

        static void Main()
        {
            // Initialize data
            _tasks = Storage.LoadData();
            PriorityManager.CheckDeadlines(_tasks);
            Storage.SaveData(_tasks);

            // GUI Setup
            Application.Init();
            var top = Application.Top;

            // Create main window
            var win = new Window("Life Planner")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            SetupTables();

            win.Add(_taskTableView, _priorityTableView);

            SetupControls(win);
            SetupColors();
            SetupKeyboardShortcuts();
            SetupAutoRefresh();

            top.Add(win);
            Application.Run();
        }

        private static void SetupTables()
        {
            // Task Table
            _taskTableView = new TableView
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(70),
                Height = Dim.Fill() - 3
            };
            UpdateTaskTable();

            // Priority Table
            _priorityTableView = new TableView
            {
                X = Pos.Right(_taskTableView),
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 3
            };
            UpdatePriorityTable();
        }

        private static void UpdateTaskTable()
        {
            // Initialize the DataTable
            var dt = new DataTable();
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Priority", typeof(Priority));
            dt.Columns.Add("Deadline", typeof(string));

            // Populate the DataTable with tasks
            foreach (var task in _tasks)
            {
                dt.Rows.Add(
                    task.Title,
                    task.Priority,
                    task.Deadline.ToString("yyyy-MM-dd"));
            }

            // Assign the DataTable to the TableView
            _taskTableView.Table = dt;

            // Apply styling to the rows
            foreach (DataColumn column in dt.Columns)
            {
                var columnStyle = _taskTableView.Style.GetOrCreateColumnStyle(column);
                columnStyle.ColorGetter = data =>
                {
                    var row = data.Table.Rows[data.RowIndex];
                    var priority = (Priority)row["Priority"];
                    return GetPriorityColor(priority);
                };
            }
        }

        private static void SetupControls(Window win)
        {
            var controlsFrame = new FrameView("Controls")
            {
                X = 0,
                Y = Pos.AnchorEnd(3),
                Width = Dim.Fill(),
                Height = 3
            };

            var btnAdd = new Button("Add Task (Ctrl+N)")
            {
                X = 0,
                Y = 0
            };
            btnAdd.Clicked += () => ShowTaskDialog(null);

            var btnSave = new Button("Save Tasks (Ctrl+S")
            {
                X = Pos.Right(btnAdd) + 2,
                Y = 0
            };

            var btnQuit = new Button("Quit (Ctrl+Q)")
            {
                X = Pos.Right(btnSave) + 2,
                Y = 0
            };
            btnQuit.Clicked += () => Application.RequestStop();

            controlsFrame.Add(btnAdd, btnSave, btnQuit);

            win.Add(controlsFrame);
        }

        private static void SetupColors()
        {
            // Define color schemes for priorities
            Colors.Base.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);
            Colors.Base.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Gray);
        }

        private static ColorScheme GetPriorityColor(Priority priority)
        {
            return priority switch
            {
                Priority.Extreme => CreateFullScheme(Color.BrightRed),
                Priority.Urgent => CreateFullScheme(Color.Red),
                Priority.High => CreateFullScheme(Color.BrightYellow),
                Priority.Medium => CreateFullScheme(Color.Green),
                Priority.Low => CreateFullScheme(Color.Blue),
                Priority.Minimum => CreateFullScheme(Color.Gray),
                _ => CreateFullScheme(Color.White)
            };
        }

        private static void SetupKeyboardShortcuts()
        {
            Application.Top.KeyPress += e =>
            {
                if (e.KeyEvent.IsCtrl)
                {
                    switch (e.KeyEvent.Key)
                    {
                        case Key.N:
                            ShowTaskDialog();
                            e.Handled = true;
                            break;
                        case Key.S:
                            Storage.SaveData(_tasks);
                            UpdateTaskTable();
                            UpdatePriorityTable();
                            e.Handled = true;
                            break;
                        case Key.Q:
                            Application.RequestStop();
                            e.Handled = true;
                            break;
                    }
                }
            };
        }

        private static void SetupAutoRefresh()
        {
            Application.MainLoop.AddTimeout(TimeSpan.FromMinutes(1), _ =>
            {
                PriorityManager.CheckDeadlines(_tasks);
                UpdateTaskTable();
                UpdatePriorityTable();
                return true;
            });
        }

        private static void UpdatePriorityTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("Priority Task", typeof(string));
            dt.Columns.Add("Score %", typeof(string));
            dt.Columns.Add("Deadline", typeof(string));

            var topTasks = PriorityManager.GetPriorityTasks(_tasks);
            foreach (var task in topTasks)
            {
                var row = dt.Rows.Add(
                    task.Title,
                    $"{task.PriorityScore:F1}%",
                    task.Deadline.ToString("yyyy-MM-dd"));

                // Apply styling to the row
                _priorityTableView.SetStyle(dt.Rows.IndexOf(row), GetPriorityColor(task.Priority));
            }

            _priorityTableView.Table = dt;
            _priorityTableView.Update();
        }

        private static void ShowTaskDialog(LifeTask? existingTask = null)
        {
            var isNew = existingTask == null;
            var task = existingTask ?? new LifeTask();

            var dialog = new Dialog(isNew ? "New Task" : "Edit Task", 50, 20)
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };

            // Title Field
            var titleLabel = new Label("Title:") { X = 1, Y = 1 };
            var titleField = new TextField(task.Title ?? "")
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
                Height = 5, // Height of the dropdown list
                Source = new ListWrapper(Enum.GetNames(typeof(Priority))), // Wrap the list
                SelectedItem = (int)task.Priority
            };

            // Category Dropdown
            var categoryLabel = new Label("Category:") { X = 1, Y = 7 };
            var categoryDropdown = new ComboBox()
            {
                X = 1,
                Y = 8,
                Width = 20,
                Height = 5, // Height of the dropdown list
                Source = new ListWrapper(Enum.GetNames(typeof(Category))), // Wrap the list
                SelectedItem = (int)task.Category
            };

            // Save Button
            var btnSave = new Button("Save")
            {
                X = 1,
                Y = 11
            };

            btnSave.Clicked += () =>
            {
                task.Title = titleField.Text.ToString();
                task.Priority = (Priority)priorityDropdown.SelectedItem;
                task.Category = (Category)categoryDropdown.SelectedItem;
                task.Deadline = DateTime.Now.AddDays(PriorityManager.GetDaysForPriority(task.Priority));

                if (isNew) _tasks.Add(task);

                Storage.SaveData(_tasks);
                UpdateTaskTable();
                UpdatePriorityTable();
                Application.RequestStop();
            };

            // Add all controls to the dialog
            dialog.Add(
                titleLabel, titleField,
                priorityLabel, priorityDropdown,
                categoryLabel, categoryDropdown,
                btnSave
            );

            Application.Run(dialog);
        }

        private static ColorScheme CreateFullScheme(Color color)
        {
            return new ColorScheme
            {
                Normal = MakeAttr(color, Color.Black),
                HotNormal = MakeAttr(color, Color.Black),
                Focus = MakeAttr(color, Color.Black),
                HotFocus = MakeAttr(color, Color.Black),

            };
        }

        private static Terminal.Gui.Attribute MakeAttr(Color fg, Color bg) => new Terminal.Gui.Attribute(fg, bg);
    }
}