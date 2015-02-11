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

            PrepColumns(gridContext, model);
            PrepRows(data, gridContext, model);

            string filename = HttpContext.Current.Server.MapPath("~/App_Data/Template.cshtml");
            string templateContents;

            using (StreamReader sr = new StreamReader(filename))
            {
                templateContents = sr.ReadToEnd();
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
                        renderingCell.Html = HttpUtility.HtmlEncode(val);
                    }
                    else
                    {
                        renderingCell.Html = val;
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
