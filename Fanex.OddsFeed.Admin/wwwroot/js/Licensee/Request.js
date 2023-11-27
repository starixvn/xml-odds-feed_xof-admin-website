var gridOptions
    ,gridOptionsDetail
    ,_pageIndex = 0
    ,_pageSize = 10
    ,_settingPageIndex = 0
    ,_settingPageSize = 10
    ,_currentLicenseeID;
var _currentDate = null;

$(document).ready(function () {

    Core.registerWeekPicker();

    $("#DateStartLicensee").on("change", function () {
        this.setAttribute(
            "data-date",
            moment(this.value, "YYYY-MM-DD")
                .format(this.getAttribute("data-date-format"))
        )
    }).trigger("change")

    $("#DateEndLicensee").on("change", function () {
        this.setAttribute(
            "data-date",
            moment(this.value, "YYYY-MM-DD")
                .format(this.getAttribute("data-date-format"))
        )
    }).trigger("change")

    $("#FromWeekLicensee").on("change", function () {
        let demo = moment(this.value, "yyyy-Www");
        this.setAttribute(
            "data-date",
            moment(this.value, "yyyy-Www")
                .format(this.getAttribute("data-date-format"))
        )
    }).trigger("change")

    $("#ToWeekLicensee").on("change", function () {
        this.setAttribute(
            "data-date",
            moment(this.value, "yyyy-Www")
                .format(this.getAttribute("data-date-format"))
        )
    }).trigger("change")

    $("#MonthLicensee").on("change", function () {
        this.setAttribute(
            "data-date",
            moment(this.value, "yyyy-MM")
                .format(this.getAttribute("data-date-format"))
        )
    }).trigger("change")

    $("#Week").weekpicker("onSelect", function (start, end) {
        $("#DateStart").datepicker("setDate", start);
    });

    $("#ToWeek").weekpicker("onSelect", function (start, end) {
        $("#DateEnd").datepicker("setDate", end);
    });

    $("#PeriodOption").change(function () {
        setVisibleDateControl($("#PeriodOption option:selected").text(), true);
    });

    setVisibleDateControl($("#PeriodOption option:selected").text(), false);

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
        $("#LicenseeId").val(0);
    }

    var licenseeId = $("#LicenseeId").val();

    if (licenseeId == 0) {
        $("#SearchResultDetail").hide();
        SearchAll();
    } else {
        $("#SearchResult").hide();
        SearchDetail(licenseeId);
    }
}

function renderBodyRows(records) {
    var rows = [];
    var totalRows = records.length;
    for (var i = 0; i < totalRows; i++) {
        var record = records[i];

        var row = {
            Columns: [
                { Name: 'Row', Value: record.row },
                {
                    Name: 'SystemName',
                    Value: "<a title='" + record.systemName +
                        "' href='javascript:SearchDetail(" + record.systemId + ",\"" + record.date + "\"," + record.year +
                        ")' onclick=\"LicenseeSection(this); return;\">" + record.systemName + "</a>"
                },
                { Name: 'Date', Value: record.date },
                { Name: 'Year', Value: record.year },
                { Name: 'TotalRequest', Value: record.totalRequest }
            ]
        };

        rows.push(row);
    }

    return rows;
}

function SearchAll() {
    $('#PageSize').val(_pageSize);
    $('#PageIndex').val(_pageIndex);

    $("#SearchResult").show();
    _currentDate = null;
    var month = $('#myFormLicenseeStatistcs #MonthLicensee').val();
    var searchParams = {
        PageSize: parseInt($('#myFormLicenseeStatistcs #PageSize').val()),
        PageIndex: parseInt($('#myFormLicenseeStatistcs #PageIndex').val()),
        PageCount: parseInt($('#myFormLicenseeStatistcs #PageCount').val()),
        LicenseeId: parseInt($('#myFormLicenseeStatistcs #LicenseeId').val()),
        PeriodOption: parseInt($('#PeriodOption').val()),
        DateStart: $('#myFormLicenseeStatistcs #DateStartLicensee').val(),
        Week: $('#myFormLicenseeStatistcs #FromWeekLicensee').val(),
        Month: parseInt(month.slice(month.length - 2, month.length)),
        DateEnd: $('#myFormLicenseeStatistcs #DateEndLicensee').val(),
        ToWeek: $('#myFormLicenseeStatistcs #ToWeekLicensee').val(),
        Year: parseInt($('#myFormLicenseeStatistcs #YearLicensee').val())
    };
    $.ajax({
        url: "GetAllLicenseeRequest",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(searchParams),
        cache: false,
        success: function (result) {
            gridOptions =
            {
                columns: [
                    { HeaderText: "#", Width: 20, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Licensee", Width: 200, Name: "SystemName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Date", Width: 120, Name: "Date", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Year", Width: 120, Name: "Year", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Total Requests", Width: 100, Name: "TotalRequest", HeaderAlign: "center", CellAlign: "center" }
                ],
                bodyRows: renderBodyRows(result.data),
                hideYBackGroundScroller: true,
                gridExpandHeight: 150,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: _pageSize,
                    currentPage: _pageIndex,
                    totalItem: result.totalItems,
                    itemsPerPageArray: [10, 20, 50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            $("#SearchResult").fanexGrid(gridOptions);

            switch ($("#PeriodOption").val()) {
                case "Week":
                    gridOptions.columns[2].HeaderText = "Week";
                    $("#SearchResult").fanexGrid("hideColumns", gridOptions, []);
                    break;
                case "Year":
                    gridOptions.columns[2].HeaderText = "Month";
                    $("#SearchResult").fanexGrid("hideColumns", gridOptions, [3]);
                    break;
                default:
                    $("#SearchResult").fanexGrid("hideColumns", gridOptions, [3]);
            }
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


function renderDetailBodyRows(records) {
    var rows = [];
    var totalRows = records.length;
    for (var i = 0; i < totalRows; i++) {
        var record = records[i];

        var row = {
            Columns: [
                { Name: 'Row', Value: record.row },
                { Name: 'Partner', Value: record.partner },
                { Name: 'Domain', Value: record.domain },
                { Name: 'IP', Value: record.ip },
                { Name: 'Des', Value: record.des },
                { Name: 'Date', Value: record.date },
                { Name: 'LastCallTime', Value: record.lastCallTime },
                { Name: 'TotalRequest', Value: record.totalRequest }
            ]
        };

        rows.push(row);
    }

    return rows;
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

    var month = $('#myFormLicenseeStatistcs #MonthLicensee').val();
    var searchDetailParams = {
        PageSize: parseInt($('#myFormLicenseeStatistcs #PageSize').val()),
        PageIndex: parseInt($('#myFormLicenseeStatistcs #PageIndex').val()),
        PageCount: parseInt($('#myFormLicenseeStatistcs #PageCount').val()),
        LicenseeId: parseInt($('#myFormLicenseeStatistcs #LicenseeId').val()),
        PeriodOption: parseInt($('#PeriodOption').val()),
        DateStart: $('#myFormLicenseeStatistcs #DateStartLicensee').val(),
        Week: $('#myFormLicenseeStatistcs #FromWeekLicensee').val(),
        Month: parseInt(month.slice(month.length - 2, month.length)),
        DateEnd: $('#myFormLicenseeStatistcs #DateEndLicensee').val(),
        ToWeek: $('#myFormLicenseeStatistcs #ToWeekLicensee').val(),
        Year: parseInt($('#myFormLicenseeStatistcs #YearLicensee').val()),
        SystemId: parseInt(licenseeID),
        date: date
    };

    $.ajax({
        url: "GetLicenseeRequestsDetail",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        cache: false,
        data: JSON.stringify(searchDetailParams),
        success: function (result) {
            gridOptionsDetail =
            {
                columns: [
                    { HeaderText: "#", Width: 20, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Affiliate", Width: 160, Name: "Partner", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Domain", Width: 160, Name: "Domain", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    { HeaderText: "IP", Width: 110, Name: "IP", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Description", Width: 110, Name: "Des", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    { HeaderText: "Date", Width: 120, Name: "Date", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Last Call Time", Width: 120, Name: "LastCallTime", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Total Requests", Width: 100, Name: "TotalRequest", HeaderAlign: "center", CellAlign: "center" }
                ],
                bodyRows: renderDetailBodyRows(result.data),
                hideYBackGroundScroller: true,
                gridExpandHeight: 150,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: _settingPageSize,
                    currentPage: _settingPageIndex,
                    totalItem: result.totalItems,
                    itemsPerPageArray: [10, 20, 50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPagedGetLicenseeRequestsDetail
                }
            };

            $("#SearchResultDetail").fanexGrid(gridOptionsDetail);
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