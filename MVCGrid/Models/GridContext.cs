using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCGrid.Models
{
    public class GridContext
    {
        internal IMVCGridDefinition GridDefinition { get; set; }
        public HttpContext CurrentHttpContext { get; set; }
        public QueryOptions QueryOptions { get; set; }
        public UrlHelper UrlHelper { get; set; }
    }
}
