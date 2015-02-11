using MVCGrid.Interfaces;
using MVCGrid.Models;
using MVCGrid.Rendering;
using MVCGrid.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace MVCGrid.Web
{
    public class MVCGridHandler : IHttpHandler
    {
        static object _lock = new object();
        static bool _init = false;
        static Dictionary<string, byte[]> _cachedBinaryResources;
        static Dictionary<string, string> _cachedTextResources;

        public void Init()
        {
            if (!_init)
            {
                lock (_lock)
                {
                    if (!_init)
                    {
                        _cachedBinaryResources = new Dictionary<string, byte[]>();
                        _cachedTextResources = new Dictionary<string, string>();


                        string script = GetTextResource("MVCGrid.js");
                        var handlerPath = HttpContext.Current.Request.CurrentExecutionFilePath;
                        script = script.Replace("%%HANDLERPATH%%", handlerPath);
                        _cachedTextResources.Add("MVCGrid.js", script);

                        _cachedBinaryResources.Add("ajaxloader.gif", GetBinaryResource("ajaxloader.gif"));
                        _cachedBinaryResources.Add("sort.png", GetBinaryResource("sort.png"));
                        _cachedBinaryResources.Add("sortdown.png", GetBinaryResource("sortdown.png"));
                        _cachedBinaryResources.Add("sortup.png", GetBinaryResource("sortup.png"));

                        _init = true;
                    }
                }
            }
        }

        private static string GetTextResource(string fileSuffix)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var s = assembly.GetManifestResourceNames();

            string resourceName = null;
            foreach (var name in s)
            {
                if (name.Contains(fileSuffix))
                {
                    resourceName = name;
                    break;
                }
            }

            string script = null;
            using (var textStreamReader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                script = textStreamReader.ReadToEnd();
            }

            return script;
        }

        private static byte[] GetBinaryResource(string fileSuffix)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var s = assembly.GetManifestResourceNames();

            string resourceName = null;
            foreach (var name in s)
            {
                if (name.Contains(fileSuffix))
                {
                    resourceName = name;
                    break;
                }
            }

            using (Stream resFilestream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resFilestream == null) return null;
                byte[] ba = new byte[resFilestream.Length];
                resFilestream.Read(ba, 0, ba.Length);
                return ba;
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            Init();

            if (context.Request.Path.ToLower().EndsWith("/script.js"))
            {
                HandleScript(context);
            }
            else if (context.Request.Path.ToLower().EndsWith("/ajaxloader.gif"))
            {
                HandelGifImage(context, "ajaxloader.gif");
            }
            else if (context.Request.Path.ToLower().EndsWith("/sort.png"))
            {
                HandelPngImage(context, "sort.png");
            }
            else if (context.Request.Path.ToLower().EndsWith("/sortdown.png"))
            {
                HandelPngImage(context, "sortdown.png");
            }
            else if (context.Request.Path.ToLower().EndsWith("/sortup.png"))
            {
                HandelPngImage(context, "sortup.png");
            }
            else
            {
                HandleTable(context);
            }
        }

        private void HandelGifImage(HttpContext context, string imageName)
        {
            context.Response.Clear();
            context.Response.ContentType = "image/gif";
            context.Response.BinaryWrite(_cachedBinaryResources[imageName]);
            context.Response.Flush();
        }

        private void HandelPngImage(HttpContext context, string imageName)
        {
            context.Response.Clear();
            context.Response.ContentType = "image/png";
            context.Response.BinaryWrite(_cachedBinaryResources[imageName]);
            context.Response.Flush();
        }

        

        private void HandleTable(HttpContext context)
        {
            string gridName = context.Request["Name"];

            StringBuilder sbDebug = new StringBuilder();
            foreach (string key in context.Request.QueryString.AllKeys)
            {
                sbDebug.Append(key);
                sbDebug.Append(" = ");
                sbDebug.Append(context.Request.QueryString[key]);
                sbDebug.Append("<br />");
            }



            var grid = MVCGridDefinitionTable.GetDefinitionInterface(gridName);

            var options = QueryStringParser.ParseOptions(grid, context.Request);

            var gridContext = GridContextUtility.Create(context, gridName, grid, options);

            IMVCGridHtmlWriter writer = (IMVCGridHtmlWriter)Activator.CreateInstance(gridContext.GridDefinition.HtmlWriterType, true);

            IMVCGridRenderingEngine renderingEngine = DetermineRenderingEngine(context, writer);
            

            //if (renderingEngine is HtmlRenderingEngine)
            //{
            //    context.Response.Write(sbDebug.ToString());
            //}

            if (!renderingEngine.AllowsPaging)
            {
                gridContext.QueryOptions.ItemsPerPage = null;
            }

            var results = ((GridDefinitionBase)grid).GetData(gridContext);

            // if a page was requested higher than available pages, requery for first page
            if (results.Rows.Count == 0 && results.TotalRecords.HasValue && results.TotalRecords.Value > 0)
            {
                gridContext.QueryOptions.PageIndex = 0;
                results = ((GridDefinitionBase)grid).GetData(gridContext);
            }

            renderingEngine.PrepareResponse(context.Response);
            renderingEngine.Render(results, gridContext, context.Response.OutputStream);
        }

        private IMVCGridRenderingEngine DetermineRenderingEngine(HttpContext context, IMVCGridHtmlWriter writer)
        {
            IMVCGridRenderingEngine engine = null;

            if (context.Request.QueryString["engine"] != null)
            {
                string re = context.Request.QueryString["engine"];
                if (String.Compare(re, "export", true) == 0)
                {
                    engine = new CsvRenderingEngine();
                }
            }

            if (engine == null)
            {
                engine = new HtmlRenderingEngine(writer);
            }

            return engine;
        }

        
        private void HandleScript(HttpContext context)
        {
            context.Response.ContentType = "application/javascript";
            context.Response.Write(_cachedTextResources["MVCGrid.js"]);
        }
    }
}
