$(document).ready(function () {
    //reset
    //    $('.content_scroll input[type="text"]').val('');
    //    $('.content_scroll input[type="checkbox"]').attr("checked", false);
    //    $('.content_scroll  textarea').val('');

    //step1
    $('#step1 input[type="text"]').val('');
    $('#step1  textarea').val('');

    //step2
    $('#step2 input[type="text"]').val('');

    //cancel
    $(".cancel").click(function () {
        confirm('Are you sure to cancel adding new administrator?', function () {
            document.location = currentDomainURL + "Administrator/AdministratorList";
        });
    });
    //cancel
    $("#btnImport").click(function () {
        SearchExistedAdmin();
    });

    // watermark input
    jQuery(function ($) {
        $("#txtUsernameImport").Watermark("Username");
    });
});
//--------------------------------------------------------------------------------------------------------------------
function SearchExistedAdmin() {

    $("#success-message-box").addClass("hidden");
    $("#step1 .error-message-box").addClass("hidden");
    // get data
    var existedUsername = $("#txtUsernameImport").val();

    if (existedUsername == "") {
        $("#errorStep1").hide();
        $("#importUsnReqErr").show();
        $("#error-message-box").empty().html('<ul><li>Please input Username.</li></ul>').removeClass("hidden");
    }
    else {
        $("#errorStep1").hide();
        $("#importUsnReqErr").hide();

        // send back info to controllers
        var url = 'GetExistedAdmin?username=' + existedUsername;
        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'html',
            cache: false,
            success: function (result) {
                var adminInfo = result.split(',');
                if (adminInfo.length == 6) {
                    $("#txtUsername").val(adminInfo[0].toString());
                    $("#txtFirstname").val(adminInfo[1].toString());
                    $("#txtLastname").val(adminInfo[2].toString());
                    $("#txtEmail").val(adminInfo[3].toString());
                    $("#txtPwd").val(adminInfo[4].toString());
                    $("#txtConfirmPwd").val(adminInfo[4].toString());
                    $('input:radio[name=role]').filter('[value=' + adminInfo[5].toString() + ']').attr('checked', true);

                    // hide error message if any
                    // display message
                    $("#errorStep1").hide();
                    $("#importUsnNotExistedErr").hide();
                    $("#step1Cancel").prop('disabled', false);
                    $("#step1Next").prop('disabled', false);
                    $("#step1Done").prop('disabled', false);

                    //show email textbox if it is empty
                    if ($("#txtEmail").val() == "") {
                        $("#txtEmail").prop('disabled', false);
                    }

                    if (adminInfo[5] == "1") {// ITAdmin
                        $("#panelStep2").hide();
                        $("#step1Next").hide();
                        $("#step1Done").show();
                    }
                    else {
                        $("#panelStep2").show();
                        $("#step1Next").show();
                        $("#step1Done").hide();
                    }

                }
                else {
                    $("#txtUsername").val("");
                    $("#txtFirstname").val("");
                    $("#txtLastname").val("");
                    $("#txtEmail").val("");
                    $("#txtPwd").val("");
                    $("#txtConfirmPwd").val("");
                    $('input:radio[name=role]').filter('[value=2]').attr('checked', true); ;

                    // display message
                    $("#errorStep1").show();
                    $("#importUsnNotExistedErr").show();
                    $("#step1Cancel").prop('disabled', true);
                    $("#step1Next").prop('disabled', true);
                    $("#error-message-box").empty().html('<ul><li>' + result + '</li></ul>').removeClass("hidden");
                }
            },
            error: function (xhr, status, err) {
            }
        });
    }
}

function AddAdministrator() {
    //step1
    var name = $.trim($("#txtUsername").val());
    var pwd = $.trim($("#txtPwd").val());
    var fname = $.trim($("#txtFirstname").val());
    var lname = $.trim($("#txtLastname").val());
    var email = $.trim($("#txtEmail").val());
    var roleid = $('input:radio[name=role]:checked').val();
    var isimport = $('input:radio[name=newORexisted]:checked').val();

    //step2
    var system = "";
    $('.chkSystem').each(function () {
        if (this.checked) {
            system += $(this).val() + ",";
        }
    });

    var addParams = {
        UserName: name,
        Password: pwd,
        FirstName: fname,
        LastName: lname,
        Email: email,
        RoleID: roleid,
        System: system,
        IsImport: isimport == '2'
    };

    $.ajax({
        url: 'AddNew',
        type: 'POST',
        cache: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(addParams),
        success: function (result) {
            var roleid = $('input:radio[name=role]:checked').val();

            $("#step1 .error-message-box").addClass("hidden");
            $("#step2 .error-message-box").addClass("hidden");
            $("#step1 #success-message-box").addClass("hidden");
            $("#step2 #success-message-box").addClass("hidden");
            if (result == 'Successfully!') {
                if (roleid == 1) {
                    $("#step1 #success-message-box").empty()
                                                .append("<ul><li>The Administrator's information has been added successfully!</li></ul>")
                                                .removeClass("hidden");
                } else {
                    $("#step2 #success-message-box").empty()
                                                .append("<ul><li>The Administrator's information has been added successfully!</li></ul>")
                                                .removeClass("hidden");
                }

                confirm('Successfull!\nPress OK to continue', function () {
                    document.location = currentDomainURL + "Administrator/AdministratorList";
                });
            }
            else {
                if (roleid == 1) {
                    $("#step1 .error-message-box").empty()
                                              .append("<ul><li>" + result + "</li></ul>")
                                              .removeClass("hidden");
                } else {
                    $("#step2 .error-message-box").empty()
                                              .append("<ul><li>" + result + "</li></ul>")
                                              .removeClass("hidden");
                }
            }
        },
        error: function (xhr, status, err) {
        }
    });
};