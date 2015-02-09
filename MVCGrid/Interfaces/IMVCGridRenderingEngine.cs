using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MVCGrid.Interfaces
{
    public interface IMVCGridRenderingEngine
    {
        bool AllowsPaging { get; }
        void Render(GridData data, GridContext gridContext, HttpResponse httpResponse);
    }
}
