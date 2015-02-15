using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MVCGrid.Models
{
    public class GridContext
    {
        public GridContext()
        {
            Items = new Dictionary<string, object>();
        }
        internal IMVCGridDefinition GridDefinition { get; set; }
        public HttpContext CurrentHttpContext { get; set; }
        public QueryOptions QueryOptions { get; set; }
        public System.Web.Mvc.UrlHelper UrlHelper { get; set; }
        public string GridName { get; set; }

        public IEnumerable<IMVCGridColumn> GetVisibleColumns()
        {
            List<IMVCGridColumn> visibleColumns = new List<IMVCGridColumn>();

            foreach (var col in this.GridDefinition.GetColumns())
            {
                if (col.Visible)
                {
                    visibleColumns.Add(col);
                }
            }

            return visibleColumns;
        }

        /// <summary>
        /// Arbitrary settings for this conext
        /// </summary>
        public Dictionary<string, object> Items { get; set; }
    }
}
