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

        private static object _lock = new object();

        public static void Add<T1>(string name, GridDefinition<T1> mapping)
        {
            if (!_table.ContainsKey(name))
            {
                lock (_lock)
                {
                    if (!_table.ContainsKey(name))
                    {
                        _table.Add(name, mapping);
                    }
                }
            }
        }

        public static GridDefinition<T1> GetMapping<T1>(string name)
        {
            lock (_lock)
            {
                return (GridDefinition<T1>)_table[name];
            }
        }

        internal static IMVCGridDefinition GetMappingInterface(string name)
        {
            lock (_lock)
            {
                return (IMVCGridDefinition)_table[name];
            }
        }

        
    }
}
