using Terminal.Gui;
using Attribute = Terminal.Gui.Attribute;

namespace LifeTasker.Utilities
{
    public static class TableViewExtensions
    {
        public static void SetStyle(this TableView tableView, int row, ColorScheme scheme)
        {
            // Ensure the row is within bounds
            /*if (row < 0 || row >= tableView.Table.Rows.Count)
                return;

            // Apply the color scheme to all columns in the row
            for (int col = 0; col < tableView.Table.Columns.Count; col++)
            {
                var column = tableView.Table.Columns[col];
                var columnStyle = tableView.Style.GetOrCreateColumnStyle(column);

                //columnStyle.ColorGetter = data => data.RowIndex == row ? (Attribute?)scheme.Normal : null;
            }*/
        }
    }
}