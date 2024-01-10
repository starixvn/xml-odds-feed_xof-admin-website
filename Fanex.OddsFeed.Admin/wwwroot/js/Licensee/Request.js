let gridOptions
    ,gridOptionsDetail
    ,_pageIndex = 0
    ,_pageSize = 10
    ,_settingPageIndex = 0
    ,_settingPageSize = 10
    ,_currentLicenseeID;
let _currentDate = null;
let monthNames = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");

$(document).ready(function () {
    Core.registerWeekPicker();

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

    $("#LicenseeId").val(0);

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

    $('#YearLicensee').change(renderMonth);
    renderMonth();

    Search();
});

//--------------------------------------------------------------------------------------------------------------------
function renderMonth() {
    $.each(monthNames, function (index, value) {
        let year = parseInt($('#YearLicensee').val());
        let isDisabled = year === new Date().getFullYear() && index > new Date().getMonth();
        $('#MonthLicensee').find('[value="' + (index + 1) + '"]').remove();
        $('#MonthLicensee').append($('<option>', {
            value: index + 1,
            text: value,
            disabled: isDisabled
        }));
    });
}
Date.prototype.MonthSubtract = function (d) {
    if (this.getFullYear() == d.getFullYear()) {
        return this.getMonth() - d.getMonth();
    } else {
        return this.getMonth() - d.getMonth() + 12 * (this.getFullYear() - d.getFullYear());
    }
};

function OnSearch() {
    _pageIndex = 0;
    _pageSize = 10;

    Search();
};

function Search() {
    $("#LicenseeId").val(0);
    $("#SearchResultDetail").hide();
    SearchAll();
}

function renderBodyRows(records) {
    let rows = [];
    let totalRows = records.length;
    for (let i = 0; i < totalRows; i++) {
        let record = records[i];

        let row = {
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

    let month = $('#MonthLicensee').val();

    let searchParams = {
        PageSize: parseInt($('#PageSize').val()),
        PageIndex: parseInt($('#PageIndex').val()),
        PageCount: parseInt($('#PageCount').val()),
        LicenseeId: parseInt($('#LicenseeId').val()),
        SystemName: $('#Systemname').val(),
        PeriodOption: $('#PeriodOption option:selected').text(),
        DateStart: $("#DateStart").val(), 
        Week: $('#Week').val(),
        Month: parseInt(month),
        DateEnd: $("#DateEnd").val(),
        ToWeek: $('#ToWeek').val(),
        Year: parseInt($('#YearLicensee').val())
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

            switch ($("#PeriodOption option:selected").text()) {
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
    let rows = [];
    let totalRows = records.length;
    for (let i = 0; i < totalRows; i++) {
        let record = records[i];

        let row = {
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

    let month = $('#MonthLicensee').val();
    let searchDetailParams = {
        PageSize: parseInt($('#PageSize').val()),
        PageIndex: parseInt($('#PageIndex').val()),
        PageCount: parseInt($('#PageCount').val()),
        LicenseeId: parseInt($('#LicenseeId').val()),
        PeriodOption: parseInt($('#PeriodOption').val()),
        DateStart: $("#DateStart").val(),
        Week: $('#FromWeekLicensee').val(),
        Month: parseInt(month),
        DateEnd: $("#DateEnd").val(), 
        ToWeek: $('#ToWeekLicensee').val(),
        Year: parseInt($('#YearLicensee').val()),
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
    let _systemName = $.trim($("#Systemname").val());
    if (_systemName == "" || _systemName == "Licensee name") {
        $("#LicenseeId").val(0); //get all systems
    }

    let url = 'ListExistedSystems?systemName=' + _systemName;

    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'html',
        cache: false,
        success: function (result) {
            result = result.replace(/"([^"]+)":/g, function ($0, $1) { return ('"' + $1.toLowerCase() + '":'); })

            let sourceAdmin = $.parseJSON(result);

            $("#Systemname").autocomplete({
                source: sourceAdmin,
                minLength: 1,
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    $("#LicenseeId").val(ui.item.value);
                    return false;
                },
                select: function (event, ui) {
                    $(this).val(ui.item.label);
                    $("#LicenseeId").val(ui.item.value);
                    return false;
                }
            });
        },
        error: function (xhr, status, err) {
        }
    });
}