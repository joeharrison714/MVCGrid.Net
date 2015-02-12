using MVCGrid.Interfaces;
using MVCGrid.Models;
using SmartFormat;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace MVCGrid.Templating
{
    public class SmartFormatTemplatingEngine : IMVCGridTemplatingEngine
    {
        public string Process(string template, TemplateModel model)
        {

            string result = Smart.Format(template, model);
            return result;

            //dynamic eo = new ExpandoObject();

            //var dict = eo as IDictionary<string, Object>;

            //foreach (var item in model.Args)
            //{
            //    dict.Add(item.Key, item.Value);
            //}

            //string result = Smart.Format(model.Template, (object)eo);
            //return result;
        }
    }
}
