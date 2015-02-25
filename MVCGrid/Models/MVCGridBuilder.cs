using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCGrid.Models
{
    public class MVCGridBuilder<T1>
    {
        ColumnDefaults _columnDefaults = null;

        public MVCGridBuilder()
        {
            GridDefinition = new GridDefinition<T1>();
        }

        public MVCGridBuilder(GridDefaults gridDefaults)
            : this(gridDefaults, null)
        {
        }

        public MVCGridBuilder(ColumnDefaults columnDefaults)
            : this(null, columnDefaults)
        {
        }

        public MVCGridBuilder(GridDefaults gridDefaults, ColumnDefaults columnDefaults)
        {
            GridDefinition = new GridDefinition<T1>(gridDefaults);

            _columnDefaults = columnDefaults;
        }

        public GridDefinition<T1> GridDefinition { get; set; }

        public MVCGridBuilder<T1> AddColumn(string name, string headerText, Func<T1, GridContext, string> valueExpression,
            bool enableSort = true, bool htmlEncode = true, Func<T1, GridContext, string> plainTextValueExpression = null,
            Func<T1, GridContext, string> cellCssClassExpression = null)
        {
            var col = new GridColumn<T1>();
            col.ColumnName = name;
            col.HeaderText = headerText;
            col.ValueExpression = valueExpression;
            col.HtmlEncode = htmlEncode;
            col.EnableSorting = enableSort;
            col.PlainTextValueExpression = plainTextValueExpression;
            col.CellCssClassExpression = cellCssClassExpression;
            this.GridDefinition.AddColumn(col);
            return this;
        }

        public MVCGridBuilder<T1> AddColumns(Action<GridColumnListBuilder<T1>> columns)
        {
            GridColumnListBuilder<T1> cols = new GridColumnListBuilder<T1>(_columnDefaults);
            columns.Invoke(cols);

            foreach (var col in cols.ColumnBuilders)
            {
                GridDefinition.AddColumn(col.GridColumn);
            }

            return this;
        }

        public MVCGridBuilder<T1> AddColumn(GridColumn<T1> column)
        {
            GridDefinition.AddColumn(column);
            return this;
        }


        /// <summary>
        /// This is the method that will actually query the data to populate the grid. Use entity framework, a module from you IoC container, direct SQL queries, etc. to get the data. Inside the providee GridContext there is a QueryOptions object which will be populated with the currently requested sorting, paging, and filtering options which you must take into account. See the QueryOptions documentation below. You must return a QueryResult object which takes an enumerable of your type and a count of the total number of records which must be provided if paging is enabled.
        /// </summary>
        public MVCGridBuilder<T1> WithRetrieveDataMethod(Func<GridContext, QueryResult<T1>> retrieveData)
        {
            GridDefinition.RetrieveData = retrieveData;
            return this;
        }

        /// <summary>
        /// Use this to specify a custom css class based on data for the current row
        /// </summary>
        public MVCGridBuilder<T1> WithRowCssClassExpression(Func<T1, GridContext, string> rowCssClassExpression)
        {
            GridDefinition.RowCssClassExpression = rowCssClassExpression;
            return this;
        }

        /// <summary>
        /// A prefix to add to all query string parameters for this grid, for when there are more than 1 grids on the same page
        /// </summary>
        public MVCGridBuilder<T1> WithQueryStringPrefix(string prefix)
        {
            GridDefinition.QueryStringPrefix = prefix;
            return this;
        }

        /// <summary>
        /// Enables data loading when the page is first loaded so that the initial ajax request can be skipped.
        /// </summary>
        public MVCGridBuilder<T1> WithPreloadData(bool preload)
        {
            GridDefinition.PreloadData = preload;
            return this;
        }

        /// <summary>
        /// Enables paging on the grid
        /// </summary>
        public MVCGridBuilder<T1> WithPaging(bool paging)
        {
            GridDefinition.Paging = paging;
            return this;
        }

        /// <summary>
        /// Number of items to display on each page
        /// </summary>
        public MVCGridBuilder<T1> WithItemsPerPage(int itemsPerPage)
        {
            GridDefinition.ItemsPerPage = itemsPerPage;
            return this;
        }

        /// <summary>
        /// Enables sorting on the grid. Note, sorting must also be enabled on each column where sorting is wanted
        /// </summary>
        public MVCGridBuilder<T1> WithSorting(bool sorting)
        {
            GridDefinition.Sorting = sorting;
            return this;
        }

        /// <summary>
        /// The default column to sort by when no sort is specified
        /// </summary>
        public MVCGridBuilder<T1> WithDefaultSortColumn(string defaultSortColumn)
        {
            GridDefinition.DefaultSortColumn = defaultSortColumn;
            return this;
        }

        /// <summary>
        /// The default order to sort by when no sort is specified
        /// </summary>
        public MVCGridBuilder<T1> WithDefaultSortDirection(SortDirection sortDirection)
        {
            GridDefinition.DefaultSortDirection = sortDirection;
            return this;
        }

        /// <summary>
        /// Text to display when there are no results.
        /// </summary>
        public MVCGridBuilder<T1> WithNoResultsMessage(string noResultsMessage)
        {
            GridDefinition.NoResultsMessage = noResultsMessage;
            return this;
        }

        /// <summary>
        /// Name of function to call before ajax call begins
        /// </summary>
        public MVCGridBuilder<T1> WithClientSideLoadingMessageFunctionName(string name)
        {
            GridDefinition.ClientSideLoadingMessageFunctionName = name;
            return this;
        }

        /// <summary>
        /// Name of function to call before ajax call ends
        /// </summary>
        public MVCGridBuilder<T1> WithClientSideLoadingCompleteFunctionName(string name)
        {
            GridDefinition.ClientSideLoadingCompleteFunctionName = name;
            return this;
        }

        /// <summary>
        /// Enables filtering on the grid. Note, filtering must also be enabled on each column where filtering is wanted
        /// </summary>
        public MVCGridBuilder<T1> WithFiltering(bool filtering)
        {
            GridDefinition.Filtering = filtering;
            return this;
        }

        public MVCGridBuilder<T1> WithRenderingEngine(Type renderingEngineType)
        {
            GridDefinition.RenderingEngine = renderingEngineType;
            return this;
        }

        public MVCGridBuilder<T1> WithTemplatingEngine(Type templatingEngine)
        {
            GridDefinition.TemplatingEngine = templatingEngine;
            return this;
        }

        /// <summary>
        /// Add an arbitrary additional settings
        /// </summary>
        public MVCGridBuilder<T1> WithAdditionalSetting(string name, object value)
        {
            GridDefinition.AdditionalSettings[name] = value;
            return this;
        }

        /// <summary>
        /// Remove an additional setting
        /// </summary>
        public MVCGridBuilder<T1> WithoutAdditionalSetting(string name)
        {
            if (GridDefinition.AdditionalSettings.ContainsKey(name))
            {
                GridDefinition.AdditionalSettings.Remove(name);
            }
            return this;
        }

        /// <summary>
        /// The rendering mode to use for this grid. By default it will use the RenderingEngine rendering mode. If you want to use a custom Razor view to display your grid, change this to Controller
        /// </summary>
        public MVCGridBuilder<T1> WithRenderingMode(RenderingMode mode)
        {
            GridDefinition.RenderingMode = mode;
            return this;
        }

        /// <summary>
        /// When RenderingMode is set to Controller, this is the path to the razor view to use.
        /// </summary>
        public MVCGridBuilder<T1> WithViewPath(string viewPath)
        {
            GridDefinition.ViewPath = viewPath;
            return this;
        }

        /// <summary>
        /// HTML to display in place of the grid when an error occurs
        /// </summary>
        public MVCGridBuilder<T1> WithErrorMessageHtml(string errorMessageHtml)
        {
            GridDefinition.ErrorMessageHtml = errorMessageHtml;
            return this;
        }

        /// <summary>
        /// Add a name to additional query options which are additional parameters that can be passed from client to server side
        /// </summary>
        public MVCGridBuilder<T1> WithAdditionalQueryOptionName(string name)
        {
            GridDefinition.AdditionalQueryOptionNames.Add(name);
            return this;
        }

        /// <summary>
        /// Names of additional parameters that can be passed from client to server side
        /// </summary>
        public MVCGridBuilder<T1> WithAdditionalQueryOptionNames(params string[] names)
        {
            foreach (var name in names)
            {
                GridDefinition.AdditionalQueryOptionNames.Add(name);
            }
            return this;
        }

        /// <summary>
        /// Allows changing of page size from client-side
        /// </summary>
        public MVCGridBuilder<T1> WithAllowChangingPageSize(bool allow)
        {
            GridDefinition.AllowChangingPageSize = allow;
            return this;
        }

        /// <summary>
        /// Sets the maximum of items per page allowed when AllowChangingPageSize is enabled
        /// </summary>
        public MVCGridBuilder<T1> WithMaxItemsPerPage(int maxItems)
        {
            GridDefinition.MaxItemsPerPage = maxItems;
            return this;
        }
    }
}
