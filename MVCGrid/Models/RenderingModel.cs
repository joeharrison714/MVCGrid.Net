using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Models
{
    public class PagingModel
    {
        public PagingModel()
        {
            PageLinks = new Dictionary<int, string>();
        }

        public int FirstRecord { get; set; }
        public int LastRecord { get; set; }
        public int TotalRecords { get; set; }
        public int CurrentPage { get; set; }
        public int NumberOfPages { get; set; }

        public Dictionary<int, string> PageLinks { get; set; }

        public void CalculatePageStartAndEnd(int pagesToDisplay, out int start, out int end)
        {
            int pageToStart = CurrentPage - ((pagesToDisplay - 1) / 2);
            if (pageToStart < 1) pageToStart = 1;

            int pageToEnd = pageToStart + (pagesToDisplay - 1);
            
            if (pageToEnd > NumberOfPages)
            {
                int diff = pageToEnd - NumberOfPages;
                
                pageToEnd = NumberOfPages;
                pageToStart = pageToStart - diff;
            }
            if (pageToStart < 1) pageToStart = 1;

            start = pageToStart;
            end = pageToEnd;
        }
    }

    public class RenderingModel
    {
        public RenderingModel()
        {
            Columns = new List<Column>();
            Rows = new List<Row>();
        }
        public string HandlerPath { get; set; }
        public string TableHtmlId { get; set; }
        public List<Column> Columns { get; set; }
        public List<Row> Rows { get; set; }

        public string NoResultsMessage { get; set; }
        public string NextButtonCaption { get; set; }
        public string PreviousButtonCaption { get; set; }
        public string SummaryMessage { get; set; }
        public string ProcessingMessage { get; set; }

        /// <summary>
        /// Paging data. Will be null if paging should not be displayed
        /// </summary>
        public PagingModel PagingModel { get; set; }

        public string ClientDataTransferHtmlBlock { get; set; }
    }

    public class Cell
    {
        public string CalculatedCssClass { get; set; }
        public string HtmlText { get; set; }
        public string PlainText { get; set; }
    }

    public class Row
    {
        public Row()
        {
            Cells = new Dictionary<string, Cell>();
        }
        public string CalculatedCssClass { get; set; }
        public Dictionary<string, Cell> Cells { get; set; }
    }

    public class Column
    {
        public string Name { get; set; }
        public string Onclick { get; set; }
        public string HeaderText { get; set; }
        public SortDirection? SortIconDirection { get; set; }
    }
}
