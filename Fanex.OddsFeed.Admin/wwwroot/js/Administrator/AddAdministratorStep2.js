$(document).ready(function () {
    //step 2
    $("#step2Previous").click(function () {
        $("div.step-active").attr("class", "step-inactive");
        $("#panelStep1").attr("class", "step-active");

        $("div.step").hide();
        $("#errorStep1").hide();
        $("#step1").show();
        $("#step2 .error-message-box").addClass("hidden");
        $("#step2 #success-message-box").addClass("hidden");
        $("#step2 #errorStep2").addClass("hidden");
        $(".licenseeList").removeClass("error");
    });

    $("#step2Done").click(function () {
        if (CheckValidation_Step2()) {
            AddAdministrator();
        }
    });

    //Sport checkboxes
    $("[name=chkSystem]").attr('checked', true); //check all sports when adding new admin by default
    $('#chkAllSystem').attr("checked", true);

    $("#chkAllSystem").click(function () {
        if ($('#chkAllSystem').is(":checked")) {
            $('.chkSystem').each(function () { this.checked = true; });
        }
        else {
            $('.chkSystem').each(function () { this.checked = false; });
        }
    });

    $(".chkSystem").click(function () {
        if ($('.chkSystem:checked').length == $('.chkSystem').length) {
            $('#chkAllSystem').attr("checked", true);
        }
        else {
            $('#chkAllSystem').attr("checked", false);
        }
    });
});
//--------------------------------------------------------------------------------------------------------------------
function CheckValidation_Step2() {
    $("#step2 #success-message-box").addClass("hidden");
    $("#step2 .error-message-box").addClass("hidden");
    var isValid_LicenseeList = true;
    $("#licenseeReqErr").addClass("hidden");
    if ($('.chkSystem:checked').length == 0) {
        isValid_LicenseeList = false;
        $("#licenseeReqErr").removeClass("hidden");
    }

    if (isValid_LicenseeList) {
        $(".licenseeList").removeClass("error");
    } else {
        $(".licenseeList").addClass("error");
    }

    var ret = isValid_LicenseeList;
    if (!ret) {
        $("#errorStep2").removeClass("hidden");
    } else {
        $("#errorStep2").addClass("hidden");
    }

    return ret;
};