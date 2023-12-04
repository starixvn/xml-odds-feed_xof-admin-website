var gridOptions,
    _pageIndex = 0,
    _pageSize = 10;

var monthNames = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");
$(document).ready(function () {

    Core.registerWeekPicker();
    FormatDatePicker();

    $("#PeriodOption").change(function () {
        setVisibleDateLicenseeControl($("#PeriodOption option:selected").text());
    });
    setVisibleDateLicenseeControl($("#PeriodOption option:selected").text());

    //if ($("#StartDateConfig").val() != "") {
    //    setVisibleDateControlConfig($("#StartDateConfig").val());
    //}

    //$("#Affiliatename").Watermark("Affiliate name");

    //$("#DomainName").Watermark("Domain name");

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

    $('#YearLicensee').change(renderMonth);
    renderMonth();

    Search();
});

//--------------------------------------------------------------------------------------------------------------------
function setVisibleDateLicenseeControl(currentValue) {
    if (currentValue == "Week") {
        $(".date-area, .month-area, .year-area").hide();
        $(".week-area").show();
    }
    else if (currentValue == "Month") {
        $(".date-area, .week-area, .year-area").hide();
        $(".month-area").show();
    }
    else if (currentValue == "Year") {
        $(".date-area, .month-area, .week-area").hide();
        $(".year-area").show();
    }
    else {
        $(".date-area").show();
        $(".week-area, .month-area, .year-area").hide();
    }
    $('#DateStartLicensee').change(setFromToDateConstraintLimit1Year);
    $('#DateEndLicensee').change(setFromToDateConstraintLimit1Year);
    $('#FromWeekLicensee').change(setWeekRangeConstraintLimit1Year);
    $('#ToWeekLicensee').change(setWeekRangeConstraintLimit1Year);
}
function setWeekRangeConstraintLimit1Year() {
    let $from = $('#FromWeekLicensee');
    let $to = $('#ToWeekLicensee');
    let from = new Date(moment($from.val(), "YYYY-Www").format("YYYY-MM-DD"));
    let to = new Date(moment($to.val(), "YYYY-Www").format("YYYY-MM-DD"));
    let i = from.MonthSubtract(to);
    if (Math.abs(i) > 12 || from > to) {
        if (this.id === 'FromWeekLicensee') {
            let numWeekChange = from.getWeek() < 10 ? ("0" + from.getWeek()) : from.getWeek();
            let weekChange = from.getFullYear() + "-W" + numWeekChange;
            $to.val(weekChange);
            $to[0].setAttribute(
                "data-date",
                moment(weekChange, "YYYY-Www")
                    .format($to[0].getAttribute("data-date-format"))
            )
        }
        else {
            let numWeekChange = to.getWeek() < 10 ? ("0" + to.getWeek()) : to.getWeek();
            let weekChange = to.getFullYear() + "-W" + numWeekChange;
            $from.val(weekChange);
            $from[0].setAttribute(
                "data-date",
                moment(weekChange, "YYYY-Www")
                    .format($from[0].getAttribute("data-date-format"))
            )
        }
    }
}
function setFromToDateConstraintLimit1Year() {
    let $dateStart = $('#DateStartLicensee');
    let $dateEnd = $('#DateEndLicensee');
    let startdate = new Date($dateStart.val().toString());
    let enddate = new Date($dateEnd.val().toString());
    if (enddate < startdate) {
        if (this.id === 'DateStartLicensee') {
            let dateChange = moment(startdate.format(), "DD MMM YYYY").format("YYYY-MM-DD");
            $dateEnd.val(dateChange);
            $dateEnd[0].setAttribute(
                "data-date",
                moment(dateChange, "YYYY-MM-DD")
                    .format($dateEnd[0].getAttribute("data-date-format"))
            )
        }
        else {
            let dateChange = moment(enddate.format(), "DD MMM YYYY").format("YYYY-MM-DD");
            $dateStart.val(dateChange);
            $dateStart[0].setAttribute(
                "data-date",
                moment(dateChange, "YYYY-MM-DD")
                    .format($dateStart[0].getAttribute("data-date-format"))
            )
        }
    }
    else if (enddate.getFullYear() != startdate.getFullYear()) {
        if (enddate.MonthSubtract(startdate) > 12) {
            if (this.id === 'DateStartLicensee') {
                let dateChange = moment(startdate.format(), "DD MMM YYYY").format("YYYY-MM-DD");
                $dateEnd.val(dateChange);
                $dateEnd[0].setAttribute(
                    "data-date",
                    moment(dateChange, "YYYY-MM-DD")
                        .format($dateEnd[0].getAttribute("data-date-format"))
                )
            }
            else {
                let dateChange = moment(enddate.format(), "DD MMM YYYY").format("YYYY-MM-DD");
                $dateStart.val(dateChange);
                $dateStart[0].setAttribute(
                    "data-date",
                    moment(dateChange, "YYYY-MM-DD")
                        .format($dateStart[0].getAttribute("data-date-format"))
                )
            }
        }
    }
}
Date.prototype.MonthSubtract = function (d) {
    if (this.getFullYear() == d.getFullYear()) {
        return this.getMonth() - d.getMonth();
    } else {
        return this.getMonth() - d.getMonth() + 12 * (this.getFullYear() - d.getFullYear());
    }
};
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

function OnSearch() {
    _pageSize = 10;
    _pageIndex = 0;
    Search();
};

function FormatDatePicker() {
    $("#DateStartLicensee").on("change", function () {
        this.setAttribute(
            "data-date",
            moment(this.value, "YYYY-MM-DD")
                .format(this.getAttribute("data-date-format"))
        )
    }).trigger("change");
    $("#DateEndLicensee").on("change", function () {
        this.setAttribute(
            "data-date",
            moment(this.value, "YYYY-MM-DD")
                .format(this.getAttribute("data-date-format"))
        )
    }).trigger("change");
    $("#FromWeekLicensee").on("change", function () {
        this.setAttribute(
            "data-date",
            moment(this.value, "YYYY-Www")
                .format(this.getAttribute("data-date-format"))
        )
    }).trigger("change");
    $("#ToWeekLicensee").on("change", function () {
        this.setAttribute(
            "data-date",
            moment(this.value, "YYYY-Www")
                .format(this.getAttribute("data-date-format"))
        )
    }).trigger("change");
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

    var month = $('#MonthLicensee').val();
    var dateStart = $("#PeriodOption option:selected").text() === 'Week' ?
        moment($('#FromWeekLicensee').val(), "YYYY-Www").format("YYYY-MM-DD").toString() :
        $('#DateStartLicensee').val();
    var dateEnd = $("#PeriodOption option:selected").text() === 'Week' ?
        moment($('#ToWeekLicensee').val(), "YYYY-Www").format("YYYY-MM-DD").toString() :
        $('#DateEndLicensee').val();

    var searchParams = {
        PartnerName: $.trim($("#Affiliatename").val()),
        DomainId: -1,
        Domainname: $.trim($("#DomainName").val()),
        PeriodOption: parseInt($('#PeriodOption').val()),
        DateStart: dateStart,
        Month: parseInt(month),
        DateEnd: dateEnd,
        ToWeek: $('#ToWeekLicensee').val(),
        Year: parseInt($('#YearLicensee').val()),
        PageSize: parseInt(_pageSize),
        PageIndex: parseInt(_pageIndex),
    };
    console.log(searchParams)

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