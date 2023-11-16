var _focusFields = '';

$(document).ready(function () {
    $("#mainForm input").prop('disabled', false);

    //    //step 1
    $("#step1Next").click(function () {
        var ret = DoAdd_Administrator(false);
    });

    $("#step1Done").click(function () {
        var ret = DoAdd_Administrator(true);
    });

    // add event handler for addNew/Import radio button
    $("#subForm input[type='radio']").click(function () {
        // hide all previous's error message
        $("#errorStep1").hide();
        $("#txtUsername").val("");
        $("#txtFirstname").val("");
        $("#txtLastname").val("");
        $("#txtEmail").val("");
        $("#txtPwd").val("");
        $("#txtConfirmPwd").val("");
        $(_focusFields).removeClass("error");

        if (this.checked && $(this).val() == '1') // add New
        {
            $("#mainForm input").prop('disabled', false);
            $("#searchToImport").hide();
        } else // import
        {
            $("#mainForm input").prop('disabled', true);
            $("#searchToImport").show();
        }
    });

    // add event handler for ITAdmin/Licensee radio button
    $("#mainForm input[type='radio']").click(function () {
        if (this.checked && $(this).val() == '1') // ITAdmin
        {
            $("#panelStep2").hide();
            $("#step1Next").hide();
            $("#step1Done").show();
        } else // Licensee
        {
            $("#panelStep2").show();
            $("#step1Next").show();
            $("#step1Done").hide();
        }
    });

});

function StartAutoSuggesttion() {
    // clear the watermark
    $.Watermark.HideAll();

    var temp = $("#txtUsernameImport").val()
    if (temp.length >= 2) {
        var url = 'ListExistedAdmin?username=' + temp;
        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'html',
            cache: false,
            success: function (result) {
                var sourceAdmin = result.split(',');
                $("#txtUsernameImport").autocomplete({
                    source: sourceAdmin,
                    minLength: 2
                });
            },
            error: function (xhr, status, err) {
            }
        });
    }
}

//---------------------------------------------------------------------------------------------
function ValidateParams_AddAdministrator() {
    if (_focusFields != '') {
        _focusFields.removeClass("error");
    }

    _focusFields = '';
    var ret = true;
    var focusFields = [];
    var validateRegex = OddsFeedAdminManager.Enums.ValidateRegex;

    //validate name
    $("#nameReqErr").hide();
    $("#nameLengErr").hide();
    $("#nameInvalidErr").hide();
    var name = $.trim($("#txtUsername").val());
    if (name == "") {
        $("#nameReqErr").show();
        focusFields.push("#txtUsername");
        ret = false;
    } else if (name.length > 50 || name.length < 2) {
        $("#nameLengErr").show();
        focusFields.push("#txtUsername");
        ret = false;
    } else if (!validateRegex.NameField.test(name)) {
        $("#nameInvalidErr").show();
        focusFields.push("#txtUsername");
        ret = false;
    }

    //validate email
    $("#emailReqErr").hide();
    $("#emailInvalidErr").hide();
    var email = $.trim($("#txtEmail").val());
    if (email == "") {
        $("#emailReqErr").show();
        focusFields.push("#txtEmail");
        ret = false;
    } else if (!validateRegex.EmailField.test(email)) {
        $("#emailInvalidErr").show();
        focusFields.push("#txtEmail");
        ret = false;
    }

    //validate password    
    $("#pwdReqErr").hide();
    $("#pwdLengErr").hide();
    $("#pwdContentErr").hide();
    $("#pwdNotMatchErr").hide();

    var pwd = $("#txtPwd").val();
    var cpwd = $("#txtConfirmPwd").val();
    if ($.trim(pwd) == "") {
        $("#pwdReqErr").show();
        focusFields.push("#txtPwd");
        ret = false;
    }
    else if (pwd.length > 50 || pwd.length < 6) {
        $("#pwdLengErr").show();
        focusFields.push("#txtPwd");
        ret = false;
    }

    //validate confirm password
    $("#confirmPwdReqErr").hide();
    $("#confirmPwdLengErr").hide();
    $("#confirmPwdContentErr").hide();

    if ($.trim(cpwd) == "") {
        $("#confirmPwdReqErr").show();
        focusFields.push("#txtConfirmPwd");
        ret = false;
    } else if (cpwd.length > 50 || cpwd.length < 6) {
        $("#confirmPwdLengErr").show();
        focusFields.push("#txtConfirmPwd");
        ret = false;
    } else if (pwd != cpwd) {
        $("#pwdNotMatchErr").show();
        focusFields.push("#txtPwd");
        ret = false;
    }

    _focusFields = $(focusFields.join(","));
    if (!ret) {
        _focusFields.addClass("error");
    }

    return ret;
};

function DoAdd_Administrator(isDone) {
    $("#success-message-box").addClass("hidden");
    var addParams = {
        Username: $.trim($("#txtUsername").val())
    };

    $.ajax({
        url: 'CheckExistAdministrator',
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
                if (result && result.isSuccessful) {
                    $("#nameExistedErr").hide('fast');
                    $('#txtUsername').removeClass('error');
                } else {
                    $("#nameExistedErr").show();
                    $('#txtUsername').addClass('error');
                    $("#errorStep1").removeClass("hidden");
                    ret = false;
                }
            } else {
                $("#nameExistedErr").hide('fast');
            }

            if (ret) {
                var isValid = ValidateParams_AddAdministrator();
                if (isValid) {
                    if (isDone) {
                        $("#errorStep1").hide();
                        AddAdministrator();
                    } else {
                        $("div.step-active").attr("class", "step-inactive");
                        $("#panelStep2").attr("class", "step-active");

                        $("div.step").hide();
                        $("#step2").show();
                        $("#errorStep1").hide();
                        $("#step2 .error-message-box").addClass("hidden");
                        $("#step2 #success-message-box").addClass("hidden");
                        $("#step2 #errorStep2").addClass("hidden");
                    }
                } else {
                    $("#errorStep1").removeClass("hidden").show();
                }
            } else {
                $("#nameReqErr").hide();
                $("#nameLengErr").hide();
                $("#nameInvalidErr").hide();
                $("#emailReqErr").hide();
                $("#emailInvalidErr").hide();
                $("#pwdReqErr").hide();
                $("#pwdLengErr").hide();
                $("#pwdContentErr").hide();
                $("#pwdNotMatchErr").hide();
                $("#confirmPwdReqErr").hide();
                $("#confirmPwdLengErr").hide();
                $("#confirmPwdContentErr").hide();
                if (_focusFields != '') {
                    _focusFields.removeClass("error");
                }

                $("#errorStep1").removeClass("hidden").show();
            }
        },
        error: function (xhr, status, err) {
        }
    });
}