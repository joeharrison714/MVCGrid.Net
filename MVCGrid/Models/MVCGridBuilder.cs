using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCGrid.Models
{
    public class MVCGridBuilder<T1>
    {
        public MVCGridBuilder()
        {
            GridDefinition = new GridDefinition<T1>();
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
            GridColumnListBuilder<T1> cols=new GridColumnListBuilder<T1>();
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

        public MVCGridBuilder<T1> WithRetrieveDataMethod(Func<GridContext, QueryResult<T1>> retrieveData)
        {
            GridDefinition.RetrieveData = retrieveData;
            return this;
        }

        public MVCGridBuilder<T1> WithRowCssClassExpression(Func<T1, GridContext, string> rowCssClassExpression)
        {
            GridDefinition.RowCssClassExpression = rowCssClassExpression;
            return this;
        }

        public MVCGridBuilder<T1> WithQueryStringPrefix(string prefix)
        {
            GridDefinition.QueryStringPrefix = prefix;
            return this;
        }

        public MVCGridBuilder<T1> WithPreloadData(bool preload)
        {
            GridDefinition.PreloadData = preload;
            return this;
        }

        public MVCGridBuilder<T1> WithPaging(bool paging)
        {
            GridDefinition.Paging = paging;
            return this;
        }

        public MVCGridBuilder<T1> WithItemsPerPage(int itemsPerPage)
        {
            GridDefinition.ItemsPerPage = itemsPerPage;
            return this;
        }

        public MVCGridBuilder<T1> WithSorting(bool sorting)
        {
            GridDefinition.Sorting = sorting;
            return this;
        }

        public MVCGridBuilder<T1> WithDefaultSortColumn(string defaultSortColumn)
        {
            GridDefinition.DefaultSortColumn = defaultSortColumn;
            return this;
        }

        public MVCGridBuilder<T1> WithNoResultsMessage(string noResultsMessage)
        {
            GridDefinition.NoResultsMessage = noResultsMessage;
            return this;
        }

        public MVCGridBuilder<T1> WithClientSideLoadingMessageFunctionName(string name)
        {
            GridDefinition.ClientSideLoadingMessageFunctionName = name;
            return this;
        }

        public MVCGridBuilder<T1> WithClientSideLoadingCompleteFunctionName(string name)
        {
            GridDefinition.ClientSideLoadingCompleteFunctionName = name;
            return this;
        }

        public MVCGridBuilder<T1> WithFiltering(bool filtering)
        {
            GridDefinition.Filtering = filtering;
            return this;
        }

        public MVCGridBuilder<T1> WithDefaultRenderingEngine(Type renderingEngineType)
        {
            GridDefinition.DefaultRenderingEngine = renderingEngineType;
            return this;
        }
    }
}
