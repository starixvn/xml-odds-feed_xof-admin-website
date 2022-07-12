var _systemID = 0;
var _systemName = "";
var _isActive = false;
var _contactEmail = "";

var gridOptions
    ,PageIndex = 0
    ,PageSize = 10;

$(document).ready(function () {
    var gridOptions;
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

function Submit() {
    $(".result-message-box").addClass("hidden");
    var searchParams = {
        SystemName: $.trim($("#SystemName").val()),
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
            result = result.replace(/\n/g, "<\/br>");
            result = eval(result);
            gridOptions =
            {
                columns: [
                    { HeaderText: "#", Width: 50, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "SystemID", Width: 100, Name: "SystemID", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Licensee Name", Width: 200, Name: "SystemName", HeaderAlign: "center", CellAlign: "left", TdClass: "boldusername" },
                    { HeaderText: "Action", Width: 80, Name: "Action", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Active", Width: 80, Name: "IsActive", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Created Date", Width: 120, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Last Update", Width: 120, Name: "LastUpdate", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Creator", Width: 120, Name: "Creator", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Contact Email", Width: 200, Name: "ContactEmail", HeaderAlign: "center", CellAlign: "left" }
                ],
                bodyRows: result[0].data,
                hideYBackGroundScroller: true,
                gridExpandHeight: 150,
                allowTruncate: true,
                enableHiddenColumns: false,
                hiddenColumns: [1],
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: PageSize,
                    currentPage: PageIndex,
                    totalItem: result[1].totalItem,
                    itemsPerPageArray: [10, 20, 50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            // Add function column
            for (var i = 0; i < gridOptions.bodyRows.length; i++) {
                // Modify "Active" column
                gridOptions.bodyRows[i].Columns[3].Row = i;
                gridOptions.bodyRows[i].Columns[3].CustomCellRender = ActiveColumn;

                // Modify "Action" column
                gridOptions.bodyRows[i].Columns = OddsFeedAdminManager.insertAtIndex(
                    gridOptions.bodyRows[i].Columns,
                    3,
                    { Name: "Action", Row: i });
                gridOptions.bodyRows[i].Columns[3].CustomCellRender = ActionColumn;
            }

            $("#SearchResult").fanexGrid(gridOptions);
            $("#SearchResult").fanexGrid("hideColumns", gridOptions, [1]);

        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(xhr);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function ActiveColumn(cell, cellFormated) {
    var partnerId = gridOptions.bodyRows[cell.Row].Columns[1].Value;
    if (cell.Value == "True") {
        return "<img title='Deactivate this licensee' class='ico-tick' alt='' src='" + currentDomainURL + "App_Themes/Default/Images/blank.png'  onclick='editActive(this," + partnerId + ")'/>";
    }
    else {
        return "<img title='Activate this licensee' class='ico-untick' alt='' src='" + currentDomainURL + "App_Themes/Default/Images/blank.png'  onclick='editActive(this," + partnerId + ")'/>";
    }
}

function ActionColumn(cell, cellFormated) {

    var licenseeId = gridOptions.bodyRows[cell.Row].Columns[1].Value;
    var licenseeName = gridOptions.bodyRows[cell.Row].Columns[2].Value;
    var isActive = gridOptions.bodyRows[cell.Row].Columns[4].Value;
    var contactEmail = gridOptions.bodyRows[cell.Row].Columns[8].Value;

    return "<div class='btn-action licenseeId_" + licenseeId + "' onclick=\"javascript:ShowAction('" + licenseeId + "','" + licenseeName + "','" + isActive +  "','" + contactEmail + "');\"></div>";
}

function ConvertFromJsonToFanexData(jsonObject) {
    var rows = [];

    for (var i = 0; i < jsonObject.length; i++) {
        var col = []
        for (var key in jsonObject[i]) {
            col.push({ Name: key.toString(), Value: jsonObject[i][key] });
        }
        rows.push({ Columns: col });
    }

    return rows;
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
    Submit();

    return false;
}

function ShowAction(systemID, systemName, isActive, contactEmail) {
    _systemID = systemID;
    _systemName = systemName;
    _isActive = isActive;
    _contactEmail = contactEmail;

    $.contextMenu({
        selector: '.btn-action.licenseeId_' + systemID,
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

                    window.location.href = currentDomainURL + "Licensee/EditSetting?" + urlParam;
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

                    window.location.href = currentDomainURL + "Licensee/EditLicenseeInfo?" + urlParam;
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

                    window.location.href = currentDomainURL + "Licensee/EditProfile?" + urlParam;
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