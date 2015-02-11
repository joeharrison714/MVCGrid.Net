using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Models
{
    public class RenderingModel
    {
        public RenderingModel()
        {
            Columns = new List<Column>();
            Rows = new List<Row>();
        }
        public List<Column> Columns { get; set; }
        public List<Row> Rows { get; set; }
    }

    public class Cell
    {
        public string CalculatedCssClass { get; set; }
        public string Html { get; set; }
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
