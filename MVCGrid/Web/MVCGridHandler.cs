using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MVCGrid.Web
{
    public class MVCGridHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            Console.WriteLine(context.Request.Path);

            if (context.Request.Path.ToLower().EndsWith("/script.js"))
            {
                HandleScript(context);
            }
            else
            {
                HandleTable(context);
            }
        }

        private void HandleTable(HttpContext context)
        {
            string gridName = context.Request["Name"];
            int pageIndex = int.Parse(context.Request["PageIndex"]);
            string sortColumn = context.Request["SortColumn"];
            string sortDirection = context.Request["SortDirection"];


            var def = MVCGridMappingTable.GetMappingInterface(gridName);
            var config = def.GridConfiguration;

            var options = new QueryOptions();
            options.PageIndex = pageIndex;
            options.ItemsPerPage = config.ItemsPerPage;
            options.SortColumn = sortColumn;
            options.SortDirection = SortDirection.Unspecified;

            if (String.Compare(sortDirection, "asc", true) == 0)
            {
                options.SortDirection = SortDirection.Asc;
            }
            else if (String.Compare(sortDirection, "dsc", true) == 0)
            {
                options.SortDirection = SortDirection.Dsc;
            }

            var results = def.GetData(options);

            var tableHtml = MVCGridHtmlGenerator.GenerateTable(gridName, def, results, options);

            context.Response.Write(tableHtml);
        }

        private static Lazy<string> javascriptContents = new Lazy<string>(() =>
            {
                var assembly = Assembly.GetExecutingAssembly();

                var s = assembly.GetManifestResourceNames();

                string resourceName = null;
                if (s.Length == 1)
                {
                    resourceName = s[0];
                }

                string script = null;
                using (var textStreamReader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
                {
                    script = textStreamReader.ReadToEnd();
                }

                return script;
            });
        private void HandleScript(HttpContext context)
        {
            context.Response.ContentType = "application/javascript";
            context.Response.Write(javascriptContents.Value);
        }
    }
}
