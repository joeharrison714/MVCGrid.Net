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

                IMVCGridHtmlWriter writer = (IMVCGridHtmlWriter)Activator.CreateInstance(gridContext.GridDefinition.HtmlWriterType, true);
                IMVCGridRenderingEngine renderingEngine = new HtmlRenderingEngine(writer);

                var results = ((MVCGrid.Models.GridDefinitionBase)grid).GetData(gridContext);

                using (MemoryStream ms = new MemoryStream())
                {
                    renderingEngine.Render(results, gridContext, ms);

                    preload = Encoding.ASCII.GetString(ms.ToArray());
                }
            }

            html = html.Replace("%%PRELOAD%%", preload);

            return MvcHtmlString.Create(html);
        }
    }
}
