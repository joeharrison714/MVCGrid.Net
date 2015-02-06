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

        public GridDefinition<T1> WithColumn(string name, string headerText, Func<T1, ControllerContext, string> valueExpression)
        {
            var col = new GridColumn<T1>();
            col.ColumnName=name;
            col.HeaderText = headerText;
            col.ValueExpression = valueExpression;
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

        public GridData GetData(QueryOptions options)
        {
            GridData result = new GridData();

            var queryResult = RetrieveData(options);
            result.TotalRecords = queryResult.TotalRecords;

            foreach (var item in queryResult.Items)
            {
                GridRow thisRow = new GridRow();

                foreach (var col in this.Columns)
                {
                    string val = col.ValueExpression(item, null);
                    Console.WriteLine(val);

                    thisRow.Data.Add(col.ColumnName, val);
                }

                result.Rows.Add(thisRow);
            }

            return result;
        }

    }

}
