using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCGrid.Models
{
    public class GridColumn<T1> : IMVCGridColumn
    {
        public GridColumn()
        {
            EnableSorting = true;
            HtmlEncode = true;
        }

        public string ColumnName { get; set; }

        public string HeaderText { get; set; }

        /// <summary>
        /// The expression for returning the cell value
        /// </summary>
        public Func<T1, GridContext, string> ValueExpression { get; set; }

        /// <summary>
        /// Optional. Only needed if different from ValueExpression
        /// </summary>
        public Func<T1, GridContext, string> PlainTextValueExpression { get; set; }

        public bool EnableSorting { get; set; }
        public bool HtmlEncode { get; set; }
    }
}
