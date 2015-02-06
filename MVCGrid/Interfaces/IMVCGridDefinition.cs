using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCGrid.Interfaces
{
    internal interface IMVCGridDefinition
    {
        IEnumerable<IMVCGridColumn> GetColumns();
        GridData GetData(QueryOptions options);
        GridConfiguration GridConfiguration { get; }
    }
}
