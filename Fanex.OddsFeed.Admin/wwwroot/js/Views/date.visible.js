function setVisibleDateControl(currentValue, isDefault) {

    if (currentValue == "Week") {
        $(".date-area, .month-area, .year-area").hide();
        $(".week-area").show();
        if (isDefault) {
            $('.weekpicker').weekpicker('setDate', Core.maxDate);
        }
    }
    else if (currentValue == "Month") {
        $(".date-area, .week-area, .year-area").hide();
        $(".month-area").show();
        if (isDefault) {
            $("#Month").val(Core.maxDate.getMonth() + 1);
            $("#Year").val(Core.maxDate.getFullYear());
        }
    }
    else if (currentValue == "Year") {
        $(".date-area, .month-area, .week-area").hide();
        $(".year-area").show();
        if (isDefault) {
            $("#Year").val(Core.maxDate.getFullYear());
        }
    }
    else {
        $(".date-area").show();
        $(".week-area, .month-area, .year-area").hide();
        if (isDefault) {
            $("#DateStart").datepicker("setDate", Core.maxDate);
            $("#DateEnd").datepicker("setDate", Core.maxDate);
        }
        else {
            $("#DateStart").datepicker("setDate", new Date());
            $("#DateEnd").datepicker("setDate", new Date());
        }
    }
    Core.setMonthConstraint();
    Core.setFromToDateConstraintLimit1Year();
    Core.setWeekRangeConstraintLimit1Year();
}


function setVisibleDateControlConfig(currentValue) {

    $("#DateStart").datepicker("setDate", new Date(currentValue));
}
