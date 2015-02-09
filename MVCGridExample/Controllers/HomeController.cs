using MVCGrid.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCGridExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new SampleDatabaseEntities())
            {
                foreach (var person in db.People)
                {
                    Console.WriteLine(person.FirstName);
                }
            }

            return View();
        }

        public ActionResult NavigateAway()
        {
            return View();
        }
    }
}