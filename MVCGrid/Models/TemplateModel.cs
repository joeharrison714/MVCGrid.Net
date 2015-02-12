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

        public TemplateModel(dynamic item, GridContext gridContext)
        {
            this.Item = item;
            this.GridContext = gridContext;
        }

        public dynamic Item { get; set; }
        public GridContext GridContext { get; set; }

        public UrlHelper Url
        {
            get
            {
                return GridContext.UrlHelper;
            }
        }
    }
}
