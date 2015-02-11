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
        public HtmlRenderingEngine()
        {
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
            RenderingModel model = PrepModel(data, gridContext);

            BootstrapHtmlWriter writer = new BootstrapHtmlWriter();
            var content = writer.WriteHtml(model);

            using (StreamWriter sw = new StreamWriter(outputStream))
            {
                sw.Write(content.ToString());
            }
        }

        private RenderingModel PrepModel(Models.GridData data, Models.GridContext gridContext)
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

                model.PagingModel.TotalRecords = data.TotalRecords.Value;

                model.PagingModel.FirstRecord = (currentPageIndex * gridContext.QueryOptions.ItemsPerPage.Value) + 1;
                model.PagingModel.LastRecord = (model.PagingModel.FirstRecord + gridContext.QueryOptions.ItemsPerPage.Value) - 1;
                if (model.PagingModel.LastRecord > data.TotalRecords)
                {
                    model.PagingModel.LastRecord = data.TotalRecords.Value;
                }
                model.PagingModel.CurrentPage = currentPageIndex + 1;

                var numberOfPagesD = (data.TotalRecords.Value + 0.0) / (gridContext.QueryOptions.ItemsPerPage.Value + 0.0);
                model.PagingModel.NumberOfPages = (int)Math.Ceiling(numberOfPagesD);

                for (int i = 1; i <= model.PagingModel.NumberOfPages; i++)
                {
                    model.PagingModel.PageLinks.Add(i, HtmlUtility.MakeGotoPageLink(gridContext.GridName, i));
                }
            }
            return model;
        }

        private void PrepRows(Models.GridData data, Models.GridContext gridContext, RenderingModel model)
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

        private void PrepColumns(Models.GridContext gridContext, RenderingModel model)
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
