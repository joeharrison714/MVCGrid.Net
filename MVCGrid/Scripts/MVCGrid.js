
var MVCGrid = new function () {

    var currentGrids = [];

    this.init = function () {
        $('.MVCGridContainer').each(function () {

            var mvcGridName = $("#" + this.id).find("input[name='MVCGridName']").val();

            currentGrids.push(
                { name: mvcGridName, type: "Douglas Adams" }
            );
        });

        for (var i = 0; i < currentGrids.length; i++) {
            var obj = currentGrids[i];

            MVCGrid.reloadGrid(obj.name);
        }
    }

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

    this.setSort = function (mvcGridName, sortColumn, sortDirection) {

        var newUrl = window.location.href;
        newUrl = updateURLParameter(newUrl, 'sort', sortColumn);
        newUrl = updateURLParameter(newUrl, 'dir', sortDirection);

        
        setURLAndReload(mvcGridName, newUrl);
    };

    this.setPage = function (mvcGridName, pageNumber) {

        var newUrl = window.location.href;
        newUrl = updateURLParameter(newUrl, 'page', pageNumber);
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

        //var pageIndex = $('#' + 'MVCGrid_' + mvcGridName + '_PageIndex').val();
        //var sortCol = $('#' + 'MVCGrid_' + mvcGridName + '_SortColumn').val();
        //var sortDir = $('#' + 'MVCGrid_' + mvcGridName + '_SortDirection').val();

        $.ajax({
            type: "GET",
            url: "/MVCGridHandler.axd" + location.search,
            data: { 'Name': mvcGridName },
            cache: false,
            beforeSend: function(){
                alert('about to ajax');
            },
            success: function (result) {
                $('#' + tableHolderHtmlId).html(result);
            }
        });
    }
};


$(function () {
    MVCGrid.init();
});

//function mvcGridSort(mvcGridName, sortColumn, sortDirection) {
//    var id = 'MVCGrid_' + mvcGridName + '_SortColumn';
//    var idd = 'MVCGrid_' + mvcGridName + '_SortDirection';

//    $('#' + id).val(sortColumn);
//    $('#' + idd).val(sortDirection);

//    loadMVCGridData(mvcGridName);
//}

//function mvcGridGotoPage(mvcGridName, pageNumber) {

//    var pageIndex = pageNumber - 1;

//    var id = 'MVCGrid_' + mvcGridName + '_PageIndex';

//    $('#' + id).val(pageIndex);

//    loadMVCGridData(mvcGridName);
//}

//function updateQueryString(mvcGridName) {
//    var pageIndex = $('#' + 'MVCGrid_' + mvcGridName + '_PageIndex').val();
//    var sortCol = $('#' + 'MVCGrid_' + mvcGridName + '_SortColumn').val();
//    var sortDir = $('#' + 'MVCGrid_' + mvcGridName + '_SortDirection').val();

//    var pageNum = 1;

//    if (pageIndex != '') {
//        pageNum = Number(pageIndex) + 1;
//    }

//    var qs = "?page=" + pageNum;

//    if (sortCol != '') {
//        qs += '&sort=' + sortCol;
//    }

//    if (sortDir != '') {
//        qs += '&sortdir=' + sortDir;
//    }

//    if (history.pushState) {
//        var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + qs;
//        window.history.pushState({ path: newurl }, '', newurl);
//    }
//}

//function getParameterByName(name) {
//    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
//    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
//        results = regex.exec(location.search);
//    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
//}

function loadMVCGridData(mvcGridName) {

    //var pageParam = getParameterByName("page");
    //alert(pageParam);

    var containerHtmlId = 'MVCGridContainer_' + mvcGridName;
    var tableHolderHtmlId = 'MVCGridTableHolder_' + mvcGridName;

    //var pageIndex = $('#' + 'MVCGrid_' + mvcGridName + '_PageIndex').val();
    //var sortCol = $('#' + 'MVCGrid_' + mvcGridName + '_SortColumn').val();
    //var sortDir = $('#' + 'MVCGrid_' + mvcGridName + '_SortDirection').val();

    $.ajax({
        type: "GET",
        url: "/MVCGridHandler.axd" + location.search,
        data: { 'Name': mvcGridName },
        cache: false,
        success: function (result) {
            $('#' + tableHolderHtmlId).html(result);
            updateQueryString(mvcGridName);
        }
    });
}