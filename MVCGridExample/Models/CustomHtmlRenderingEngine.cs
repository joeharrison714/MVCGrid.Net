using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MVCGrid.Web.Models
{
    public class CustomHtmlRenderingEngine : IMVCGridRenderingEngine
    {
        public bool AllowsPaging
        {
            get { return true; }
        }

        public void PrepareResponse(HttpResponse response)
        {
        }

        public void Render(MVCGrid.Models.RenderingModel model, System.IO.TextWriter outputStream)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table class='customStyleTable'><thead><tr>");
            foreach (var col in model.Columns)
            {
                sb.Append("<th");
                if (!String.IsNullOrWhiteSpace(col.Onclick))
                {
                    sb.AppendFormat(" onclick='{0}'", col.Onclick);
                }
                sb.Append(">");

                sb.Append(col.HeaderText);

                if (col.SortIconDirection.HasValue)
                {
                    switch (col.SortIconDirection.Value)
                    {
                        case MVCGrid.Models.SortDirection.Asc:
                            sb.Append(" (Ascending)");
                            break;
                        case MVCGrid.Models.SortDirection.Dsc:
                            sb.Append(" (Descending)");
                            break;
                        case MVCGrid.Models.SortDirection.Unspecified:
                            sb.Append(" (Sort)");
                            break;
                    }
                }
                sb.Append("</th>");
            }
            sb.Append("</tr></thead><tbody>");

            foreach (var row in model.Rows)
            {
                sb.Append("<tr");
                if (!String.IsNullOrWhiteSpace(row.CalculatedCssClass))
                {
                    sb.AppendFormat(" class='{0}'", row.CalculatedCssClass);
                }
                sb.Append(">");

                foreach (var col in model.Columns)
                {
                    var cell = row.Cells[col.Name];

                    sb.Append("<td");
                    if (!String.IsNullOrWhiteSpace(cell.CalculatedCssClass))
                    {
                        sb.AppendFormat(" class='{0}'", cell.CalculatedCssClass);
                    }
                    sb.Append(">");

                    sb.Append(cell.HtmlText);
                    sb.Append("</td>");
                }

                sb.Append("</tr>");
            }
            sb.Append("</tbody></table>");

            if (model.PagingModel != null)
            {
                sb.Append("<div><ul>");
                foreach (var pl in model.PagingModel.PageLinks)
                {
                    sb.Append("<li class='pageItem'>");
                    sb.AppendFormat("<a href='#' onclick='{0}'>{1}</a>", pl.Value, pl.Key);
                    sb.Append("</li>");
                }
                sb.Append("</ul></div>");
            }


            outputStream.Write(sb.ToString());

        }
    }
}