using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVCGrid.Models
{
    public class TemplateModel
    {
        public TemplateModel()
        {
        }

        public IMVCGridColumn GridColumn { get; set; }
        public dynamic Item { get; set; }
        public GridContext GridContext { get; set; }
        public Row Row { get; set; }

        public UrlHelper Url
        {
            get
            {
                return GridContext.UrlHelper;
            }
        }
    }
}
