using MVCGrid.Engine;
using MVCGrid.Interfaces;
using MVCGrid.Models;
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
            GridEngine ge = new GridEngine();

            string html = ge.GetBasePageHtml(helper, name, grid);

            return MvcHtmlString.Create(html);
        }

        
    }
}
