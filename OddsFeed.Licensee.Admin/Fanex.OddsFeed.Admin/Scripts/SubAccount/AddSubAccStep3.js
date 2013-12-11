$(document).ready(function () {
    $('.chkPerSettings').attr("checked", false);
    $('input[isdisabled="disabled"]').attr("disabled", "disabled");

    //step 3
    $("#step3Previous").click(function () {
        $("div.step-active").attr("class", "step-inactive");
        $("#panelStep2").attr("class", "step-active");

        $("div.step").hide();
        $("#step2").show();
        $("#errorStep3").addClass("hidden");
        $("#step3 .result-message-box").addClass("hidden");
    });

    $("#Save").click(function () {
        DoAddSubAccount();
    });
});

function CheckValidation_Step3() {
    var ret = true;
    $("#step3 .error-message-box").addClass("hidden");
    //    if ($('.chkPerSettings:checked').length == 0) {
    //        ret = false;
    //        $("#permissionSettingsReqErr").removeClass("hidden");
    //    } else {
    //        $("#permissionSettingsReqErr").addClass("hidden");
    //    }

    if (!ret) {
        $("#step3 .form").addClass("error");
        $("#errorStep3").removeClass("hidden");
    } else {
        $("#step3 .form").removeClass("error");
        $("#errorStep3").addClass("hidden");
    }

    return ret;
};

function ResetFilter() {
    $("#txtUserName").val("");
    $("#txtPassword").val("");
    $("#txtFirstName").val("");
    $("#txtLastName").val("");
    $("#txtEmail").val("");
    $("#txtConfirmPwd").val("");

    //Step 2
    $(".chkSystem").prop("checked", true);
    $("#chkAllSystem").prop("checked", true);

    //Step 3
    $("#chkOddsTypeFlag").prop("checked", true);
    $("#chkOddsStyleFlag").prop("checked", true);
    $("#chkLanguageListFlag").prop("checked", true);
    $("#chkSportIDListFlag").prop("checked", true);
    $("#chkCountryCodeListFlag").prop("checked", true);
    $("#chkBetTypeIDListFlag").prop("checked", true);
};

function DoAddSubAccount() {
    $("#step3 .result-message-box").addClass("hidden");
    var isValid = CheckValidation_Step3();
    if (!isValid) {
        return;
    }

    var licenseeList = [];
    $('.chkSystem:checked').each(function () {
        licenseeList.push($(this).attr("value"));
    });

    var addParams = {
        //Step1
        UserName: $("#txtUserName").val(),
        Password: $("#txtPassword").val(),
        FirstName: $("#txtFirstName").val(),
        LastName: $("#txtLastName").val(),
        Email: $("#txtEmail").val(),
        IsActive: $("#chkIsActive").prop("checked") ? true : false,

        //Step 2
        Systems: licenseeList.join(","),

        //Step 3
        OddsTypeFlag: $("#chkOddsTypeFlag").prop("checked") ? true : false,
        OddsStyleFlag: $("#chkOddsStyleFlag").prop("checked") ? true : false,
        LanguageListFlag: $("#chkLanguageListFlag").prop("checked") ? true : false,
        SportIDListFlag: $("#chkSportIDListFlag").prop("checked") ? true : false,
        CountryCodeListFlag: $("#chkCountryCodeListFlag").prop("checked") ? true : false,
        BetTypeIDListFlag: $("#chkBetTypeIDListFlag").prop("checked") ? true : false
    };

    $.ajax({
        type: 'POST',
        url: 'Add',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(addParams),
        cache: false,
        beforeSend: function (xhr) {
        },
        success: function (result) {
            var success = result != null && result.IsSuccess == true;
            OddsFeedAdminManager.informActionResult(result, $("#step3 .result-message-box"));
            if (success) {
                OddsFeedAdminManager.confirmAfterSuccess(currentDomainURL + "/SubAccount/List");
            }
        },
        error: function (xhr, status, err) {
        }
    });
};