using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MVCGrid.Web.Models
{
    public class CodeSnippetHelper
    {
        public static string GetCodeSnippet(string gridName)
        {
            string cacheKey=String.Format("GetCodeSnippet_{0}", gridName);

            var cached = HttpContext.Current.Cache[cacheKey] as string;

            if (cached == null)
            {
                string s = GetCodeSnippetInternal(gridName);

                if (s != null)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, s);
                }
                return s;
            }
            else
            {
                return cached;
            }
        }

        public static string GetCodeSnippetInternal(string gridName)
        {
            try
            {
                string appDataPath = HttpContext.Current.Server.MapPath("~/Content");
                string codeFilename = Path.Combine(appDataPath, "MVCGridConfig.txt");

                string contents;
                using (StreamReader sr = new StreamReader(codeFilename))
                {
                    contents = sr.ReadToEnd();
                }

                int startPos = contents.IndexOf(String.Format("MVCGridDefinitionTable.Add(\"{0}\"", gridName));
                startPos = contents.LastIndexOf("\n", startPos) + 1;
                int endPos = contents.IndexOf("MVCGridDefinitionTable.Add", startPos + 19);

                string snippet = contents.Substring(startPos, endPos - startPos);

                int indentLength = snippet.IndexOf("MVCGridDefinitionTable");
                StringBuilder sbNew = new StringBuilder();
                foreach (var line in snippet.Split('\n', '\r'))
                {
                    string newLine = line;

                    if (String.IsNullOrWhiteSpace(newLine)) continue;

                    //for (int i = 0; i < indentLength; i++)
                    //{

                    //}

                    if (newLine.Length > indentLength)
                    {
                        newLine = line.Substring(indentLength);
                    }
                    sbNew.AppendLine(newLine);
                }

                snippet = sbNew.ToString();
                return snippet;
            }
            catch { }
            return null;
        }
    }
}