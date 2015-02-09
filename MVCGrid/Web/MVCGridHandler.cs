using MVCGrid.Interfaces;
using MVCGrid.Models;
using MVCGrid.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
            


            var grid = MVCGridMappingTable.GetMappingInterface(gridName);

            var options = QueryStringParser.ParseOptions(grid, context.Request);

            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var urlHelper = new UrlHelper(new RequestContext(httpContext, new RouteData()));

            var gridContext = new GridContext()
            {
                GridName = gridName,
                CurrentHttpContext = context,
                GridDefinition = grid,
                QueryOptions = options,
                UrlHelper = urlHelper
            };

            IMVCGridRenderingEngine renderingEngine = DetermineRenderingEngine(context);

            if (renderingEngine is HtmlRenderingEngine)
            {
                context.Response.Write(sbDebug.ToString());
            }

            if (!renderingEngine.AllowsPaging)
            {
                gridContext.QueryOptions.ItemsPerPage = null;
            }

            var results = grid.GetData(gridContext);

            renderingEngine.Render(results, gridContext, context.Response);
        }

        private IMVCGridRenderingEngine DetermineRenderingEngine(HttpContext context)
        {
            IMVCGridRenderingEngine engine = null;

            if (context.Request.QueryString["engine"] != null)
            {
                string re = context.Request.QueryString["engine"];
                if (String.Compare(re, "export", true) == 0)
                {
                    engine = new CsvRenderingEngine();
                }
            }

            if (engine == null)
            {
                 engine = new HtmlRenderingEngine();
            }

            return engine;
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
