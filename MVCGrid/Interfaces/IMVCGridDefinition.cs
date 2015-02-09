using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Interfaces
{
    internal interface IMVCGridDefinition
    {
        IEnumerable<IMVCGridColumn> GetColumns();
        GridData GetData(GridContext context);
        GridConfiguration GridConfiguration { get; }
        string QueryStringPrefix { get; }
        bool PreloadData { get; }
        bool Paging { get; set; }
        int ItemsPerPage { get; set; }
        bool Sorting { get; set; }
        string DefaultSortColumn { get; set; }
        string NoResultsMessage { get; set; }
    }
}
