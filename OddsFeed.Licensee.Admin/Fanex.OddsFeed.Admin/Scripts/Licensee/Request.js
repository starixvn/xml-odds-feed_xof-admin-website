var gridOptions
        , gridOptionsDetail
        , _pageIndex = 0
        , _pageSize = 10
        , _settingPageIndex = 0
        , _settingPageSize = 10
        , _currentLicenseeID;
var _currentDate = null;

$(document).ready(function () {

    Core.registerWeekPicker();

    $("#Week").weekpicker("onSelect", function (start, end) {
        $("#DateStart").datepicker("setDate", start);
    });

    $("#ToWeek").weekpicker("onSelect", function (start, end) {
        $("#DateEnd").datepicker("setDate", end);
    });

    $("#PeriodOption").change(function () {
        setVisibleDateControl($(this).val(), true);
    });

    setVisibleDateControl($("#PeriodOption").val(), false);

    $("#Systemname").Watermark("Licensee name");
    $("#LicenseeId").val(0); //get all systems

    $("#Systemname").autocomplete({
        source: [],
        minLength: 1
    });

    $("#Systemname").keyup(function (e) {
        StartAutoSuggesttion();
        if (e.keyCode == $.ui.keyCode.ENTER) {
            Search();
        }
    });

    Search();
});

//--------------------------------------------------------------------------------------------------------------------

function OnSearch() {
    _pageIndex = 0;
    _pageSize = 10;

    Search();
};

function Search() {
    var _systemName = $.trim($("#Systemname").val());
    if (_systemName == "" || _systemName == "Licensee name") {
        $("#LicenseeId").val(0); //get all systems
    }

    var licenseeId = $("#LicenseeId").val();

    if (licenseeId == 0) {
        $("#SearchResultDetail").hide();
        SearchAll();
    }
    else {
        //        SearchAll();
        $("#SearchResult").hide();
        SearchDetail(licenseeId);
    }
}

function SearchAll() {
    $('#PageSize').val(_pageSize);
    $('#PageIndex').val(_pageIndex);

    $("#SearchResult").show();
    _currentDate = null;
    $.ajax({
        url: "GetAllLicenseeRequest",
        type: "POST",
        data: $('#myForm').serialize(),
        success: function (result) {
            result = result.replace(/\n/g, "<\/br>");
            result = eval(result);

            var tempResult = result[0].data;

            // generate and prepare data to get detail
            for (var i = 0; i < tempResult.length; i++) {
                tempResult[i].Columns[2].Value =
                {
                    licenseeId: tempResult[i].Columns[1].Value,
                    licenseeName: tempResult[i].Columns[2].Value,
                    searchDate: tempResult[i].Columns[3].Value,
                    yearOrTotalRequest: tempResult[i].Columns[4].Value
                };
                tempResult[i].Columns[2].CustomCellRender = function (cell) {
                    return "<a title='" + cell.Value.licenseeName + "' href='javascript:SearchDetail(" + cell.Value.licenseeId + ",\"" + cell.Value.searchDate + "\"," + cell.Value.yearOrTotalRequest + ")' onclick=\"LicenseeSection(this); return;\">" + cell.Value.licenseeName + "</a>";
                };
            }

            gridOptions =
            {
                columns: [
                    { HeaderText: "#", Width: 20, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "LicenseeId", Width: 20, Name: "SystemId", HeaderAlign: "center", CellAlign: "right" },
                    { HeaderText: "Licensee", Width: 200, Name: "SystemName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Date", Width: 120, Name: "Date", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Year", Width: 120, Name: "Year", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Total Requests", Width: 100, Name: "TotalRequest", HeaderAlign: "center", CellAlign: "center" }
                ],
                bodyRows: tempResult,
                hideYBackGroundScroller: true,
                gridExpandHeight: 150,
                //numberOfFrozenColumn: 2,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: _pageSize,
                    currentPage: _pageIndex,
                    totalItem: result[1].totalItem,
                    itemsPerPageArray: [10, 20, 50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                },
                allowTruncate: true
            };

            $("#SearchResult").fanexGrid(gridOptions);

            switch ($("#PeriodOption").val()) {
                case "Week":
                    gridOptions.columns[3].HeaderText = "Week";
                    $("#SearchResult").fanexGrid("hideColumns", gridOptions, [1]);
                    break;
                case "Year":
                    gridOptions.columns[3].HeaderText = "Month";
                    $("#SearchResult").fanexGrid("hideColumns", gridOptions, [1, 4]);
                    break;
                default:
                    $("#SearchResult").fanexGrid("hideColumns", gridOptions, [1, 4]);
            }

        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
    if (itemsPerPage != $('#PageSize').val()) {
        $('#PageIndex').val(0);
        _pageIndex = 0;
    } else {
        $('#PageIndex').val(page_index);
        _pageIndex = page_index
    }

    $('#PageSize').val(itemsPerPage);
    _pageSize = itemsPerPage;

    gridOptions.pagerOption.currentPage = page_index;
    Search();

    return false;
}

function LicenseeSection(btn) {
    _settingPageSize = 10;
    _settingPageIndex = 0;
    if (btn) {
        $("#tblContent_SearchResult").find(".freegrid-active-row").removeClass("freegrid-active-row");
        $(btn).parent().parent().parent().addClass("freegrid-active-row");
    }
}

function SearchDetail(licenseeID, date, year) {
    if (typeof (date) === "undefined") {
        date = "";
    }

    $('#PageSize').val(_settingPageSize);
    $('#PageIndex').val(_settingPageIndex);

    $("#SearchResultDetail").show();

    _currentLicenseeID = licenseeID;
    _currentDate = date;
    $.ajax({
        url: "GetLicenseeRequestsDetail",
        type: "POST",
        data: $('#myForm').serialize() + "&SystemId=" + licenseeID + "&date=" + date + "&year=" + year,
        success: function (result) {
            result = result.replace(/\n/g, "<\/br>");
            result = eval(result);
            gridOptionsDetail =
            {
                columns: [
                   { HeaderText: "#", Width: 20, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Affiliate", Width: 160, Name: "Partner", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Domain", Width: 160, Name: "Domain", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "IP", Width: 110, Name: "IP", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Description", Width: 110, Name: "Des", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Date", Width: 120, Name: "Date", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Last Call Time", Width: 120, Name: "LastCallTime", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Total Requests", Width: 100, Name: "TotalRequest", HeaderAlign: "center", CellAlign: "center" }
                ],
                bodyRows: result[0].data,
                hideYBackGroundScroller: true,
                gridExpandHeight: 150,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: _settingPageSize,
                    currentPage: _settingPageIndex,
                    totalItem: result[1].totalItem,
                    itemsPerPageArray: [10, 20, 50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPagedGetLicenseeRequestsDetail
                },
                allowTruncate: true
            };

            $("#SearchResultDetail").fanexGrid(gridOptionsDetail);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function ReRenderAfterPagedGetLicenseeRequestsDetail(page_index, itemsPerPage, jq) {
    if (itemsPerPage != $('#PageSize').val()) {
        $('#PageIndex').val(0);
        _settingPageIndex = 0;
    } else {
        $('#PageIndex').val(page_index);
        _settingPageIndex = page_index;
    }

    $('#PageSize').val(itemsPerPage);
    _settingPageSize = itemsPerPage;

    SearchDetail(_currentLicenseeID, _currentDate)

    return false;
}

function StartAutoSuggesttion() {
    var _systemName = $.trim($("#Systemname").val());
    if (_systemName == "" || _systemName == "Licensee name") {
        $("#LicenseeId").val(0); //get all systems
    }

    var url = 'ListExistedSystems?systemName=' + _systemName;

    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'html',
        cache: false,
        success: function (result) {
            //lower case keys
            result = result.replace(/"([^"]+)":/g, function ($0, $1) { return ('"' + $1.toLowerCase() + '":'); })

            var sourceAdmin = $.parseJSON(result);
//            if (sourceAdmin[0] != null) {
//                $("#LicenseeId").val(sourceAdmin[0].value);
//            } else {
//                $("#LicenseeId").val(-1);
//            }

            $("#Systemname").autocomplete({
                source: sourceAdmin,
                minLength: 1,
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    //$("#LicenseeId").val(ui.item.value);
                    return false;
                },
                select: function (event, ui) {
                    $(this).val(ui.item.label);
                    //$("#LicenseeId").val(ui.item.value);
                    return false;
                }
            });
        },
        error: function (xhr, status, err) {
        }
    });
}
