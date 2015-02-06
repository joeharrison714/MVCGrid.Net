using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
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
            var currentMapping = MVCGridMappingTable.GetMappingInterface(name);

            return MVCGrid(helper, name, currentMapping);
        }

        internal static IHtmlString MVCGrid(this HtmlHelper helper, string name, IMVCGridDefinition grid)
        {
            string gridName = name;

            var currentMapping = MVCGridMappingTable.GetMappingInterface(name);

            string html = MVCGridHtmlGenerator.GenerateBasePageHtml(name, currentMapping);

            return MvcHtmlString.Create(html);
        }
    }
}
