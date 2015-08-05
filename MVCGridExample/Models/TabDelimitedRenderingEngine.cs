using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MVCGrid.Web.Models
{
    public class TabDelimitedRenderingEngine : IMVCGridRenderingEngine
    {
        public bool AllowsPaging
        {
            get { return false; }
        }

        public void PrepareResponse(HttpResponse httpResponse)
        {
            httpResponse.Clear();
            httpResponse.ContentType = "text/tab-separated-values";
            httpResponse.AddHeader("content-disposition", "attachment; filename=\"" + "export" + ".tsv\"");
            httpResponse.BufferOutput = false;
        }

        public void Render(MVCGrid.Models.RenderingModel model, MVCGrid.Models.GridContext gridContext, System.IO.TextWriter outputStream)
        {
            var sw = outputStream;

            StringBuilder sbHeaderRow = new StringBuilder();
            foreach (var col in model.Columns)
            {
                if (sbHeaderRow.Length != 0)
                {
                    sbHeaderRow.Append("\t");
                }
                sbHeaderRow.Append(Encode(col.Name));
            }
            sbHeaderRow.AppendLine();
            sw.Write(sbHeaderRow.ToString());

            foreach (var item in model.Rows)
            {
                StringBuilder sbRow = new StringBuilder();
                foreach (var col in model.Columns)
                {
                    var cell = item.Cells[col.Name];

                    if (sbRow.Length != 0)
                    {
                        sbRow.Append("\t");
                    }

                    string val = cell.PlainText;

                    sbRow.Append(Encode(val));
                }
                sbRow.AppendLine();
                sw.Write(sbRow.ToString());
            }
        }

        private string Encode(string s)
        {
            if (String.IsNullOrWhiteSpace(s))
            {
                return "";
            }

            if (s.Contains("\t"))
            {
                s = s.Replace("\t", " ");
            }

            return s;
        }

        public void RenderContainer(MVCGrid.Models.ContainerRenderingModel model, System.IO.TextWriter outputStream)
        {
        }
    }
}