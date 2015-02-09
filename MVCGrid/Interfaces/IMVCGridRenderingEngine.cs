using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MVCGrid.Interfaces
{
    public interface IMVCGridRenderingEngine
    {
        bool AllowsPaging { get; }
        void PrepareResponse(HttpResponse response);
        void Render(GridData data, GridContext gridContext, Stream outputStream);
    }
}
