$(document).ready(function () {
    // step 1
    $('#errorStep1').addClass('hidden');

    $('#errorStep1').addClass('hidden');
    $('#SystemName').removeClass('error');
    $('.licensee-name').hide();
    $('.licensee-selection').show();

    $('#SystemName').focusout(function () {
        RefreshErrorBox();
        if (CheckSystemNameValidation()) {
            if ($('#errorStep1').is(':visible')) {
                $('#errorStep1').addClass('hidden');
            }
        } else {
            if (!$('#errorStep1').is(':visible')) {
                $('#errorStep1').removeClass('hidden');
            }
        }
    });

    $('#ContactEmail').focusout(function () {
        RefreshErrorBox();
        if (CheckEmailValidation()) {
            if ($('#errorStep1').is(':visible')) {
                $('#errorStep1').addClass('hidden');
            }
        } else {
            if (!$('#errorStep1').is(':visible')) {
                $('#errorStep1').removeClass('hidden');
            }
        }
    });

    $('#step1Next').click(function () {
        $('#SystemName').val($('#LicenseeID>option:selected').text());

        var ret = CheckValidation();

        if (ret) {
            $('#errorStep1').addClass('hidden');
            $('div.step-active').attr('class', 'step-inactive licensee-info-settings');
            $('#panelStep2').attr('class', 'step-active');

            $('div.step').hide();
            $('#step2').show();

            showingDomainList();
        }
        else {
            $('#errorStep1').removeClass('hidden');
        }
    });

});
//---------------------------------------------------------------------------------------------

function RefreshErrorBox() {
    // Refresh error box
    $('#errorStep1').addClass('hidden');
    $('#errorStep1').html('');
    $('.error').removeClass('error');
}

function CheckValidation() {
    RefreshErrorBox();

    var res1 = CheckSystemNameValidation();
    var res2 = CheckEmailValidation();
    return res1 && res2;
}

//---------------------------------------------------------------------------------------------
function CheckSystemNameValidation() {
    res = true;
    var validateRegex = OddsFeedAdminManager.Enums.ValidateRegex;

    //validate name
    var name = $('#SystemName').val();
    name = $.trim(name);
    if (name == '' || name == undefined) {
        $('#errorStep1').append('<ul><li>' + viewResources.Error_LicenseeNameRequired + '</li></ul>');
        $('#errorStep1').removeClass('hidden');
        $('#SystemName').addClass('error');
        return false;
    }

    if (name.length > 50 || name.length < 2) {
        $('#errorStep1').append('<ul><li>' + viewResources.Error_LicenseeNameLength + '</li></ul>');
        $('#errorStep1').removeClass('hidden');
        $('#SystemName').addClass('error');
        return false;
    }

    if (!validateRegex.NameField.test(name)) {
        $('#errorStep1').append('<ul><li>' + viewResources.Error_LicenseeNameContent + '</li></ul>');
        $('#errorStep1').removeClass('hidden');
        $('#SystemName').addClass('error');
        return false;
    }

    return res;
}

function CheckEmailValidation() {
    var validateRegex = OddsFeedAdminManager.Enums.ValidateRegex;

    var contactEmail = $('#ContactEmail').val();
    contactEmail = $.trim(contactEmail);
    if (contactEmail == '' || contactEmail == undefined) {
        $('#errorStep1').append('<ul><li>' + viewResources.Error_EmailRequired + '</li></ul>');
        $('#errorStep1').removeClass('hidden');
        $('#ContactEmail').addClass('error');
        return false;
    }

    if (!validateRegex.EmailField.test(contactEmail)) {
        $('#errorStep1').append('<ul><li>' + viewResources.Error_EmailContent + '</li></ul>');
        $('#errorStep1').removeClass('hidden');
        $('#ContactEmail').addClass('error');
        return false;
    }

    return true;
}