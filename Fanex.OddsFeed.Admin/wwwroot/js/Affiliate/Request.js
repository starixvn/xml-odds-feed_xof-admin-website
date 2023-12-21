var gridOptions,
    _pageIndex = 0,
    _pageSize = 10;

var monthNames = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");
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

    if ($("#StartDateConfig").val() != "") {
        setVisibleDateControlConfig($("#StartDateConfig").val());
    }

    $("#Affiliatename").autocomplete({
        source: [],
        minLength: 1
    });

    $("#DomainName").autocomplete({
        source: [],
        minLength: 1
    });

    $("#Affiliatename").keyup(function (e) {
        if (e.keyCode == $.ui.keyCode.ENTER) {
            Search();
        }
    });

    $("#DomainName").keyup(function (e) {
        if (e.keyCode == $.ui.keyCode.ENTER) {
            Search();
        }
    });

    $("#DateStart").attr("readonly", "true");
    $("#DateEnd").attr("readonly", "true");

    $('#YearAffiliate').change(renderMonth);
    renderMonth();

    Search();
});

//--------------------------------------------------------------------------------------------------------------------
Date.prototype.MonthSubtract = function (d) {
    if (this.getFullYear() == d.getFullYear()) {
        return this.getMonth() - d.getMonth();
    } else {
        return this.getMonth() - d.getMonth() + 12 * (this.getFullYear() - d.getFullYear());
    }
};
function renderMonth() {
    $.each(monthNames, function (index, value) {
        let year = parseInt($('#YearAffiliate').val());
        let isDisabled = year === new Date().getFullYear() && index > new Date().getMonth();
        $('#MonthAffiliate').find('[value="' + (index + 1) + '"]').remove();
        $('#MonthAffiliate').append($('<option>', {
            value: index + 1,
            text: value,
            disabled: isDisabled
        }));
    });
}

function OnSearch() {
    _pageSize = 10;
    _pageIndex = 0;
    Search();
};

function renderBodyRows(records) {
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

function Search() {
    $('#PageSize').val(_pageSize);
    $('#PageIndex').val(_pageIndex);

    var month = $('#MonthAffiliate').val();

    var searchParams = {
        PartnerName: $.trim($("#Affiliatename").val()),
        DomainId: -1,
        Domainname: $.trim($("#DomainName").val()),
        PeriodOption: parseInt($('#PeriodOption').val()),
        DateStart: $("#DateStart").val(),
        Month: parseInt(month),
        DateEnd: $("#DateEnd").val(),
        ToWeek: $('#ToWeekAffiliate').val(),
        Year: parseInt($('#YearAffiliate').val()),
        PageSize: parseInt(_pageSize),
        PageIndex: parseInt(_pageIndex),
    };

    $.ajax({
        url: "GetPartnerRequestStatistic",
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
                    { HeaderText: "Affiliate", Width: 160, Name: "Partner", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Domain", Width: 160, Name: "Domain", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    { HeaderText: "IP", Width: 110, Name: "IP", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Description", Width: 110, Name: "Des", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    { HeaderText: "Date", Width: 120, Name: "Date", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Last Call Time", Width: 120, Name: "LastCallTime", HeaderAlign: "center", CellAlign: "center" },
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

function AffiliateAutoSuggesttion() {

    var urlParam = $.param({
        userName: $("#Affiliatename").val()
    });

    var url = 'ListExistedAffJson?' + urlParam;

    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'html',
        cache: false,
        success: function (result) {
            //lower case keys
            result = result.replace(/"([^"]+)":/g, function ($0, $1) { return ('"' + $1.toLowerCase() + '":'); })

            var sourceAdmin = $.parseJSON(result);

            $("#Affiliatename").autocomplete({
                source: sourceAdmin,
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


function DomainAutoSuggesttion() {
    var urlParam = $.param({
        domainName: $("#DomainName").val()
    });

    var url = 'ListExistedDomainsJson?' + urlParam;

    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'html',
        cache: false,
        success: function (result) {
            //lower case keys
            result = result.replace(/"([^"]+)":/g, function ($0, $1) { return ('"' + $1.toLowerCase() + '":'); })
            var sourceAdmin = $.parseJSON(result);

            $("#DomainName").autocomplete({
                source: sourceAdmin,
                minLength: 1,
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    $("#DomainId").val(ui.item.value);
                    return false;
                },
                select: function (event, ui) {
                    $(this).val(ui.item.label);
                    $("#DomainId").val(ui.item.value);
                    return false;
                }
            });
        },
        error: function (xhr, status, err) {
        }
    });
}