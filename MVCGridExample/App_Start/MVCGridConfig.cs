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

            var grid = new MVCGridBuilder<TestItem>()
                .AddColumn("Col1", "Col1", ((p, h) => p.Col1),
                    cellCssClassExpression: ((p, c) =>
                    {
                        if (p.Col1 == "Row3")
                        {
                            return "success";
                        }
                        return null;
                    }))
                .AddColumn("Col2", "Col2", ((p, h) => p.Col2))
                .AddColumn(
                    name: "Col3",
                    headerText: "Column3",
                    valueExpression: ((p, h) => String.Format("<a href='{1}'>{0}</a>", p.Col3, h.UrlHelper.Action("detail", "item", new { id = "test" }))),
                    enableSort: false,
                    htmlEncode: false,
                    plainTextValueExpression: ((p, c) => p.Col3))
                .WithRetrieveDataMethod(((options) =>
                {
                    TestItemRepository repo = new TestItemRepository();
                    int totalRecords;
                    var items = repo.GetData(out totalRecords, options.GetLimitOffset(), options.GetLimitRowcount(), options.SortColumn, options.SortDirection == SortDirection.Dsc);

                    return new QueryResult<TestItem>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                }))
                .WithPreloadData(true)
                .WithRowCssClassExpression((p, h) =>
                    {
                        if (p.Col1 == "Row1")
                        {
                            return "success";
                        }
                        return null;
                    });
            MVCGridMappingTable.Add("TestMapping", grid);


            //var grid2 = new MVCGridBuilder<TestItem>();
            //grid2.AddColumns(cols =>
            //{
            //    cols.Add()
            //        .WithColumnName("Col1")
            //        .WithHeaderText("blah")
            //        .WithValueExpression(((i,c) => i.Col1));
            //    cols.Add()
            //        .WithColumnName("Col2")
            //        .WithHeaderText("Col2")
            //        .WithValueExpression(((i, c) => i.Col2));
            //});

            //MVCGridMappingTable.Add("TestMapping2", grid2);
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