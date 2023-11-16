$(document).ready(function () {
    $("#editStep1Save").click(function () {
        var updateParams = {
            PartnerID: $("#PartnerID").val(),
            UserName: $.trim($("#txtUsername").val()),
            Email: $.trim($("#txtEmail").val()),
            Website: $.trim($("#txtWebsite").val()),
            Note: $.trim($("#txtNote").val())
        };

        var paramsIsValid = CheckValidation_EditInfo(updateParams);
        if (!paramsIsValid) {
            return;
        }

        var url = currentDomainURL + "Affiliate/EditAffiliateInfo?PartnerID=" + updateParams.PartnerID +
            "&UserName=" + updateParams.UserName +
            "&Email=" + updateParams.Email +
            "&Website=" + updateParams.Website +
            "&Note=" + updateParams.Note

        $.ajax({
            url: url,
            type: 'POST',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            cache: false,
            beforeSend: function (xhr) {
            },
            success: function (result) {
                if (result && result.isSuccessful) {
                    $("#editStep1 .result-message-box").empty()
                        .append("<ul><li>" + result.message + "</li></ul>")
                        .addClass("success-message-box")
                        .removeClass("error-message-box")
                        .removeClass("hidden");

                    OddsFeedAdminManager.confirmAfterSuccess(currentDomainURL + "Affiliate/List");
                } else {
                    $("#editStep1 .result-message-box").empty()
                        .append("<ul><li>" + result.message + "</li></ul>")
                        .addClass("error-message-box")
                        .removeClass("success-message-box")
                        .removeClass("hidden");
                }
            },
            error: function (xhr, status, err) {
            }
        });
    });
});

function CheckValidation_EditInfo(updateParams) {
    var errorMessages = [];
    var validateRegex = OddsFeedAdminManager.Enums.ValidateRegex;

    //validate name
    var isUserNameValid = true;
    if ($.trim(updateParams.UserName) == "") {
        errorMessages.push("<li>" + viewResources.Error_NameRequired + "</li>");
        isUserNameValid = false;
    }
    else if (updateParams.UserName.length < 2 || 50 < updateParams.UserName.length) {
        errorMessages.push("<li>" + viewResources.Error_NameLength + "</li>");
        isUserNameValid = false;
    }
    else if (!validateRegex.NameField.test(updateParams.UserName)) {
        errorMessages.push("<li>" + viewResources.Error_NameLength + "</li>");
        isUserNameValid = false;
    }

    //validate email
    var isEmailValid = true;
    var email = updateParams.Email;
    if ($.trim(email) == "") {
        errorMessages.push("<li>" + viewResources.Error_EmailRequired + "</li>");
        isEmailValid = false;
    }
    else if (!validateRegex.EmailField.test(email)) {
        errorMessages.push("<li>" + viewResources.Error_EmailContent + "</li>");
        isEmailValid = false;
    }

    //validate website
    var isWebsiteValid = true;
    var website = updateParams.Website;
    if ($.trim(website) == "") {
        errorMessages.push("<li>" + viewResources.Error_WebsiteRequired + "</li>");
        isWebsiteValid = false;
    } else if (!validateRegex.WebsiteField.test(website)) {
        errorMessages.push("<li>" + viewResources.Error_WebsiteContent + "</li>");
        isWebsiteValid = false;
    }

    if (isEmailValid) {
        $("#txtEmail").removeClass("error");
    } else {
        $("#txtEmail").addClass("error");
    }

    if (isWebsiteValid) {
        $("#txtWebsite").removeClass("error");
    } else {
        $("#txtWebsite").addClass("error");
    }

    var ret = isUserNameValid && isEmailValid && isWebsiteValid;
    if (ret) {
        $("#editStep1 .result-message-box").addClass("hidden")
                                           .empty()
                                           .removeClass("error-message-box")
                                           .removeClass("success-message-box");
    } else {
        $("#editStep1 .result-message-box").empty()
                                           .append("<ul>" + errorMessages.join("") + "</ul>")
                                           .addClass("validation-summary-errors")
                                           .removeClass("success-message-box")
                                           .removeClass("hidden");
    }

    return ret;
}