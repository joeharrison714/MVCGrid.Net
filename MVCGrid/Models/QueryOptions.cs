using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Models
{
    public class QueryOptions
    {
        private object _sortColumnData = null;

        public QueryOptions()
        {
            Filters = new Dictionary<string, string>();
            AdditionalQueryOptions = new Dictionary<string, string>();
        }

        public Dictionary<string, string> AdditionalQueryOptions { get; set; }

        public string RenderingEngineName { get; set; }

        public SortDirection SortDirection { get; set; }
        public string SortColumnName { get; set; }
        public object SortColumnData
        {
            get
            {
                if (_sortColumnData == null)
                {
                    return SortColumnName;
                }
                return _sortColumnData;
            }
            set
            {
                _sortColumnData = value;
            }
        }

        public T GetSortColumnData<T>()
        {
            return (T)SortColumnData;
        }

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
            string val = Filters[columnName].Trim();

            if (String.IsNullOrWhiteSpace(val))
            {
                return null;
            }

            return Filters[columnName];
        }

        public string GetAdditionalQueryOptionString(string name)
        {
            if (!AdditionalQueryOptions.ContainsKey(name))
            {
                return null;
            }
            if (AdditionalQueryOptions[name] == null)
            {
                return null;
            }
            string val = AdditionalQueryOptions[name].Trim();

            if (String.IsNullOrWhiteSpace(val))
            {
                return null;
            }

            return val;
        }
    }
}
