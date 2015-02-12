using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Interfaces
{
    public interface IMVCGridTemplatingEngine
    {
        string Process(TemplateModel model, GridContext gridContect);
    }
}
