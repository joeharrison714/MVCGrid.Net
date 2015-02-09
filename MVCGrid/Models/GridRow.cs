using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Models
{
    public class GridRow
    {
        public GridRow()
        {
            Values = new Dictionary<string, string>();
            PlainTextValues = new Dictionary<string, string>();
            CellCssClasses = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Values { get; set; }
        public Dictionary<string, string> PlainTextValues { get; set; }
        public Dictionary<string, string> CellCssClasses { get; set; }

        public string RowCssClass { get; set; }
    }
}
