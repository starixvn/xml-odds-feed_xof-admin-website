var gridOptions
    ,gridOptionsDetail
    ,domainPageIndex = 0
    ,domainPageSize = 10
    ,domainSettingPageIndex = 0
    ,domainSettingPageSize = 10
    ,currentDomainID;

var currentPageTotal = 0;

$(document).ready(function () {
    $("#DomainName", $("#dialog-form-update")).attr('disabled', 'disabled');
    $('#PageSize').val(domainPageSize);
    $('#PageIndex').val(domainPageIndex);

    Submit();
    RegisterDialog();
    OddsFeedAdminManager.makeWatermask($("#DomainName"), viewResources.Domain_Name, Submit);
});

function renderBodyRows(domains) {
    var rows = [];
    var totalRows = domains.length;
    for (var i = 0; i < totalRows; i++) {
        var domain = domains[i];
        var row = {
            Columns: [
                { Name: 'Row', Value: domain.Row },
                { Name: 'DomainID', Value: domain.DomainId },
                {
                    Name: 'DomainName',
                    Value: "<a title='" + domain.DomainName + "' href='javascript:GetDomainSetting(" +
                        domain.DomainId + ")' onclick='DomainSection(this); return;'>" + domain.DomainName + "</a>"
                },
                {
                    Name: 'Action',
                    Value: "<div style='text-align:center;'>" +
                        "<a title='Edit' href='javascript:UpdateDomain(" + JSON.stringify(domain) + ")'><img src='" +
                        currentDomainURL + "App_Themes/Default/Images/Edit.png' alt='Edit'></a>&nbsp&nbsp;" +
                        "<a title='Delete' href='javascript:DeleteDomain(" + domain.DomainId + ")'><img src='" +
                        currentDomainURL + "App_Themes/Default/Images/Delete.gif' alt='Delete'></a></div>"
                },
                { Name: 'CreatedDate', Value: domain.CreatedDateToString },
                { Name: 'IP', Value: domain.IP },
                { Name: 'ISP', Value: domain.ISP },
                { Name: 'Creator', Value: domain.Creator },
                { Name: 'LastUpdate', Value: domain.LastUpdateToString }
            ]
        };

        rows.push(row);
    }

    return rows;
}

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
            gridOptions =
            {
                columns: [
                    { HeaderText: "#", Width: 20, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Domain ID", Width: 70, Name: "DomainID", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Domain Name", Width: 200, Name: "DomainName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Action", Width: 50, Name: "Action", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Created Date", Width: 110, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "IP", Width: 110, Name: "IP", HeaderAlign: "center" },
                    { HeaderText: "ISP", Width: 110, Name: "ISP", HeaderAlign: "center" },
                    { HeaderText: "Creator", Width: 160, Name: "Creator", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Last Update", Width: 120, Name: "LastUpdate", HeaderAlign: "center", CellAlign: "center" }
                ],
                bodyRows: renderBodyRows(result.Data),
                hideYBackGroundScroller: true,
                gridExpandHeight: 150,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: domainPageSize,
                    currentPage: domainPageIndex,
                    totalItem: result.TotalItems,
                    itemsPerPageArray: [10, 20, 50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            currentPageTotal = result.Data.length;

            $("#SearchResult").removeClass("hidden").fanexGrid(gridOptions);
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

function GetDomainSetting(domainId) {
    $(".success-message-box").empty().addClass("hidden");
    domainSettingPageSize = 10;
    domainSettingPageIndex = 0;
    $('#PageSize').val(domainSettingPageSize);
    $('#PageIndex').val(domainSettingPageIndex);
    $("#DomainId").val(domainId);
    QueryDomainSetting(domainId);
}

function renderDomainSettingBodyRows(domains) {
    var rows = [];
    var totalRows = domains.length;
    for (var i = 0; i < totalRows; i++) {
        var domain = domains[i];
        var row = {
            Columns: [
                { Name: 'Row', Value: domain.Row },
                { Name: 'Licensee', Value: domain.SystemName },
                { Name: 'DomainId', Value: domain.DomainId },
                { Name: 'DomainName', Value: domain.DomainName },
                { Name: 'Affiliate', Value: domain.Username },
                { Name: 'Site', Value: domain.Site },
                { Name: 'IsAllowed', Value: domain.IsAllowed ? 'True' : 'False' },
                { Name: 'Remark', Value: domain.Remark }
            ]
        };

        rows.push(row);
    }

    return rows;
}

function QueryDomainSetting(domainId) {
    currentDomainID = domainId;
    $.ajax({
        url: "GetDomainSetting",
        type: "POST",
        data: $('#myForm').serialize(),
        success: function (result) {
            gridOptionsDetail =
            {
                columns: [
                    { HeaderText: "#", Width: 20, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Licensee", Width: 120, Name: "Licensee", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Domain ID", Width: 70, Name: "DomainId", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Domain Name", Width: 200, Name: "DomainName", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    { HeaderText: "Affiliate", Width: 110, Name: "Affiliate", HeaderAlign: "center", AllowTruncate: true },
                    { HeaderText: "Site", Width: 160, Name: "Site", HeaderAlign: "center", AllowTruncate: true },
                    { HeaderText: "Is Allowed", Width: 80, Name: "IsAllowed", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Remark", Width: 195, Name: "Remark", HeaderAlign: "center", AllowTruncate: true }
                ],
                bodyRows: renderDomainSettingBodyRows(result.Data),
                hideYBackGroundScroller: true,
                gridExpandHeight: 150,
                showPager: true,
                customPager: true,
                fullWidth: false,
                gridWidth: 978,
                pagerOption: {
                    itemsPerPage: domainSettingPageSize,
                    currentPage: domainSettingPageIndex,
                    totalItem: result.TotalItems,
                    itemsPerPageArray: [10, 20, 50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPagedDomainSetting
                }
            };

            $("#DomainDetail").removeClass("hidden").fanexGrid(gridOptionsDetail);
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

    return false;
}

function UpdateDomain(domain) {
    $(".success-message-box").empty().addClass("hidden");
    $(".listdomain-result-message-box").addClass("hidden");
    $("#DomainName", $("#dialog-form-update")).val(domain.DomainName);
    $("#IP", $("#dialog-form-update")).val(domain.IP);
    $("#ISP", $("#dialog-form-update")).val(domain.ISP);
    $("#DomainID", $("#dialog-form-update")).val(domain.DomainId);
    OpenDialog("dialog-form-update");
}

function DeleteDomain(domainId) {
    $(".success-message-box").empty().addClass("hidden");
    $(".listdomain-result-message-box").addClass("hidden");
    $("#DomainID", $("#dialog-form-delete")).val(domainId);
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