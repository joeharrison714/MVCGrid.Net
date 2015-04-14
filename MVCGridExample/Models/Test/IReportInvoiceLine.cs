using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCGrid.Web.Models.Test
{
    public class ReportInvoiceLine : IReportInvoiceLine
    {
        public int Year { get; set; }
        public int InvoiceNumber { get; set; }
        public string City { get; set; }
    }

    public interface IReportInvoiceLine
    {
        int Year { get; set; }
        int InvoiceNumber { get; set; }
        string City { get; set; }
    }
}