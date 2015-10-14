using MVCGrid.Interfaces;
using MVCGrid.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MVCGrid.Rendering
{
    /// <summary>
    /// A rendering engine that uses boostrap and redraws the grid to represent the data vertically 
    /// which allows for a lot of table columns even when displayed on lower resolutions.
    /// Fully supports sorting from the header as well.
    /// </summary>
    public class BootstrapVerticalRenderingEngine : IMVCGridRenderingEngine
    {
        private readonly string _defaultTableCss;
        private string _htmlImageSortAsc;
        private string _htmlImageSortDsc;

        public BootstrapVerticalRenderingEngine()
        {
            _defaultTableCss = "table table-striped table-bordered";
        }
        public void PrepareResponse(HttpResponse response)
        {
            
        }

        public void Render(RenderingModel model, GridContext gridContext, TextWriter outputStream)
        {
            _htmlImageSortAsc = "<span class='glyphicon glyphicon-triangle-top pull-right' />";
            _htmlImageSortDsc = "<span class='glyphicon glyphicon-triangle-bottom pull-right' />";

            var sbHtml = new StringBuilder();

            sbHtml.AppendFormat("<table id='{0}'", model.TableHtmlId);
            AppendCssAttribute(_defaultTableCss, sbHtml);
            sbHtml.Append(">");

            RenderHeader(model, sbHtml);

            if (model.Rows.Count > 0)
            {
                RenderBody(model, sbHtml);
            }
            else
            {
                sbHtml.Append("<tbody>");
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td colspan='{0}'>", model.Columns.Count());
                sbHtml.Append(model.NoResultsMessage);
                sbHtml.Append("</td>");
                sbHtml.Append("</tr>");
                sbHtml.Append("</tbody>");
            }
            sbHtml.AppendLine("</table>");

            RenderPaging(model, sbHtml);

            outputStream.Write(sbHtml.ToString());
            outputStream.Write(model.ClientDataTransferHtmlBlock);
        }
        private void AppendCssAttribute(string classString, StringBuilder sbHtml)
        {
            if (!String.IsNullOrWhiteSpace(classString))
            {
                sbHtml.Append(String.Format(" class='{0}'", classString));
            }
        }

        private void RenderBody(RenderingModel model, StringBuilder sbHtml)
        {
            sbHtml.AppendLine("<tbody>");

            foreach (var row in model.Rows)
            {
                sbHtml.Append("<tr");
                AppendCssAttribute(row.CalculatedCssClass, sbHtml);
                sbHtml.AppendLine(">");

                foreach (var col in model.Columns)
                {
                    var cell = row.Cells[col.Name];

                    sbHtml.Append("<td");
                    AppendCssAttribute(cell.CalculatedCssClass, sbHtml);
                    sbHtml.Append(">");

                    sbHtml.Append("<span style='display:none;' class='verticalCellLabel'");
                    
                    if (!String.IsNullOrWhiteSpace(col.Onclick))
                    {
                        sbHtml.Append(" style='cursor: pointer;'");
                        sbHtml.AppendFormat(" onclick='{0}'", col.Onclick);
                    }
                    sbHtml.Append(">");
                    sbHtml.Append(col.HeaderText);

                    if (col.SortIconDirection.HasValue)
                    {
                        switch (col.SortIconDirection)
                        {
                            case SortDirection.Asc:
                                sbHtml.Append("<span class='glyphicon glyphicon-triangle-top'></span>");
                                break;
                            case SortDirection.Dsc:
                                sbHtml.Append("<span class='glyphicon glyphicon-triangle-bottom'></span>");
                                break;
                        }
                    }

                    sbHtml.Append("</span>");
                    
                    sbHtml.Append("<span class='verticalCellContent'>");
                    sbHtml.Append(cell.HtmlText);
                    sbHtml.Append("</span>");
                    sbHtml.Append("</td>");
                }
                sbHtml.AppendLine("  </tr>");
            }

            sbHtml.AppendLine("</tbody>");
        }

        private void RenderHeader(RenderingModel model, StringBuilder sbHtml)
        {
            sbHtml.AppendLine("<thead>");
            sbHtml.AppendLine("  <tr>");
            foreach (var col in model.Columns)
            {
                sbHtml.Append("<th");

                if (!String.IsNullOrWhiteSpace(col.Onclick))
                {
                    sbHtml.Append(" style='cursor: pointer;'");
                    sbHtml.AppendFormat(" onclick='{0}'", col.Onclick);
                }
                sbHtml.Append(">");

                sbHtml.Append(col.HeaderText);

                if (col.SortIconDirection.HasValue)
                {
                    switch (col.SortIconDirection)
                    {
                        case SortDirection.Asc:
                            sbHtml.Append(" ");
                            sbHtml.Append(_htmlImageSortAsc);
                            break;
                        case SortDirection.Dsc:
                            sbHtml.Append(" ");
                            sbHtml.Append(_htmlImageSortDsc);
                            break;
                    }
                }
                sbHtml.AppendLine("</th>");
            }
            sbHtml.AppendLine("  </tr>");
            sbHtml.AppendLine("</thead>");
        }

        private void RenderPaging(RenderingModel model, StringBuilder sbHtml)
        {
            if (model.PagingModel == null)
            {
                return;
            }

            PagingModel pagingModel = model.PagingModel;

            sbHtml.Append("<div class=\"row\">");
            sbHtml.Append("<div class=\"col-xs-6\">");
            sbHtml.AppendFormat("Showing {0} to {1} of {2} entries",
                pagingModel.FirstRecord, pagingModel.LastRecord, pagingModel.TotalRecords
                );
            sbHtml.Append("</div>");


            sbHtml.Append("<div class=\"col-xs-6\">");
            int pageToStart;
            int pageToEnd;
            pagingModel.CalculatePageStartAndEnd(5, out pageToStart, out pageToEnd);

            sbHtml.Append("<ul class='pagination pull-right' style='margin-top: 0;'>");

            sbHtml.Append("<li");
            if (pageToStart == pagingModel.CurrentPage)
            {
                sbHtml.Append(" class='disabled'");
            }
            sbHtml.Append(">");

            sbHtml.Append("<a href='#' aria-label='Previous' ");
            if (pageToStart < pagingModel.CurrentPage)
            {
                sbHtml.AppendFormat("onclick='{0}'", pagingModel.PageLinks[pagingModel.CurrentPage - 1]);
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
                if (i == pagingModel.CurrentPage)
                {
                    sbHtml.Append(" class='active'");
                }
                sbHtml.Append(">");
                sbHtml.AppendFormat("<a href='#' onclick='{0}'>{1}</a></li>", pagingModel.PageLinks[i], i);
            }


            sbHtml.Append("<li");
            if (pageToEnd == pagingModel.CurrentPage)
            {
                sbHtml.Append(" class='disabled'");
            }
            sbHtml.Append(">");

            sbHtml.Append("<a href='#' aria-label='Next' ");
            if (pageToEnd > pagingModel.CurrentPage)
            {
                sbHtml.AppendFormat("onclick='{0}'", pagingModel.PageLinks[pagingModel.CurrentPage + 1]);
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


        public void RenderContainer(ContainerRenderingModel model, TextWriter outputStream)
        {
            outputStream.Write(model.InnerHtmlBlock);
        }

        public bool AllowsPaging { get { return true; } }
    }
}
