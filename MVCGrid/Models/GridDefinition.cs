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
                this.RenderingEngine = gridDefaults.RenderingEngine;
                this.TemplatingEngine = gridDefaults.TemplatingEngine;
                this.AdditionalSettings = gridDefaults.AdditionalSettings;
                this.RenderingMode = gridDefaults.RenderingMode;
                this.ViewPath = gridDefaults.ViewPath;
                this.QueryStringPrefix = gridDefaults.QueryStringPrefix;
                this.ErrorMessageHtml = gridDefaults.ErrorMessageHtml;
                this.AdditionalQueryOptionNames = gridDefaults.AdditionalQueryOptionNames;
                this.AllowChangingPageSize = gridDefaults.AllowChangingPageSize;
                this.MaxItemsPerPage = gridDefaults.MaxItemsPerPage;
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

            if (column.ValueExpression == null && column.ValueTemplate == null)
            {
                throw new ArgumentException(
                    String.Format("Column '{0}' is missing a value expression.", column.ColumnName), 
                    "column.ValueExpression");
            }

            Columns.Add(column);
        }

        private List<GridColumn<T1>> Columns { get; set; }

        /// <summary>
        /// This is the method that will actually query the data to populate the grid. Use entity framework, a module from you IoC container, direct SQL queries, etc. to get the data. Inside the providee GridContext there is a QueryOptions object which will be populated with the currently requested sorting, paging, and filtering options which you must take into account. See the QueryOptions documentation below. You must return a QueryResult object which takes an enumerable of your type and a count of the total number of records which must be provided if paging is enabled.
        /// </summary>
        public Func<GridContext, QueryResult<T1>> RetrieveData { get; set; }

        /// <summary>
        /// Use this to specify a custom css class based on data for the current row
        /// </summary>
        public Func<T1, GridContext, string> RowCssClassExpression { get; set; }

        internal override List<Row> GetData(GridContext context, out int? totalRecords)
        {
            List<Row> resultRows = new List<Row>();

            var queryResult = RetrieveData(context);
            totalRecords = queryResult.TotalRecords;

            if (context.GridDefinition.Paging && !totalRecords.HasValue)
            {
                throw new Exception("When paging is enabled, QueryResult must contain the TotalRecords");
            }

            IMVCGridTemplatingEngine templatingEngine = (IMVCGridTemplatingEngine)Activator.CreateInstance(context.GridDefinition.TemplatingEngine, true);

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

                    thisCell.HtmlText = "";

                    if (col.ValueExpression != null)
                    {
                        thisCell.HtmlText = col.ValueExpression(item, context);
                    }
                    
                    if (!String.IsNullOrWhiteSpace(col.ValueTemplate))
                    {
                        var templateModel = new TemplateModel()
                        {
                            Item = item,
                            GridContext = context,
                            GridColumn = col,
                            Row = thisRow,
                            Value = thisCell.HtmlText
                        };

                        thisCell.HtmlText = templatingEngine.Process(col.ValueTemplate, templateModel);
                    }

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
        /// A prefix to add to all query string parameters for this grid, for when there are more than 1 grids on the same page
        /// </summary>
        public string QueryStringPrefix { get; set; }

        /// <summary>
        /// Enables data loading when the page is first loaded so that the initial ajax request can be skipped.
        /// </summary>
        public bool PreloadData { get; set; }

        /// <summary>
        /// Enables paging on the grid
        /// </summary>
        public bool Paging { get; set; }

        /// <summary>
        /// Number of items to display on each page
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Enables sorting on the grid. Note, sorting must also be enabled on each column where sorting is wanted
        /// </summary>
        public bool Sorting { get; set; }

        /// <summary>
        /// The default column to sort by when no sort is specified
        /// </summary>
        public string DefaultSortColumn { get; set; }

        /// <summary>
        /// The default order to sort by when no sort is specified
        /// </summary>
        public SortDirection DefaultSortDirection { get; set; }

        /// <summary>
        /// Enables filtering on the grid. Note, filtering must also be enabled on each column where filtering is wanted
        /// </summary>
        public bool Filtering { get; set; }

        /// <summary>
        /// Text to display when there are no results.
        /// </summary>
        public string NoResultsMessage { get; set; }

        /// <summary>
        /// Name of function to call before ajax call begins
        /// </summary>
        public string ClientSideLoadingMessageFunctionName { get; set; }

        /// <summary>
        /// Name of function to call before ajax call ends
        /// </summary>
        public string ClientSideLoadingCompleteFunctionName { get; set; }

        public Type RenderingEngine { get; set; }
        public Type TemplatingEngine { get; set; }

        /// <summary>
        /// Arbitrary additional settings
        /// </summary>
        public Dictionary<string, object> AdditionalSettings { get; set; }

        /// <summary>
        /// The rendering mode to use for this grid. By default it will use the RenderingEngine rendering mode. If you want to use a custom Razor view to display your grid, change this to Controller
        /// </summary>
        public RenderingMode RenderingMode { get; set; }

        /// <summary>
        /// When RenderingMode is set to Controller, this is the path to the razor view to use.
        /// </summary>
        public string ViewPath { get; set; }

        /// <summary>
        /// HTML to display in place of the grid when an error occurs
        /// </summary>
        public string ErrorMessageHtml { get; set; }

        /// <summary>
        /// Names of additional parameters that can be passed from client to server side
        /// </summary>
        public HashSet<string> AdditionalQueryOptionNames { get; set; }

        /// <summary>
        /// Allows changing of page size from client-side
        /// </summary>
        public bool AllowChangingPageSize { get; set; }

        /// <summary>
        /// Sets the maximum of items per page allowed when AllowChangingPageSize is enabled
        /// </summary>
        public int? MaxItemsPerPage { get; set; }

        public T GetAdditionalSetting<T>(string name, T defaultValue)
        {
            //return (T)SortColumnData;
            if (!AdditionalSettings.ContainsKey(name))
            {
                return defaultValue;
            }

            var val = (T)AdditionalSettings[name];

            return val;
        }
    }

}
