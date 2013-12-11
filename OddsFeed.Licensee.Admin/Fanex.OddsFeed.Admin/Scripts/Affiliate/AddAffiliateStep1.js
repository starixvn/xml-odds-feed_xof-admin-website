$(document).ready(function () {
    //    //step 1    
    $("#errorStep1").addClass("hidden");

    $("#step1Next").click(function () {
        var ret = DoAdd_Affiliate();
    });
});
//---------------------------------------------------------------------------------------------
function CheckValidation() {
    var validateRegex = OddsFeedAdminManager.Enums.ValidateRegex;

    //validate name
    var name = $.trim($("#txtName").val());
    var isValid_Name = true;
    $("#nameReqErr").hide();
    $("#nameLengErr").hide();
    $("#nameInvalidErr").hide();
    if (name == "") {
        $("#nameReqErr").show();
        isValid_Name = false;
    }
    else if (name.length > 50 || name.length < 2) {
        $("#nameLengErr").show();
        isValid_Name = false;
    }
    else if (!validateRegex.NameField.test(name)) {
        $("#nameInvalidErr").show();
        isValid_Name = false;
    }

    //validate email
    var email = $.trim($("#txtEmail").val());
    $("#emailReqErr").hide();
    $("#emailInvalidErr").hide();
    var isValid_Email = true;
    if (email == "") {
        $("#emailReqErr").show();
        isValid_Email = false;
    }
    else if (!validateRegex.EmailField.test(email)) {
        $("#emailInvalidErr").show();
        isValid_Email = false;
    }

    //validate website
    var website = $.trim($("#txtWebsite").val());
    $("#websiteReqErr").hide();
    $("#websiteInvalidErr").hide();
    var isValid_Website = true;
    if (website == "") {
        $("#websiteReqErr").show();
        isValid_Website = false;
    } else if (!validateRegex.WebsiteField.test(website)) {
        $("#websiteInvalidErr").show();
        isValid_Website = false;
    }

    if (isValid_Name) {
        $("#txtName").removeClass("error");
    } else {
        $("#txtName").addClass("error");
    }

    if (isValid_Email) {
        $("#txtEmail").removeClass("error");
    } else {
        $("#txtEmail").addClass("error");
    }

    if (isValid_Website) {
        $("#txtWebsite").removeClass("error");
    } else {
        $("#txtWebsite").addClass("error");
    }

    var ret = isValid_Name && isValid_Email && isValid_Website;
    if (ret) {
        $("#errorStep1").addClass("hidden");
    }
    else {
        $("#errorStep1").removeClass("hidden");
    }

    return ret;
};

function DoAdd_Affiliate() {
    var addParams = {
        UserName: $.trim($("#txtName").val())
    };

    $.ajax({
        url: 'CheckExistAffiliate',
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
                if (result && result.IsSuccess) {
                    $("#nameExistedErr").hide('fast');
                    $('#txtName').removeClass('error');
                } else {
                    $("#nameExistedErr").show();
                    $('#txtName').addClass('error');
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
                    showingDomainList();
                }
            }
            else {
                $("#nameReqErr").hide();
                $("#nameLengErr").hide();
                $("#nameInvalidErr").hide();
                $("#emailReqErr").hide();
                $("#emailInvalidErr").hide();
                $("#websiteReqErr").hide();
                $("#websiteInvalidErr").hide();
                $("#txtName").removeClass("error");
                $("#txtEmail").removeClass("error");
                $("#txtWebsite").removeClass("error");

                $("#errorStep1").removeClass("hidden").show();
            }
        },
        error: function (xhr, status, err) {
        }
    });
};


