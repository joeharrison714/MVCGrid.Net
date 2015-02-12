using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MVCGrid.Models
{
    public class TemplateModel
    {

        public TemplateModel(string template, object args)
        {
            Template = template;

            Args = new Dictionary<string, object>();

            if (args != null)
            {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(args))
                {
                    object obj2 = descriptor.GetValue(args);
                    Args.Add(descriptor.Name, obj2);
                }
            }
        }

        public TemplateModel(string template, Dictionary<string, object> args)
        {
            Template = template;
            Args = args;
        }

        public string Template { get; set; }
        public Dictionary<string,object> Args { get; set; } 
    }
}
