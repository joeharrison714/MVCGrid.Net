using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Models
{
    public class ColumnDefaults : IMVCGridColumn
    {
        public ColumnDefaults()
        {
            ColumnName = null;
            HeaderText = null;
            EnableSorting = false;
            HtmlEncode = true;
            EnableFiltering = false;
            Visible = true;
            SortColumnData = null;
            AllowChangeVisibility = false;
            HeaderTextExpression = null;
        }

        public string ColumnName { get; set; }
        public string HeaderText { get; set; }
        public Func<string> HeaderTextExpression { get; set; }
        public bool EnableSorting { get; set; }
        public bool HtmlEncode { get; set; }
        public bool EnableFiltering { get; set; }
        public bool Visible { get; set; }
        public object SortColumnData { get; set; }
        public bool AllowChangeVisibility { get; set; }
    }
}
