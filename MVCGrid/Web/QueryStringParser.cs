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

            if (httpRequest.QueryString["engine"] != null)
            {
                string re = httpRequest.QueryString["engine"];
                options.RenderingEngineName = re;
            }

            if (!grid.Paging)
            {
                options.ItemsPerPage = null;
                options.PageIndex = null;
            }
            else
            {
                options.ItemsPerPage = grid.ItemsPerPage;

                if (options.ItemsPerPage <= 0)
                {
                    options.ItemsPerPage = 20;
                }

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
            }

            if (!grid.Filtering)
            {
                //options.Filters
            }
            else
            {
                var filterableColumns = grid.GetColumns().Where(p => p.EnableFiltering);

                foreach (var col in filterableColumns)
                {
                    string qsKey = grid.QueryStringPrefix + col.ColumnName;

                    if (httpRequest.QueryString[qsKey] != null)
                    {
                        string filterValue = httpRequest.QueryString[qsKey];

                        if (!String.IsNullOrWhiteSpace(filterValue))
                        {
                            options.Filters.Add(col.ColumnName, filterValue);
                        }
                    }
                }
            }

            if (!grid.Sorting)
            {
                options.SortColumn = null;
                options.SortDirection = SortDirection.Asc;
            }
            else
            {
                options.SortColumn = null;

                string sortColName = null;
                if (httpRequest.QueryString[qsKeySort] != null)
                {
                    sortColName = httpRequest.QueryString[qsKeySort];
                }

                if (String.IsNullOrWhiteSpace(sortColName))
                {
                    sortColName = grid.DefaultSortColumn;
                }

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
                

                options.SortDirection = SortDirection.Asc;
                if (httpRequest.QueryString[qsKeyDirection] != null)
                {
                    string sortDir = httpRequest.QueryString[qsKeyDirection];
                    if (String.Compare(sortDir, "dsc", true) == 0)
                    {
                        options.SortDirection = SortDirection.Dsc;
                    }
                }
            }

            return options;
        }
    }
}
