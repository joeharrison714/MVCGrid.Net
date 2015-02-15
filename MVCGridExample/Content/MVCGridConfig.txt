using MVCGrid.Models;
using MVCGrid.Web;
using MVCGrid.Web.Data;
using MVCGrid.Web.Models;
using MVCGridExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCGridExample
{
    public class MVCGridConfig
    {
        public static void RegisterGrids()
        {
            MVCGridDefinitionTable.Add("TestGrid", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithSorting(true)
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) =>
                        {
                            return String.Format("<a href='{0}'>{1}</a>",
                                c.UrlHelper.Action("detail", "demo", new { id = p.Id }), p.Id);
                        })
                        .WithPlainTextValueExpression((p,c) => p.Id.ToString());
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName);
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName);
                    cols.Add().WithColumnName("StartDate")
                        .WithHeaderText("Start Date")
                        .WithValueExpression((p, c) => p.StartDate.HasValue ? p.StartDate.Value.ToShortDateString() : "");
                    cols.Add().WithColumnName("Status")
                        .WithHeaderText("Status")
                        .WithValueExpression((p, c) => p.Active ? "Active" : "Inactive")
                        .WithCellCssClassExpression((p, c) => p.Active ? "success" : "danger"); ;
                    cols.Add().WithColumnName("Gender")
                        .WithValueExpression((p, c) => p.Gender);
                    cols.Add().WithColumnName("Url")
                        .WithVisibility(false)
                        .WithValueExpression((p, c) => c.UrlHelper.Action("detail", "demo", new { id = p.Id }));
//                    cols.Add().WithColumnName("Button")  //Templating demo
//                        .WithHtmlEncoding(false)
//                        .WithValueTemplate(@"
//<a class='btn btn-default' href='{Row.Url}' role='button'>
//    {Model.FirstName}
//</a>
//");

                })
                //.WithAdditionalSetting(MVCGrid.Rendering.BootstrapRenderingEngine.SettingNameTableClass, "notreal") // Excample of changing table css class
                .WithSorting(true)
                .WithDefaultSortColumn("Id")
                .WithPaging(true)
                .WithItemsPerPage(10)
                .WithPreloadData(true)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;

                    int totalRecords;
                    var repo = DependencyResolver.Current.GetService<IPersonRepository>();

                    string sortColumn = options.SortColumn;
                    if (String.Compare(sortColumn, "status", true) == 0) sortColumn = "active";

                    var items = repo.GetData(out totalRecords, options.GetLimitOffset(), options.GetLimitRowcount(),
                        sortColumn, options.SortDirection == SortDirection.Dsc);

                    return new QueryResult<Person>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                })
            );


            MVCGridDefinitionTable.Add("EmployeeGrid", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithValueExpression((p, c) => p.Id.ToString());
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName);
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName);
                })
                .WithRetrieveDataMethod((options) =>
                {
                    var result = new QueryResult<Person>();

                    using (var db = new SampleDatabaseEntities())
                    {
                        result.Items = db.People.Where(p => p.Employee).ToList();
                    }

                    return result;
                })
            );

            MVCGridDefinitionTable.Add("SortableGrid", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithSorting(false)
                        .WithValueExpression((p, c) => p.Id.ToString());
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName);
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName);
                })
                .WithSorting(true)
                .WithDefaultSortColumn("LastName")
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<Person>();

                    using (var db = new SampleDatabaseEntities())
                    {
                        var query = db.People.Where(p => p.Employee);

                        if (!String.IsNullOrWhiteSpace(options.SortColumn))
                        {
                            switch (options.SortColumn.ToLower())
                            {
                                case "firstname":
                                    if (options.SortDirection == SortDirection.Asc)
                                        query = query.OrderBy(p => p.FirstName);
                                    else
                                        query = query.OrderByDescending(p => p.FirstName);
                                    break;
                                case "lastname":
                                    if (options.SortDirection == SortDirection.Asc)
                                        query = query.OrderBy(p => p.LastName);
                                    else
                                        query = query.OrderByDescending(p => p.LastName);
                                    break;
                            }
                        }

                        result.Items = query.ToList();
                    }

                    return result;
                })
            );

            MVCGridDefinitionTable.Add("PagingGrid", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithSorting(false)
                        .WithValueExpression((p, c) => p.Id.ToString());
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName);
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName);
                })
                .WithSorting(true)
                .WithDefaultSortColumn("LastName")
                .WithPaging(true)
                .WithItemsPerPage(10)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;

                    var result = new QueryResult<Person>();

                    using (var db = new SampleDatabaseEntities())
                    {
                        var query = db.People.AsQueryable();

                        result.TotalRecords = query.Count();

                        if (!String.IsNullOrWhiteSpace(options.SortColumn))
                        {
                            switch (options.SortColumn.ToLower())
                            {
                                case "firstname":
                                    if (options.SortDirection == SortDirection.Asc)
                                        query = query.OrderBy(p => p.FirstName);
                                    else
                                        query = query.OrderByDescending(p => p.FirstName);
                                    break;
                                case "lastname":
                                    if (options.SortDirection == SortDirection.Asc)
                                        query = query.OrderBy(p => p.LastName);
                                    else
                                        query = query.OrderByDescending(p => p.LastName);
                                    break;
                            }
                        }

                        if (options.GetLimitOffset().HasValue)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }

                        result.Items = query.ToList();
                    }

                    return result;
                })
            );

            MVCGridDefinitionTable.Add("DIGrid", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithSorting(false)
                        .WithValueExpression((p, c) => p.Id.ToString());
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName);
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName);
                })
                .WithSorting(true)
                .WithDefaultSortColumn("LastName")
                .WithPaging(true)
                .WithItemsPerPage(10)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;

                    int totalRecords;
                    var repo = DependencyResolver.Current.GetService<IPersonRepository>();

                    var items = repo.GetData(out totalRecords, options.GetLimitOffset(), options.GetLimitRowcount(),
                        options.SortColumn, options.SortDirection == SortDirection.Dsc);

                    return new QueryResult<Person>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                })
            );

            MVCGridDefinitionTable.Add("FormattingGrid", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithSorting(false)
                        .WithValueExpression((p, c) => p.Id.ToString());
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName);
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName);
                    cols.Add().WithColumnName("StartDate")
                        .WithHeaderText("Start Date")
                        .WithValueExpression((p, c) => p.StartDate.HasValue ? p.StartDate.Value.ToShortDateString() : "");
                    cols.Add().WithColumnName("ViewLink")
                        .WithSorting(false)
                        .WithHeaderText("")
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) => {
                            return String.Format("<a href='{0}'>View</a>",
                                c.UrlHelper.Action("detail", "demo", new { id = p.Id }));
                            });
                })
                .WithSorting(true)
                .WithDefaultSortColumn("LastName")
                .WithPaging(true)
                .WithItemsPerPage(10)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;

                    int totalRecords;
                    var repo = DependencyResolver.Current.GetService<IPersonRepository>();

                    var items = repo.GetData(out totalRecords, options.GetLimitOffset(), options.GetLimitRowcount(),
                        options.SortColumn, options.SortDirection == SortDirection.Dsc);

                    return new QueryResult<Person>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                })
            );


            MVCGridDefinitionTable.Add("StyledGrid", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithSorting(false)
                        .WithValueExpression((p, c) => p.Id.ToString());
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName);
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName);
                    cols.Add().WithColumnName("StartDate")
                        .WithHeaderText("Start Date")
                        .WithValueExpression((p, c) => p.StartDate.HasValue ? p.StartDate.Value.ToShortDateString() : "");
                    cols.Add().WithColumnName("Status")
                        .WithHeaderText("Status")
                        .WithValueExpression((p, c) => p.Active ? "Active" : "Inactive");
                    cols.Add().WithColumnName("Gender")
                        .WithValueExpression((p, c)=> p.Gender)
                        .WithCellCssClassExpression((p, c) => p.Gender == "Female" ? "danger" : "warning");
                    cols.Add().WithColumnName("ViewLink")
                        .WithSorting(false)
                        .WithHeaderText("")
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) =>
                        {
                            return String.Format("<a href='{0}'>View</a>",
                                c.UrlHelper.Action("detail", new { id = p.Id }));
                        });
                })
                .WithRowCssClassExpression((p, c) => p.Active ? "success" : "")
                .WithSorting(true)
                .WithDefaultSortColumn("LastName")
                .WithPaging(true)
                .WithItemsPerPage(10)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;

                    int totalRecords;
                    var repo = DependencyResolver.Current.GetService<IPersonRepository>();

                    string sortColumn = options.SortColumn;
                    if (String.Compare(sortColumn, "status", true) == 0) sortColumn = "active";

                    var items = repo.GetData(out totalRecords, options.GetLimitOffset(), options.GetLimitRowcount(),
                        sortColumn, options.SortDirection == SortDirection.Dsc);

                    return new QueryResult<Person>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                })
            );

            MVCGridDefinitionTable.Add("Preloading", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithSorting(false)
                        .WithValueExpression((p, c) => p.Id.ToString());
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName);
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName);
                })
                .WithPreloadData(false)
                .WithSorting(true)
                .WithDefaultSortColumn("LastName")
                .WithPaging(true)
                .WithItemsPerPage(10)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;

                    int totalRecords;
                    var repo = DependencyResolver.Current.GetService<IPersonRepository>();

                    var items = repo.GetData(out totalRecords, options.GetLimitOffset(), options.GetLimitRowcount(),
                        options.SortColumn, options.SortDirection == SortDirection.Dsc);

                    return new QueryResult<Person>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                })
            );

            MVCGridDefinitionTable.Add("CustomLoading", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithSorting(false)
                        .WithValueExpression((p, c) => p.Id.ToString());
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName);
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName);
                })
                .WithSorting(true)
                .WithDefaultSortColumn("LastName")
                .WithPaging(true)
                .WithItemsPerPage(10)
                .WithClientSideLoadingMessageFunctionName("showLoading")
                .WithClientSideLoadingCompleteFunctionName("hideLoading")
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;

                    int totalRecords;
                    var repo = DependencyResolver.Current.GetService<IPersonRepository>();

                    var items = repo.GetData(out totalRecords, options.GetLimitOffset(), options.GetLimitRowcount(),
                        options.SortColumn, options.SortDirection == SortDirection.Dsc);

                    // pause to test loading message
                    System.Threading.Thread.Sleep(1000);

                    return new QueryResult<Person>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                })
            );

            MVCGridDefinitionTable.Add("Filtering", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithSorting(false)
                        .WithValueExpression((p, c) => p.Id.ToString());
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName)
                        .WithFiltering(true);
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName)
                        .WithFiltering(true);
                    cols.Add().WithColumnName("Status")
                        .WithHeaderText("Status")
                        .WithValueExpression((p, c) => p.Active ? "Active" : "Inactive")
                        .WithFiltering(true);
                })
                .WithSorting(true)
                .WithDefaultSortColumn("LastName")
                .WithPaging(true)
                .WithItemsPerPage(10)
                .WithFiltering(true)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;

                    int totalRecords;
                    var repo = DependencyResolver.Current.GetService<IPersonRepository>();

                    bool? active = null;
                    string fa=options.GetFilterString("Status");
                    if (!String.IsNullOrWhiteSpace(fa))
                    {
                        if (String.Compare(fa, "active", true) == 0)
                        {
                            active = true;
                        }
                        else
                        {
                            active = false;
                        }
                    }

                    string sortColumn = options.SortColumn;
                    if (String.Compare(sortColumn, "status", true) == 0) sortColumn = "active";

                    var items = repo.GetData(out totalRecords,
                        options.GetFilterString("FirstName"),
                        options.GetFilterString("LastName"),
                        active,
                        options.GetLimitOffset(), options.GetLimitRowcount(),
                        sortColumn, options.SortDirection == SortDirection.Dsc);

                    return new QueryResult<Person>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                })
            );


            MVCGridDefinitionTable.Add("ExportGrid", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithSorting(false)
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) =>
                        {
                            return String.Format("<a href='{0}'>{1}</a>",
                                c.UrlHelper.Action("detail", "demo", new { id = p.Id }), p.Id);
                        })
                        .WithPlainTextValueExpression((p, c) => p.Id.ToString());
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName);
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName);
                    cols.Add().WithColumnName("Status")
                        .WithHeaderText("Status")
                        .WithValueExpression((p, c) => p.Active ? "Active" : "Inactive");
                })
                .WithSorting(true)
                .WithDefaultSortColumn("LastName")
                .WithPaging(true)
                .WithItemsPerPage(10)
                .WithClientSideLoadingMessageFunctionName("showLoading")
                .WithClientSideLoadingCompleteFunctionName("hideLoading")
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;

                    int totalRecords;
                    var repo = DependencyResolver.Current.GetService<IPersonRepository>();

                    string sortColumn = options.SortColumn;
                    if (String.Compare(sortColumn, "status", true) == 0) sortColumn = "active";

                    var items = repo.GetData(out totalRecords,
                        options.GetLimitOffset(), options.GetLimitRowcount(),
                        sortColumn, options.SortDirection == SortDirection.Dsc);

                    return new QueryResult<Person>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                })
            );

            MVCGridDefinitionTable.Add("Multiple1", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithSorting(false)
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) =>
                        {
                            return String.Format("<a href='{0}'>{1}</a>",
                                c.UrlHelper.Action("detail", "demo", new { id = p.Id }), p.Id);
                        })
                        .WithPlainTextValueExpression((p, c) => p.Id.ToString());
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName);
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName);
                    cols.Add().WithColumnName("Status")
                        .WithHeaderText("Status")
                        .WithValueExpression((p, c) => p.Active ? "Active" : "Inactive");
                })
                .WithSorting(true)
                .WithDefaultSortColumn("LastName")
                .WithPaging(true)
                .WithItemsPerPage(10)
                .WithQueryStringPrefix("grid1")
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;

                    int totalRecords;
                    var repo = DependencyResolver.Current.GetService<IPersonRepository>();

                    string sortColumn = options.SortColumn;
                    if (String.Compare(sortColumn, "status", true) == 0) sortColumn = "active";

                    var items = repo.GetData(out totalRecords,
                        options.GetLimitOffset(), options.GetLimitRowcount(),
                        sortColumn, options.SortDirection == SortDirection.Dsc);

                    return new QueryResult<Person>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                })
            );

            MVCGridDefinitionTable.Add("Multiple2", new MVCGridBuilder<TestItem>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Col1")
                        .WithValueExpression((p, c) => p.Col1);
                    cols.Add().WithColumnName("Col2")
                        .WithValueExpression((p, c) => p.Col2);
                    cols.Add().WithColumnName("Col3")
                        .WithValueExpression((p, c) => p.Col3);
                })
                .WithSorting(true)
                .WithDefaultSortColumn("Col1")
                .WithPaging(true)
                .WithItemsPerPage(10)
                .WithQueryStringPrefix("grid2")
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;

                    TestItemRepository repo = new TestItemRepository();
                    int totalRecords;
                    var items = repo.GetData(out totalRecords, options.GetLimitOffset(), options.GetLimitRowcount(), options.SortColumn, options.SortDirection == SortDirection.Dsc);

                    return new QueryResult<TestItem>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                })
            );


            MVCGridDefinitionTable.Add("CustomStyle", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithSorting(false)
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) =>
                        {
                            return String.Format("<a href='{0}'>{1}</a>",
                                c.UrlHelper.Action("detail", "demo", new { id = p.Id }), p.Id);
                        })
                        .WithPlainTextValueExpression((p, c) => p.Id.ToString());
                    cols.Add().WithColumnName("FirstName")
                        .WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName);
                    cols.Add().WithColumnName("LastName")
                        .WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName);
                    cols.Add().WithColumnName("Status")
                        .WithHeaderText("Status")
                        .WithValueExpression((p, c) => p.Active ? "Active" : "Inactive");
                })
                .WithRenderingEngine(typeof(CustomHtmlRenderingEngine))
                .WithSorting(true)
                .WithDefaultSortColumn("LastName")
                .WithPaging(true)
                .WithItemsPerPage(20)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;

                    int totalRecords;
                    var repo = DependencyResolver.Current.GetService<IPersonRepository>();

                    string sortColumn = options.SortColumn;
                    if (String.Compare(sortColumn, "status", true) == 0) sortColumn = "active";

                    var items = repo.GetData(out totalRecords,
                        options.GetLimitOffset(), options.GetLimitRowcount(),
                        sortColumn, options.SortDirection == SortDirection.Dsc);

                    return new QueryResult<Person>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                })
            );


            var docsReturnTypeColumn = new GridColumn<MethodDocItem>()
            {
                ColumnName = "ReturnType",
                HeaderText = "Return Type",
                HtmlEncode = false,
                ValueExpression = (p, c) => String.Format("<code>{0}</code>", HttpUtility.HtmlEncode(p.Return))
            };
            var docsNameColumn = new GridColumn<MethodDocItem>()
            {
                ColumnName = "Name",
                HtmlEncode = false,
                ValueExpression = (p, c) => String.Format("<code>{0}</code>", HttpUtility.HtmlEncode(p.Name))
            };
            var docsDescriptionColumn = new GridColumn<MethodDocItem>()
            {
                ColumnName = "Description",
                ValueExpression = (p, c) => p.Description
            };

            Func<GridContext, QueryResult<MethodDocItem>> docsLoadData = (context) =>
            {
                var result = new QueryResult<MethodDocItem>();

                DocumentationRepository repo = new DocumentationRepository();
                result.Items = repo.GetData(context.GridName);

                return result;
            };

            MVCGridDefinitionTable.Add("GridDefinition", new MVCGridBuilder<MethodDocItem>()
                .AddColumn(docsReturnTypeColumn)
                .AddColumn(docsNameColumn)
                .AddColumn(docsDescriptionColumn)
                .WithRetrieveDataMethod(docsLoadData)
            );

            MVCGridDefinitionTable.Add("GridColumn", new MVCGridBuilder<MethodDocItem>()
                .AddColumn(docsReturnTypeColumn)
                .AddColumn(docsNameColumn)
                .AddColumn(docsDescriptionColumn)
                .WithRetrieveDataMethod(docsLoadData)
            );

            MVCGridDefinitionTable.Add("QueryOptions", new MVCGridBuilder<MethodDocItem>()
                .AddColumn(docsReturnTypeColumn)
                .AddColumn(docsNameColumn)
                .AddColumn(docsDescriptionColumn)
                .WithRetrieveDataMethod(docsLoadData)
            );


            //MVCGridDefinitionTable.Add DO NOT DELETE - Needed for demo code parsing
        }
    }
}