using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCGrid.Interfaces
{
    public interface IMVCGridColumn
    {
        string HeaderText { get; set; }
        string ColumnName { get; set; }
        bool EnableSorting { get; }
    }
}
