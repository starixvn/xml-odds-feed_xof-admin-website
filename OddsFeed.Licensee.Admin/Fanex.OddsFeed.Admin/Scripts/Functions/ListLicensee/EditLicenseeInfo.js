$(document).ready(function () {
    //reset
    //    $('.content_scroll input[type="text"]').val('');
    //    $('.content_scroll input[type="checkbox"]').attr("checked", false);
    //    $('.content_scroll  textarea').val('');

    $("#errorStep1").hide();

    $('#AddExistingLicensee').change(function () {
        if ($(this).is(':checked')) {
            $('.licensee-name').hide();
            $('.licensee-selection').show();
        } else {
            $('.licensee-name').show();
            $('.licensee-selection').hide();
        }
    });
    
    if ($('#AddExistingLicensee').is(':checked'))
        $('#AddExistingLicensee').change();

    //step1
    $('#step1 input[type="text"]').val('');
    $('#step1  textarea').val('');

    //cancel
    $(".cancel").click(function () {
        confirm('Are you sure to cancel adding new licensee?', function () {
            document.location = currentDomainURL + "/Licensee/ListLicensee";
        });
    });

    $("#editStep1Save").click(function () {
        var ret = CheckValidation();

        if (ret) {
            $.ajax({
                url: 'EditLicenseeInfoPost',
                type: 'POST',
                data: $('#formEditLicenseeInfo').serialize(),
                dataType: 'html',
                cache: false,
                success: function (errCode) {
                    if (errCode != 0) {
                        GetErrorMessage(errCode);
                    }
                    else {
                        OddsFeedAdminManager.informActionResult({
                            IsSuccess: true,
                            ResultMessage: "The licensee's information has been updated successfully!"
                        }, $(".result-message-box"));
                        OddsFeedAdminManager.confirmAfterSuccess(currentDomainURL + "/Licensee/ListLicensee");
                    }            
                },
                error: function (xhr, status, err) {
                }
            });
        }
        else {
            $("#errorStep1").show();
        }
    });
});
//--------------------------------------------------------------------------------------------------------------------

function CheckValidation() {
    
    var validateRegex = OddsFeedAdminManager.Enums.ValidateRegex;

    //validate name
    var name = $("#SystemName").val();
    name = $.trim(name);
    if (name == "" || name == undefined) {
        //$("#nameReqErr").show('fast');
        $("#errorStep1").append("<ul><li>" + viewResources.Error_LicenseeNameRequired + "</li></ul>");
        $('#errorStep1').removeClass('hidden');
        $('#SystemName').addClass('error');
        return false;
    }

    if (name.length > 50 || name.length < 2) {
        //$("#nameLengErr").show('fast');
        $("#errorStep1").append("<ul><li>" + viewResources.Error_LicenseeNameLength + "</li></ul>");
        $('#errorStep1').removeClass('hidden');
        $('#SystemName').addClass('error');
        return false;
    }

    if (!validateRegex.NameField.test(name)) {
        //        $("#nameInvalidErr").show('fast');
        $("#errorStep1").append("<ul><li>" + viewResources.Error_LicenseeNameContent + "</li></ul>");
        $('#errorStep1').removeClass('hidden');
        $('#SystemName').addClass('error');
        return false;
    }

    //validate contact email
    var contactEmail = $("#ContactEmail").val();
    contactEmail = $.trim(contactEmail);
    if (contactEmail == "" || contactEmail == undefined) {
        $("#errorStep1").append("<ul><li>" + viewResources.Error_EmailRequired + "</li></ul>");
        $('#errorStep1').removeClass('hidden');
        $('#ContactEmail').addClass('error');
        return false;
    }

    if (!validateRegex.EmailField.test(contactEmail)) {
        $("#errorStep1").append("<ul><li>" + viewResources.Error_EmailContent + "</li></ul>");
        $('#errorStep1').removeClass('hidden');
        $('#ContactEmail').addClass('error');
        return false;
    }

    return true;
}