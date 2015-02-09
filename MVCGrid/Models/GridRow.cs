using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCGrid.Models
{
    public class GridRow
    {
        public GridRow()
        {
            Values = new Dictionary<string, string>();
            PlainTextValues = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Values { get; set; }
        public Dictionary<string, string> PlainTextValues { get; set; }
    }
}
