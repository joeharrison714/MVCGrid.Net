using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCGrid.Interfaces
{
    public interface IMVCGridHtmlWriter
    {
        string WriteHtml(RenderingModel model); //, Models.GridContext gridContext
    }
}
