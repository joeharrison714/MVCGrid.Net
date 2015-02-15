using MVCGrid.Interfaces;
using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCGrid.Web
{
    public class MVCGridDefinitionTable
    {
        private static Dictionary<string, object> _table = new Dictionary<string, object>();

        public static void Add<T1>(string name, MVCGridBuilder<T1> builder)
        {
            Add(name, builder.GridDefinition);
        }

        public static void Add<T1>(string name, GridDefinition<T1> mapping)
        {

            if (_table.ContainsKey(name))
            {
                throw new ArgumentException(
                    String.Format("There is already a grid definition with the name '{0}'.", name),
                    "name");
            }

            if (mapping.RetrieveData == null)
            {
                throw new ArgumentException(
                    String.Format("There is no RetrieveData expression defined for grid '{0}'.", name),
                    "RetrieveData");
            }

            if (mapping.Sorting && String.IsNullOrWhiteSpace(mapping.DefaultSortColumn))
            {
                throw new Exception(
                    String.Format("Grid '{0}': When sorting is enabled, a default sort column must be specified", name));
            }

            _table.Add(name, mapping);

        }

        public static GridDefinition<T1> GetDefinition<T1>(string name)
        {

            return (GridDefinition<T1>)GetDefinitionInterface(name);

        }

        internal static IMVCGridDefinition GetDefinitionInterface(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (!_table.ContainsKey(name))
            {
                throw new Exception(
                    String.Format("There is no grid defined with the name '{0}'", name));
            }
            return (IMVCGridDefinition)_table[name];

        }


    }
}
