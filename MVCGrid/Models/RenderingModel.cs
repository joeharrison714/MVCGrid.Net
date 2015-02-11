using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Models
{
    public class PagingModel
    {
        public int FirstRecord { get; set; }
        public int LastRecord { get; set; }
        public int TotalRecords { get; set; }
        public string GotoPageLinkFormatString { get; set; }
        public int CurrentPage { get; set; }
        public int NumberOfPages { get; set; }
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

        /// <summary>
        /// Paging data. Will be null if paging should not be displayed
        /// </summary>
        public PagingModel PagingModel { get; set; }
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
