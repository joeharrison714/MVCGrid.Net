using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MVCGrid.Interfaces
{
    public interface IMVCGridDefinition
    {
        IEnumerable<IMVCGridColumn> GetColumns();

        /// <summary>
        /// A prefix to add to all query string parameters for this grid, for when there are more than 1 grids on the same page
        /// </summary>
        string QueryStringPrefix { get; }


        /// <summary>
        /// Enables data loading when the page is first loaded so that the initial ajax request can be skipped.
        /// </summary>
        bool PreloadData { get; }

        /// <summary>
        /// Specified if the data should be loaded as soon as the page loads
        /// </summary>
        bool QueryOnPageLoad { get; set; }

        /// <summary>
        /// Enables paging on the grid
        /// </summary>
        bool Paging { get; set; }

        /// <summary>
        /// Number of items to display on each page
        /// </summary>
        int ItemsPerPage { get; set; }

        /// <summary>
        /// Enables sorting on the grid. Note, sorting must also be enabled on each column where sorting is wanted
        /// </summary>
        bool Sorting { get; set; }

        /// <summary>
        /// The default column to sort by when no sort is specified
        /// </summary>
        string DefaultSortColumn { get; set; }

        /// <summary>
        /// The default order to sort by when no sort is specified
        /// </summary>
        SortDirection DefaultSortDirection { get; set; }

        /// <summary>
        /// Text to display when there are no results.
        /// </summary>
        string NoResultsMessage { get; set; }

        /// <summary>
        /// Name of function to call before ajax call begins
        /// </summary>
        string ClientSideLoadingMessageFunctionName { get; set; }

        /// <summary>
        /// Name of function to call before ajax call ends
        /// </summary>
        string ClientSideLoadingCompleteFunctionName { get; set; }

        /// <summary>
        /// Enables filtering on the grid. Note, filtering must also be enabled on each column where filtering is wanted
        /// </summary>
        bool Filtering { get; set; }

        [Obsolete("RenderingEngine is obsolete. Please user RenderingEngines and DefaultRenderingEngineName")]
        Type RenderingEngine { get; set; }
        Type TemplatingEngine { get; set; }

        /// <summary>
        /// Arbitrary additional settings
        /// </summary>
        Dictionary<string, object> AdditionalSettings { get; set; }

        /// <summary>
        /// The rendering mode to use for this grid. By default it will use the RenderingEngine rendering mode. If you want to use a custom Razor view to display your grid, change this to Controller
        /// </summary>
        RenderingMode RenderingMode { get; set; }

        /// <summary>
        /// When RenderingMode is set to Controller, this is the path to the razor view to use.
        /// </summary>
        string ViewPath { get; set; }

        /// <summary>
        /// When RenderingMode is set to Controller, this is the path to the container razor view to use.
        /// </summary>
        string ContainerViewPath { get; set; }

        /// <summary>
        /// HTML to display in place of the grid when an error occurs
        /// </summary>
        string ErrorMessageHtml { get; set; }

        /// <summary>
        /// Names of additional parameters that can be passed from client to server side
        /// </summary>
        HashSet<string> AdditionalQueryOptionNames { get; set; }

        /// <summary>
        /// Names of page parameters that will be passed from the view
        /// </summary>
        HashSet<string> PageParameterNames { get; set; }

        /// <summary>
        /// Allows changing of page size from client-side
        /// </summary>
        bool AllowChangingPageSize { get; set; }

        /// <summary>
        /// Sets the maximum of items per page allowed when AllowChangingPageSize is enabled
        /// </summary>
        int? MaxItemsPerPage { get; set; }

        T GetAdditionalSetting<T>(string name, T defaultValue);

        /// <summary>
        /// Indicated the authorization type. Anonymous access is the default.
        /// </summary>
        AuthorizationType AuthorizationType { get; set; }

        /// <summary>
        /// Sets the browser navigation mode for the grid.  PreserveAllGridActions is the default.
        /// </summary>
        BrowserNavigationMode BrowserNavigationMode { get; set; }

        /// <summary>
        /// Perists the latest grid state in a cookie so that it will be reloaded the next time the user navigates to the page. Default is false.
        /// </summary>
        bool PersistLastState { get; set; }

        /// <summary>
        /// The list of configured rendering engines availble for this grid
        /// </summary>
        /// <value>
        /// The rendering engines, each with a unique name
        /// </value>
        ProviderSettingsCollection RenderingEngines { get; set; }

        /// <summary>
        /// The unique rendering engine name to use when none is specified in the request
        /// </summary>
        /// <value>
        /// The default name of the rendering engine.
        /// </value>
        string DefaultRenderingEngineName { get; set; }

        /// <summary>
        /// Enables or disables spinner for the grid
        /// </summary>
        bool SpinnerEnabled { get; set; }

        /// <summary>
        /// The target DOM element ID for the spinner
        /// </summary>
        string SpinnerTargetElementId { get; set; }

        /// <summary>
        /// Sets the size of the spinner
        /// </summary>
        int SpinnerRadius { get; set; }

        /// <summary>
        /// Enables the ability to select by row
        /// </summary>
        bool EnableRowSelect { get; set; }

        /// <summary>
        /// Client side function to call when a row is selected
        /// </summary>
        string ClientSideRowSelectFunctionName { get; set; }

        /// <summary>
        /// Arguments to pass to the client side row select function
        /// </summary>
        List<string> ClientSideRowSelectProperties { get; set; }
    }
}
