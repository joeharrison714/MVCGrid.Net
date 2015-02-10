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
    }
}