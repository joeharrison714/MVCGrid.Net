using MVCGrid.Models;
using MVCGrid.Web;
using MVCGridExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCGridExample
{
    public class MVCGridConfig
    {
        public static void RegisterMappings()
        {
            GridConfiguration globalConfig = SetupGlobalConfiguration();

            var grid = new GridDefinition<TestItem>(globalConfig)
                .WithColumn("Col1", "Col1", ((p, h) => p.Col1))
                .WithColumn("Col2", "Col2", ((p, h) => p.Col2))
                .WithColumn("Col3", "Col3", ((p, h) => p.Col3))
                .WithRetrieveData(((options) =>
                {
                    TestItemRepository repo = new TestItemRepository();
                    int totalRecords;
                    var items = repo.GetData(out totalRecords, options.GetLimitOffset(), options.GetLimitRowcount(), options.SortColumn, options.SortDirection == SortDirection.Dsc);

                    return new QueryResult<TestItem>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                }));
            grid.QueryStringPrefix = "grid1";
            MVCGridMappingTable.Add("TestMapping", grid);
        }

        private static GridConfiguration SetupGlobalConfiguration()
        {
            GridConfiguration globalConfig = new GridConfiguration();
            globalConfig.ItemsPerPage = 10;
            globalConfig.TableCssClasses = new HashSet<string>() { "table", "table-striped", "table-bordered" };
            return globalConfig;
        }
    }
}