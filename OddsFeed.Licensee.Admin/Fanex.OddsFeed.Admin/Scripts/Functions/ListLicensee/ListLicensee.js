var _systemID = 0;
var _systemName = "";
var _isActive = false;
var _contactEmail = "";

var gridOptions,
    _sortColumnName,
    _sortDirection,
    PageIndex = 0,
    PageSize = 10;

$(document).ready(function () {
    $('#PageSize').val(PageSize);
    $('#PageIndex').val(PageIndex);
    Submit();

    $("#SystemName").Watermark("Licensee Name");

    $("#SystemName").autocomplete({
        source: [],
        minLength: 1
    });

    $("#SystemName").keyup(function (e) {
        if (e.keyCode == $.ui.keyCode.ENTER) {
            Submit();
        }
    });
});

function renderBodyRows(licensees) {
    var rows = [];
    var totalRows = licensees.length;
    for (var i = 0; i < totalRows; i++) {
        var licensee = licensees[i];
        var colactive = '';
        if (licensee.isActive) {
            colactive = "<img title='Deactivate this licensee' class='ico-tick' src='"
                + currentDomainURL + "App_Themes/Default/Images/blank.png' onclick=\"javascript:editActive(this, " + licensee.systemID + ")\" />";
        } else {
            colactive = "<img title='Activate this licensee' class='ico-untick' src='"
                + currentDomainURL + "App_Themes/Default/Images/blank.png' onclick='javascript:editActive(this, " + licensee.systemID + ")' />";
        }

        var row = {
            Columns: [
                { Name: 'Row', Value: licensee.row },
                { Name: 'SystemName', Value: licensee.systemName },
                {
                    Name: 'Action',
                    Value: "<div class='btn-action licenseeId_" + licensee.systemID + "' onclick='javascript:ShowAction(" + JSON.stringify(licensee) + ")'></div>"
                },
                { Name: 'IsActive', Value: colactive },
                { Name: 'CreatedDate', Value: licensee.createdDate },
                { Name: 'LastUpdate', Value: licensee.lastUpdate },
                { Name: 'Creator', Value: licensee.creator },
                { Name: 'ContactEmail', Value: licensee.contactEmail }
            ]
        };

        rows.push(row);
    }

    return rows;
}

function Submit(sortColumnName, sortDirection) {
    var typeSort = 0;
    //1 --> Active || 2 --> CreatedDate || 3 --> Creator || 4 --> LastUpdate
    switch (sortColumnName) {
        case "IsActive":
            typeSort = 1;
            break;
        case "CreatedDate":
            typeSort = 2;
            break;
        case "Creator":
            typeSort = 3;
            break;
        case "LastUpdate":
            typeSort = 4;
            break;
        default:
            break;
    }


    $(".result-message-box").addClass("hidden");
    var searchParams = {
        SystemName: $.trim($("#SystemName").val()),
        IsActive: $("#isActive option:selected").val(),
        SortByID: typeSort,
        IsSortAsc: sortDirection === "asc" ? 1 : 0,
        PageSize: PageSize,
        PageIndex: PageIndex
    };

    $.ajax({
        url: "GetLicenseeByName",
        type: 'POST',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(searchParams),
        cache: false,
        success: function (result) {
            gridOptions =
            {
                columns: [
                    { HeaderText: "#", Width: 50, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Licensee Name", Width: 200, Name: "SystemName", HeaderAlign: "center", CellAlign: "left", TdClass: "boldusername" },
                    { HeaderText: "Action", Width: 80, Name: "Action", HeaderAlign: "center", CellAlign: "center" },
                    {
                        HeaderText: "Active", Width: 80, Name: "IsActive", HeaderAlign: "center", CellAlign: "center", CssClass: "header-mt",
                        Sort: true,
                        SortDirection: sortColumnName === "IsActive" ? sortDirection : ''
                    },
                    {
                        HeaderText: "Created Date", Width: 120, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center", CssClass: "header-mt",
                        Sort: true, FormatDateTime: 'DD MMM YYYY HH:mm:ss',
                        SortDirection: sortColumnName === "CreatedDate" ? sortDirection : ''
                     },
                    {
                        HeaderText: "Last Update", Width: 120, Name: "LastUpdate", HeaderAlign: "center", CellAlign: "center", CssClass: "header-mt",
                        Sort: true, FormatDateTime: 'DD MMM YYYY HH:mm:ss',
                        SortDirection: sortColumnName === "LastUpdate" ? sortDirection : ''
                    },
                    {
                        HeaderText: "Creator", Width: 120, Name: "Creator", HeaderAlign: "center", CellAlign: "left", CssClass: "header-mt",
                        Sort: true,
                        SortDirection: sortColumnName === "Creator" ? sortDirection : ''
                    },
                    { HeaderText: "Contact Email", Width: 200, Name: "ContactEmail", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true }
                ],
                bodyRows: renderBodyRows(result.data),
                hideYBackGroundScroller: true,
                gridExpandHeight: 250,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: PageSize,
                    currentPage: PageIndex,
                    totalItem: result.totalItems,
                    itemsPerPageArray: [10, 20, 50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                },
                sortOption: {
                    serverSortFunction: function (sortColumnName, sortDirection) {
                        _sortColumnName = sortColumnName;
                        _sortDirection = sortDirection;
                        Submit(sortColumnName, sortDirection);
                    }
                }
            };

            $("#SearchResult").fanexGrid(gridOptions);
        }
    });
}

function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
    if (itemsPerPage != $('#PageSize').val()) {
        $('#PageIndex').val(0);
        PageIndex = 0;
    } else {
        $('#PageIndex').val(page_index);
        PageIndex = page_index;
    }

    $('#PageSize').val(itemsPerPage);
    PageSize = itemsPerPage;

    gridOptions.pagerOption.currentPage = page_index;
    Submit(_sortColumnName, _sortDirection);

    return false;
}

function ShowAction(licensee) {
    _systemID = licensee.systemID;
    _systemName = licensee.systemName;
    _isActive = licensee.isActive;
    _contactEmail = licensee.contactEmail;

    $.contextMenu({
        selector: '.btn-action.licenseeId_' + licensee.systemID,
        items: {
            deleteAffiliate: {
                name: "Delete this licensee",
                height: 25,
                className: "iconDelAff",
                callback: function (key, opt) {
                    deleteLicensee(_systemID, _systemName);
                }
            },
            editSetting: {
                name: "Edit authentication settings",
                height: 25, className: "iconEditSetting",
                callback: function (key, opt) {
                    var urlParam = $.param({
                        SystemID: _systemID,
                        SystemName: _systemName
                    });

                    window.location = currentDomainURL + "Licensee/EditSetting?" + urlParam;
                }
            },
            editInfo: {
                name: "Edit information",
                height: 25, className: "iconEditInfo",
                callback: function (key, opt) {
                    var urlParam = $.param({
                        SystemID: _systemID,
                        SystemName: _systemName,
                        IsActive: _isActive,
                        ContactEmail: _contactEmail
                    });

                    window.location = currentDomainURL + "Licensee/EditLicenseeInfo?" + urlParam;
                }
            },
            editProfile: {
                name: "Edit profile",
                className: "iconOddProfile",
                callback: function (key, opt) {
                    var urlParam = $.param({
                        SystemID: _systemID,
                        SystemName: _systemName
                    });

                    window.location = currentDomainURL + "Licensee/EditProfile?" + urlParam;
                }
            }
        },
        trigger: "left",
        ignoreRightClick: true,
        autoHide: true
    });
}

function StartAutoSuggesttion() {
    var urlParam = $.param({
        Systemname: $("#SystemName").val()
    });

    var url = 'LicenseeList_GetTop10?' + urlParam;
    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'html',
        cache: false,
        success: function (result) {
            var sourceLicensee = result.split(',');
            $("#SystemName").autocomplete({
                source: sourceLicensee,
                minLength: 1
            });
        },
        error: function (xhr, status, err) {
        }
    });
}

function Search() {
    PageIndex = 0;
    PageSize = 10;
    $('#PageSize').val(PageSize);
    $('#PageIndex').val(PageIndex);
    Submit();
}