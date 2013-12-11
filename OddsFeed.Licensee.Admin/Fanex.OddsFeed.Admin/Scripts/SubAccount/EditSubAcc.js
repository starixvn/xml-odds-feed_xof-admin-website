$(document).ready(function () {
    //Process redirect to SubAcc list.
    var successed = $("#successed").val();
    if (successed == "true") {
        confirm('Successfull!\nPress OK to continue', function () {
            document.location = currentDomainURL + "/SubAccount/List";
        });
    }

    if ($('.chkSystem:checked').length == $('.chkSystem').length) {
        $('#chkAllSystem').attr("checked", true);
    } else {
        $('#chkAllSystem').attr("checked", false);
    }

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

    $('input[isdisabled="disabled"]').attr("disabled", "disabled");
    $("#Save").click(function () {
        UpdateSubAccountInfo();
    });

    $("#Save.updatePassword").unbind("click");
    $("#Save.updatePassword").click(function () {
        DoUpdatePassword();
    });
});

//--------------------------------------------------------------------------------------------------------------------

function editActive(obj, userId) {

    if ($(obj).hasClass('ico-tick')) {

        confirm('Do you want to deactivate this sub-account ?', function () {
            $(obj).removeClass('ico-tick').addClass('ico-untick');

            var url = 'EditActive?userId=' + userId + '&isActive=false';

            //todo
            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'html',
                cache: false,
                beforeSend: function (xhr) {
                },
                success: function (result) {
                    result = result.toLowerCase();

                    if (result == 'successfully!') {
                        document.location = currentDomainURL + "/SubAccount/List";
                    }
                    else {
                        alert(result);
                    }
                },
                error: function (xhr, status, err) {
                }
            });
        });
    }
    else {
        confirm('Do you want to activate this sub-account ?', function () {
            $(obj).removeClass('ico-untick').addClass('ico-tick');

            var url = 'EditActive?userId=' + userId + '&isActive=true';

            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'html',
                cache: false,
                beforeSend: function (xhr) {
                },
                success: function (result) {
                    result = result.toLowerCase();

                    if (result == 'successfully!') {
                        document.location = currentDomainURL + "/SubAccount/List";
                    }
                    else {
                        alert(result);
                    }
                },
                error: function (xhr, status, err) {
                }
            });
        });
    }

};

function deleteSubAccount(userId, name) {

    confirm('Do you want to delete this sub-account  (' + name + ') ?', function () {

        var url = 'Delete?userId=' + userId

        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
            },
            success: function (result) {
                result = result.toLowerCase();

                if (result == 'successfully!') {
                    document.location = currentDomainURL + "/SubAccount/List";
                }
                else {
                    alert(result);
                }
            },
            error: function (xhr, status, err) {
            }
        });
    });
}

//--------------------------------------------------------------------------------------------------------------------

function CheckValidation() {
    $("#success-message-box").hide();

    //validate newpass
    $("#newPassReqErr").hide();
    $("#newPassWhiteSpaceErr").hide();
    $("#newPassLengErr").hide();
    var newPass = $.trim($("#txtPassword").val());
    var newPass_IsValid = true;
    if (newPass == "") {
        $("#newPassReqErr").show();
        newPass_IsValid = false;
    }
//    else if (newPass.indexOf(' ') != -1) {
//        $("#newPassWhiteSpaceErr").show();
//        newPass_IsValid = false;
//    }
    else if (newPass.length > 50 || newPass.length < 6) {
        $("#newPassLengErr").show();
        newPass_IsValid = false;
    }

    //validate confirm pass
    $("#confirmPassReqErr").hide();
    $("#confirmPassWhiteSpaceErr").hide();
    $("#confirmPassLengErr").hide();
    $("#pwdNotMatchErr").hide();
    var confirmpass = $.trim($("#txtConfirmPassword").val());
    var confirmPass_IsValid = true;
    if (confirmpass == "") {
        $("#confirmPassReqErr").show();
        confirmPass_IsValid = false;
    }
//    else if (confirmpass.indexOf(' ') != -1) {
//        $("#confirmPassWhiteSpaceErr").show();
//        confirmPass_IsValid = false;
//    }
    else if (confirmpass.length > 50 || confirmpass.length < 6) {
        $("#confirmPassLengErr").show();
        confirmPass_IsValid = false;
    }
    else if (newPass != confirmpass) {
        $("#pwdNotMatchErr").show();
        confirmPass_IsValid = false;
        newPass_IsValid = false;
    }

    if (newPass_IsValid) {
        $("#txtPassword").removeClass("error");
    } else {
        $("#txtPassword").addClass("error");
    }

    if (confirmPass_IsValid) {
        $("#txtConfirmPassword").removeClass("error");
    } else {
        $("#txtConfirmPassword").addClass("error");
    }

    var ret = newPass_IsValid & confirmPass_IsValid;
    if (ret) {
        $("#errorUpdatePass").addClass("hidden");
    }
    else {
        $("#errorUpdatePass").removeClass("hidden");
    }

    return ret;
};

//--------------------------------------------------------------------------------------------------------------------

function CheckValidation_Step3() {
    $("#success-message-box").hide();

    var ret = true;
    $("#step3 .error-message-box").addClass("hidden");
    //    if ($('.chkPerSettings:checked').length == 0) {
    //        ret = false;
    //        $("#permissionSettingsReqErr").removeClass("hidden");
    //    } else {
    //        $("#permissionSettingsReqErr").addClass("hidden");
    //    }

    if (!ret) {
        $("#editStep1 .form").addClass("error");
        $("#errorStep3").removeClass("hidden");
    } else {
        $("#editStep1 .form").removeClass("error");
        $("#errorStep3").addClass("hidden");
    }

    return ret;
};

//--------------------------------------------------------------------------------------------------------------------

function CheckValidation_EditInfo() {
    $("#editStep1 .result-message-box").addClass("hidden");
    var ret = true;
    var validateRegex = OddsFeedAdminManager.Enums.ValidateRegex;

    //validate email
    $("#emailReqErr").hide();
    $("#emailInvalidErr").hide();
    var email = $.trim($("#txtEmail").val());
    var isValid_Email = true;
    if (email == "") {
        $("#emailReqErr").show();
        isValid_Email = false;
    }
    else if (!validateRegex.EmailField.test(email)) {
        $("#emailInvalidErr").show();
        isValid_Email = false;
    }

    if (isValid_Email) {
        $("#txtEmail").removeClass("error");
    } else {
        $("#txtEmail").addClass("error");
    }

    var ret = isValid_Email;
    if (ret) {
        $("#errorStep1").addClass("hidden");
    } else {
        $("#errorStep1").removeClass("hidden");
    }

    return ret;
}

function UpdateSubAccountInfo() {
    $("#editStep1 .result-message-box").addClass("hidden");
    if (!CheckValidation_EditInfo()) {
        return;
    }

    var updateParams = {
        UserName: $.trim($("#txtUsername").val()),
        SubAccountID: $("#SubAccountID").val(),
        FirstName: $.trim($("#txtFirstName").val()),
        LastName: $.trim($("#txtLastName").val()),
        Email: $.trim($("#txtEmail").val()),
        IsActive: $("#chkIsActive").prop("checked") ? true : false
    };

    $.ajax({
        type: 'POST',
        url: 'EditSubAccInfo',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(updateParams),
        cache: false,
        beforeSend: function (xhr) {
        },
        success: function (result) {
            if (result && result.IsSuccess) {
                $("#editStep1 .result-message-box").html(" <ul><li>" + viewResources.ActionResult_Success_UpdateSubAccount + "</li></ul>")
                                               .addClass("success-message-box")
                                               .removeClass("hidden");

                confirm('Successfull!\nPress OK to continue', function () {
                    document.location = currentDomainURL + "/SubAccount/List";
                });
            } else {
                $("#editStep1 .result-message-box").html(" <ul><li>" + result + "</li></ul>")
                                               .addClass("error-message-box")
                                               .removeClass("hidden");
            }
        },
        error: function (xhr, status, err) {
        }
    });
};

function DoUpdatePassword() {
    $(".result-message-box").addClass("hidden");

    var isParamsValid = CheckValidation();
    if (!isParamsValid) {
        return;
    }

    var updateParams = {
        SubAccountID: $("#SubAccountID").val(),
        Password: $.trim($("#txtPassword").val()),
        ConfirmPassword: $.trim($("#txtConfirmPassword").val())
    };

    $.ajax({
        url: 'ResetPassword',
        type: 'POST',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(updateParams),
        cache: false,
        beforeSend: function (xhr) {
        },
        success: function (result) {
            var success = result != null && result.IsSuccess == true;
            OddsFeedAdminManager.informActionResult(result, $(".result-message-box"));
            if (success) {
                OddsFeedAdminManager.confirmAfterSuccess(currentDomainURL + "/SubAccount/List");
            }
        },
        error: function (xhr, status, err) {
        }
    });
};

function CheckValidation_AssignLicensee() {
    var ret = true;
    $("#success-message-box").addClass("hidden");

    // validate licensee list
    if ($('.chkSystem:checked').length > 0) {
        $("#licenseeRequiredErr").addClass("hidden");
    } else {
        ret = false;
        $("#licenseeRequiredErr").removeClass("hidden");
    }

    if (ret) {
        $("#errorStep1").addClass("hidden");
        $(".licenseeList").removeClass("error");
    } else {
        $("#errorStep1").removeClass("hidden");
        $(".licenseeList").addClass("error");
    }

    return ret;
}


