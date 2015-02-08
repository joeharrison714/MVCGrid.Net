using MVCGrid.Interfaces;
using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MVCGrid.Web
{
    internal class QueryStringParser
    {
        public static QueryOptions ParseOptions(IMVCGridDefinition grid, HttpRequest httpRequest)
        {
            string qsKeyPage = grid.QueryStringPrefix + "page";
            string qsKeySort = grid.QueryStringPrefix + "sort";
            string qsKeyDirection = grid.QueryStringPrefix + "dir";

            var options = new QueryOptions();
            options.ItemsPerPage = 20;

            options.PageIndex = 0;
            if (httpRequest.QueryString[qsKeyPage] != null)
            {
                int pageNum;
                if (Int32.TryParse(httpRequest.QueryString[qsKeyPage], out pageNum))
                {
                    options.PageIndex = pageNum - 1;
                    if (options.PageIndex < 0) options.PageIndex = 0;
                }
            }

            options.SortColumn = null;
            if (httpRequest.QueryString[qsKeySort] != null)
            {
                string sortColName = httpRequest.QueryString[qsKeySort];

                // validate SortColumn
                var colDef = grid.GetColumns().SingleOrDefault(p => p.ColumnName == sortColName);

                if (colDef == null)
                {
                    sortColName = null;
                }
                else
                {
                    if (!colDef.EnableSorting)
                    {
                        sortColName = null;
                    }
                }

                options.SortColumn = sortColName;
            }

            options.SortDirection = SortDirection.Asc;
            if (httpRequest.QueryString[qsKeyDirection] != null)
            {
                string sortDir = httpRequest.QueryString[qsKeyDirection];
                if (String.Compare(sortDir, "dsc", true) == 0)
                {
                    options.SortDirection = SortDirection.Dsc;
                }
            }

            return options;
        }
    }
}
