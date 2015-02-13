using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorEngine;
using RazorEngine.Templating;

namespace MVCGrid.RazorTemplates
{
    public class RazorTemplatingEngine : IMVCGridTemplatingEngine
    {
        public string Process(string template, Models.TemplateModel model)
        {
            string templateKey = String.Format("{0}_{1}", model.GridContext.GridName, model.GridColumn.ColumnName);

            var result = RazorEngine.Engine.Razor.RunCompile(template, templateKey, typeof(Models.TemplateModel), model);

            return result;
        }
    }
}
