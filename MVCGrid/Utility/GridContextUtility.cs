using MVCGrid.Interfaces;
using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCGrid.Utility
{
    public class GridContextUtility
    {
        internal static GridContext Create(HttpContext context, string gridName, IMVCGridDefinition grid, QueryOptions options)
        {
            var httpContext = new HttpContextWrapper(context);
            var urlHelper = new UrlHelper(new RequestContext(httpContext, new RouteData()));

            var gridContext = new GridContext()
            {
                GridName = gridName,
                CurrentHttpContext = context,
                GridDefinition = grid,
                QueryOptions = options,
                UrlHelper = urlHelper
            };

            return gridContext;
        }
    }
}
