[assembly: WebActivatorEx.PreApplicationStartMethod(typeof($rootnamespace$.MVCGridConfig), "RegisterGrids")]

namespace $rootnamespace$
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
			/*
            MVCGridDefinitionTable.Add("UsageExample", new MVCGridBuilder<YourModelItem>()
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add().WithColumnName("UniqueColumnName")
                        .WithHeaderText("Any Header")
                        .WithValueExpression((i, c) => i.YourProperty); // use the Value Expression to return the cell text for this column
                })
                .WithRetrieveDataMethod((context) =>
                {
                    // Query your data here. Obey Ordering, paging and filtering paramters given in the context.QueryOptions.
                    // Use Entity Framwork, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>

                    return new QueryResult<YourModelItem>()
                    {
                        Items = new List<YourModelItem>(),
                        TotalRecords = 0 // if paging is enabled, return the total number of records of all pages
                    };

                })
            );
            */
		}
    }
}
