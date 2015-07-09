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
        public const string QueryStringSuffix_ItemsPerPage = "pagesize";
        public const string QueryStringSuffix_Columns = "cols";

        public const string QueryStringPrefix_PageParameter = "_pp_";

        public static QueryOptions ParseOptions(IMVCGridDefinition grid, HttpRequest httpRequest)
        {
            string qsKeyPage = grid.QueryStringPrefix + QueryStringSuffix_Page;
            string qsKeySort = grid.QueryStringPrefix + QueryStringSuffix_Sort;
            string qsKeyDirection = grid.QueryStringPrefix + QueryStringSuffix_SortDir;
            string qsKeyEngine = grid.QueryStringPrefix + QueryStringSuffix_Engine;
            string qsKeyPageSize = grid.QueryStringPrefix + QueryStringSuffix_ItemsPerPage;
            string qsColumns = grid.QueryStringPrefix + QueryStringSuffix_Columns;

            var options = new QueryOptions();

            if (httpRequest[qsKeyEngine] != null)
            {
                string re = httpRequest[qsKeyEngine];
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

                if (grid.AllowChangingPageSize)
                {
                    if (httpRequest[qsKeyPageSize] != null)
                    {
                        int pageSize;
                        if (Int32.TryParse(httpRequest[qsKeyPageSize], out pageSize))
                        {
                            options.ItemsPerPage = pageSize;
                        }
                    }

                    if (grid.MaxItemsPerPage.HasValue && grid.MaxItemsPerPage.Value < options.ItemsPerPage)
                    {
                        options.ItemsPerPage = grid.MaxItemsPerPage.Value;
                    }
                }

                if (options.ItemsPerPage <= 0)
                {
                    options.ItemsPerPage = 20;
                }

                options.PageIndex = 0;
                if (httpRequest[qsKeyPage] != null)
                {
                    int pageNum;
                    if (Int32.TryParse(httpRequest[qsKeyPage], out pageNum))
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

                    if (httpRequest[qsKey] != null)
                    {
                        string filterValue = httpRequest[qsKey];

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
                options.SortColumnData = null;
                options.SortDirection = SortDirection.Unspecified;
            }
            else
            {
                options.SortColumnName = null;

                string sortColName = null;
                if (httpRequest[qsKeySort] != null)
                {
                    sortColName = httpRequest[qsKeySort];
                }

                if (String.IsNullOrWhiteSpace(sortColName))
                {
                    sortColName = grid.DefaultSortColumn;
                }

                string thisSortColName = sortColName.Trim().ToLower();

                // validate SortColumn
                var colDef = grid.GetColumns().SingleOrDefault(p => p.ColumnName.ToLower() == thisSortColName);


                if (colDef != null && !colDef.EnableSorting)
                {
                    colDef = null;
                }
                

                if (colDef != null)
                {
                    options.SortColumnName = colDef.ColumnName;
                    options.SortColumnData = colDef.SortColumnData;
                }
                

                options.SortDirection = grid.DefaultSortDirection;
                if (httpRequest[qsKeyDirection] != null)
                {
                    string sortDir = httpRequest[qsKeyDirection];
                    if (String.Compare(sortDir, "dsc", true) == 0)
                    {
                        options.SortDirection = SortDirection.Dsc;
                    }
                    else if (String.Compare(sortDir, "asc", true) == 0)
                    {
                        options.SortDirection = SortDirection.Asc;
                    }
                }
            }

            if (grid.AdditionalQueryOptionNames.Count > 0)
            {
                foreach (var aqon in grid.AdditionalQueryOptionNames)
                {
                    string qsKeyAQO = grid.QueryStringPrefix + aqon;
                    string val = "";

                    if (httpRequest[qsKeyAQO] != null)
                    {
                        val = httpRequest[qsKeyAQO];
                    }

                    options.AdditionalQueryOptions.Add(aqon, val);
                }
            }

            if (grid.PageParameterNames.Count > 0)
            {
                foreach (var aqon in grid.PageParameterNames)
                {
                    string qsKeyAQO = QueryStringPrefix_PageParameter + grid.QueryStringPrefix + aqon;
                    string val = "";

                    if (httpRequest[qsKeyAQO] != null)
                    {
                        val = httpRequest[qsKeyAQO];
                    }

                    options.PageParameters.Add(aqon, val);
                }
            }


            var gridColumns = grid.GetColumns();
            List<ColumnVisibility> requestedColumns = new List<ColumnVisibility>();
            if (httpRequest[qsColumns] == null)
            {
                foreach (var gridColumn in gridColumns)
                {
                    requestedColumns.Add(
                        new ColumnVisibility() {
                            ColumnName = gridColumn.ColumnName,
                            Visible = gridColumn.Visible
                        });
                }
            }
            else
            {
                string cols = httpRequest[qsColumns];

                string[] colParts = cols.Split(',', ';');

                foreach (var colPart in colParts)
                {
                    if (String.IsNullOrWhiteSpace(colPart))
                    {
                        continue;
                    }
                    string thisColPart = colPart.ToLower().Trim();

                    var gridColumn = gridColumns.SingleOrDefault(p => p.ColumnName.ToLower() == thisColPart);

                    if (gridColumn != null)
                    {
                        if (requestedColumns.SingleOrDefault(p=>p.ColumnName== gridColumn.ColumnName) == null)
                        {
                            requestedColumns.Add(
                                new ColumnVisibility()
                                {
                                    ColumnName = gridColumn.ColumnName,
                                    Visible = true
                                });
                        }
                    }
                }
            }

            foreach (var gridColumn in gridColumns)
            {
                var requestedCol = requestedColumns.SingleOrDefault(p => p.ColumnName == gridColumn.ColumnName);

                if (requestedCol == null)
                {
                    requestedCol = new ColumnVisibility() { ColumnName = gridColumn.ColumnName, Visible = false };
                    requestedColumns.Add(requestedCol);
                }

                if (!requestedCol.Visible && gridColumn.Visible && !gridColumn.AllowChangeVisibility)
                {
                    requestedCol.Visible = true;
                }
            }
            options.ColumnVisibility.AddRange(requestedColumns);

            return options;
        }
    }
}
