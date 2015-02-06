using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCGrid.Models
{
    public class QueryResult<T1>
    {
        public int TotalRecords { get; set; }
        public IEnumerable<T1> Items { get; set; }
    }
}
