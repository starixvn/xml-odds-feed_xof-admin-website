$(document).ready(function () {
    $("#PreController").val(refController);
    $("#PreAction").val(refAction);
    $("#Save").click(function () {
        DoUpdatePassword();
    });
});

function CheckValidation() {
    $("#error-message-box").hide();
    //validate oldpass
    $("#oldPassReqErr").hide();
    $("#oldPassWhiteSpaceErr").hide();
    $("#oldPassLengErr").hide();
    var oldpass = $.trim($("#OldPassword").val());
    var oldPass_IsValid = true;
    if (oldpass == "") {
        $("#oldPassReqErr").show();
        oldPass_IsValid = false;
    } else if (oldpass.length > 50 || oldpass.length < 6) {
        $("#oldPassLengErr").show();
        oldPass_IsValid = false;
    }

    // validate newpass
    $("#newPassReqErr").hide();
    $("#newPassWhiteSpaceErr").hide();
    $("#newPassLengErr").hide();
    var newpass = $.trim($("#Password").val());
    var newPass_IsValid = true;
    if (newpass == "") {
        $("#newPassReqErr").show();
        newPass_IsValid = false;
    } else if (newpass.length > 50 || newpass.length < 6) {
        $("#newPassLengErr").show();
        newPass_IsValid = false;
    }

    // validate confirm pass
    $("#confirmPassReqErr").hide();
    $("#confirmPassWhiteSpaceErr").hide();
    $("#confirmPassLengErr").hide();
    $("#pwdNotMatchErr").hide();
    var confirmpass = $.trim($("#ConfirmPassword").val());
    var confirmPass_IsValid = true;
    if (confirmpass == "") {
        $("#confirmPassReqErr").show();
        confirmPass_IsValid = false;
    } else if (confirmpass.length > 50 || confirmpass.length < 6) {
        $("#confirmPassLengErr").show();
        confirmPass_IsValid = false;
    } else if (newpass != confirmpass) {
        $("#pwdNotMatchErr").show();
        confirmPass_IsValid = false;
        newPass_IsValid = false;
    }

    // Hightlight error
    if (oldPass_IsValid) {
        $("#OldPassword").removeClass("error");
    } else {
        $("#OldPassword").addClass("error");
    }

    if (newPass_IsValid) {
        $("#Password").removeClass("error");
    } else {
        $("#Password").addClass("error");
    }

    if (confirmPass_IsValid) {
        $("#ConfirmPassword").removeClass("error");
    } else {
        $("#ConfirmPassword").addClass("error");
    }

    var ret = oldPass_IsValid && newPass_IsValid && confirmPass_IsValid;
    if (ret) {
        $("#errorUpdatePass").addClass("hidden");
    } else {
        $("#errorUpdatePass").removeClass("hidden");
    }

    return ret;
}

function IsPassword(str) {
    var regex1 = /^(?=.*\d).{8,100}$/;
    var regex2 = /^(?=.*[a-z]).{8,100}$/;
    var regex3 = /^(?=.*[A-Z]).{8,100}$/;
    var regex4 = /^(?=.*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]).{8,100}$/;

    var case1 = str.search(regex1) != -1 ? 1 : 0;
    var case2 = str.search(regex2) != -1 ? 1 : 0;
    var case3 = str.search(regex3) != -1 ? 1 : 0;
    var case4 = str.search(regex4) != -1 ? 1 : 0;
    var case5 = str.indexOf(' ') != -1;
    return (case1 + case2 + case3 + case4 >= 3) && case5;
}

function DoUpdatePassword() {
    var isParamsValid = CheckValidation();
    if (!isParamsValid) {
        $(".result-message-box").addClass("hidden");
        return;
    }

    var preController = $("#PreController").val();
    var preAction = $("#PreAction").val();
    var updateParams = {
        UserID: $("#UserID").val(),
        OldPassword: $("#OldPassword").val(),
        Password: $("#Password").val(),
        ConfirmPassword: $("#ConfirmPassword").val(),
        PreController: preController != '' ? preController : "Dashboard",
        PreAction: preAction != '' ? preAction : "ShowDashboard"
    };

    $.ajax({
        url: currentDomainURL + 'Login/UpdateUserPassword',
        type: 'POST',
        cache: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(updateParams),
        success: function (result) {
            if (result.IsSuccessful == true) {
                $(".result-message-box").empty()
                    .append("<ul><li>" + result.Message + "</li></ul>")
                    .addClass("success-message-box")
                    .removeClass("error-message-box")
                    .removeClass("hidden");

                OddsFeedAdminManager.confirmAfterSuccess(currentDomainURL + "" +
                    updateParams.PreController.replace(/\/$/g, '') + "/" +
                    updateParams.PreAction.replace(/^\//g, ''));
            } else {
                $(".result-message-box").empty()
                    .append("<ul><li>" + result.Message + "</li></ul>")
                    .addClass("error-message-box")
                    .removeClass("success-message-box")
                    .removeClass("hidden");
            }
        },
        error: function (xhr, status, err) {
        }
    });
};