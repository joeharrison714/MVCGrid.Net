using MVCGrid.Interfaces;
using MVCGrid.Models;
using MVCGrid.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MVCGrid.Rendering
{
    public class RazorRenderingEngine : IMVCGridRenderingEngine
    {
        public bool AllowsPaging
        {
            get { return true; }
        }

        public void PrepareResponse(System.Web.HttpResponse response)
        {
        }

        public void Render(Models.GridData data, Models.GridContext gridContext, System.IO.Stream outputStream)
        {
            RenderingModel model = new RenderingModel();

            model.HandlerPath = HtmlUtility.GetHandlerPath();
            model.TableHtmlId = HtmlUtility.GetTableHtmlId(gridContext.GridName);
            
            PrepColumns(gridContext, model);
            PrepRows(data, gridContext, model);

            if (model.Rows.Count == 0)
            {
                model.NoResultsMessage = gridContext.GridDefinition.NoResultsMessage;
            }

            model.PagingModel = null;
            if (gridContext.QueryOptions.ItemsPerPage.HasValue)
            {
                model.PagingModel = new PagingModel();

                int currentPageIndex = gridContext.QueryOptions.PageIndex.Value;

                model.PagingModel.FirstRecord = (currentPageIndex * gridContext.QueryOptions.ItemsPerPage.Value) + 1;
                model.PagingModel.LastRecord = (model.PagingModel.FirstRecord + gridContext.QueryOptions.ItemsPerPage.Value) - 1;
                if (model.PagingModel.LastRecord > data.TotalRecords)
                {
                    model.PagingModel.LastRecord = data.TotalRecords.Value;
                }
                model.PagingModel.CurrentPage = currentPageIndex + 1;
                model.PagingModel.GotoPageLinkFormatString = String.Format("MVCGrid.setPage(\"{0}\", ", gridContext.GridName);
                model.PagingModel.GotoPageLinkFormatString += "{0}); return false;";

                var numberOfPagesD = (data.TotalRecords.Value + 0.0) / (gridContext.QueryOptions.ItemsPerPage.Value + 0.0);
                model.PagingModel.NumberOfPages = (int)Math.Ceiling(numberOfPagesD);
            }

            BootstrapHtmlWriter writer = new BootstrapHtmlWriter();
            var test = writer.WriteHtml(model);

            using (StreamWriter sw = new StreamWriter(outputStream))
            {
                sw.Write(test);
            }
        }

        private static void PrepRows(Models.GridData data, Models.GridContext gridContext, RenderingModel model)
        {
            foreach (var item in data.Rows)
            {
                Row renderingRow = new Row();
                model.Rows.Add(renderingRow);

                if (!String.IsNullOrWhiteSpace(item.RowCssClass))
                {
                    renderingRow.CalculatedCssClass = item.RowCssClass;
                }

                foreach (var col in gridContext.GetVisibleColumns())
                {
                    string val = "";

                    if (item.Values.ContainsKey(col.ColumnName))
                    {
                        val = item.Values[col.ColumnName];
                    }

                    Cell renderingCell = new Cell();
                    renderingRow.Cells.Add(col.ColumnName, renderingCell);
                    if (item.CellCssClasses.ContainsKey(col.ColumnName))
                    {
                        string cellCss = item.CellCssClasses[col.ColumnName];
                        if (!String.IsNullOrWhiteSpace(cellCss))
                        {
                            renderingCell.CalculatedCssClass = cellCss;
                        }
                    }

                    if (col.HtmlEncode)
                    {
                        renderingCell.HtmlText = HttpUtility.HtmlEncode(val);
                    }
                    else
                    {
                        renderingCell.HtmlText = val;
                    }
                }
            }
        }

        private static void PrepColumns(Models.GridContext gridContext, RenderingModel model)
        {
            foreach (var col in gridContext.GetVisibleColumns())
            {
                Column renderingColumn = new Column();
                model.Columns.Add(renderingColumn);
                renderingColumn.Name = col.ColumnName;
                renderingColumn.HeaderText = col.HeaderText;

                if (gridContext.GridDefinition.Sorting && col.EnableSorting)
                {
                    SortDirection linkDirection = SortDirection.Asc;
                    SortDirection iconDirection = SortDirection.Unspecified;

                    if (gridContext.QueryOptions.SortColumn == col.ColumnName && gridContext.QueryOptions.SortDirection == SortDirection.Asc)
                    {
                        iconDirection = SortDirection.Asc;
                        linkDirection = SortDirection.Dsc;
                    }
                    else if (gridContext.QueryOptions.SortColumn == col.ColumnName && gridContext.QueryOptions.SortDirection == SortDirection.Dsc)
                    {
                        iconDirection = SortDirection.Dsc;
                        linkDirection = SortDirection.Asc;
                    }
                    else
                    {
                        iconDirection = SortDirection.Unspecified;
                        linkDirection = SortDirection.Dsc;
                    }

                    renderingColumn.Onclick = HtmlUtility.MakeSortLink(gridContext.GridName, col.ColumnName, linkDirection);
                    renderingColumn.SortIconDirection = iconDirection;
                }
            }
        }
    }
}
