var gridOptions
        , gridOptionsDetail
        , domainPageIndex = 0
        , domainPageSize = 10
        , domainSettingPageIndex = 0
        , domainSettingPageSize = 10
        , currentDomainID;

var currentPageTotal = 0;

$(document).ready(function () {
    $("#DomainName", $("#dialog-form-update")).attr('disabled', 'disabled');
    $('#PageSize').val(domainPageSize);
    $('#PageIndex').val(domainPageIndex);

    Submit();
    RegisterDialog();
    OddsFeedAdminManager.makeWatermask($("#DomainName"), viewResources.Domain_Name, Submit);
});

function Submit() {
    $("#DomainDetail").addClass("hidden");
    var searchParams = {
        DomainName: $.trim($("#DomainName").val()),
        PageSize: $("#PageSize").val(),
        PageIndex: $("#PageIndex").val()
    };

    $.ajax({
        url: "GetAllDomain",
        type: 'POST',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(searchParams),
        cache: false,
        success: function (result) {
            result = result.replace(/\n/g, "<\/br>");
            result = OddsFeedAdminManager.formatFanexGridStringResult(result);
            gridOptions =
            {
                columns: [
                    { HeaderText: "#", Width: 20, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Domain ID", Width: 70, Name: "DomainID", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Domain Name", Width: 200, Name: "DomainName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Action", Width: 50, Name: "Function", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Created Date", Width: 110, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "IP", Width: 110, Name: "IP", HeaderAlign: "center" },
                    { HeaderText: "ISP", Width: 110, Name: "ISP", HeaderAlign: "center" },
                    { HeaderText: "Creator", Width: 160, Name: "Creator", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Last Update", Width: 120, Name: "LastUpdate", HeaderAlign: "center", CellAlign: "center" }
                  ],
                bodyRows: result[0].data,
                hideYBackGroundScroller: true,
                gridExpandHeight: 150,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: domainPageSize,
                    currentPage: domainPageIndex,
                    totalItem: result[1].totalItem,
                    itemsPerPageArray: [10, 20, 50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                },
                allowTruncate: true
            };

            currentPageTotal = result[0].data.length;

            // Add function column
            for (var i = 0; i < gridOptions.bodyRows.length; i++) {
                var domainName = gridOptions.bodyRows[i].Columns[2].Value;
                gridOptions.bodyRows[i].Columns[2].Value = "<a title='" + domainName + "' href='javascript:GetDomainSetting(" + gridOptions.bodyRows[i].Columns[1].Value + ")' onclick='DomainSection(this); return;'>" + domainName + "</a>";
                
                gridOptions.bodyRows[i].Columns = OddsFeedAdminManager.insertAtIndex(
                    gridOptions.bodyRows[i].Columns,
                    3,
                    { Name: "Function", Value: i });
                //                gridOptions.bodyRows[i].Columns.push({ Name: "Function", Value: i });
                gridOptions.bodyRows[i].Columns[3].CustomCellRender = FunctionColumn;
            }

            $("#SearchResult").removeClass("hidden").fanexGrid(gridOptions);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(xhr);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function OnEnter_DomainName() {
    var domainName = $.trim($("#DomainName").val());
    domainName = domainName == "Domain Name" ? "" : domainName;
    if (domainName == "") {
        $("#DomainDetail").addClass("hidden");
        Submit();
    } else {
        $("#SearchResult").addClass("hidden");
        GetDomainSetting($("#DomainId").val());
    }
};

function DomainSection(btn) {
    if (btn) {
        $("#SearchResult").find(".freegrid-active-row").removeClass("freegrid-active-row");
        $(btn).parent().parent().parent().addClass("freegrid-active-row");
    }
}

function FunctionColumn(cell, cellFormated) {
    return "<div style='text-align:center;'>" +
                "<a title='Edit' href='javascript:UpdateDomain(" + cell.Value.toString() + ")'>" +
                    "<img src='" + currentDomainURL + "App_Themes/Default/Images/Edit.png' alt='Edit'>" +
                "</a>&nbsp&nbsp;" +
                "<a title='Delete' href='javascript:DeleteDomain(" + cell.Value.toString() + ")'>" +
                    "<img src='" + currentDomainURL + "App_Themes/Default/Images/Delete.gif' alt='Delete'>" +
                "</a>" +
            "</div>";
}

function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
    if (itemsPerPage != $('#PageSize').val()) {
        $('#PageIndex').val(0);
        domainPageIndex = 0;
    } else {
        $('#PageIndex').val(page_index);
        domainPageIndex = page_index;
    }

    $('#PageSize').val(itemsPerPage);
    domainPageSize = itemsPerPage;

    gridOptions.pagerOption.currentPage = page_index;
    Submit();
    return false;
}

function GetDomainSetting(domainID) {
    $(".success-message-box").empty().addClass("hidden");
    domainSettingPageSize = 10;
    domainSettingPageIndex = 0;
    $('#PageSize').val(domainSettingPageSize);
    $('#PageIndex').val(domainSettingPageIndex);
    $("#DomainId").val(domainID);
    QueryDomainSetting(domainID);
}

function QueryDomainSetting(domainID) {
    currentDomainID = domainID;
    $.ajax({
        url: "GetDomainSetting",
        type: "POST",
        data: $('#myForm').serialize(),
        success: function (result) {
            //            result = result.replace(/\n/g, "<\/br>");
            result = result.replace(/\n/g, "<\/br>");
            result = eval(result);
            gridOptionsDetail =
            {
                columns: [
                    { HeaderText: "#", Width: 20, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Licensee ID", Width: 120, Name: "SystemID", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Licensee", Width: 120, Name: "SystemName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Domain ID", Width: 70, Name: "DomainID", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Domain Name", Width: 200, Name: "DomainName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Affiliate", Width: 110, Name: "Username", HeaderAlign: "center" },
                    { HeaderText: "Site", Width: 160, Name: "Site", HeaderAlign: "center" },
                    { HeaderText: "Is Allowed", Width: 80, Name: "IsAllowed", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Remark", Width: 195, Name: "Remark", HeaderAlign: "center" }
                ],
                bodyRows: result[0].data,
                hideYBackGroundScroller: true,
                gridExpandHeight: 150,
                showPager: true,
                customPager: true,
                fullWidth: false,
                gridWidth: 978,
                pagerOption: {
                    itemsPerPage: domainSettingPageSize,
                    currentPage: domainSettingPageIndex,
                    totalItem: result[1].totalItem,
                    itemsPerPageArray: [10, 20, 50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPagedDomainSetting
                },
                allowTruncate: true,
                columnsNeedHide: [1],
                showColumnsShowHide: false
            };

            $("#DomainDetail").removeClass("hidden").fanexGrid(gridOptionsDetail);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(xhr);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function ReRenderAfterPagedDomainSetting(page_index, itemsPerPage, jq) {
    if (itemsPerPage != $('#PageSize').val()) {
        $('#PageIndex').val(0);
        domainSettingPageIndex = 0;
    } else {
        $('#PageIndex').val(page_index);
        domainSettingPageIndex = page_index;
    }

    $('#PageSize').val(itemsPerPage);
    domainSettingPageSize = itemsPerPage;

    QueryDomainSetting(currentDomainID)
    //    gridOptions.pagerOption.itemsPerPage = itemsPerPage;

    //    $("#SearchResult").fanexGrid(gridOptions);
    return false;
}

function UpdateDomain(row) {
    $(".success-message-box").empty().addClass("hidden");
    $(".listdomain-result-message-box").addClass("hidden");
    var div = document.createElement('div');
    div.innerHTML = gridOptions.bodyRows[row].Columns[2].Value;
    $("#DomainName", $("#dialog-form-update")).val(OddsFeedAdminManager.unescapeHTML(div.firstChild.innerHTML));
    $("#IP", $("#dialog-form-update")).val(gridOptions.bodyRows[row].Columns[5].Value);
    $("#ISP", $("#dialog-form-update")).val(OddsFeedAdminManager.unescapeHTML(gridOptions.bodyRows[row].Columns[6].Value));
    $("#DomainID", $("#dialog-form-update")).val(gridOptions.bodyRows[row].Columns[1].Value);
    OpenDialog("dialog-form-update");
}

function DeleteDomain(row) {
    $(".success-message-box").empty().addClass("hidden");
    $(".listdomain-result-message-box").addClass("hidden");
    $("#DomainID", $("#dialog-form-delete")).val(gridOptions.bodyRows[row].Columns[1].Value);
    OpenDialog("dialog-form-delete");
}

function btnAddDomainClick() {
    $(".success-message-box").empty().addClass("hidden");
    $(".listdomain-result-message-box").addClass("hidden");
    OpenDialog('dialog-form');
}

function StartAutoSuggesttion() {
    var urlParam = $.param({
        DomainName: $.trim($("#DomainName").val())
    });

    var url = 'DomainList_GetTop10?' + urlParam;
    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'html',
        cache: false,
        success: function (result) {
            //lower case keys
            result = result.replace(/"([^"]+)":/g, function ($0, $1) { return ('"' + $1.toLowerCase() + '":'); })

            var sourceDomains = $.parseJSON(result);
//            if (sourceDomains[0] != null) {
//                $("#DomainId").val(sourceDomains[0].value);
//            } else {
//                $("#DomainId").val(-1);
//            }

            $("#DomainName").autocomplete({
                source: sourceDomains,
                minLength: 1,
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    return false;
                },
                select: function (event, ui) {
                    $(this).val(ui.item.label);
                    return false;
                }
            });
        },
        error: function (xhr, status, err) {
        }
    });
}