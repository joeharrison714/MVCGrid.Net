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
        internal abstract GridData GetData(GridContext context);
    }

    public class GridDefinition<T1> : GridDefinitionBase, IMVCGridDefinition
    {
        const string DefaultNoResultsMessage = "No results.";

        public GridDefinition() : this(null)
        {
            
        }

        public GridDefinition(GridConfiguration copyFromConfig):base()
        {
            if (copyFromConfig != null)
            {
                GridConfiguration = GridConfiguration.CopyFrom(copyFromConfig);
            }

            Columns = new List<GridColumn<T1>>();
            NoResultsMessage = DefaultNoResultsMessage;
        }

        public GridConfiguration GridConfiguration { get; set; }

        public IEnumerable<IMVCGridColumn> GetColumns()
        {
            List<IMVCGridColumn> interfaceList = new List<IMVCGridColumn>();
            foreach (var col in Columns)
            {
                interfaceList.Add(col);
            }
            return interfaceList;
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

        internal override GridData GetData(GridContext context)
        {
            GridData result = new GridData();

            var queryResult = RetrieveData(context.QueryOptions);
            result.TotalRecords = queryResult.TotalRecords;

            if (context.GridDefinition.Paging && !result.TotalRecords.HasValue)
            {
                throw new Exception("When paging is enabled, QueryResult must contain the TotalRecords");
            }

            foreach (var item in queryResult.Items)
            {
                GridRow thisRow = new GridRow();

                if (RowCssClassExpression != null)
                {
                    string rowCss = RowCssClassExpression(item, context);
                    if (!String.IsNullOrWhiteSpace(rowCss))
                    {
                        thisRow.RowCssClass = rowCss;
                    }
                }

                foreach (var col in this.Columns)
                {
                    string val = col.ValueExpression(item, context);

                    string plainVal = val;
                    if (col.PlainTextValueExpression != null)
                    {
                        plainVal = col.PlainTextValueExpression(item, context);
                    }

                    thisRow.Values.Add(col.ColumnName, val);
                    thisRow.PlainTextValues.Add(col.ColumnName, plainVal);

                    if (col.CellCssClassExpression != null)
                    {
                        string cellCss = col.CellCssClassExpression(item, context);
                        if (!String.IsNullOrWhiteSpace(cellCss))
                        {
                            thisRow.CellCssClasses.Add(col.ColumnName, cellCss);
                        }
                    }
                }

                result.Rows.Add(thisRow);
            }

            return result;
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

    }

}
