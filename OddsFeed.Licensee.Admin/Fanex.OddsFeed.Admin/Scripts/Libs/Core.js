var currentTheme;
var _monthNames = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
var DATE_FORMAT = "dd M yy";
var Core = Core || {};

/// Scripts run on page load complete
$(document).ready(function () {
      
    Core.maxDate = new Date($("#maxDate").val());
    Core.minDate = new Date($("#minDate").val());
    Core.ajaxTimeout = 3600000;

    currentTheme = $('#currentTheme').val();
    
    if ($('.datepicker').length > 0) {
        var mindate = $('.datepicker').attr('mindate');
        var maxdate = $('.datepicker').attr('maxdate');
        $('.datepicker').datepicker({
            dateFormat: DATE_FORMAT,
            showOn: "button",
            firstDay: 1, // monday
            changeYear: true,
            changeMonth: true,
            buttonImageOnly: true,
            buttonText: "",
            buttonImage: currentDomainURL + "App_Themes/" + currentTheme + "/Images/calendar.gif"
        });
        if (mindate != undefined && mindate != 'null') {
            mindate = mindate.split("/");
            $('.datepicker').datepicker("option", "minDate", new Date(mindate[0], mindate[1] - 1, mindate[2]));
        }
        if (maxdate != undefined && maxdate != 'null') {
            maxdate = maxdate.split("/");
            $('.datepicker').datepicker("option", "maxDate", new Date(maxdate[0], maxdate[1] - 1, maxdate[2]));
        }

    }
});

Date.prototype.compareTo = function (date) {
    if (isNaN(this)) { throw new Error(this); }
    if (date instanceof Date && !isNaN(date)) { return (this > date) ? 1 : (this < date) ? -1 : 0; } else { throw new TypeError(date); }
}


Core.registerWeekPicker = function () {
    // week picker plugin

    var configs = {};

    $.fn.weekpicker = function (options, func) {

        var $this = $(this);
        $this.css("margin", "2px");
        var $calendar = $this.nextAll("div:first");

        var id = $this.attr("id");
        if (!configs[id])
            configs[id] = { date: Core.maxDate };
        var defaults = configs[id];

        var displayYear = defaults.date.getFullYear();
        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                $calendar.find('.ui-datepicker-current-day a').addClass('ui-state-active')
            }, 1);
        };

        var selectWeek = function (date, hideOnSelect) {

            if (!date) {
                date = $calendar.datepicker('getDate');
                if (!date)
                    date = Core.maxDate;
            }
            var dat = date.getDate();
            var day = date.getDay();

            defaults.startDate = new Date(date.getFullYear(), date.getMonth(), 1);
            if (day == 0)
                day = 7;
            defaults.startDate.setDate(dat - day + 1);
            defaults.endDate = new Date(date.getFullYear(), date.getMonth(), 1);
            defaults.endDate.setDate(dat - day + 7);

           
            if (date) {
                var week = $.datepicker.iso8601Week(defaults.startDate);
                $this.val(week + " (" + defaults.startDate.format() + ")");
            }

            if (defaults.selectHandlers) {
                for (var i in defaults.selectHandlers)
                    defaults.selectHandlers[i](defaults.startDate, defaults.endDate, $this[0]);
            }
            
            if (hideOnSelect)
                $this.weekpicker("hide");
            //if (defaults.startDate == date) {
            $calendar.datepicker('setDate', defaults.startDate);
            //}
            displayYear = defaults.startDate.getFullYear();
            selectCurrentWeek();
        };

        if (typeof options == "string") {

            if (options == "hide") {
                $calendar.fadeOut("fast");
            }
            else if (options == "show") {
                if ($calendar.is(":visible"))
                    return;
                $calendar.show();
            }
            else if (options == "onSelect") {
                if (!defaults.selectHandlers)
                    defaults.selectHandlers = [];
                defaults.selectHandlers.push(func);
            }
            else if (options == "getDates") {
                return { start: defaults.startDate, end: defaults.endDate.compareTo(Core.maxDate) == 1 ? Core.maxDate : defaults.endDate };
            }
            else if (options == "setDate") {
                selectWeek(func);
            }
        }
        else {

            $.extend(defaults, options);

            $this.addClass("datepicker hasDatepicker");
            $this.after("<div class='weekcalendar'></div><img style='position: relative; margin-top:4px;' class='ui-datepicker-trigger' src='" + currentDomainURL + "App_Themes/" + currentTheme + "/Images/calendar.gif' alt='' title=''>");
            $calendar = $this.next();

            $calendar.css({ "position": "absolute", "z-index": 1000, "margin-top": "2px", "float": "left" });
            var ishover = false;
            $.extend(defaults, {
                dateFormat: DATE_FORMAT,
                showOn: null,
                buttonImageOnly: true,
                buttonText: "",
                buttonImage: currentDomainURL + "App_Themes/" + currentTheme + "/Images/calendar.gif",
                firstDay: 1,
                showOtherMonths: true,
                selectOtherMonths: true,
                showWeek: true,
                //       minDate: new Date(2010,11,11),// defaults.minDate,
                //       maxDate: new Date(2012,11,11), //defaults.maxDate,
                changeYear: true,
                changeMonth: true,
                onSelect: function (dateText, inst) {
                    selectWeek(null, true);
                },
                beforeShowDay: function (date) {
                    var cssClass = '';
                    if (date >= defaults.startDate && date <= defaults.endDate)
                        cssClass = 'ui-datepicker-current-day';
                    return [true, cssClass];
                },
                onChangeMonthYear: function (year, month, inst) {
                    selectCurrentWeek();
                    displayYear = year;
                }
            });

            $calendar.datepicker(defaults);

            var show = function () {
                ishover = true;
                $this.weekpicker("show");
            }

            $calendar.hide();

            $this.width(144).mousedown(show);

            function enter() {
                ishover = true;
            }
            function leave() {
                ishover = false;
            }

            $calendar.mouseenter(enter).mouseleave(leave);
            $this.mouseenter(enter).mouseleave(leave);

            $("html").bind("click", function (e) {
                if (!ishover) {
                    $this.weekpicker("hide");
                }
            });
            $this.nextAll("img.ui-datepicker-trigger:first").click(function () {
                if ($calendar.is(":visible"))
                    $this.weekpicker("hide");
                else
                    show();
            });

            // set initial value    
            selectWeek(defaults.date);
        }
    }

    if ($('.weekpicker').length > 0) {
        $('.weekpicker').each(function () {
            $(this).weekpicker({
                date: new Date($(this).attr('data-value')),
                minDate: new Date($(this).attr('data-mindate')),
                maxDate: new Date($(this).attr('data-maxdate'))
            });
        });
    }
    
    $('.weekcalendar table.ui-datepicker-calendar tr').die().live({
        mousemove: function () { $(this).find('td a').addClass('ui-state-hover'); },
        mouseleave: function () { $(this).find('td a').removeClass('ui-state-hover'); }
    });
}
Date.prototype.format = function () {
    return this.getDate() + " " + _monthNames[this.getMonth()] + " " + this.getFullYear();
}

Date.prototype.getWeek = function () {
    // Copy date so don't modify original
    d = new Date(this);
    d.setHours(0, 0, 0);
    // Set to nearest Thursday: current date + 4 - current day number
    // Make Sunday's day number 7
    d.setDate(d.getDate() + 4 - (d.getDay() || 7));
    // Get first day of year
    var yearStart = new Date(d.getFullYear(), 0, 1);
    // Calculate full weeks to nearest Thursday
    var weekNo = Math.ceil((((d - yearStart) / 86400000) + 1) / 7) + 1;
    // Return array of year and week number
    return weekNo;
}

function convertDateToString(date) {
    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear();

    var returnDate = "";
    if (day < 10)
        returnDate += "0" + day;
    else
        returnDate += day;

    if (month < 10)
        returnDate += "/0" + month + "/" + year;
    else
        returnDate += "/" + month + "/" + year;

    return returnDate;
}

function convertStringToDate(stringDate) {
    var temp = stringDate.split("/");
    if (temp.length != 3) {
        return stringDate + " is not valid format";
    }

    var day = temp[0];
    var month = temp[1];
    var year = temp[2];

    return new Date(Number(year), Number(month) - 1, Number(day));
}

function setDateTimeForModel(srcId, destId, isFullFormat) {
    var srcDate = $("#" + srcId).val();
    if (isFullFormat) {
        srcDate = srcDate.split(" ")[0];
        var temp1 = srcDate.split("/");
        srcDate = temp1[0].length > 1 ? temp1[0] : "0" + temp1[0];
        srcDate += "/" + (temp1[1].length > 1 ? temp1[1] : "0" + temp1[1]);
        srcDate += "/" + temp1[2];
    }
    var temp = srcDate.split("/");
    $("#" + destId).val(temp[1] + "/" + temp[0] + "/" + temp[2]);
}


// Date time constraints

Core.setMonthConstraint = function (monthId, yearId) {

    var $month = monthId ? $("#" + monthId) : $("#Month");
    var $year = yearId ? $("#" + yearId) : $("#Year");

    $year.change(setConstraint);

    setConstraint();
    function setConstraint() {

        $("option", $month).removeAttr("disabled");
        var year = $year.val();

        if (year == Core.maxDate.getFullYear()) {
            var month = Core.maxDate.getMonth() + 1;

            if ($month.val() > month)
                $month.val(month).change();
            $("option:gt(" + (month - 1) + ")", $month).attr("disabled", "disabled");
        }

        else if (year == Core.minDate.getFullYear()) {
            var month = Core.minDate.getMonth() + 1;
            if ($month.val() < month)
                $month.val(month).change();
            $("option:lt(" + (month - 1) + ")", $month).attr("disabled", "disabled");

        }
    }

}

Core.setWeekRangeConstraint = function (fromWeekId, toWeekId) {

    fromWeekId = fromWeekId || "Week";
    toWeekId = toWeekId || "ToWeek";
    var $from = $("#" + fromWeekId);
    var $to = $("#" + toWeekId);

    $from.weekpicker("onSelect", setConstraint);
    $to.weekpicker("onSelect", setConstraint);

    setConstraint();
    function setConstraint(start, end, element) {
        var from = $from.weekpicker("getDates");
        var to = $to.weekpicker("getDates");

        if (from.start > to.start) {
            if (element.id == fromWeekId) {
                $to.weekpicker("setDate", start);
            }
            else {
                $from.weekpicker("setDate", start);
            }
        }
    }
}

Core.setWeekRangeConstraintLimit2Months = function (fromWeekId, toWeekId) {

    fromWeekId = fromWeekId || "Week";
    toWeekId = toWeekId || "ToWeek";
    var $from = $("#" + fromWeekId);
    var $to = $("#" + toWeekId);

    $from.weekpicker("onSelect", setConstraint);
    $to.weekpicker("onSelect", setConstraint);

    setConstraint();
    function setConstraint(start, end, element) {
        if (start == undefined || end == undefined || element == undefined)
            return;

        var from = $from.weekpicker("getDates");
        var to = $to.weekpicker("getDates");
        var i = from.start.MonthSubtract(to.end);
        if (Math.abs(i) > 1 || from.start > to.start) {
            if (element.id == fromWeekId) {
                $to.weekpicker("setDate", start);
            }
            else {
                $from.weekpicker("setDate", start);
            }
        }
    }
}

Core.setWeekRangeConstraintLimit1Year = function (fromWeekId, toWeekId) {

    fromWeekId = fromWeekId || "Week";
    toWeekId = toWeekId || "ToWeek";
    var $from = $("#" + fromWeekId);
    var $to = $("#" + toWeekId);

    $from.weekpicker("onSelect", setConstraint);
    $to.weekpicker("onSelect", setConstraint);

    setConstraint();
    function setConstraint(start, end, element) {
        if (start == undefined || end == undefined || element == undefined)
            return;

        var from = $from.weekpicker("getDates");
        var to = $to.weekpicker("getDates");
        var i = from.start.MonthSubtract(to.end);
        if (Math.abs(i) > 12 || from.start > to.start) {
            if (element.id == fromWeekId) {
                $to.weekpicker("setDate", start);
            }
            else {
                $from.weekpicker("setDate", start);
            }
        }
    }
}

Core.setFromToDateConstraintLimit1Year = function (dateStartId, dateEndId, callback) {

    dateStartId = dateStartId || "DateStart";
    dateEndId = dateEndId || "DateEnd";
    var $dateStart = $("#" + dateStartId);
    var $dateEnd = $("#" + dateEndId);

    $dateStart.change(setConstraint);
    $dateEnd.change(setConstraint);

    function setConstraint() {

        var startdate = $dateStart.datepicker("getDate");
        var enddate = $dateEnd.datepicker("getDate");

        if (enddate < startdate) {

            if (this.id == dateStartId) {
                $dateEnd.datepicker("setDate", startdate);
            }
            else {

                $dateStart.datepicker("setDate", enddate);
            }
            //  return;
        }
        else if (enddate.getYear() != startdate.getYear()) {
            if (enddate.MonthSubtract(startdate) > 12) {

                if (this.id == dateStartId) {
                    $dateEnd.datepicker("setDate", startdate);
                }
                else {
                    $dateStart.datepicker("setDate", enddate);
                }
            }
        }

        if (callback)
            callback();
    }

}

Core.setFromToDateConstraintOnlyToGreaterThanFrom = function (dateStartId, dateEndId, callback) {

    dateStartId = dateStartId || "DateStart";
    dateEndId = dateEndId || "DateEnd";
    var $dateStart = $("#" + dateStartId);
    var $dateEnd = $("#" + dateEndId);

    $dateStart.change(setConstraint);
    $dateEnd.change(setConstraint);

    function setConstraint() {

        var startdate = $dateStart.datepicker("getDate");
        var enddate = $dateEnd.datepicker("getDate");

        if (enddate < startdate) {

            if (this.id == dateStartId) {
                $dateEnd.datepicker("setDate", startdate);
            }
            else {

                $dateStart.datepicker("setDate", enddate);
            }
            //  return;
        }

        if (callback)
            callback();
    }
}

// from/to date constraint
Core.setFromToDateConstraint = function (dateStartId, dateEndId, callback) {

    dateStartId = dateStartId || "DateStart";
    dateEndId = dateEndId || "DateEnd";
    var $dateStart = $("#" + dateStartId);
    var $dateEnd = $("#" + dateEndId);

    $dateStart.change(setConstraint);
    $dateEnd.change(setConstraint);

    function setConstraint() {

        var startdate = $dateStart.datepicker("getDate");
        var enddate = $dateEnd.datepicker("getDate");

        if (enddate < startdate) {

            if (this.id == dateStartId) {
                $dateEnd.datepicker("setDate", startdate);
            }
            else {

                $dateStart.datepicker("setDate", enddate);
            }
            //  return;
        }
        else if ((enddate.getYear() != startdate.getYear()) ||
                (enddate.getMonth() != startdate.getMonth())) {

            if (enddate.getMonth() - 1 != startdate.getMonth() || enddate.getYear() != startdate.getYear()) {

                if (this.id == dateStartId) {
                    var month = startdate.getMonth() + 1;
                    if (month > 11)
                        month = 0;

                    var end = new Date(startdate);
                    end.setMonth(end.getMonth() + 1);

                    if (end.getMonth() > month ||
                    (enddate.getMonth() != end.getMonth() || enddate.getYear() != end.getYear())) {

                        if (end.getMonth() > month)
                            end.setDate(0);
                        if (enddate - end > 0)
                            enddate = end;

                        $dateEnd.datepicker("setDate", enddate);
                    }
                }
                else {
                    var month = enddate.getMonth() - 1;
                    if (month < 0)
                        month = 12;

                    var start = new Date(enddate);
                    start.setMonth(start.getMonth() - 1);

                    if (start.getMonth() > month ||
                        (startdate.getMonth() != start.getMonth() || startdate.getYear() != start.getYear())) {
                        if (start.getMonth() > month)
                            start.setDate(0);
                        if (startdate - start < 0)
                            startdate = start;
                        $dateStart.datepicker("setDate", startdate);
                    }
                }
            }
        }

        if (callback)
            callback();
    }

}

Core.validators = (function () {

    // return 0: email is invalid
    // return 1: email is valid
    function ValidateEmail(email) {
        var emailReg = /^([\w|\-|\.]+@([\w]+.)+[\w]{2,4})?$/;
        if (!emailReg.test($.trim(email)))
            return 0;
        return 1;
    }

    function ValidateIsRequired(value) {

        if (!value || value == 'undefined')
            return 0;
        if ($.trim(value) == '')
            return 0;
        return 1;
    }

    function ValidateHTMLAndSQLInjection(inputVal) {
        var message = { ErrCode: 0, ErrMsg: '' };

        if (/((\%3D)|(=)|(\%27)|(\')|(\-\-)|(\%3B)|(;))()/i.test(inputVal) === true) {
            message.ErrMsg = 'Please do not use SQL meta-characters.';
            message.ErrCode = 1
            return message;
        }

        //Cross site script attack
        if (/((\%3C)|<)*[a-z0-9\%]+((\%3E)|>)/i.test(inputVal) === true) {
            message.ErrMsg = 'Please do not use html syntax.';
            message.ErrCode = 2
            return message;
        }

        //SQL Injection 
        if (/\w*((\s)|(\%32))((\%6F)|o|(\%4F))((\%72)|r|(\%52))((\s)|(\%32))/i.test(inputVal) === true) {
            message.ErrMsg = 'Please do not use word "or".';
            message.ErrCode = 3
            return message;
        }

        // valid characters
        if (/[^\w\s\-\*\(\)\&\~\/\!\.\+\<\>\%\$\\]/.test(inputVal) === true) {
            message.ErrMsg = 'Please do not use special characters.';
            message.ErrCode = 4
            return message;
        }

        return message;
    }

    return {
        email: ValidateEmail,
        isRequired: ValidateIsRequired,
        injection: ValidateHTMLAndSQLInjection
    }
} ());

Date.prototype.MonthSubtract = function (d) {
    if (this.getFullYear() == d.getFullYear()) {
        return this.getMonth() - d.getMonth();
    } else {
        return this.getMonth() - d.getMonth() + 12 * (this.getFullYear() - d.getFullYear());
    }

    return 1;
};

function myConfirm(dialogText, okFunc, cancelFunc, dialogTitle) {
    $('<div style="padding: 10px; max-width: 500px; word-wrap: break-word;">' + dialogText + '</div>').dialog({
        dialogClass: 'my-extra-class',
        draggable: false,
        modal: true,
        resizable: false,
        width: 'auto',
        title: dialogTitle || 'Confirm',
        minHeight: 75,
        buttons: {
            OK: function () {
                if (typeof (okFunc) == 'function') { okFunc(); }
                $(this).dialog('destroy');
            },
            Abort: function () {
                if (typeof (cancelFunc) == 'function') { cancelFunc(); }
                $(this).dialog('destroy');
            }
        }
    });

    $('.my-extra-class').find('.ui-dialog-titlebar-close').css('display', 'none');
};

function uuid() {
    var chars = '0123456789abcdef'.split('');

    var uuid = [], rnd = Math.random, r;
    uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
    uuid[14] = '4'; // version 4

    for (var i = 0; i < 36; i++) {
        if (!uuid[i]) {
            r = 0 | rnd() * 16;

            uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r & 0xf];
        }
    }

    return uuid.join('');
};

function pause(millis) {
    var date = new Date();
    var curDate = null;
    do { curDate = new Date(); }
    while (curDate - date < millis)
};

function getCurrentFormatDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();
    var hours = today.getHours();
    var minutes = today.getMinutes();
    var seconds = today.getSeconds();
    if (dd < 10) { dd = '0' + dd }
    if (mm < 10) { mm = '0' + mm }
    d = mm + '/' + dd + '/' + yyyy + ' ' + hours + ':' + minutes + ':' + seconds

    return d;
}

function setScreenResolution() {

    //    minHeight = screen.height * percentageContent;
    //    minGridHeight = screen.height * percentageGrid;
    if (screen.width == 1024 && screen.height == 768) {
        minHeight = 500;
        minGridHeight = 270;
        maxRowCount = 6;
    }
    else if (screen.width == 1152 && screen.height == 864) {
        minHeight = 600;
        minGridHeight = 380;
        maxRowCount = 9;
    }
    else if (screen.width == 1280 && screen.height == 1024) {
        minHeight = 750;
        minGridHeight = 550;
        maxRowCount = 20;
    }
    else {
        minHeight = 550;
        minGridHeight = 280;
        maxRowCount = 7;
    }

    $('#mainPage .body > .content').css('min-height', minHeight + 'px');
}


function formatNumber(num, isPercent) {
    if (num == null || num == undefined || num == "null")
        return "";
    sign = (num == (num = Math.abs(num)));

    if (!isPercent) {
        num = Math.floor(num * 100 + 0.50000000001);
        if (num == 0)
            return "0.00";

        var str = "";
        var temp2 = num % 100;
        if (temp2 == 0)
            str = ".00";
        else if (temp2 < 10)
            str = ".0" + temp2;
        else
            str = "." + temp2;
        temp = Math.floor(num / 100);
        if (temp == 0) {
            if (temp2 == 0) {
                str = "0" + str;
                return str;
            }
            else {
                str = "0" + str;
            }
        }

        while (temp > 0) {
            temp2 = temp % 1000;
            temp = Math.floor(temp / 1000);
            if (temp > 0) {
                if (temp2 < 10)
                    str = ",00" + temp2 + str;
                else if (temp2 < 100)
                    str = ",0" + temp2 + str;
                else
                    str = "," + temp2 + str;
            }
            else
                str = temp2 + str;
        }
    }
    else {
        num = Math.floor(num * 10000 + 0.50000000001);
        num = Math.abs(num / 100);
        str = num + "%";
    }

    if (!sign) {
        str = "<span style='color:red;'>(" + str + ")</span>";
    }

    return str;
}

function int2Formatter(num, options, rowObject) {
    if (num == null || num == undefined || num == "null")
        return "";

    var str = "";
    if (num != 0) {
        str = Math.round(num);
    }

    return str;
}

function intFormatter(num, options, rowObject) {
    // if use intFormatter but input num is not a number then return num
    if (isNaN(Number(num)) || num == "") {
        return num;
    }

    if (num == null || num == undefined || num == "null")
        return "";

    if (num == 0)
        return 0;

    var str = "";
    var temp = Math.abs(Math.round(num));
    while (temp > 0) {
        var temp2 = temp % 1000;
        temp = Math.floor(temp / 1000);
        if (temp > 0) {
            if (temp2 < 10)
                str = ",00" + temp2 + str;
            else if (temp2 < 100)
                str = ",0" + temp2 + str;
            else
                str = "," + temp2 + str;
        }
        else
            str = temp2 + str;
    }

    if (num < 0)
        str = "<span style='color:red;'>(" + str + ")</span>";
    return str;
}

function currencyFomatter(num, options, rowObject) {
    // if use currencyFomatter but input num is not a number then return num
    if (isNaN(Number(num)) || num == "") {
        return num;
    }

    var percentFormat = false;
    if (percentRowCollection != null) {
        var rowNum = $(this).getGridParam("rowNum");
        var page = $(this).getGridParam("page");
        var rowId = rowNum * (page - 1) + options.rowId;

        if (percentRowCollection.indexOf(rowId) > -1)
            percentFormat = true;
    }

    return formatNumber(num, percentFormat);
}

function percentFomatter(num, options, rowObject) {
    return formatNumber(num, true);
}

function normalFomatter(num, options, rowObject) {

    if (num == null || num == undefined) {
        return "";
    }

    return num;
}

