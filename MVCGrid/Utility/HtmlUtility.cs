using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Utility
{
    public class HtmlUtility
    {
        public const string ContainerCssClass = "MVCGridContainer";

        public static string GetContainerHtmlId(string name)
        {
            return String.Format("MVCGridContainer_{0}", name);
        }

        public static string GetTableHolderHtmlId(string name)
        {
            return String.Format("MVCGridTableHolder_{0}", name);
        }

        public static string GetTableHtmlId(string name)
        {
            return String.Format("MVCGridTable_{0}", name);
        }

        public static string MakeCssClassStirng(HashSet<string> classes)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var c in classes)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string MakeCssClassAttributeStirng(HashSet<string> classes)
        {
            if (classes == null || classes.Count == 0)
                return "";

            return String.Format(" class='{0}'", MakeCssClassStirng(classes));
        }

        public static string MakeCssClassAttributeStirng(string classString)
        {
            if (String.IsNullOrWhiteSpace(classString))
                return "";

            return String.Format(" class='{0}'", classString);
        }

        public static string MakeGotoPageLink(string gridName, int pageNum)
        {
            return String.Format("MVCGrid.setPage(\"{0}\", {1}); return false;", gridName, pageNum);
        }

        public static string MakeSortLink(string gridName, string columnName, MVCGrid.Models.SortDirection direction)
        {
            return String.Format("MVCGrid.setSort(\"{0}\", \"{1}\", \"{2}\"); return false;", gridName, columnName, direction.ToString());
        }
    }
}
