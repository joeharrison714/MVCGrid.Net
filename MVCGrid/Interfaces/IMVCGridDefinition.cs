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
        string QueryStringPrefix { get; }
        bool PreloadData { get; }
        bool Paging { get; set; }
        int ItemsPerPage { get; set; }
        bool Sorting { get; set; }
        string DefaultSortColumn { get; set; }
        string NoResultsMessage { get; set; }
        string ClientSideLoadingMessageFunctionName { get; set; }
        string ClientSideLoadingCompleteFunctionName { get; set; }
        bool Filtering { get; set; }

        Type RenderingEngine { get; set; }
        Type TemplatingEngine { get; set; }

        Dictionary<string, string> AdditionalSettings { get; set; }
        RenderingMode RenderingMode { get; set; }
        string ViewPath { get; set; }

        string ErrorMessageHtml { get; set; }
    }
}
