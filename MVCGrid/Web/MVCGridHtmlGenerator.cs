using MVCGrid.Interfaces;
using MVCGrid.Models;
using MVCGrid.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MVCGrid.Web
{
    //Feature Requests
    //Show/hide fields
    //turn off paging (maybe ajax)
    // ajax loading function
    // ajax error handling
    // filtering

    internal class MVCGridHtmlGenerator
    {
        internal static string GenerateBasePageHtml(string gridName, IMVCGridDefinition def)
        {
            GridConfiguration config = def.GridConfiguration;

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.AppendFormat("<div id='{0}' class='{1}'>", HtmlUtility.GetContainerHtmlId(gridName), HtmlUtility.ContainerCssClass);

            sbHtml.AppendFormat("<input type='hidden' name='MVCGridName' value='{0}' />", gridName);
            sbHtml.AppendFormat("<input type='hidden' id='MVCGrid_{0}_Prefix' value='{1}' />", gridName, def.QueryStringPrefix);
            sbHtml.AppendFormat("<input type='hidden' id='MVCGrid_{0}_Preload' value='{1}' />", gridName, def.PreloadData.ToString().ToLower());

            sbHtml.AppendFormat("<div id='MVCGrid_Loading_{0}' class='text-center' style='display:none;'>", gridName);
            sbHtml.AppendFormat("&nbsp;&nbsp;&nbsp;<img src='{0}/ajaxloader.gif' alt='Processing' />", HtmlUtility.GetHandlerPath());
            sbHtml.Append("Processing...");
            sbHtml.Append("</div>");

            sbHtml.AppendFormat("<div id='{0}'>", HtmlUtility.GetTableHolderHtmlId(gridName));
            sbHtml.Append("%%PRELOAD%%");
            sbHtml.Append("</div>");



            sbHtml.AppendLine("</div>");

            return sbHtml.ToString();
        }
    }
}
