[assembly: WebActivatorEx.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.RegisterGrids), "Start")]

namespace $rootnamespace$.App_Start
{
    using System;
    using System.Web;
	using System.Web.Mvc;
	using System.Linq;
	using System.Collections.Generic;

	using MVCGrid.Models;
	using MVCGrid.Web;

    public static class MVCGridConfig 
    {
        public static void RegisterGrids()
        {
		}    
    }
}
