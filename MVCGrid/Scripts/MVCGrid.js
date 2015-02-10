
var MVCGrid = new function () {

    var handlerPath = '/MVCGridHandler.axd';
    var currentGrids = [];

    this.init = function () {
        $('.MVCGridContainer').each(function () {

            var mvcGridName = $("#" + this.id).find("input[name='MVCGridName']").val();

            //var qsPrefix = $('#' + 'MVCGrid_' + mvcGridName + '_Prefix').val();

            //var preload = $('#' + 'MVCGrid_' + mvcGridName + '_Preload').val() === 'true';
            var jsonData = $('#' + 'MVCGrid_' + mvcGridName + '_JsonData').val();

            currentGrids.push(
                JSON.parse(jsonData)
            );
        });

        for (var i = 0; i < currentGrids.length; i++) {
            var obj = currentGrids[i];

            if (!obj.preloaded) {
                MVCGrid.reloadGrid(obj.name);
            }
        }
    }

    var findGridDef = function (mvcGridName) {
        var gridDef;
        for (var i = 0; i < currentGrids.length; i++) {
            var obj = currentGrids[i];

            if (obj.name == mvcGridName) {
                gridDef = obj;
                break;
            }
        }
        return gridDef;
    };

    var updateURLParameter = function (url, param, paramVal) {
        var TheAnchor = null;
        var newAdditionalURL = "";
        var tempArray = url.split("?");
        var baseURL = tempArray[0];
        var additionalURL = tempArray[1];
        var temp = "";

        if (additionalURL) {
            var tmpAnchor = additionalURL.split("#");
            var TheParams = tmpAnchor[0];
            TheAnchor = tmpAnchor[1];
            if (TheAnchor)
                additionalURL = TheParams;

            tempArray = additionalURL.split("&");

            for (i = 0; i < tempArray.length; i++) {
                if (tempArray[i].split('=')[0] != param) {
                    newAdditionalURL += temp + tempArray[i];
                    temp = "&";
                }
            }
        }
        else {
            var tmpAnchor = baseURL.split("#");
            var TheParams = tmpAnchor[0];
            TheAnchor = tmpAnchor[1];

            if (TheParams)
                baseURL = TheParams;
        }

        if (TheAnchor)
            paramVal += "#" + TheAnchor;

        var rows_txt = temp + "" + param + "=" + paramVal;
        return baseURL + "?" + newAdditionalURL + rows_txt;
    };

    this.setFilters = function (mvcGridName, filters) {

        var gridDef = findGridDef(mvcGridName);

        var newUrl = window.location.href;

        for (var i = 0; i < filters.length; i++) {
            var obj = filters[i];
            newUrl = updateURLParameter(newUrl, gridDef.qsPrefix + obj.columnName, obj.value);
        }

        setURLAndReload(mvcGridName, newUrl);
    }

    this.setSort = function (mvcGridName, sortColumn, sortDirection) {

        var gridDef = findGridDef(mvcGridName);

        var newUrl = window.location.href;
        newUrl = updateURLParameter(newUrl, gridDef.qsPrefix + 'sort', sortColumn);
        newUrl = updateURLParameter(newUrl, gridDef.qsPrefix + 'dir', sortDirection);

        
        setURLAndReload(mvcGridName, newUrl);
    };

    this.setPage = function (mvcGridName, pageNumber) {

        var gridDef = findGridDef(mvcGridName);

        var newUrl = window.location.href;
        newUrl = updateURLParameter(newUrl, gridDef.qsPrefix + 'page', pageNumber);
        setURLAndReload(mvcGridName, newUrl);

    };

    var setURLAndReload = function (mvcGridName, newUrl) {

        if (history.pushState) {
            window.history.pushState({ path: newUrl }, '', newUrl);
            MVCGrid.reloadGrid(mvcGridName);
        }
        else {
            location.href = newUrl;
        }

    };

    this.reloadGrid = function(mvcGridName){
        var tableHolderHtmlId = 'MVCGridTableHolder_' + mvcGridName;
        var loadingHtmlId = 'MVCGrid_Loading_' + mvcGridName;

        var gridDef = findGridDef(mvcGridName);;

        $.ajax({
            type: "GET",
            url: handlerPath + location.search,
            data: { 'Name': mvcGridName },
            cache: false,
            beforeSend: function () {
                if (gridDef.clientLoading != '') {
                    window[gridDef.clientLoading]();
                }
                else {
                    $('#' + loadingHtmlId).show();
                }
            },
            success: function (result) {
                $('#' + tableHolderHtmlId).html(result);
            },
            error: function (request, status, error) {
                var errorhtml = '<div class="alert alert-warning" role="alert">There was a problem loading the grid.</div>'
                $('#' + tableHolderHtmlId).html(errorhtml);
            },
            complete: function() {
                if (gridDef.clientLoadingComplete != '') {
                    window[gridDef.clientLoadingComplete]();
                }
                else {
                    $('#' + loadingHtmlId).hide();
                }
            }
        });
    }

    this.getExportUrl = function (mvcGridName) {
        var gridDef = findGridDef(mvcGridName);

        var exportUrl = "/MVCGridHandler.axd" + location.search;
        exportUrl = updateURLParameter(exportUrl, 'engine', 'export');
        exportUrl = updateURLParameter(exportUrl, 'Name', mvcGridName);

        return exportUrl;
    }
};


$(function () {
    MVCGrid.init();
});
