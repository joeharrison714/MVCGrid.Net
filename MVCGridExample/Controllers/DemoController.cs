using MVCGrid.Models;
using MVCGrid.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCGrid.Web.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sorting()
        {
            return View();
        }

        public ActionResult Paging()
        {
            return View();
        }

        public ActionResult DependencyInjection()
        {
            return View();
        }

        public ActionResult Formatting()
        {
            return View();
        }

        public ActionResult Styling()
        {
            return View();
        }

        public ActionResult Preloading()
        {
            return View();
        }

        public ActionResult LoadingMessage()
        {
            return View();
        }

        public ActionResult Filtering()
        {
            return View();
        }

        public ActionResult Detail(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult ClientSideApi()
        {
            return View();
        }

        public ActionResult Export()
        {
            return View();
        }

        public ActionResult Multiple()
        {
            return View();
        }

        public ActionResult CustomStyleMenu()
        {
            return View();
        }

        public ActionResult CustomStyle()
        {
            return View();
        }

        public ActionResult CustomRazorView()
        {
            return View();
        }

        public ActionResult CustomRazorView2()
        {
            return View();
        }

        public ActionResult ValueTemplate()
        {
            return View();
        }

        public ActionResult CustomErrorMessage()
        {
            return View();
        }

        public ActionResult GlobalSearch()
        {
            return View();
        }

        public ActionResult PageSizeDemo()
        {
            return View();
        }

        public ActionResult ColumnVisibilityDemo()
        {
            return View();
        }

        public ActionResult ContainerDemo()
        {
            return View();
        }

        public ActionResult NestedObject()
        {
            return View();
        }

        public ActionResult Toolbar()
        {
            return View();
        }

        public ActionResult PageParameters()
        {
            return View();
        }

        public ActionResult NoQueryOnPageLoad()
        {
            return View();
        }

        public ActionResult CustomExport()
        {
            return View();
        }

        public ActionResult AdditionalQueryOptions()
        {
            return View();
        }
    }

    public class DemoControllerGrids : GridRegistration
    {
        public override void RegisterGrids()
        {
            MVCGridDefinitionTable.Add("ContainerGrid", new MVCGridBuilder<Person>()
                .AddColumns(cols =>
                {
                    cols.Add("Id").WithValueExpression((p, c) => p.Id.ToString());
                    cols.Add("FirstName").WithHeaderText("First Name")
                        .WithValueExpression((p, c) => p.FirstName);
                    cols.Add("LastName").WithHeaderText("Last Name")
                        .WithValueExpression((p, c) => p.LastName);
                })
                .WithRenderingMode(RenderingMode.Controller)
                .WithViewPath("~/Views/MVCGrid/_Grid.cshtml")
                .WithContainerViewPath("~/Views/MVCGrid/_Container.cshtml")
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
        }
    }
}