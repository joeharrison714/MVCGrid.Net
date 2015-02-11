using MVCGrid.Interfaces;
using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCGrid.Models
{
    public abstract class GridDefinitionBase
    {
        internal abstract List<Row> GetData(GridContext context, out int? totalRecords);
    }

    public class GridDefinition<T1> : GridDefinitionBase, IMVCGridDefinition
    {
        public GridDefinition() : this(new GridDefaults())
        {
        }

        public GridDefinition(GridDefaults gridDefaults):base()
        {
            Columns = new List<GridColumn<T1>>();

            if (gridDefaults != null)
            {
                this.PreloadData = gridDefaults.PreloadData;
                this.Paging = gridDefaults.Paging;
                this.ItemsPerPage = gridDefaults.ItemsPerPage;
                this.Sorting = gridDefaults.Sorting;
                this.DefaultSortColumn = gridDefaults.DefaultSortColumn;
                this.NoResultsMessage = gridDefaults.NoResultsMessage;
                this.ClientSideLoadingMessageFunctionName = gridDefaults.ClientSideLoadingMessageFunctionName;
                this.ClientSideLoadingCompleteFunctionName = gridDefaults.ClientSideLoadingCompleteFunctionName;
                this.Filtering = gridDefaults.Filtering;
                this.DefaultRenderingEngine = gridDefaults.DefaultRenderingEngine;
            }
        }

        public IEnumerable<IMVCGridColumn> GetColumns()
        {
            return Columns.Cast<IMVCGridColumn>();
        }

        public void AddColumn(GridColumn<T1> column)
        {
            string thisName = column.ColumnName;
            if (String.IsNullOrWhiteSpace(thisName))
            {
                throw new ArgumentException("Please specify a unique column name for each column", "column.ColumnName");
            }
            column.ColumnName = column.ColumnName.Trim();

            if (Columns.Any(p => p.ColumnName.ToLower() == column.ColumnName.ToLower()))
            {
                throw new ArgumentException(
                    String.Format("There is already a column added with the name '{0}'", column.ColumnName),
                    "column.ColumnName");
            }

            if (column.ValueExpression == null)
            {
                throw new ArgumentException(
                    String.Format("Column '{0}' is missing a value expression.", column.ColumnName), 
                    "column.ValueExpression");
            }

            Columns.Add(column);
        }

        private List<GridColumn<T1>> Columns { get; set; }

        public Func<QueryOptions, QueryResult<T1>> RetrieveData { get; set; }
        public Func<T1, GridContext, string> RowCssClassExpression { get; set; }

        internal override List<Row> GetData(GridContext context, out int? totalRecords)
        {
            List<Row> resultRows = new List<Row>();

            var queryResult = RetrieveData(context.QueryOptions);
            totalRecords = queryResult.TotalRecords;

            if (context.GridDefinition.Paging && !totalRecords.HasValue)
            {
                throw new Exception("When paging is enabled, QueryResult must contain the TotalRecords");
            }

            foreach (var item in queryResult.Items)
            {
                Row thisRow = new Row();

                if (RowCssClassExpression != null)
                {
                    string rowCss = RowCssClassExpression(item, context);
                    if (!String.IsNullOrWhiteSpace(rowCss))
                    {
                        thisRow.CalculatedCssClass = rowCss;
                    }
                }

                foreach (var col in this.Columns)
                {
                    Cell thisCell = new Cell();
                    thisRow.Cells.Add(col.ColumnName, thisCell);

                    thisCell.HtmlText = col.ValueExpression(item, context);

                    if (col.HtmlEncode)
                    {
                        thisCell.HtmlText = System.Web.HttpUtility.HtmlEncode(thisCell.HtmlText);
                    }

                    thisCell.PlainText = thisCell.HtmlText;
                    if (col.PlainTextValueExpression != null)
                    {
                        thisCell.PlainText = col.PlainTextValueExpression(item, context);
                    }

                    if (col.CellCssClassExpression != null)
                    {
                        string cellCss = col.CellCssClassExpression(item, context);
                        if (!String.IsNullOrWhiteSpace(cellCss))
                        {
                            thisCell.CalculatedCssClass = cellCss;
                        }
                    }
                }

                resultRows.Add(thisRow);
            }

            return resultRows;
        }

        /// <summary>
        /// Prefix for query string names. Only needed if there is more than 1 grid on the same page.
        /// </summary>
        public string QueryStringPrefix { get; set; }

        public bool PreloadData { get; set; }

        public bool Paging { get; set; }
        public int ItemsPerPage { get; set; }
        public bool Sorting { get; set; }
        public string DefaultSortColumn { get; set; }
        public bool Filtering { get; set; }
        public string NoResultsMessage { get; set; }

        public string ClientSideLoadingMessageFunctionName { get; set; }
        public string ClientSideLoadingCompleteFunctionName { get; set; }

        public Type DefaultRenderingEngine { get; set; }
    }

}
