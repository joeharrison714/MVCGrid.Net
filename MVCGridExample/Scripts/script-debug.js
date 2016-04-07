
var MVCGrid = new function () {

    var handlerPath = '/MVCGridHandler.axd';
    var controllerPath = '/MVCGrid/Grid';
    var showErrorDetails = false;
    var currentGrids = [];

    // public
    this.init = function () {
        $('.MVCGridContainer').each(function () {

            var mvcGridName = $("#" + this.id).find("input[name='MVCGridName']").val();

            var jsonData = $('#' + 'MVCGrid_' + mvcGridName + '_JsonData').html();

            currentGrids.push(
                $.parseJSON(jsonData)
            );
        });

        for (var i = 0; i < currentGrids.length; i++) {
            var obj = currentGrids[i];

            if (!obj.preloaded) {
                MVCGrid.reloadGrid(obj.name);
            }
        }

        bindToolbarEvents();
    };

    var applyBoundFilters = function (mvcGridName){

        var o = {};

        $("[data-mvcgrid-type='filter']").each(function () {

            var gridName =  getGridName($(this));
            if (gridName == mvcGridName){

                var option = $(this).attr('data-mvcgrid-option');
                var val = $(this).val();
            
                o[option] = val;
            }
        });

        MVCGrid.setFilters(mvcGridName, o);
    };

    var loadBoundFilters = function(){
        $("[data-mvcgrid-type='filter']").each(function () {
            var gridName =  getGridName($(this));
            var option = $(this).attr('data-mvcgrid-option');

            var val = MVCGrid.getFilters(gridName)[option];
            $(this).val(val);
        });
    };

    var applyAdditionalQueryOptions = function (mvcGridName){

        var o = {};

        $("[data-mvcgrid-type='additionalQueryOption']").each(function () {
            var gridName =  getGridName($(this));

            if (gridName == mvcGridName){
                var option = $(this).attr('data-mvcgrid-option');
                var val = $(this).val();
            
                o[option] = val;
            }
        });

        MVCGrid.setAdditionalQueryOptions(mvcGridName, o);
    };

    var loadAdditionalQueryOptions = function(){
        $("[data-mvcgrid-type='additionalQueryOption']").each(function () {
            var gridName =  getGridName($(this));
            var option = $(this).attr('data-mvcgrid-option');

            var val = MVCGrid.getAdditionalQueryOptions(gridName)[option];
            $(this).val(val);
        });
    };

    var getGridName = function(elem){
        var gridName = currentGrids[0].name;
        var nameAttr =  elem.attr('data-mvcgrid-name');
        if (typeof nameAttr !== typeof undefined && nameAttr !== false) {
            gridName = nameAttr;
        }
        return gridName;
    };

    var bindToolbarEvents = function (){

        loadBoundFilters();
        loadAdditionalQueryOptions();

        //$("[data-mvcgrid-apply-filter]").each(function () {

        //    var eventName = $(this).attr("data-mvcgrid-apply-filter");

        //    $(this).on(eventName, function () {
        //        var gridName =  getGridName($(this));

        //        applyBoundFilters(gridName);
        //    });

        //});

        //$("[data-mvcgrid-apply-additional]").each(function () {

        //    var eventName = $(this).attr("data-mvcgrid-apply-additional");

        //    $(this).on(eventName, function () {
        //        var gridName =  getGridName($(this));

        //        applyAdditionalQueryOptions(gridName);
        //    });

        //});

        //$("[data-mvcgrid-type='export']").each(function () {

        //    $(this).click(function () {
        //        var gridName =  getGridName($(this));

        //        location.href = MVCGrid.getExportUrl(gridName);
        //    });

        //});

        //$("[data-mvcgrid-type='pageSize']").each(function () {
            
        //    var gridName =  getGridName($(this));
        //    $(this).val(MVCGrid.getPageSize(gridName));

        //    $(this).change(function () {
        //        var gridName =  getGridName($(this));
        //        MVCGrid.setPageSize(gridName, $(this).val());
        //    });


        //});


        //$("[data-mvcgrid-type='columnVisibilityList']").each(function () {

        //    var listElement = $(this);
        //    var gridName =  getGridName($(this));

        //    var colVis = MVCGrid.getColumnVisibility(gridName);
        //    $.each(colVis, function (colName, col) {
                
        //        if (!col.visible && !col.allow) {
        //            return true;
        //        }
        //        var html = '<li><a><label><input type="checkbox" name="' + gridName + 'cols" value="' + colName + '"';
        //        if (col.visible) {
        //            html += ' checked';
        //        }
        //        if (!col.allow) {
        //            html += ' disabled';
        //        }
        //        html += '> ' + col.headerText + '</label></a></div></li>';
        //        listElement.append(html);
        //    });

        //    $("input:checkbox[name='" + gridName + "cols']").change(function() {
        //        var jsonData = {};
        //        var gridName = getGridName($(this).closest('ul'));
                
        //        $("input:checkbox[name='" + gridName + "cols']:checked").each(function () {
        //            var columnName = $(this).val();
        //            jsonData[columnName] = true;
        //        });
        //        MVCGrid.setColumnVisibility(gridName, jsonData);
        //    });
        //});

    };

    // private
    var getClientData = function (mvcGridName){
        var jsonData = $('#' + 'MVCGrid_' + mvcGridName + '_ContextJsonData').html();
        alert(jsonData);
        return $.parseJSON(jsonData);
    };

    // private
    var findGridDef = function (mvcGridName) {
        var gridDef;
        for (var i = 0; i < currentGrids.length; i++) {
            var obj = currentGrids[i];

            if (obj.name == mvcGridName) {
                gridDef = obj;
                break;
            }
        }

        if (gridDef == undefined){
            window.console && console.log('Grid not found: ' + mvcGridName);
        }

        return gridDef;
    };

    // private
    var updateURLParameter = function (url, param, paramVal) {

        param = param.toLowerCase();

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

    // public
    this.getColumnVisibility = function (mvcGridName) {
        var clientJson = getClientData(mvcGridName);
        return clientJson.columnVisibility;
    };

    // public
    this.setColumnVisibility = function (mvcGridName, obj) {

        var gridDef = findGridDef(mvcGridName);

        var colString = '';
        $.each(obj, function (k, v) {
            if (v) {
                if (colString != '') colString += ',';
                colString += k;
            }
        });

        var newUrl = window.location.href;

        $.each(obj, function (k, v) {
            newUrl = updateURLParameter(newUrl, gridDef.qsPrefix + 'cols', colString);
        });

        setURLAndReload(mvcGridName, newUrl);
    };

    // public
    this.getFilters = function (mvcGridName) {
        var clientJson = getClientData(mvcGridName);
        return clientJson.filters;
    };

    // public
    this.setFilters = function (mvcGridName, obj) {

        var gridDef = findGridDef(mvcGridName);

        var newUrl = window.location.href;

        $.each(obj, function (k, v) {
            newUrl = updateURLParameter(newUrl, gridDef.qsPrefix + k, v);
        });

        setURLAndReload(mvcGridName, newUrl);
    };

    // public
    this.getSortColumn = function (mvcGridName) {
        var clientJson = getClientData(mvcGridName);
        return clientJson.sortColumn;
    };

    // public
    this.getSortDirection = function (mvcGridName) {
        var clientJson = getClientData(mvcGridName);
        return clientJson.sortDirection;
    };

    // public
    this.setSort = function (mvcGridName, sortColumn, sortDirection) {

        var gridDef = findGridDef(mvcGridName);

        var newUrl = window.location.href;
        newUrl = updateURLParameter(newUrl, gridDef.qsPrefix + 'sort', sortColumn);
        newUrl = updateURLParameter(newUrl, gridDef.qsPrefix + 'dir', sortDirection);

        
        setURLAndReload(mvcGridName, newUrl);
    };

    // public
    this.getPage = function (mvcGridName) {
        var clientJson = getClientData(mvcGridName);
        return clientJson.pageNumber;
    };

    // public
    this.setPage = function (mvcGridName, pageNumber) {

        var gridDef = findGridDef(mvcGridName);

        var newUrl = window.location.href;
        newUrl = updateURLParameter(newUrl, gridDef.qsPrefix + 'page', pageNumber);
        setURLAndReload(mvcGridName, newUrl);
    };

    // public
    this.getPageSize = function (mvcGridName) {
        var clientJson = getClientData(mvcGridName);
        return clientJson.itemsPerPage;
    };

    // public
    this.setPageSize = function (mvcGridName, pageSize) {

        var gridDef = findGridDef(mvcGridName);

        var newUrl = window.location.href;
        newUrl = updateURLParameter(newUrl, gridDef.qsPrefix + 'pagesize', pageSize);
        setURLAndReload(mvcGridName, newUrl);
    };

    // public
    this.getAdditionalQueryOptions = function (mvcGridName) {
        var clientJson = getClientData(mvcGridName);
        return clientJson.additionalQueryOptions;
    };

    // public
    this.setAdditionalQueryOptions = function (mvcGridName, obj) {

        var gridDef = findGridDef(mvcGridName);

        var newUrl = window.location.href;

        $.each(obj, function (k, v) {
            newUrl = updateURLParameter(newUrl, gridDef.qsPrefix + k, v);
        });

        setURLAndReload(mvcGridName, newUrl);
    };

    // private
    var setURLAndReload = function (mvcGridName, newUrl) {

        if (history.pushState) {
            window.history.pushState({ path: newUrl }, '', newUrl);
            MVCGrid.reloadGrid(mvcGridName);
        }
        else {
            location.href = newUrl;
        }

    };

    // public
    this.reloadGrid = function(mvcGridName){
        var tableHolderHtmlId = 'MVCGridTableHolder_' + mvcGridName;
        var loadingHtmlId = 'MVCGrid_Loading_' + mvcGridName;
        var errorHtmlId = 'MVCGrid_ErrorMessage_' + mvcGridName;

        var gridDef = findGridDef(mvcGridName);;

        var ajaxBaseUrl = handlerPath;

        if (gridDef.renderingMode == 'controller') {
            ajaxBaseUrl = controllerPath;
        }


        var fullAjaxUrl = ajaxBaseUrl + location.search;

        $.each(gridDef.pageParameters, function (k, v) {
            var thisPP = "_pp_" + gridDef.qsPrefix + k;
            fullAjaxUrl = updateURLParameter(fullAjaxUrl, thisPP, v);
        });

        $.ajax({
            type: "GET",
            url: fullAjaxUrl,
            data: { 'Name': mvcGridName },
            cache: false,
            beforeSend: function () {
                if (gridDef.clientLoading != '') {
                    window[gridDef.clientLoading]();
                }
                
                $('#' + loadingHtmlId).css("visibility", "visible");
            },
            success: function (result) {
                $('#' + tableHolderHtmlId).html(result);
            },
            error: function (request, status, error) {
                var errorhtml = $('#' + errorHtmlId).html();

                if (showErrorDetails){
                    $('#' + tableHolderHtmlId).html(request.responseText);
                }else{
                    $('#' + tableHolderHtmlId).html(errorhtml);
                }
            },
            complete: function() {
                if (gridDef.clientLoadingComplete != '') {
                    window[gridDef.clientLoadingComplete]();
                }

                $('#' + loadingHtmlId).css("visibility", "hidden");
            }
        });
    };

    // public
    this.getExportUrl = function (mvcGridName) {
        return MVCGrid.getEngineExportUrl(mvcGridName, 'export');
    };

    // public
    this.getEngineExportUrl = function (mvcGridName, engineName) {
        var gridDef = findGridDef(mvcGridName);

        var exportBaseUrl = handlerPath;

        var fullExportUrl = exportBaseUrl + location.search;
        fullExportUrl = updateURLParameter(fullExportUrl, 'engine', engineName);
        fullExportUrl = updateURLParameter(fullExportUrl, 'Name', mvcGridName);

        $.each(gridDef.pageParameters, function (k, v) {
            var thisPP = "_pp_" + gridDef.qsPrefix + k;
            fullExportUrl = updateURLParameter(fullExportUrl, thisPP, v);
        });

        return fullExportUrl;
    };
};


$(function () {
    MVCGrid.init();
});
