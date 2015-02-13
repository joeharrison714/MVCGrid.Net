using MVCGrid.Engine;
using MVCGrid.Interfaces;
using MVCGrid.Rendering;
using MVCGrid.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCGrid.Web
{
    public static class HtmlExtensions
    {
        public static IHtmlString MVCGrid(this HtmlHelper helper, string name)
        {
            var currentMapping = MVCGridDefinitionTable.GetDefinitionInterface(name);

            return MVCGrid(helper, name, currentMapping);
        }

        internal static IHtmlString MVCGrid(this HtmlHelper helper, string name, IMVCGridDefinition grid)
        {
            string gridName = name;

            string html = MVCGridHtmlGenerator.GenerateBasePageHtml(name, grid);

            string preload = "";

            if (grid.PreloadData)
            {
                var options = QueryStringParser.ParseOptions(grid, System.Web.HttpContext.Current.Request);

                var gridContext = GridContextUtility.Create(HttpContext.Current, gridName, grid, options);

                GridEngine engine = new GridEngine();

                IMVCGridRenderingEngine renderingEngine = engine.GetRenderingEngine(gridContext);
                
                using (MemoryStream ms = new MemoryStream())
                {
                    using (TextWriter tw = new StreamWriter(ms))
                    {
                        engine.Run(renderingEngine, gridContext, tw);
                    }

                    preload = Encoding.ASCII.GetString(ms.ToArray());
                }
            }

            html = html.Replace("%%PRELOAD%%", preload);

            return MvcHtmlString.Create(html);
        }
    }
}
