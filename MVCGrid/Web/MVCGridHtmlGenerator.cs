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
            StringBuilder sbJson = new StringBuilder();

            sbJson.Append("{");
            sbJson.AppendFormat("\"name\": \"{0}\"", gridName);
            sbJson.Append(",");
            sbJson.AppendFormat("\"qsPrefix\": \"{0}\"", def.QueryStringPrefix);
            sbJson.Append(",");
            sbJson.AppendFormat("\"preloaded\": {0}", def.PreloadData.ToString().ToLower());

            sbJson.Append(",");
            sbJson.AppendFormat("\"clientLoading\": \"{0}\"", def.ClientSideLoadingMessageFunctionName);

            sbJson.Append(",");
            sbJson.AppendFormat("\"clientLoadingComplete\": \"{0}\"", def.ClientSideLoadingCompleteFunctionName);

            sbJson.Append(",");
            sbJson.AppendFormat("\"renderingMode\": \"{0}\"", def.RenderingMode.ToString().ToLower());

            sbJson.Append("}");
            //mvcGridName, qsPrefix: qsPrefix, preloaded: preload }

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.AppendFormat("<div id='{0}' class='{1}'>", HtmlUtility.GetContainerHtmlId(gridName), HtmlUtility.ContainerCssClass);

            sbHtml.AppendFormat("<input type='hidden' name='MVCGridName' value='{0}' />", gridName);
            //sbHtml.AppendFormat("<input type='hidden' id='MVCGrid_{0}_Prefix' value='{1}' />", gridName, def.QueryStringPrefix);
            //sbHtml.AppendFormat("<input type='hidden' id='MVCGrid_{0}_Preload' value='{1}' />", gridName, def.PreloadData.ToString().ToLower());

            sbHtml.AppendFormat("<input type='hidden' id='MVCGrid_{0}_JsonData' value='{1}' />", gridName, sbJson.ToString());

            sbHtml.AppendFormat("<div id='MVCGrid_ErrorMessage_{0}' style='display: none;'>", gridName);
            if (String.IsNullOrWhiteSpace(def.ErrorMessageHtml))
            {
                sbHtml.Append("An error has occured.");
            }
            else
            {
                sbHtml.Append(def.ErrorMessageHtml);
            }
            sbHtml.Append("</div>");


            sbHtml.AppendFormat("<div id='MVCGrid_Loading_{0}' class='text-center' style='visibility: hidden'>", gridName);
            sbHtml.AppendFormat("&nbsp;&nbsp;&nbsp;<img src='{0}/ajaxloader.gif' alt='Processing' style='width: 15px; height: 15px;' />", HtmlUtility.GetHandlerPath());
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
