using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCGrid.Rendering
{
    public class CsvRenderingEngine : IMVCGridRenderingEngine
    {
        public bool AllowsPaging
        {
            get { return false; }
        }

        public void PrepareResponse(System.Web.HttpResponse httpResponse)
        {
            httpResponse.Clear();
            httpResponse.ContentType = "text/csv";
            httpResponse.AddHeader("content-disposition", "attachment; filename=\"" + "export" + ".csv\"");
        }

        public void Render(Models.GridData data, Models.GridContext gridContext, Stream outputStream)
        {

            using (StreamWriter sw = new StreamWriter(outputStream))
            {
                StringBuilder sbHeaderRow = new StringBuilder();
                foreach (var col in gridContext.GridDefinition.GetColumns())
                {
                    if (sbHeaderRow.Length != 0)
                    {
                        sbHeaderRow.Append(",");
                    }
                    sbHeaderRow.Append(CsvEncode(col.ColumnName));
                }
                sbHeaderRow.AppendLine();
                sw.Write(sbHeaderRow.ToString());

                foreach (var item in data.Rows)
                {
                    StringBuilder sbRow = new StringBuilder();
                    foreach (var col in gridContext.GridDefinition.GetColumns())
                    {
                        if (sbRow.Length != 0)
                        {
                            sbRow.Append(",");
                        }

                        string val = "";

                        if (item.PlainTextValues.ContainsKey(col.ColumnName))
                        {
                            val = item.PlainTextValues[col.ColumnName];
                        }

                        sbRow.Append(CsvEncode(val));
                    }
                    sbRow.AppendLine();
                    sw.Write(sbRow.ToString());
                }
            }
        }

        private string CsvEncode(string s)
        {
            if (String.IsNullOrWhiteSpace(s))
            {
                return "\"\"";
            }

            string esc = s.Replace("\"", "\"\"");

            return String.Format("\"{0}\"", esc);
        }
    }
}
