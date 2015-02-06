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
        }

        public string ColumnName { get; set; }

        public string HeaderText { get; set; }

        public Func<T1, ControllerContext, string> ValueExpression { get; set; }

        public bool EnableSorting { get; set; }
    }
}
