using MVCGrid.Models;
using MVCGrid.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCGrid.Web.Controllers
{
    public class TestController : Controller
    {

        public ActionResult Issue6()
        {
            return View();
        }
    }

    public class TestControllerGrids : GridRegistration
    {
        public override void RegisterGrids()
        {
            //Issue6Grid
            MVCGridDefinitionTable.Add("Issue6Grid", new MVCGridBuilder<Job>()
                .WithSorting(true)
                .WithDefaultSortColumn("Id")
                .WithPaging(true)
                .WithAllowChangingPageSize(true)
                .WithMaxItemsPerPage(100)
                .WithItemsPerPage(10)
                .WithAdditionalQueryOptionName("globalsearch")
                .AddColumns(cols =>
                {
                    cols.Add("Id", "Id", (row, context) => row.JobId.ToString()).WithSorting(true);
                    cols.Add("Name", "Name", (row, context) => row.Name).WithSorting(true);

                    cols.Add("Contact")
                        .WithHeaderText("Contact")
                        .WithSorting(false)
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) => p.Contact != null ? c.UrlHelper.Action("Edit", "Contact", new { id = p.Contact.Id }) : "")
                        .WithValueTemplate("<a href='{Value}'>{Model.Contact.FullName}</a>").WithPlainTextValueExpression((p, c) => p.Contact != null ? p.Contact.FullName : "");

                    cols.Add("Delete")
                      .WithHtmlEncoding(false)
                      .WithSorting(false)
                      .WithFiltering(false)
                      .WithHeaderText("<input type='checkbox' id='chkselectall'>")
                      .WithValueExpression((p, c) => c.UrlHelper.Action("Save", "Country", new { area = "General", id = p.JobId }))
                      .WithValueTemplate("<input type='checkbox' class='select' value='{Model.JobId}'>")
                      .WithPlainTextValueExpression((p, c) => "");
                })
                .WithRetrieveDataMethod((context) =>
                {

                    var options = context.QueryOptions;

                    string globalsearch = options.GetAdditionalQueryOptionString("globalsearch");

                    JobRepo repo = new JobRepo();
                    int totalRecords;
                    var data = repo.GetData(out totalRecords, globalsearch, options.GetLimitOffset(),
                        options.GetLimitRowcount(), options.GetSortColumnData<string>(), options.SortDirection == SortDirection.Dsc);

                    return new QueryResult<Job>()
                    {
                        Items = data,
                        TotalRecords = totalRecords
                    };
                })
            );
        }
    }
}