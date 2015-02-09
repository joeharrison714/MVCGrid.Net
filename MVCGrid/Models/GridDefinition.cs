using MVCGrid.Interfaces;
using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCGrid.Models
{
    public class GridDefinition<T1> : IMVCGridDefinition
    {
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

        public GridData GetData(GridContext context)
        {
            GridData result = new GridData();

            var queryResult = RetrieveData(context.QueryOptions);
            result.TotalRecords = queryResult.TotalRecords;

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

    }

}
