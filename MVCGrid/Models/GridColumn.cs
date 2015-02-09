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
        {
            EnableSorting = true;
            HtmlEncode = true;
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
    }
}
