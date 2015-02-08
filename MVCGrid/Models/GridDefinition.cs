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

        public GridDefinition<T1> WithColumn(string name, string headerText, Func<T1, GridContext, string> valueExpression, bool htmlEncode=true)
        {
            var col = new GridColumn<T1>();
            col.ColumnName=name;
            col.HeaderText = headerText;
            col.ValueExpression = valueExpression;
            col.HtmlEncode = htmlEncode;
            this.Columns.Add(col);
            return this;
        }

        public GridDefinition<T1> WithColumn(GridColumn<T1> column)
        {
            this.Columns.Add(column);
            return this;
        }

        public GridDefinition<T1> WithRetrieveData(Func<QueryOptions, QueryResult<T1>> retrieveData)
        {
            this.RetrieveData = retrieveData;
            return this;
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

        public List<GridColumn<T1>> Columns { get; set; }

        public Func<QueryOptions, QueryResult<T1>> RetrieveData { get; set; }

        public GridData GetData(GridContext context)
        {
            GridData result = new GridData();

            var queryResult = RetrieveData(context.QueryOptions);
            result.TotalRecords = queryResult.TotalRecords;

            foreach (var item in queryResult.Items)
            {
                GridRow thisRow = new GridRow();

                foreach (var col in this.Columns)
                {
                    string val = col.ValueExpression(item, context);
                    Console.WriteLine(val);

                    thisRow.Data.Add(col.ColumnName, val);
                }

                result.Rows.Add(thisRow);
            }

            return result;
        }

        /// <summary>
        /// Prefix for query string names. Only needed if there is more than 1 grid on the same page.
        /// </summary>
        public string QueryStringPrefix { get; set; }

    }

}
