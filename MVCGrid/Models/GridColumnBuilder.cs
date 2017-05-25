using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Models
{
    public class GridColumnListBuilder<T1>
    {
        private ColumnDefaults _columnDefaults = null;

        public GridColumnListBuilder()
            : this(null)
        {
        }

        public GridColumnListBuilder(ColumnDefaults columnDefaults)
        {
            ColumnBuilders = new List<GridColumnBuilder<T1>>();

            _columnDefaults = columnDefaults;
        }

        public List<GridColumnBuilder<T1>> ColumnBuilders { get; set; }

        public GridColumnBuilder<T1> Add()
        {
            return Add(null, null, null);
        }

        public GridColumnBuilder<T1> Add(string columnName)
        {
            return Add(columnName, null, null);
        }

        public GridColumnBuilder<T1> Add(string columnName, string headerText, Func<T1, string> valueExpression)
        {
            GridColumnBuilder<T1> col = new GridColumnBuilder<T1>(columnName, headerText, valueExpression, _columnDefaults);

            ColumnBuilders.Add(col);

            return col;
        }

        public GridColumnBuilder<T1> Add(GridColumn<T1> column)
        {
            GridColumnBuilder<T1> col = new GridColumnBuilder<T1>();
            col.GridColumn = column;
            ColumnBuilders.Add(col);
            return col;
        }
    }

    public class GridColumnBuilder<T1>
    {
        public GridColumnBuilder()
            : this(null, null, null, null)
        {
        }

        public GridColumnBuilder(string columnName)
            : this(columnName, null, null, null)
        {
        }

        public GridColumnBuilder(string columnName, string headerText, Func<T1, string> valueExpression)
            :this(columnName, headerText, valueExpression, null)
        {

        }

        public GridColumnBuilder(string columnName, string headerText, Func<T1, string> valueExpression, ColumnDefaults columnDefaults)
        {
            Func<T1, GridContext, string> newVe = null;
            if (valueExpression != null)
            {
                newVe = (T1, GridContext) => valueExpression(T1);
            }

            GridColumn = new GridColumn<T1>(columnName, headerText, newVe, columnDefaults);
        }


        public GridColumn<T1> GridColumn { get; set; }


        /// <summary>
        /// A unique name for this column
        /// </summary>
        public GridColumnBuilder<T1> WithColumnName(string name)
        {
            GridColumn.ColumnName = name;
            return this;
        }

        /// <summary>
        /// Header text to display for the current column, if different from ColumnName.
        /// </summary>
        public GridColumnBuilder<T1> WithHeaderText(string text)
        {
            GridColumn.HeaderText = text;
            return this;
        }

        public GridColumnBuilder<T1> WithHeaderTextExpression(Func<string> expression)
        {
            GridColumn.HeaderTextExpression = expression;
            return this;
        }


        /// <summary>
        /// Enables sorting on this column
        /// </summary>
        public GridColumnBuilder<T1> WithSorting(bool enableSorting)
        {
            GridColumn.EnableSorting = enableSorting;
            return this;
        }

        /// <summary>
        /// Disables html encoding on the data for the current cell. Turn this off if your ValueExpression or ValueTemplate returns HTML.
        /// </summary>
        public GridColumnBuilder<T1> WithHtmlEncoding(bool htmlEncode)
        {
            GridColumn.HtmlEncode = htmlEncode;
            return this;
        }

        /// <summary>
        /// This is how to specify the contents of the current cell. If this contains HTML, set HTMLEncode to false
        /// </summary>
        public GridColumnBuilder<T1> WithValueExpression(Func<T1, GridContext, string> expression)
        {
            GridColumn.ValueExpression = expression;
            return this;
        }

        /// <summary>
        /// This is how to specify the contents of the current cell. If this contains HTML, set HTMLEncode to false
        /// </summary>
        public GridColumnBuilder<T1> WithValueExpression(Func<T1, string> expression)
        {
            GridColumn.ValueExpression = (T1, GridContext) => expression(T1);
            return this;
        }


        /// <summary>
        /// This is how to specify the contents of the current cell when used in an export file, if different that ValueExpression
        /// </summary>
        public GridColumnBuilder<T1> WithPlainTextValueExpression(Func<T1, GridContext, string> expression)
        {
            GridColumn.PlainTextValueExpression = expression;
            return this;
        }

        /// <summary>
        /// This is how to specify the contents of the current cell when used in an export file, if different that ValueExpression
        /// </summary>
        public GridColumnBuilder<T1> WithPlainTextValueExpression(Func<T1, string> expression)
        {
            GridColumn.PlainTextValueExpression = (T1, GridContext) => expression(T1);
            return this;
        }

        /// <summary>
        /// Use this to return a custom css class based on data for the current cell
        /// </summary>
        public GridColumnBuilder<T1> WithCellCssClassExpression(Func<T1, GridContext, string> expression)
        {
            GridColumn.CellCssClassExpression = expression;
            return this;
        }

        /// <summary>
        /// Use this to return a custom css class based on data for the current cell
        /// </summary>
        public GridColumnBuilder<T1> WithCellCssClassExpression(Func<T1, string> expression)
        {
            GridColumn.CellCssClassExpression = (T1, GridContext) => expression(T1);
            return this;
        }

        /// <summary>
        /// Enables filtering on this column
        /// </summary>
        public GridColumnBuilder<T1> WithFiltering(bool enableFiltering)
        {
            GridColumn.EnableFiltering = enableFiltering;
            return this;
        }

        /// <summary>
        /// Indicates whether column is visible.
        /// </summary>
        public GridColumnBuilder<T1> WithVisibility(bool visible)
        {
            GridColumn.Visible = visible;
            return this;
        }

        /// <summary>
        /// Indicates whether column is visible.
        /// </summary>
        public GridColumnBuilder<T1> WithVisibility(bool visible, bool allowChangeVisibility)
        {
            GridColumn.Visible = visible;
            GridColumn.AllowChangeVisibility = allowChangeVisibility;
            return this;
        }



        /// <summary>
        /// Template for formatting cell value
        /// </summary>
        public GridColumnBuilder<T1> WithValueTemplate(string template)
        {
            GridColumn.ValueTemplate = template;
            return this;
        }

        /// <summary>
        /// Template for formatting cell value
        /// </summary>
        public GridColumnBuilder<T1> WithValueTemplate(string template, bool htmlEncode)
        {
            GridColumn.ValueTemplate = template;
            GridColumn.HtmlEncode = htmlEncode;
            return this;
        }

        /// <summary>
        /// Object to pass to QueryOptions when this column is sorted on. Only specify if different from ColumnName
        /// </summary>
        public GridColumnBuilder<T1> WithSortColumnData(object sortColumnData)
        {
            GridColumn.SortColumnData = sortColumnData;
            return this;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the column visibility can be changed.
        /// </summary>
        public GridColumnBuilder<T1> WithAllowChangeVisibility(bool allow)
        {
            GridColumn.AllowChangeVisibility = allow;
            return this;
        }
    }
}
