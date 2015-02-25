using MVCGrid.Engine;
using MVCGrid.Interfaces;
using MVCGrid.Models;
using MVCGrid.Rendering;
using MVCGrid.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCGrid.Web
{
    public static class HtmlExtensions
    {
        public static IHtmlString MVCGrid(this HtmlHelper helper, string name)
        {
            var currentMapping = MVCGridDefinitionTable.GetDefinitionInterface(name);

            return MVCGrid(helper, name, currentMapping);
        }

        internal static IHtmlString MVCGrid(this HtmlHelper helper, string name, IMVCGridDefinition grid)
        {
            string preload = "";
            if (grid.PreloadData)
            {
                preload = RenderPreloadedGridHtml(helper, grid, name);
            }

            string baseGridHtml = MVCGridHtmlGenerator.GenerateBasePageHtml(name, grid);
            baseGridHtml = baseGridHtml.Replace("%%PRELOAD%%", preload);

            ContainerRenderingModel containerRenderingModel = new ContainerRenderingModel() { InnerHtmlBlock = baseGridHtml };

            string html = RenderContainerHtml(helper, grid, name, containerRenderingModel);

            return MvcHtmlString.Create(html);
        }

        private static string RenderContainerHtml(HtmlHelper helper, IMVCGridDefinition grid, string gridName, ContainerRenderingModel containerRenderingModel)
        {
            string container = containerRenderingModel.InnerHtmlBlock;
            switch (grid.RenderingMode)
            {
                case Models.RenderingMode.RenderingEngine:
                    //TODO: rendering engine containers
                    break;
                case Models.RenderingMode.Controller:
                    if (!String.IsNullOrWhiteSpace(grid.ContainerViewPath))
                    {
                        container = RenderContainerUsingController(grid, helper, containerRenderingModel);
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }

            if (!container.Contains(containerRenderingModel.InnerHtmlBlock))
            {
                throw new Exception("When rendering a container, you must output Model.InnerHtmlBlock inside the container (Raw).");
            }

            return container;
        }

        private static string RenderPreloadedGridHtml(HtmlHelper helper, IMVCGridDefinition grid, string gridName)
        {
            string preload = "";

            var options = QueryStringParser.ParseOptions(grid, System.Web.HttpContext.Current.Request);

            var gridContext = GridContextUtility.Create(HttpContext.Current, gridName, grid, options);

            GridEngine engine = new GridEngine();

            switch (grid.RenderingMode)
            {
                case Models.RenderingMode.RenderingEngine:
                    preload = RenderUsingRenderingEngine(engine, gridContext);
                    break;
                case Models.RenderingMode.Controller:
                    preload = RenderUsingController(engine, gridContext, helper);
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return preload;
        }

        private static string RenderUsingController(GridEngine engine, Models.GridContext gridContext, HtmlHelper helper)
        {
            var model = engine.GenerateModel(gridContext);

            var controllerContext = helper.ViewContext.Controller.ControllerContext;
            ViewDataDictionary vdd = new ViewDataDictionary(model);
            TempDataDictionary tdd = new TempDataDictionary();
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controllerContext,
                                                                         gridContext.GridDefinition.ViewPath);
                var viewContext = new ViewContext(controllerContext, viewResult.View, vdd, tdd, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private static string RenderUsingRenderingEngine(GridEngine engine, Models.GridContext gridContext)
        {
            IMVCGridRenderingEngine renderingEngine = engine.GetRenderingEngine(gridContext);

            using (MemoryStream ms = new MemoryStream())
            {
                using (TextWriter tw = new StreamWriter(ms))
                {
                    engine.Run(renderingEngine, gridContext, tw);
                }

                return Encoding.ASCII.GetString(ms.ToArray());
            }
        }

        private static string RenderContainerUsingController(IMVCGridDefinition gridDefinition, HtmlHelper helper, ContainerRenderingModel model)
        {
            var controllerContext = helper.ViewContext.Controller.ControllerContext;
            ViewDataDictionary vdd = new ViewDataDictionary(model);
            TempDataDictionary tdd = new TempDataDictionary();
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controllerContext,
                                                                         gridDefinition.ContainerViewPath);
                var viewContext = new ViewContext(controllerContext, viewResult.View, vdd, tdd, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
