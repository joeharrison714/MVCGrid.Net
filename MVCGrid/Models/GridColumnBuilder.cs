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
            GridColumnBuilder<T1> col = new GridColumnBuilder<T1>();

            ColumnBuilders.Add(col);

            return col;
        }
    }

    public class GridColumnBuilder<T1>
    {
        public GridColumnBuilder()
        {
            GridColumn = new GridColumn<T1>();
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

        public GridColumnBuilder<T1> WithValueExpression(Func<T1, GridContext, string> expression)
        {
            GridColumn.ValueExpression = expression;
            return this;
        }
    }
}
