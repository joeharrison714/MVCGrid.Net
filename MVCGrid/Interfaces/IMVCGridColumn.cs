using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Interfaces
{
    public interface IMVCGridColumn
    {
        string HeaderText { get; }
        string ColumnName { get; }
        bool EnableSorting { get; }
        bool HtmlEncode { get; }
        bool EnableFiltering { get; }
    }
}
