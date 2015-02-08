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

            StringBuilder sbDebug = new StringBuilder();
            foreach (string key in context.Request.QueryString.AllKeys)
            {
                sbDebug.Append(key);
                sbDebug.Append(" = ");
                sbDebug.Append(context.Request.QueryString[key]);
                sbDebug.Append("<br />");
            }

            

            var options = new QueryOptions();
            options.ItemsPerPage = 20;

            options.PageIndex = 0;
            if (context.Request.QueryString["page"] != null)
            {
                int pageNum;
                if (Int32.TryParse(context.Request.QueryString["page"], out pageNum))
                {
                    options.PageIndex = pageNum - 1;
                    if (options.PageIndex < 0) options.PageIndex = 0;
                }
            }

            options.SortColumn = null;
            if (context.Request.QueryString["sort"] != null)
            {
                options.SortColumn = context.Request.QueryString["sort"];
            }

            options.SortDirection = SortDirection.Asc;
            if (context.Request.QueryString["dir"] != null)
            {
                string sortDir = context.Request.QueryString["dir"];
                if (String.Compare(sortDir, "dsc", true) == 0)
                {
                    options.SortDirection = SortDirection.Dsc;
                }
            }
            


            var def = MVCGridMappingTable.GetMappingInterface(gridName);
            var config = def.GridConfiguration;

            var results = def.GetData(options);

            var tableHtml = MVCGridHtmlGenerator.GenerateTable(gridName, def, results, options);

            context.Response.Write(sbDebug.ToString());
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
