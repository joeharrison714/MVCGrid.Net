using System.IO;
using System.Reflection;
using MVCGrid.Interfaces;
using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MVCGrid.Models
{
    public abstract class GridDefinitionBase
    {
        internal abstract List<Row> GetData(GridContext context, out int? totalRecords);
    }

    public class GridDefinition<T1> : GridDefinitionBase, IMVCGridDefinition
    {
        public GridDefinition() : this(null)
        {
        }

        public GridDefinition(GridDefaults gridDefaults)
            : base()
        {
            Columns = new List<GridColumn<T1>>();

            if (gridDefaults == null)
            {
                gridDefaults = new GridDefaults();
            }
            this.PreloadData = gridDefaults.PreloadData;
            this.QueryOnPageLoad = gridDefaults.QueryOnPageLoad;
            this.Paging = gridDefaults.Paging;
            this.ItemsPerPage = gridDefaults.ItemsPerPage;
            this.Sorting = gridDefaults.Sorting;
            this.DefaultSortColumn = gridDefaults.DefaultSortColumn;
            this.NoResultsMessage = gridDefaults.NoResultsMessage;
            this.ClientSideLoadingMessageFunctionName = gridDefaults.ClientSideLoadingMessageFunctionName;
            this.ClientSideLoadingCompleteFunctionName = gridDefaults.ClientSideLoadingCompleteFunctionName;
            this.Filtering = gridDefaults.Filtering;
            //this.RenderingEngine = gridDefaults.RenderingEngine;
            this.TemplatingEngine = gridDefaults.TemplatingEngine;
            this.AdditionalSettings = gridDefaults.AdditionalSettings;
            this.RenderingMode = gridDefaults.RenderingMode;
            this.ViewPath = gridDefaults.ViewPath;
            this.ContainerViewPath = gridDefaults.ContainerViewPath;
            this.QueryStringPrefix = gridDefaults.QueryStringPrefix;
            this.ErrorMessageHtml = gridDefaults.ErrorMessageHtml;
            this.AdditionalQueryOptionNames = gridDefaults.AdditionalQueryOptionNames;
            this.PageParameterNames = gridDefaults.PageParameterNames;
            this.AllowChangingPageSize = gridDefaults.AllowChangingPageSize;
            this.MaxItemsPerPage = gridDefaults.MaxItemsPerPage;
            this.AuthorizationType = gridDefaults.AuthorizationType;
            this.BrowserNavigationMode = gridDefaults.BrowserNavigationMode;

            this.RenderingEngines = gridDefaults.RenderingEngines;
            this.DefaultRenderingEngineName = gridDefaults.DefaultRenderingEngineName;
            this.SpinnerEnabled = gridDefaults.SpinnerEnabled;
            this.SpinnerRadius = gridDefaults.SpinnerRadius;
            this.EnableRowSelect = gridDefaults.EnableRowSelect;
            this.ClientSideRowSelectFunctionName = gridDefaults.ClientSideRowSelectFunctionName;
            this.ClientSideRowSelectProperties = gridDefaults.ClientSideRowSelectProperties;
        }

        [Obsolete("RenderingEngine is obsolete. Please user RenderingEngines and DefaultRenderingEngineName")]
        public Type RenderingEngine
        {
            get
            {
                if (RenderingEngines[DefaultRenderingEngineName] == null)
                {
                    return null;
                }
                string typeName = RenderingEngines[DefaultRenderingEngineName].Type;

                Type t = Type.GetType(typeName, true);
                return t;
            }
            set
            {
                string fullyQualifiedName = value.AssemblyQualifiedName;
                string name = value.Name;

                RenderingEngines.Add(new ProviderSettings(name, fullyQualifiedName));
                DefaultRenderingEngineName = name;
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

        /// <summary>
        /// Use this to specify a custom css class for the current row
        /// </summary>
        public string RowCssClass { get; set; }

        internal override List<Row> GetData(GridContext context, out int? totalRecords)
        {
            var resultRows = new List<Row>();

            var queryResult = RetrieveData(context);
            totalRecords = queryResult.TotalRecords;

            if (context.GridDefinition.Paging && !totalRecords.HasValue)
            {
                throw new Exception("When paging is enabled, QueryResult must contain the TotalRecords");
            }

            var templatingEngine = (IMVCGridTemplatingEngine)Activator.CreateInstance(context.GridDefinition.TemplatingEngine, true);
            var rowSelectProperties = GetRowSelectProperties(context);

            foreach (var item in queryResult.Items)
            {
                var thisRow = new Row
                {
                    CalculatedCssClass = String.Empty,
                    RowSelectEventParameters = rowSelectProperties != null ? GetEventParameters(rowSelectProperties, item) : null
                };

                if (!String.IsNullOrEmpty(RowCssClass))
                {
                    thisRow.CalculatedCssClass = RowCssClass.Trim();
                }

                if (RowCssClassExpression != null)
                {
                    string rowCss = RowCssClassExpression(item, context);
                    if (!String.IsNullOrWhiteSpace(rowCss))
                    {
                        thisRow.CalculatedCssClass = String.Join(" ", thisRow.CalculatedCssClass, rowCss.Trim());
                    }
                }

                foreach (var col in this.Columns)
                {
                    Cell thisCell = new Cell
                    {
                        CalculatedCssClass = String.Empty
                    };
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

                    if (!String.IsNullOrEmpty(col.CellCssClass))
                    {
                        thisCell.CalculatedCssClass = col.CellCssClass.Trim();
                    }

                    if (col.CellCssClassExpression != null)
                    {
                        string cellCss = col.CellCssClassExpression(item, context);
                        if (!String.IsNullOrWhiteSpace(cellCss))
                        {
                            thisCell.CalculatedCssClass = String.Join(" ", thisCell.CalculatedCssClass, cellCss.Trim());
                        }
                    }
                }

                resultRows.Add(thisRow);
            }

            return resultRows;
        }

        private IEnumerable<PropertyInfo> GetRowSelectProperties(GridContext context)
        {
            IEnumerable<PropertyInfo> properties = null;
            if (context.GridDefinition.EnableRowSelect && context.GridDefinition.ClientSideRowSelectProperties != null &&
                context.GridDefinition.ClientSideRowSelectProperties.Any())
            {
                var objectType = typeof(T1);
                properties = objectType.GetProperties()
                    .Where(x => x.CanRead && context.GridDefinition.ClientSideRowSelectProperties.Contains(x.Name, StringComparer.OrdinalIgnoreCase));
            }

            return properties;
        }

        private string GetEventParameters(IEnumerable<PropertyInfo> properties, T1 item)
        {
            if (properties == null || !properties.Any())
                return String.Empty;

            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.None;
                writer.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
                writer.WriteStartObject();
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(item, null);

                    writer.WritePropertyName(property.Name);
                    writer.WriteValue(propertyValue ?? String.Empty);
                }
                writer.WriteEndObject();
            }

            return sb.ToString();
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
        /// Specified if the data should be loaded as soon as the page loads
        /// </summary>
        public bool QueryOnPageLoad { get; set; }

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

        //public Type RenderingEngine { get; set; }
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
        /// When RenderingMode is set to Controller, this is the path to the container razor view to use.
        /// </summary>
        public string ContainerViewPath { get; set; }

        /// <summary>
        /// HTML to display in place of the grid when an error occurs
        /// </summary>
        public string ErrorMessageHtml { get; set; }

        /// <summary>
        /// Names of additional parameters that can be passed from client to server side
        /// </summary>
        public HashSet<string> AdditionalQueryOptionNames { get; set; }

        /// <summary>
        /// Names of page parameters that will be passed from the view
        /// </summary>
        public HashSet<string> PageParameterNames { get; set; }

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

        /// <summary>
        /// Indicated the authorization type. Anonymous access is the default.
        /// </summary>
        public AuthorizationType AuthorizationType { get; set; }

        /// <summary>
        /// Sets the browser navigation mode for the grid.  PreserveAllGridActions is the default.
        /// </summary>
        public BrowserNavigationMode BrowserNavigationMode { get; set; }

        /// <summary>
        /// Perists the latest grid state in a cookie so that it will be reloaded the next time the user navigates to the page. Default is false.
        /// </summary>
        public bool PersistLastState { get; set; }

        public ProviderSettingsCollection RenderingEngines { get; set; }
        public string DefaultRenderingEngineName { get; set; }

        /// <summary>
        /// Enables or disables spinner for the grid
        /// </summary>
        public bool SpinnerEnabled { get; set; }

        /// <summary>
        /// The target DOM element ID for the spinner
        /// </summary>
        public string SpinnerTargetElementId { get; set; }

        /// <summary>
        /// Sets the size of the spinner
        /// </summary>
        public int SpinnerRadius { get; set; }

        /// <summary>
        /// Enables the ability to select by row
        /// </summary>
        public bool EnableRowSelect { get; set; }

        /// <summary>
        /// Client side function to call when a row is selected
        /// </summary>
        public string ClientSideRowSelectFunctionName { get; set; }

        /// <summary>
        /// Arguments to pass to the client side row select function
        /// </summary>
        public List<string> ClientSideRowSelectProperties { get; set; }
    }

}
