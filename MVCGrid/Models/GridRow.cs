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
            Data = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Data { get; set; }
    }
}
