using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCGrid.Models
{
    public class GridConfiguration
    {
        public GridConfiguration()
        {
            TableCssClasses = new HashSet<string>();
            ItemsPerPage = 20;
        }

        public HashSet<string> TableCssClasses { get; set; }

        public int ItemsPerPage { get; set; }

        public static GridConfiguration CopyFrom(GridConfiguration sourceConfig)
        {
            GridConfiguration destConfig = new GridConfiguration();

            foreach (string item in sourceConfig.TableCssClasses)
            {
                destConfig.TableCssClasses.Add(item);
            }

            destConfig.ItemsPerPage = sourceConfig.ItemsPerPage;

            return destConfig;
        }
    }
}
