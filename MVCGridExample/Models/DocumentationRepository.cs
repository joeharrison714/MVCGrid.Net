using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCGrid.Web.Models
{
    public class DocumentationRepository
    {
        private readonly static Lazy<List<MethodDocItem>> _documentation =
           new Lazy<List<MethodDocItem>>(() =>
           {
               FileHelperEngine engine = new FileHelperEngine(typeof(MethodDocItem));

               string filename = HttpContext.Current.Server.MapPath("~/App_Data/documentation.csv");

               MethodDocItem[] res = engine.ReadFile(filename) as MethodDocItem[];

               return new List<MethodDocItem>(res);
           });

        public List<MethodDocItem> GetData(string className)
        {
            return _documentation.Value.Where(p => p.Class == className).OrderBy(p => p.Order).ToList();
        }
    }

    [DelimitedRecord(",")]
    public class MethodDocItem
    {
        public string Class;
        public int Order;

        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.NotAllow)]
        public string Return;

        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.NotAllow)]
        public string Name;

        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.NotAllow)]
        public string Description;
    }

}