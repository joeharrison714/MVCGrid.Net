using MVCGrid.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorEngine;
using RazorEngine.Templating;
using System.IO;

namespace MVCGrid.RazorTemplates
{
    public class RazorRenderingEngine : IMVCGridRenderingEngine
    {
        public bool AllowsPaging
        {
            get { return true; }
        }

        public void PrepareResponse(System.Web.HttpResponse response)
        {
        }

        public void Render(Models.RenderingModel model, System.IO.TextWriter outputStream)
        {
            //model.
            string template = @"
@using MVCGrid.Models
@helper SortImage(Column col){
    
    if (col.SortIconDirection.HasValue)
    {
        switch (col.SortIconDirection)
        {
            case SortDirection.Asc:
                <img src='@(Model.HandlerPath)/sortup.png' class='pull-right' />
                break;
            case SortDirection.Dsc:
                <img src='@(Model.HandlerPath)/sortdown.png' class='pull-right' />
                break;
            case SortDirection.Unspecified:
                <img src='@(Model.HandlerPath)/sort.png' class='pull-right' />
                break;
        }
    }
}

@helper PageLink(int pageNum, string link, int currentPage){

    if (pageNum == currentPage){
        <li class='active'><a href='#' onclick='@Raw(link)'>@pageNum</a></li>
    }
    else{
        <li><a href='#' onclick='@Raw(link)'>@pageNum</a></li>
    }
}
@helper PageNextLink(int pageToEnd, PagingModel pagingModel){
    string attr="""";
    if (pageToEnd == pagingModel.CurrentPage){
        attr="" class='disabled'"";
    }
    string onclick = """";
    if (pageToEnd > pagingModel.CurrentPage){
        onclick = pagingModel.PageLinks[pagingModel.CurrentPage + 1] + ""; "";
    }

    <li@(Raw(attr))>
        <a href='#' aria-label='Next' onclick='@(Raw(onclick))return false;'><span aria-hidden='true'>Next &raquo;</span></a>
    </li>
}
@helper PagePreviousLink (int pageToStart, PagingModel pagingModel){
    string attr="""";
    if (pageToStart == pagingModel.CurrentPage){
        attr="" class='disabled'"";
    }
    string onclick = """";
    if (pageToStart < pagingModel.CurrentPage){
        onclick = pagingModel.PageLinks[pagingModel.CurrentPage - 1] + ""; "";
    }

    <li@(Raw(attr))>
        <a href='#' aria-label='Previous' onclick='@(Raw(onclick))return false;'><span aria-hidden='true'>&laquo; Previous</span></a>
    </li>
}

@functions {
    // Pass a user name to this method.
    string HeaderAttributes(Column col)
    {
        string str="""";
        if (col.Onclick != null){
            str="" style='cursor: pointer;'"";
            str += String.Format("" onclick='{0}'"", col.Onclick);
        }
        return str;
    }

    string AppendCssAttribute(string classString)
    {
        if (!String.IsNullOrWhiteSpace(classString))
        {
            return String.Format("" class='{0}'"", classString);
        }
        return """";
    }
    
}

<table id=""@Model.TableHtmlId"" class=""table table-striped table-bordered"">
    <thead>
        <tr>
            @foreach (var col in Model.Columns){
                <th@(Raw(HeaderAttributes(col)))>@col.HeaderText @(SortImage(col))</th>
            }
        </tr>
    </thead>
    <tbody>
        @foreach (var row in Model.Rows)
        {
            <tr@(Raw(AppendCssAttribute(row.CalculatedCssClass)))>
                @foreach (var col in Model.Columns)
                {
                    var cell = row.Cells[col.Name];
                    <td@(Raw(AppendCssAttribute(cell.CalculatedCssClass)))>@Raw(cell.HtmlText)</td>
                }
            </tr>
        }
    </tbody>
</table>

@if (Model.PagingModel != null){
    var pagingModel = Model.PagingModel;
    int pageToStart;
    int pageToEnd;
    pagingModel.CalculatePageStartAndEnd(5, out pageToStart, out pageToEnd);

    <div class='row'>
        <div class='col-xs-6'>
            Showing @pagingModel.FirstRecord to @pagingModel.LastRecord of @pagingModel.TotalRecords entries
        </div>
        <div class='col-xs-6'>
            <ul class='pagination pull-right' style='margin-top: 0;'>
                @PagePreviousLink(pageToStart, pagingModel)
                @for (int i = pageToStart; i <= pageToEnd; i++)
                {
                    <text>@PageLink(i, pagingModel.PageLinks[i], pagingModel.CurrentPage)</text>
                }
                @PageNextLink(pageToEnd, pagingModel)
            </ul>
        </div>
    </div>
}
";//

            string templateKey = "Output";

            var result = RazorEngine.Engine.Razor.RunCompile(template, templateKey, typeof(Models.RenderingModel), model);


            outputStream.Write(result);

        }
    }
}
