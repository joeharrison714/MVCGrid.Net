using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Models
{
    public class QueryOptions
    {
        public QueryOptions()
        {
            Filters = new Dictionary<string, string>();
        }

        public string RenderingEngineName { get; set; }

        public SortDirection SortDirection { get; set; }
        public string SortColumn { get; set; }

        public int? PageIndex { get; set; }
        public int? ItemsPerPage { get; set; }

        public int? GetLimitOffset()
        {
            if (!ItemsPerPage.HasValue) return null;

            if (!PageIndex.HasValue)
            {
                PageIndex = 0;
            }

            return PageIndex * ItemsPerPage;
        }

        public int? GetLimitRowcount()
        {
            return ItemsPerPage;
        }

        public Dictionary<string, string> Filters { get; set; }

        public string GetFilterString(string columnName)
        {
            if (!Filters.ContainsKey(columnName))
            {
                return null;
            }
            if (String.IsNullOrWhiteSpace(Filters[columnName]))
            {
                return null;
            }
            return Filters[columnName];
        }
    }
}
