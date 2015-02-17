using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Models
{
    public class GridColumn<T1> : IMVCGridColumn
    {
        private string _headerText = null;

        public GridColumn()
            : this(null, null, null)
        {
        }

        public GridColumn(string columnName, string headerText, Func<T1, GridContext, string> valueExpression)
        {
            if (!String.IsNullOrWhiteSpace(columnName))
            {
                this.ColumnName = columnName;
            }

            if (!String.IsNullOrWhiteSpace(headerText))
            {
                this.HeaderText = headerText;
            }

            if (valueExpression != null)
            {
                this.ValueExpression = valueExpression;
            }

            EnableSorting = true;
            HtmlEncode = true;
            EnableFiltering = false;
            Visible = true;
        }

        public string ColumnName { get; set; }

        public string HeaderText
        {
            get
            {
                if (_headerText == null)
                    return ColumnName;
                else
                    return _headerText;
            }
            set
            {
                _headerText = value;
            }
        }

        /// <summary>
        /// Template for formatting cell value
        /// </summary>
        public string ValueTemplate { get; set; }

        /// <summary>
        /// The expression for returning the cell value
        /// </summary>
        public Func<T1, GridContext, string> ValueExpression { get; set; }

        /// <summary>
        /// Optional. Only needed if different from ValueExpression
        /// </summary>
        public Func<T1, GridContext, string> PlainTextValueExpression { get; set; }

        public Func<T1, GridContext, string> CellCssClassExpression { get; set; }

        public bool EnableSorting { get; set; }
        public bool HtmlEncode { get; set; }
        public bool EnableFiltering { get; set; }
        public bool Visible { get; set; }

        /// <summary>
        /// Object to pass to QueryOptions when this coumn is sorted on. Only specify if different from ColumnName
        /// </summary>
        public object SortColumnData { get; set; }
    }
}
