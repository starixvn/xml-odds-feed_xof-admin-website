
function RegisterDialog() {
    // Register Create Dialog box
    $("#dialog-form").dialog({
        autoOpen: false,
        height: 'auto',
        width: 350,
        modal: true,
        buttons: {
            "OK": function () {
                var dialog = $(this);
                var bValid = true;
                $(".result-message-box", dialog).addClass("hidden");
                $(".ui-state-error", dialog).removeClass("ui-state-error error");
                if ($.trim($('#DomainName', dialog).val()) == "") {
                    $('#DomainName', dialog).addClass("ui-state-error error");
                    OddsFeedAdminManager.informActionResult({
                        IsSuccess: false,
                        ResultMessage: "The Domain Name field is required."
                    }, $(".result-message-box", dialog));

                    bValid = false;
                    return;
                }

                var txbIP = $('#IP', dialog);
                var txbIPVal = $.trim(txbIP.val());
                ipValid = txbIPVal == null ||
                          txbIPVal == '' ||
                          OddsFeedAdminManager.validateIPString(txbIPVal);
                if (!ipValid) {
                    txbIP.addClass("ui-state-error error");
                    OddsFeedAdminManager.informActionResult({
                        IsSuccess: false,
                        ResultMessage: "'" + txbIP.val() + "' is not a valid IP."
                    }, $(".result-message-box", dialog));

                    return;
                }

                bValid = ipValid && bValid;
                if (bValid) {
                    var addParams = {
                        DomainName: $.trim($("#DomainName", dialog).val()),
                        IP: $.trim($("#IP", dialog).val()),
                        ISP: $.trim($("#ISP", dialog).val())
                    };

                    $.ajax({
                        url: "CreateDomain",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(addParams),
                        success: function (errCode) {
                            if (errCode != 0) {
                                GetMessage_DomainList(errCode, dialog);
                            }
                            else {
                                dialog.dialog("close");
                                Submit();
                                $(".success-message-box").empty()
                                                         .append("<ul><li>" + viewResources.ActionResult_Success_AddDomain + "</li></ul>")
                                                         .removeClass("hidden");
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            dialog.dialog("close");
                            OddsFeedAdminManager.informActionResult({
                                IsSuccess: false,
                                ResultMessage: "500 internal server error."
                            }, $(".result-message-box"));
                        },
                        complete: function (jqXHR, textStatus) {
                        }
                    });
                }
            },
            Cancel: function () {
                var dialog = $(this);
                $(".result-message-box", dialog).addClass("hidden");
                $(".ui-state-error", dialog).removeClass("ui-state-error error");
                dialog.dialog("close");
            }
        },
        close: function () {
            $('input, select', $(this)).val("").removeClass("ui-state-error");
        }
    });

    // Register Update Dialog box
    $("#dialog-form-update").dialog({
        autoOpen: false,
        height: 'auto',        
        width: 350,
        modal: true,
        buttons: {
            "OK": function () {
                var dialog = $(this);
                var bValid = true;
                $(".result-message-box", dialog).addClass("hidden");
                $(".ui-state-error", dialog).removeClass("ui-state-error error");
                if ($.trim($('#DomainName', dialog).val()) == "") {
                    $('#DomainName', dialog).addClass("ui-state-error error");
                    OddsFeedAdminManager.informActionResult({
                        IsSuccess: false,
                        ResultMessage: "The Domain Name field is required."
                    }, $(".result-message-box", dialog));

                    bValid = false;
                    return;
                }

                var txbIP = $('#IP', dialog);
                var ipValid = true;
                if ($.trim(txbIP.val()) != null && $.trim(txbIP.val()) != '') {
                    ipValid = OddsFeedAdminManager.validateIPString(txbIP.val());
                }

                if (!ipValid) {
                    txbIP.addClass("ui-state-error error");
                    OddsFeedAdminManager.informActionResult({
                        IsSuccess: false,
                        ResultMessage: "'" + $.trim(txbIP.val()) + "' is not a valid IP."
                    }, $(".result-message-box", dialog));

                    return;
                }

                bValid = ipValid && bValid;
                if (bValid) {
                    var updateParams = {
                        DomainID: $("#DomainID", dialog).val(),
                        DomainName: $.trim($("#DomainName", dialog).val()),
                        IP: $.trim($("#IP", dialog).val()),
                        ISP: $.trim($("#ISP", dialog).val())
                    };

                    $.ajax({
                        url: "UpdateDomain",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(updateParams),
                        success: function (errCode) {
                            if (errCode != 0) {
                                GetMessage_DomainList(errCode, dialog);
                            }
                            else {
                                dialog.dialog("close");
                                Submit();
                                $(".success-message-box").empty()
                                                         .append("<ul><li>" + viewResources.ActionResult_Success_UpdateDomain + "</li></ul>")
                                                         .removeClass("hidden");
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            dialog.dialog("close");
                            OddsFeedAdminManager.informActionResult({
                                IsSuccess: false,
                                ResultMessage: "500 internal server error."
                            }, $(".result-message-box"));
                        },
                        complete: function (jqXHR, textStatus) {
                        }
                    });
                }
            },
            Cancel: function () {
                var dialog = $(this);
                $(".result-message-box", dialog).addClass("hidden");
                $(".ui-state-error", dialog).removeClass("ui-state-error error");
                dialog.dialog("close");
            }
        },
        close: function () {
            $('input, select', $(this)).val("").removeClass("ui-state-error");
        }
    });

    // Register Delete Dialog box
    $("#dialog-form-delete").dialog({
        autoOpen: false,
        height: 'auto',
        minHeight: 0,
        width: 350,
        modal: true,
        buttons: {
            "OK": function () {
                var dialog = $(this);
                var bValid = true;
                $(".result-message-box", dialog).addClass("hidden");
                $(".ui-state-error", dialog).removeClass("ui-state-error error");
                $.ajax({
                    url: "DeleteDomain",
                    type: "POST",
                    data: $("form", $("#dialog-form-delete")).eq(0).serialize(),
                    success: function (errCode) {
                        if (errCode != 0) {
                            //                            $("#deleteConfirmMessage").hide();
                            //                            $(":button:contains('Submit')", dialog).prop("disabled", true).addClass("hidden");
                            //                            $(":button:contains('Cancel')", dialog).find(".ui-button-text").html("OK");
                            $("#dialog-form-delete").dialog("close");
                            GetMessage_DomainListDelete(errCode, dialog);
                        }
                        else {
                            $("#deleteConfirmMessage").show();
                            if (currentPageTotal == 1) {
                                currentPageTotal = 0;
                                domainPageIndex--;
                                $('#PageIndex').val(domainPageIndex);
                            }

                            Submit();
                            $("#dialog-form-delete").dialog("close");
                            $(".success-message-box").empty()
                                                     .append("<ul><li>" + viewResources.ActionResult_Success_DeleteDomain + "</li></ul>")
                                                     .removeClass("hidden");
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        dialog.dialog("close");
                        OddsFeedAdminManager.informActionResult({
                            IsSuccess: false,
                            ResultMessage: "500 internal server error."
                        }, $(".result-message-box"));
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });
            },
            Cancel: function () {
                var dialog = $(this);
                $("#deleteConfirmMessage", dialog).show();
                $(":button:contains('OK')", dialog).prop("disabled", false).removeClass("hidden");
                $(":button:contains('Cancel')", dialog).find(".ui-button-text").html("Cancel");
                $(".result-message-box", dialog).addClass("hidden");
                $(".ui-state-error", dialog).removeClass("ui-state-error error");
                dialog.dialog("close");
            }
        },
        close: function () {
            $('input, select', $(this)).val("").removeClass("ui-state-error");
            $(":button:contains('OK')").prop("disabled", false).removeClass("hidden");
            $(":button:contains('Cancel')").find(".ui-button-text").html("Cancel");
        }
    });

    $("#dialog-form, #dialog-form-update, #dialog-form-delete").keypress(function (e) {
        if (e.keyCode == $.ui.keyCode.ENTER) {
            return false;
        }
    });
}

function GetMessage_DomainList(errCode, dialog) {
    $.ajax({
        url: "GetMessage",
        type: "POST",
        data: "ErrCode=" + errCode,
        success: function (result) {
            OddsFeedAdminManager.informActionResult({
                IsSuccess: false,
                ResultMessage: result
            }, $(".result-message-box", dialog));
        }
    });
};


function GetMessage_DomainListDelete(errCode, dialog) {
    $.ajax({
        url: "GetMessage",
        type: "POST",
        data: "ErrCode=" + errCode,
        success: function (result) {
            OddsFeedAdminManager.informActionResult({
                IsSuccess: false,
                ResultMessage: result
            }, $(".listdomain-result-message-box"));
        }
    });
};