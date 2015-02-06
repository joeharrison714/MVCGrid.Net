$(function () {

    $('.MVCGridContainer').each(function () {


        var mvcGridName = $("#" + this.id).find("input[name='MVCGridName']").val();

        loadMVCGridData(mvcGridName);
    });


});

function mvcGridSort(mvcGridName, sortColumn, sortDirection) {
    var id = 'MVCGrid_' + mvcGridName + '_SortColumn';
    var idd = 'MVCGrid_' + mvcGridName + '_SortDirection';

    $('#' + id).val(sortColumn);
    $('#' + idd).val(sortDirection);

    loadMVCGridData(mvcGridName);
}

function mvcGridGotoPage(mvcGridName, pageNumber) {

    var pageIndex = pageNumber - 1;

    var id = 'MVCGrid_' + mvcGridName + '_PageIndex';

    $('#' + id).val(pageIndex);

    loadMVCGridData(mvcGridName);
}

function updateQueryString(mvcGridName) {
    var pageIndex = $('#' + 'MVCGrid_' + mvcGridName + '_PageIndex').val();
    var sortCol = $('#' + 'MVCGrid_' + mvcGridName + '_SortColumn').val();
    var sortDir = $('#' + 'MVCGrid_' + mvcGridName + '_SortDirection').val();

    var pageNum = 1;

    if (pageIndex != '') {
        pageNum = pageIndex + 1;
    }

    var qs = "?page=" + pageNum;

    if (sortCol != '') {
        qs += '&sort=' + sortCol;
    }

    if (sortDir != '') {
        qs += '&sortdir=' + sortDir;
    }

    if (history.pushState) {
        var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + qs;
        window.history.pushState({ path: newurl }, '', newurl);
    }
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function loadMVCGridData(mvcGridName) {

    //var pageParam = getParameterByName("page");
    //alert(pageParam);

    var containerHtmlId = 'MVCGridContainer_' + mvcGridName;
    var tableHolderHtmlId = 'MVCGridTableHolder_' + mvcGridName;

    var pageIndex = $('#' + 'MVCGrid_' + mvcGridName + '_PageIndex').val();
    var sortCol = $('#' + 'MVCGrid_' + mvcGridName + '_SortColumn').val();
    var sortDir = $('#' + 'MVCGrid_' + mvcGridName + '_SortDirection').val();

    $.ajax({
        type: "GET",
        url: "/MVCGridHandler.axd",
        data: { 'Name': mvcGridName, 'PageIndex': pageIndex, 'SortColumn': sortCol, 'SortDirection': sortDir },
        cache: false,
        success: function (result) {
            //do somthing here
            //alert(result);

            $('#' + tableHolderHtmlId).html(result);
            updateQueryString(mvcGridName);
        }
    });
}