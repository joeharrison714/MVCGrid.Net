using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Models
{
    public class GridDefaults
    {
        public GridDefaults()
        {
            PreloadData = true;
            Paging = false;
            ItemsPerPage = 20;
            Sorting = false;
            DefaultSortColumn = null;
            NoResultsMessage = "No results.";
            ClientSideLoadingMessageFunctionName = null;
            ClientSideLoadingCompleteFunctionName = null;
            Filtering = false;
            RenderingEngine = typeof(MVCGrid.Rendering.BootstrapRenderingEngine);
            TemplatingEngine = typeof(MVCGrid.Templating.SimpleTemplatingEngine);
        }

        public bool PreloadData { get; set; }
        public bool Paging { get; set; }
        public int ItemsPerPage { get; set; }
        public bool Sorting { get; set; }
        public string DefaultSortColumn { get; set; }
        public string NoResultsMessage { get; set; }
        public string ClientSideLoadingMessageFunctionName { get; set; }
        public string ClientSideLoadingCompleteFunctionName { get; set; }
        public bool Filtering { get; set; }
        public Type RenderingEngine { get; set; }
        public Type TemplatingEngine { get; set; }
    }
}
