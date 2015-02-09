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
            //using (var db = new SampleDatabaseEntities())
            //{
            //    DateTime dt = DateTime.Now;
            //    foreach (var person in db.People.ToList().OrderBy(p=>Guid.NewGuid()))
            //    {
            //        if (!person.StartDate.HasValue)
            //        {
            //            person.StartDate = dt;
            //            dt = dt.Subtract(new TimeSpan(30, 55, 0));
            //        }
            //    }
            //    db.SaveChanges();
            //}

            return View();
        }

        public ActionResult NavigateAway()
        {
            return View();
        }
    }
}