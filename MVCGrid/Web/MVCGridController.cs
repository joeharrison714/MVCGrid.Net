using MVCGrid.Engine;
using MVCGrid.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVCGrid.Web
{
    public class MVCGridController : Controller
    {
        public ActionResult Grid()
        {
            var context = System.Web.HttpContext.Current;

            string gridName = context.Request["Name"];

            var grid = MVCGridDefinitionTable.GetDefinitionInterface(gridName);

            var options = QueryStringParser.ParseOptions(grid, context.Request);

            var gridContext = GridContextUtility.Create(context, gridName, grid, options);

            GridEngine engine = new GridEngine();
            if (!engine.CheckAuthorization(gridContext))
            {
                return new HttpStatusCodeResult(403, "Access denied");
            }

            var renderingModel = engine.GenerateModel(gridContext);

            return PartialView(grid.ViewPath, renderingModel);
        }
    }

}
