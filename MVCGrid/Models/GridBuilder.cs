using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCGrid.Models
{
    public class GridBuilder<T1>
    {
        public GridBuilder()
        {
            GridDefinition = new GridDefinition<T1>();
        }

        public GridDefinition<T1> GridDefinition { get; set; }

        //public GridColumnBuilder<T1> NewColumn()
        //{
        //    return new GridColumnBuilder<T1>();
        //}

        //public GridBuilder<T1> AddColumn(GridColumnBuilder<T1> col)
        //{
        //    GridDefinition.Columns.Add(col.GridColumn);
        //    return this;
        //}

        public GridBuilder<T1> AddColumn(Action<GridColumnBuilder<T1>> col)
        {
            var newCol = new GridColumnBuilder<T1>();
            col.Invoke(newCol);
            GridDefinition.Columns.Add(newCol.GridColumn);
            return this;
        }

        public Func<T1, GridContext, string> ValueExpression { get; set; }

        public GridBuilder<T1> WithRetrieveData(Func<QueryOptions, QueryResult<T1>> retrieveData)
        {
            GridDefinition.RetrieveData = retrieveData;
            return this;
        }
    }
}
