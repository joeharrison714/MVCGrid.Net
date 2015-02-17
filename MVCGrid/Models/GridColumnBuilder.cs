using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Models
{
    public class GridColumnListBuilder<T1>
    {
        public GridColumnListBuilder()
        {
            ColumnBuilders = new List<GridColumnBuilder<T1>>();
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

        public GridColumnBuilder<T1> Add(string columnName, string headerText, Func<T1, GridContext, string> valueExpression)
        {
            GridColumnBuilder<T1> col = new GridColumnBuilder<T1>(columnName, headerText, valueExpression);

            ColumnBuilders.Add(col);

            return col;
        }
    }

    public class GridColumnBuilder<T1>
    {
        public GridColumnBuilder():this(null,null,null)
        {
        }

        public GridColumnBuilder(string columnName)
            : this(columnName, null, null)
        {
        }

        public GridColumnBuilder(string columnName, string headerText, Func<T1, GridContext, string> valueExpression)
        {
            GridColumn = new GridColumn<T1>(columnName, headerText, valueExpression);
        }

        public GridColumn<T1> GridColumn { get; set; }

        public GridColumnBuilder<T1> WithColumnName(string name)
        {
            GridColumn.ColumnName = name;
            return this;
        }

        public GridColumnBuilder<T1> WithHeaderText(string text)
        {
            GridColumn.HeaderText = text;
            return this;
        }

        public GridColumnBuilder<T1> WithSorting(bool enableSorting)
        {
            GridColumn.EnableSorting = enableSorting;
            return this;
        }

        public GridColumnBuilder<T1> WithHtmlEncoding(bool htmlEncode)
        {
            GridColumn.HtmlEncode = htmlEncode;
            return this;
        }

        /// <summary>
        /// The expression for returning the cell value
        /// </summary>
        public GridColumnBuilder<T1> WithValueExpression(Func<T1, GridContext, string> expression)
        {
            GridColumn.ValueExpression = expression;
            return this;
        }


        /// <summary>
        /// Optional. Only needed if different from ValueExpression
        /// </summary>
        public GridColumnBuilder<T1> WithPlainTextValueExpression(Func<T1, GridContext, string> expression)
        {
            GridColumn.PlainTextValueExpression = expression;
            return this;
        }

        public GridColumnBuilder<T1> WithCellCssClassExpression(Func<T1, GridContext, string> expression)
        {
            GridColumn.CellCssClassExpression = expression;
            return this;
        }

        public GridColumnBuilder<T1> WithFiltering(bool enableFiltering)
        {
            GridColumn.EnableFiltering = enableFiltering;
            return this;
        }

        public GridColumnBuilder<T1> WithVisibility(bool visible)
        {
            GridColumn.Visible = visible;
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
        /// Object to pass to QueryOptions when this coumn is sorted on. Only specify if different from ColumnName
        /// </summary>
        public GridColumnBuilder<T1> WithSortColumnData(object sortColumnData)
        {
            GridColumn.SortColumnData = sortColumnData;
            return this;
        }
    }
}
