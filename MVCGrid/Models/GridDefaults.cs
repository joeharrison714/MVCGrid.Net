using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Models
{
    public class GridDefaults : IMVCGridDefinition
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
            AdditionalSettings = new Dictionary<string, string>();
            RenderingMode = Models.RenderingMode.RenderingEngine;
            ViewPath = "~/Views/MVCGrid/_Grid.cshtml";
            ErrorMessageHtml= @"<div class=""alert alert-warning"" role=""alert"">There was a problem loading the grid.</div>";
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
        public Dictionary<string, string> AdditionalSettings { get; set; }
        public RenderingMode RenderingMode { get; set; }
        public string ViewPath { get; set; }
        public string QueryStringPrefix { get; set; }

        public IEnumerable<IMVCGridColumn> GetColumns()
        {
            throw new NotImplementedException();
        }

        public string ErrorMessageHtml { get; set; }
    }
}
