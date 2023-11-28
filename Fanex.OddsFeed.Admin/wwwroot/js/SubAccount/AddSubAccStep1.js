$(document).ready(function () {
    //step 1
    $("#step1Next").click(function () {
        $("#success-message-box").hide();
        var ret = DoAdd_SubAccount();
    });

});
//---------------------------------------------------------------------------------------------
function CheckValidation() {
    $("#step1 .result-message-box").addClass("hidden");
    var validateRegex = OddsFeedAdminManager.Enums.ValidateRegex;

    // validate name
    $("#nameReqErr").hide();
    $("#nameLengErr").hide();
    $("#nameInvalidErr").hide();
    var name = $.trim($("#txtUserName").val());
    var isValid_Name = true;
    if (name == "") {
        $("#nameReqErr").show();
        isValid_Name = false;
    } else if (name.length > 50 || name.length < 2) {
        $("#nameLengErr").show();
        isValid_Name = false;
    } else if (!validateRegex.NameField.test(name)) {
        $("#nameInvalidErr").show();
        isValid_Name = false;
    }

    // validate password
    var pwd = $("#txtPassword").val();
    var cpwd = $("#txtConfirmPwd").val();
    var isValid_Pass = true;
    var isValid_ConfirmPass = true;
    $("#passReqErr").hide();
    $("#passLengErr").hide();
    $("#pwdNotMatchErr").hide();
    $("#passWhiteSpaceErr").hide();
    $("#confirmPassWhiteSpaceErr").hide();
    $("#confirmPassReqErr").hide();
    if ($.trim(pwd) == "") {
        $("#passReqErr").show();
        isValid_Pass = false;
    } else if (pwd.length > 50 || pwd.length < 4) {
        $("#passReqErr").show();
        isValid_Pass = false;
    }

    if ($.trim(cpwd) == "") {
        $("#confirmPassReqErr").show();
        isValid_ConfirmPass = false;
    } else if (pwd != cpwd) {
        $("#pwdNotMatchErr").show();
        isValid_Pass = false;
        isValid_ConfirmPass = false;
    }

    // validate email
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

    if (isValid_Name) {
        $("#txtUserName").removeClass("error");
    } else {
        $("#txtUserName").addClass("error");
    }

    if (isValid_Pass) {
        $("#txtPassword").removeClass("error");
    } else {
        $("#txtPassword").addClass("error");
    }

    if (isValid_ConfirmPass) {
        $("#txtConfirmPwd").removeClass("error");
    } else {
        $("#txtConfirmPwd").addClass("error");
    }

    if (isValid_Email) {
        $("#txtEmail").removeClass("error");
    } else {
        $("#txtEmail").addClass("error");
    }

    var ret = isValid_Name && isValid_Pass && isValid_ConfirmPass && isValid_Email;
    if (ret) {
        $("#errorStep1").addClass("hidden");
    }

    return ret;
}

function DoAdd_SubAccount() {
    var addParams = {
        UserName: $.trim($("#txtUserName").val())
    };

    $.ajax({
        url: 'CheckExistSubAccount',
        type: 'POST',
        cache: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(addParams),
        async: true,
        cache: false,
        success: function (result) {
            var ret = true;
            if (addParams.UserName != "") {
                if (result.isSuccessful) {
                    $("#nameExistedErr").hide('fast');
                    $('#txtUserName').removeClass('error');
                } else {
                    $("#nameExistedErr").show();
                    $('#txtUserName').addClass('error');
                    $("#errorStep1").removeClass("hidden");
                    ret = false;
                }
            } else {
                $("#nameExistedErr").hide('fast');
            }

            if (ret) {
                var isValid = CheckValidation();
                if (isValid) {
                    $("div.step-active").attr("class", "step-inactive");
                    $("#panelStep2").attr("class", "step-active");
                    $("div.step").hide();
                    $("#step2").show();
                }
                else {
                    $("#errorStep1").removeClass("hidden");
                }
            }
            else {
                $("#nameReqErr").hide();
                $("#nameLengErr").hide();
                $("#nameInvalidErr").hide();
                $("#passReqErr").hide();
                $("#passLengErr").hide();
                $("#pwdNotMatchErr").hide();
                $("#passWhiteSpaceErr").hide();
                $("#confirmPassWhiteSpaceErr").hide();
                $("#confirmPassReqErr").hide();
                $("#emailReqErr").hide();
                $("#emailInvalidErr").hide();
                $("#txtUserName").removeClass("error");
                $("#txtPassword").removeClass("error");
                $("#txtConfirmPwd").removeClass("error");
                $("#txtEmail").removeClass("error");

                $("#errorStep1").removeClass("hidden");
            }
        },
        error: function (xhr, status, err) {
        }
    });
};
