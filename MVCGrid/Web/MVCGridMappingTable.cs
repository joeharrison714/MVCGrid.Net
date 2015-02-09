using MVCGrid.Interfaces;
using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCGrid.Web
{
    public class MVCGridMappingTable
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
                    "name");
            }

            _table.Add(name, mapping);

        }

        public static GridDefinition<T1> GetMapping<T1>(string name)
        {

            return (GridDefinition<T1>)_table[name];

        }

        internal static IMVCGridDefinition GetMappingInterface(string name)
        {

            return (IMVCGridDefinition)_table[name];

        }


    }
}
