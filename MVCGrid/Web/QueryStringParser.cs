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
        // NOTE: when adding a new suffix, add code to MVCGridDefinitionTable to verify there is no conflict
        public const string QueryStringSuffix_Page = "page";
        public const string QueryStringSuffix_Sort = "sort";
        public const string QueryStringSuffix_SortDir = "dir";
        public const string QueryStringSuffix_Engine = "engine";
        public const string QueryStringSuffix_ItemsPerPage = "rows";

        public static QueryOptions ParseOptions(IMVCGridDefinition grid, HttpRequest httpRequest)
        {
            string qsKeyPage = grid.QueryStringPrefix + QueryStringSuffix_Page;
            string qsKeySort = grid.QueryStringPrefix + QueryStringSuffix_Sort;
            string qsKeyDirection = grid.QueryStringPrefix + QueryStringSuffix_SortDir;
            string qsKeyEngine = grid.QueryStringPrefix + QueryStringSuffix_Engine;

            var options = new QueryOptions();

            if (httpRequest.QueryString[qsKeyEngine] != null)
            {
                string re = httpRequest.QueryString[qsKeyEngine];
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
                options.SortColumnName = null;
                options.SortDirection = SortDirection.Unspecified;
            }
            else
            {
                options.SortColumnName = null;

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

                options.SortColumnName = sortColName;
                options.SortColumnData = colDef.SortColumnData;
                

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

            if (grid.AdditionalQueryOptionNames.Count > 0)
            {
                foreach (var aqon in grid.AdditionalQueryOptionNames)
                {
                    string qsKeyAQO = grid.QueryStringPrefix + aqon;
                    string val = "";

                    if (httpRequest.QueryString[qsKeyAQO] != null)
                    {
                        val = httpRequest.QueryString[qsKeyAQO];
                    }

                    options.AdditionalQueryOptions.Add(aqon, val);
                }
            }

            return options;
        }
    }
}
