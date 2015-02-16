using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Templating
{
    public class SimpleTemplatingEngine : IMVCGridTemplatingEngine
    {
        public string Process(string template, Models.TemplateModel model)
        {
            if (String.IsNullOrWhiteSpace(template)) return "";

            return Format(template, model);
        }

        private string Format(string format, Models.TemplateModel model)
        {
            int currentPos = 0;

            StringBuilder sbResult = new StringBuilder();
            StringBuilder sbItem = new StringBuilder();

            int fStart = 0;
            bool inside = false;
            int len = format.Length;

            while (true)
            {

                char c = format[currentPos];

                if (c == '{')
                {
                    if (inside)
                    {
                        FormatError();
                    }
                    else if (currentPos < (len - 1) && format[currentPos + 1] == '{') //escape char
                    {
                        sbResult.Append('{');
                        currentPos++;
                    }
                    else
                    {
                        fStart = currentPos;
                        inside = true;
                    }
                }
                else if (c == '}')
                {
                    if (currentPos < (len - 1) && format[currentPos + 1] == '}')
                    {
                        sbResult.Append('}');
                        currentPos++;
                    }
                    else
                    {
                        if (!inside)
                        {
                            FormatError();
                        }
                        inside = false;
                        string name = sbItem.ToString();
                        Console.WriteLine(name);

                        sbItem.Clear();

                        sbResult.Append(EvaluateParameter(name, model));
                    }
                }
                else
                {
                    if (inside)
                    {
                        sbItem.Append(c);
                    }
                    else
                    {
                        sbResult.Append(c);
                    }
                }

                currentPos++;
                if (currentPos == format.Length)
                {
                    break;
                }
            }

            if (inside)
            {
                FormatError();
            }

            return sbResult.ToString();
        }

        private object EvaluateParameter(string name, Models.TemplateModel model)
        {
            object val = "";

            if (String.Compare(name, "value", true) == 0)
            {
                val = model.Value;
            }
            else
            {
                int dotPos = name.IndexOf(".");

                if (dotPos == -1)
                {
                    throw new FormatException("Format item missing prefix: " + name);
                }

                string prefix = name.Substring(0, dotPos).Trim().ToLower();
                string suffix = name.Substring(dotPos + 1);



                switch (prefix)
                {
                    case "model":
                        val = ReflectPropertyValue(model.Item, suffix);
                        break;
                    case "row":
                        if (!model.Row.Cells.ContainsKey(suffix))
                        {
                            throw new Exception("Cannot access cell '" + suffix + "' in current row. It does not exist or has not yet been evaluated");
                        }
                        val = model.Row.Cells[suffix].HtmlText;
                        break;
                    default:
                        throw new Exception("Invalid prefix in format string: " + prefix);
                }
            }

            return val;
        }

        private static object ReflectPropertyValue(object source, string property)
        {
            return source.GetType().GetProperty(property).GetValue(source, null);
        }

        private static void FormatError()
        {
            throw new FormatException("Input string was not in a correct format.");
        }
    }
}
