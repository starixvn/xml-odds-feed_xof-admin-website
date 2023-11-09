$(document).ready(function () {
    //    //step 1
    $('#txtUsername').prop('disabled', true);
    var successed = $('#successed').val();
    if (successed == 'true') {
        confirm('Successfull!\nPress OK to continue', function () {
            document.location = currentDomainURL + 'Administrator/AdministratorList';
        });
    }

    //Sport checkboxes
    //$('[name=chkSystem]').attr('checked', true); //check all sports when adding new aff
    //$('#chkAllSystem').attr('checked', true);
    if ($('.chkSystem:checked').length == $('.chkSystem').length) {
        $('#chkAllSystem').attr('checked', true);
    }

    $('#chkAllSystem').click(function () {
        if ($('#chkAllSystem').is(':checked')) {
            $('.chkSystem').each(function () { this.checked = true; });
        }
        else {
            $('.chkSystem').each(function () { this.checked = false; });
        }
    });

    $('.chkSystem').click(function () {
        if ($('.chkSystem:checked').length == $('.chkSystem').length) {
            $('#chkAllSystem').attr('checked', true);
        }
        else {
            $('#chkAllSystem').attr('checked', false);
        }
    });

    $('#btnSave').click(function () {
        $('#success-message-box').hide();
        UpdateAdministratorInfo();
    });

    $('#Save.resetPassword').click(function () {
        ResetPassword();
    });
    
    $('#search-licensee-txt').on('keyup', function () {
        const searchKey = $('#search-licensee-txt').val().trim().toLowerCase();

        $('div.licensee-list > div').addClass('hidden');
        const labelElements = $('div.licensee-list label');
        if (labelElements.length > 0) {
            labelElements.each(function () {
                const labelText = $(this).text().toLowerCase();
                if (labelText.includes(searchKey)) {
                    $(this).parent().removeClass('hidden');
                }
            });
        }
    });
});

//--------------------------------------------------------------------------------------------------------------------

function editActive(obj, user) {
    if ($(obj).hasClass('ico-tick')) {
        var temp = 'Do you want to deactivate administrator <b>\"' + user.username + '\"</b>?';
        confirm(temp, function () {
            $(obj).removeClass('ico-tick').addClass('ico-untick');

            var url = 'UpdateActive?adminid=' + user.id + '&isactive=false';

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
                        document.location = currentDomainURL + 'Administrator/AdministratorList';
                    }
                    else {
                        alert(result);
                    }
                },
                error: function (xhr, status, err) {
                }
            });
        });
    } else {
        var temp = 'Do you want to activate administrator <b>\"' + user.username + '\"</b>?';
        confirm(temp, function () {
            $(obj).removeClass('ico-untick').addClass('ico-tick');
            var url = 'UpdateActive?adminid=' + user.id + '&isactive=true';

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
                        document.location = currentDomainURL + 'Administrator/AdministratorList';
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

function UpdateInfo() {
    if (CheckValidation()) {
        //step1
        var id = $('#txtid').val();
        var name = $.trim($('#txtUsername').val());
        var fname = $.trim($('#txtFirstname').val());
        var lname = $.trim($('#txtLastname').val());
        var email = $.trim($('#txtEmail').val());
        var roleid = $('input:radio[name=role]:checked').val();
        var url = 'UpdateInfo?id=' + id + '&username=' + name + '&fname=' + fname + '&lname=' + lname + '&email=' + email + '&role=' + roleid;

        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'html',
            cache: false,
            success: function (result) {
                result = result.toLowerCase();

                if (result == 'successfully!') {
                    confirm('Successfull!\nPress OK to continue', function () {
                        document.location = currentDomainURL + 'Administrator/AdministratorList';
                    });
                }
                else {
                    alert(result);
                }
            },
            error: function (xhr, status, err) {
                alert('d');
            }
        });
    }
}

function CheckValidation() {
    var ret = true;
    var validateRegex = OddsFeedAdminManager.Enums.ValidateRegex;

    //validate name
    $('#nameReqErr').hide();
    $('#nameLengErr').hide();
    $('#nameInvalidErr').hide();
    var name = $.trim($('#txtUsername').val());
    if (name == '') {
        $('#nameReqErr').show();
        ret = false;
    }
    else if (name.length > 50 || name.length < 2) {
        $('#nameLengErr').show();
        ret = false;
    }
    else if (!validateRegex.NameField.test(name)) {
        $('#nameInvalidErr').show();
        ret = false;
    }

    //validate email
    var email = $.trim($('#txtEmail').val());
    $('#emailReqErr').hide();
    $('#emailInvalidErr').hide();
    if (email == '') {
        $('#emailReqErr').show();
        ret = false;
    }
    else if (!validateRegex.EmailField.test(email)) {
        $('#emailInvalidErr').show();
        ret = false;
    }

    return ret;
}

//--------------------------------------------------------------------------------------------------------------------

function CheckValidation_ResetPassword() {
    $('#success-message-box').hide();

    //validate newpass
    $('#newPassReqErr').hide();
    $('#newPassWhiteSpaceErr').hide();
    $('#newPassLengErr').hide();
    var newpass = $.trim($('#txtPassword').val());
    var newPass_IsValid = true;
    if (newpass == '') {
        $('#newPassReqErr').show();
        newPass_IsValid = false;
    }
    //    else if (newpass.indexOf(' ') != -1) {
    //        $('#newPassWhiteSpaceErr').show();
    //        newPass_IsValid = false;
    //    }
    else if (newpass.length > 50 || newpass.length < 6) {
        $('#newPassLengErr').show();
        newPass_IsValid = false;
    }

    //validate confirm pass
    $('#confirmPassReqErr').hide();
    $('#confirmPassWhiteSpaceErr').hide();
    $('#confirmPassLengErr').hide();
    $('#pwdNotMatchErr').hide();
    var confirmpass = $.trim($('#txtConfirmPassword').val());
    var confirmPass_IsValid = true;
    if (confirmpass == '') {
        $('#confirmPassReqErr').show();
        confirmPass_IsValid = false;
    }
    //    else if (confirmpass.indexOf(' ') != -1) {
    //        $('#confirmPassWhiteSpaceErr').show();
    //        confirmPass_IsValid = false;
    //    }
    else if (confirmpass.length > 50 || confirmpass.length < 6) {
        $('#confirmPassLengErr').show();
        confirmPass_IsValid = false;
    } else if (newpass != confirmpass) {
        $('#pwdNotMatchErr').show();
        confirmPass_IsValid = false;
        newPass_IsValid = false;
    }

    //Hightlight error
    $('#editStep1 .result-message-box').addClass('hidden');
    if (newPass_IsValid) {
        $('#txtPassword').removeClass('error');
    } else {
        $('#txtPassword').addClass('error');
    }

    if (confirmPass_IsValid) {
        $('#txtConfirmPassword').removeClass('error');
    } else {
        $('#txtConfirmPassword').addClass('error');
    }

    var ret = newPass_IsValid && confirmPass_IsValid;
    if (ret) {
        $('#errorStep1').addClass('hidden');
    }
    else {
        $('#errorStep1').removeClass('hidden');
    }

    return ret;
};

function CheckValidation_EditInfo() {
    $('#editStep1 .result-message-box').addClass('hidden');
    var validateRegex = OddsFeedAdminManager.Enums.ValidateRegex;

    //validate email
    $('#emailReqErr').hide();
    $('#emailInvalidErr').hide();
    var email = $.trim($('#txtEmail').val());
    var isValid_Email = true;
    if (email == '') {
        $('#emailReqErr').show();
        isValid_Email = false;
    }
    else if (!validateRegex.EmailField.test(email)) {
        $('#emailInvalidErr').show();
        isValid_Email = false;
    }

    if (isValid_Email) {
        $('#txtEmail').removeClass('error');
    } else {
        $('#txtEmail').addClass('error');
    }

    var ret = isValid_Email;
    if (ret) {
        $('#editStep1 #errorStep1').addClass('hidden');
    } else {
        $('#editStep1 #errorStep1').removeClass('hidden');
    }

    return ret;
}

function UpdateAdministratorInfo() {
    $('#editStep1 .result-message-box').addClass('hidden');
    if (!CheckValidation_EditInfo()) {
        return;
    }

    var updateParams = {
        Username: $.trim($('#txtUsername').val()),
        UserID: $('#txtid').val(),
        FirstName: $.trim($('#txtFirstname').val()),
        LastName: $.trim($('#txtLastname').val()),
        Email: $.trim($('#txtEmail').val()),
        RoleID: $('.administratorRole').length > 0 ? $('.administratorRole:checked').val() : 3 //SubAcc's role = 3
    };

    $.ajax({
        type: 'POST',
        url: 'AdministratorEditProfile',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(updateParams),
        cache: false,
        beforeSend: function (xhr) {
        },
        success: function (result) {
            if (result && result.IsSuccessful) {
                $('#editStep1 .result-message-box')
                    .html("<ul><li>The Administrator's information has been updated successfully!</li></ul>")
                    .addClass('success-message-box')
                    .removeClass('error-message-box')
                    .removeClass('hidden');

                if (currentSessionUserId != updateParams.UserID ||
                    updateParams.RoleID == OddsFeedAdminManager.Enums.UserRole.ITAdministrator) {
                    confirm('Successfull!\nPress OK to continue', function () {
                        document.location = currentDomainURL + 'Administrator/AdministratorList';
                    });
                } else {
                    document.location = currentDomainURL + 'Dashboard/ShowDashboard';
                }
            } else {
                $('#editStep1 .result-message-box')
                    .html('<ul><li>' + result.Message + '</li></ul>')
                    .addClass('success-message-box')
                    .removeClass('error-message-box')
                    .removeClass('hidden');
            }
        },
        error: function (xhr, status, err) {
        }
    });
};

function ResetPassword() {
    var paramsIsValid = CheckValidation_ResetPassword();
    if (!paramsIsValid) {
        return;
    }

    var updateParams = {
        UserID: $('#UserID').val(),
        Password: $.trim($('#txtPassword').val()),
        ConfirmPassword: $.trim($('#txtConfirmPassword').val())
    };

    $.ajax({
        type: 'POST',
        url: 'AdministratorResetPwd',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(updateParams),
        cache: false,
        beforeSend: function (xhr) {
        },
        success: function (result) {
            if (result && result.IsSuccessful) {
                $('#editStep1 .result-message-box').empty()
                    .append('<ul><li>' + result.Message + '</li></ul>')
                    .addClass('success-message-box')
                    .removeClass('error-message-box')
                    .removeClass('hidden');
                OddsFeedAdminManager.confirmAfterSuccess(currentDomainURL + 'Administrator/AdministratorList');
            } else {
                $('#editStep1 .result-message-box').empty()
                    .append('<ul><li>' + result.Message + '</li></ul>')
                    .addClass('error-message-box')
                    .removeClass('success-message-box')
                    .removeClass('hidden');
            }
        },
        error: function (xhr, status, err) {
        }
    });
};

function CheckValidation_AssignLicensee() {
    var ret = true;
    $('#success-message-box').addClass('hidden');

    // validate licensee list
    if ($('.chkSystem:checked').length > 0) {
        $('#licenseeRequiredErr').addClass('hidden');
    } else {
        ret = false;
        $('#licenseeRequiredErr').removeClass('hidden');
    }

    if (ret) {
        $('#errorStep1').addClass('hidden');
        $('.licenseeList').removeClass('error');
    } else {
        $('#errorStep1').removeClass('hidden');
        $('.licenseeList').addClass('error');
    }

    return ret;
}