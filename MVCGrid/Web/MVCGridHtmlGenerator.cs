using MVCGrid.Interfaces;
using MVCGrid.Models;
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
    //row/cell style functions

    internal class MVCGridHtmlGenerator
    {
        const string ContainerCssClass = "MVCGridContainer";

        internal static string GetContainerHtmlId(string name)
        {
            return String.Format("MVCGridContainer_{0}", name);
        }

        internal static string GetTableHolderHtmlId(string name)
        {
            return String.Format("MVCGridTableHolder_{0}", name);
        }

        internal static string GetTableHtmlId(string name)
        {
            return String.Format("MVCGridTable_{0}", name);
        }

        internal static string MakeCssClassStirng(HashSet<string> classes)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var c in classes)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        internal static string GenerateBasePageHtml(string gridName, IMVCGridDefinition def)
        {
            GridConfiguration config = def.GridConfiguration;

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.AppendFormat("<div id='{0}' class='{1}'>", GetContainerHtmlId(gridName), ContainerCssClass);

            sbHtml.AppendFormat("<input type='hidden' name='MVCGridName' value='{0}' />", gridName);
            sbHtml.AppendFormat("<input type='hidden' id='MVCGrid_{0}_PageIndex' value='0' />", gridName);
            sbHtml.AppendFormat("<input type='hidden' id='MVCGrid_{0}_SortColumn' value='' />", gridName);
            sbHtml.AppendFormat("<input type='hidden' id='MVCGrid_{0}_SortDirection' value='' />", gridName);

            sbHtml.AppendFormat("<div id='{0}'>", GetTableHolderHtmlId(gridName));
            sbHtml.Append("</div>");



            sbHtml.AppendLine("</div>");

            return sbHtml.ToString();
        }

        internal static string GenerateTable(string gridName, IMVCGridDefinition def, GridData data, QueryOptions options)
        {
            var columns = def.GetColumns();

            StringBuilder sbHtml = new StringBuilder();

            GridConfiguration config = def.GridConfiguration;

            sbHtml.AppendFormat("<table id='{0}'", GetTableHtmlId(gridName));
            if (config.TableCssClasses != null && config.TableCssClasses.Count > 0)
            {
                sbHtml.AppendFormat(" class='{0}'", MakeCssClassStirng(config.TableCssClasses));
            }
            sbHtml.Append(">");


            sbHtml.AppendLine("<thead>");
            sbHtml.AppendLine("  <tr>");
            foreach (var col in columns)
            {
                sbHtml.Append("<th");

                if (col.EnableSorting)
                {
                    SortDirection direction = SortDirection.Asc;
                    if (options.SortColumn == col.ColumnName && options.SortDirection == SortDirection.Asc)
                    {
                        direction = SortDirection.Dsc;
                    }

                    sbHtml.AppendFormat(" onclick='{0}'", MakeSortLink(gridName, col.ColumnName, direction));
                }
                sbHtml.Append(">");

                sbHtml.Append(HttpUtility.HtmlEncode(col.HeaderText));

                if (options.SortColumn == col.ColumnName && options.SortDirection == SortDirection.Asc)
                {
                    sbHtml.Append(" <img src='/content/icon_up_sort_arrow.png' class='pull-right' />");
                }
                else if (options.SortColumn == col.ColumnName && options.SortDirection == SortDirection.Dsc)
                {
                    sbHtml.Append(" <img src='/content/icon_down_sort_arrow.png' class='pull-right' />");
                }
                else
                {
                    sbHtml.Append(" <img src='/content/icon_sort_arrow.png' class='pull-right' />");
                }
                

                sbHtml.Append("</th>");
            }
            sbHtml.AppendLine("  </tr>");
            sbHtml.AppendLine("</thead>");
            sbHtml.AppendLine("<tbody>");

            foreach (var item in data.Rows)
            {
                sbHtml.AppendLine("  <tr>");
                foreach (var col in columns)
                {
                    //string val = col.ValueExpression(item, helper.ViewContext);

                    string val = "";

                    if (item.Data.ContainsKey(col.ColumnName))
                    {
                        val = item.Data[col.ColumnName];
                    }

                    //if (col.HtmlEncode)
                    if (true)
                    {
                        sbHtml.AppendLine(String.Format("    <td>{0}</td>", HttpUtility.HtmlEncode(val)));
                    }
                    else
                    {
                        sbHtml.AppendLine(String.Format("    <td>{0}</td>", val));
                    }
                }
                sbHtml.AppendLine("  </tr>");
            }

            sbHtml.AppendLine("</tbody>");
            sbHtml.AppendLine("</table>");

            MakePaging(gridName, data, options, sbHtml);
            //sbHtml.AppendLine("<nav><ul class='pagination'><li class='disabled'><a href='#' aria-label='Previous'><span aria-hidden='true'>&laquo;</span></a></li><li class='active'><a href='#'>1 <span class='sr-only'>(current)</span></a></li></ul></nav>");

            return sbHtml.ToString();
        }

        private static void MakePaging(string gridName, GridData data, QueryOptions options, StringBuilder sbHtml)
        {
            var numberOfPagesD = (data.TotalRecords + 0.0) / (options.ItemsPerPage + 0.0);
            int numberOfPages = (int)Math.Ceiling(numberOfPagesD);
            int currentPageIndex = options.PageIndex;

            int firstRecord = (currentPageIndex * options.ItemsPerPage) + 1;
            int lastRecord = (firstRecord + options.ItemsPerPage) - 1;
            if (lastRecord > data.TotalRecords)
            {
                lastRecord = data.TotalRecords;
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
                sbHtml.AppendFormat("onclick='{0}'", MakeGotoPageLink(gridName, currentPage - 1));
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
                sbHtml.AppendFormat("<a href='#' onclick='{0}'>{1}</a></li>", MakeGotoPageLink(gridName, i), i);
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
                sbHtml.AppendFormat("onclick='{0}'", MakeGotoPageLink(gridName, currentPage + 1));
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

        private static string MakeGotoPageLink(string gridName, int pageNum)
        {
            return String.Format("MVCGrid.setPage(\"{0}\", {1}); return false;", gridName, pageNum);
        }

        private static string MakeSortLink(string gridName, string columnName, SortDirection direction)
        {
            return String.Format("MVCGrid.setSort(\"{0}\", \"{1}\", \"{2}\"); return false;", gridName, columnName, direction.ToString());
        }
    }
}
