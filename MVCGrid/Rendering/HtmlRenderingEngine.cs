using MVCGrid.Interfaces;
using MVCGrid.Models;
using MVCGrid.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MVCGrid.Rendering
{
    public class HtmlRenderingEngine : IMVCGridRenderingEngine
    {
        private readonly string CssTable;
        private readonly string HtmlImageSortAsc;
        private readonly string HtmlImageSortDsc;
        private readonly string HtmlImageSort;

        public HtmlRenderingEngine()
        {
            CssTable = "table table-striped table-bordered";

            HtmlImageSortAsc = String.Format("<img src='{0}/sortup.png' class='pull-right' />", HtmlUtility.GetHandlerPath());
            HtmlImageSortDsc = String.Format("<img src='{0}/sortdown.png' class='pull-right' />", HtmlUtility.GetHandlerPath());
            HtmlImageSort = String.Format("<img src='{0}/sort.png' class='pull-right' />", HtmlUtility.GetHandlerPath());
        }

        public void PrepareResponse(HttpResponse response)
        {
        }

        public bool AllowsPaging
        {
            get { return true; }
        }

        public void Render(GridData data, GridContext gridContext, Stream outputStream)//HttpResponse httpResponse
        {
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.AppendFormat("<table id='{0}'", HtmlUtility.GetTableHtmlId(gridContext.GridName));
            sbHtml.Append(HtmlUtility.MakeCssClassAttributeStirng(CssTable));
            sbHtml.Append(">");


            RenderHeader(gridContext, sbHtml);

            if (data.Rows.Count > 0)
            {
                RenderBody(data, gridContext, sbHtml);
            }
            else
            {
                sbHtml.Append("<tbody>");
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td colspan='{0}'>", gridContext.GridDefinition.GetColumns().Count());
                sbHtml.Append(gridContext.GridDefinition.NoResultsMessage);
                sbHtml.Append("</td>");
                sbHtml.Append("</tr>");
                sbHtml.Append("</tbody>");
            }
            sbHtml.AppendLine("</table>");

            if (gridContext.GridDefinition.Paging)
            {
                MakePaging(data, gridContext, sbHtml);
            }

            using (StreamWriter sw = new StreamWriter(outputStream))
            {
                sw.Write(sbHtml.ToString());
            }
        }

        private void RenderBody(GridData data, GridContext gridContext, StringBuilder sbHtml)
        {
            sbHtml.AppendLine("<tbody>");

            foreach (var item in data.Rows)
            {
                sbHtml.Append("  <tr");
                if (!String.IsNullOrWhiteSpace(item.RowCssClass))
                {
                    sbHtml.Append(HtmlUtility.MakeCssClassAttributeStirng(item.RowCssClass));
                }
                sbHtml.AppendLine(">");

                foreach (var col in gridContext.GridDefinition.GetColumns())
                {
                    string val = "";

                    if (item.Values.ContainsKey(col.ColumnName))
                    {
                        val = item.Values[col.ColumnName];
                    }

                    sbHtml.Append("<td");
                    if (item.CellCssClasses.ContainsKey(col.ColumnName))
                    {
                        string cellCss = item.CellCssClasses[col.ColumnName];
                        if (!String.IsNullOrWhiteSpace(cellCss))
                        {
                            sbHtml.Append(HtmlUtility.MakeCssClassAttributeStirng(cellCss));
                        }
                    }
                    sbHtml.AppendLine(">");

                    if (col.HtmlEncode)
                    {
                        sbHtml.Append(HttpUtility.HtmlEncode(val));
                    }
                    else
                    {
                        sbHtml.Append(val);
                    }
                    sbHtml.AppendLine("</td>");
                }
                sbHtml.AppendLine("  </tr>");
            }

            sbHtml.AppendLine("</tbody>");
        }

        private void RenderHeader(GridContext gridContext, StringBuilder sbHtml)
        {
            sbHtml.AppendLine("<thead>");
            sbHtml.AppendLine("  <tr>");
            foreach (var col in gridContext.GridDefinition.GetColumns())
            {
                sbHtml.Append("<th");

                if (gridContext.GridDefinition.Sorting && col.EnableSorting)
                {
                    SortDirection direction = SortDirection.Asc;
                    if (gridContext.QueryOptions.SortColumn == col.ColumnName && gridContext.QueryOptions.SortDirection == SortDirection.Asc)
                    {
                        direction = SortDirection.Dsc;
                    }

                    sbHtml.Append(" style='cursor: pointer;'");
                    sbHtml.AppendFormat(" onclick='{0}'", HtmlUtility.MakeSortLink(gridContext.GridName, col.ColumnName, direction));
                }
                sbHtml.Append(">");

                sbHtml.Append(HttpUtility.HtmlEncode(col.HeaderText));

                if (gridContext.GridDefinition.Sorting && col.EnableSorting)
                {
                    if (gridContext.QueryOptions.SortColumn == col.ColumnName && gridContext.QueryOptions.SortDirection == SortDirection.Asc)
                    {
                        sbHtml.Append(" ");
                        sbHtml.Append(HtmlImageSortAsc);
                    }
                    else if (gridContext.QueryOptions.SortColumn == col.ColumnName && gridContext.QueryOptions.SortDirection == SortDirection.Dsc)
                    {
                        sbHtml.Append(" ");
                        sbHtml.Append(HtmlImageSortDsc);
                    }
                    else
                    {
                        sbHtml.Append(" ");
                        sbHtml.Append(HtmlImageSort);
                    }
                }


                sbHtml.Append("</th>");
            }
            sbHtml.AppendLine("  </tr>");
            sbHtml.AppendLine("</thead>");
        }

        private static void MakePaging(GridData data, GridContext gridContext, StringBuilder sbHtml)
        {
            if (!gridContext.QueryOptions.ItemsPerPage.HasValue)
            {
                return;
            }

            var numberOfPagesD = (data.TotalRecords.Value + 0.0) / (gridContext.QueryOptions.ItemsPerPage.Value + 0.0);
            int numberOfPages = (int)Math.Ceiling(numberOfPagesD);
            int currentPageIndex = gridContext.QueryOptions.PageIndex.Value;

            int firstRecord = (currentPageIndex * gridContext.QueryOptions.ItemsPerPage.Value) + 1;
            int lastRecord = (firstRecord + gridContext.QueryOptions.ItemsPerPage.Value) - 1;
            if (lastRecord > data.TotalRecords)
            {
                lastRecord = data.TotalRecords.Value;
            }

            string recordText = String.Format("Showing {0} to {1} of {2} entries",
                firstRecord, lastRecord, data.TotalRecords
                );

            sbHtml.Append("<div class=\"row\">");
            sbHtml.Append("<div class=\"col-xs-6\">");
            sbHtml.AppendLine(recordText);
            sbHtml.Append("</div>");


            sbHtml.Append("<div class=\"col-xs-6\">");
            int currentPage = currentPageIndex + 1;
            int pageToStart = currentPage - 2;
            while (pageToStart < 1)
            {
                pageToStart++;
            }
            int pageToEnd = pageToStart + 4;
            while (pageToEnd > numberOfPages)
            {
                pageToStart--;
                pageToEnd = pageToStart + 4;
            }
            while (pageToStart < 1)
            {
                pageToStart++;
            }

            sbHtml.Append("<ul class='pagination pull-right' style='margin-top: 0;'>");

            sbHtml.Append("<li");
            if (pageToStart == currentPage)
            {
                sbHtml.Append(" class='disabled'");
            }
            sbHtml.Append(">");

            sbHtml.Append("<a href='#' aria-label='Previous' ");
            if (pageToStart < currentPage)
            {
                sbHtml.AppendFormat("onclick='{0}'", HtmlUtility.MakeGotoPageLink(gridContext.GridName, currentPage - 1));
            }
            else
            {
                sbHtml.AppendFormat("onclick='{0}'", "return false;");
            }
            sbHtml.Append(">");
            sbHtml.Append("<span aria-hidden='true'>&laquo; Previous</span></a></li>");

            for (int i = pageToStart; i <= pageToEnd; i++)
            {
                sbHtml.Append("<li");
                if (i == currentPage)
                {
                    sbHtml.Append(" class='active'");
                }
                sbHtml.Append(">");
                sbHtml.AppendFormat("<a href='#' onclick='{0}'>{1}</a></li>", HtmlUtility.MakeGotoPageLink(gridContext.GridName, i), i);
            }


            sbHtml.Append("<li");
            if (pageToEnd == currentPage)
            {
                sbHtml.Append(" class='disabled'");
            }
            sbHtml.Append(">");

            sbHtml.Append("<a href='#' aria-label='Next' ");
            if (pageToEnd > currentPage)
            {
                sbHtml.AppendFormat("onclick='{0}'", HtmlUtility.MakeGotoPageLink(gridContext.GridName, currentPage + 1));
            }
            else
            {
                sbHtml.AppendFormat("onclick='{0}'", "return false;");
            }
            sbHtml.Append(">");
            sbHtml.Append("<span aria-hidden='true'>Next &raquo;</span></a></li>");

            sbHtml.Append("</ul>");
            sbHtml.Append("</div>");
            sbHtml.Append("</div>");
        }



    }
}
