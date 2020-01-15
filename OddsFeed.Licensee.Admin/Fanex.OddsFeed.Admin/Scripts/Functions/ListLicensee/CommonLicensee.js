$(document).ready(function () {
    $("#newIP").click(function (IPFrom, IPTo) {
        addRow(IPFrom, IPTo);
    });

    //step 2
    $('#btnRight').click(function () {
        !$('#ListDomain option:selected').remove().appendTo('#AssignedDomainList');
        showingDomainList();
        if ($("#errorStep2").length > 0 && $('#AssignedDomainList').length) {
            $("#errorStep2").addClass("hidden");
            $('#AssignedDomainList').removeClass('error');
        }
    });
    $('#btnLeft').click(function () {
        !$('#AssignedDomainList option:selected').remove().appendTo('#ListDomain');
        showingDomainList();
    });

    $('#btnDoubleRight').click(function () {
        !$('#ListDomain option').remove().appendTo('#AssignedDomainList');
        showingDomainList();
        if ($("#errorStep2").length > 0 && $('#AssignedDomainList').length) {
            $("#errorStep2").addClass("hidden");
            $('#AssignedDomainList').removeClass('error');
        }
    });
    $('#btnDoubleLeft').click(function () {
        !$('#AssignedDomainList option').remove().appendTo('#ListDomain');
        showingDomainList();
    });

    $('#LeagueKeyword').keyup(function () {
        return LeagueFilter($(this).val(), 'LeagueIDList');
    });
    $('#LeagueKeyword_O').keyup(function () {
        return LeagueFilter($(this).val(), 'OutRightIDList');
    });
});

function ShowHideMarket(action) {
    if ($('#OddsType2:checked').length + $('#OddsType4:checked').length > 0) {
        if (action == 'checkall') {
            $("#chkAllMarkets").attr('checked', true);
            $(".chkMarket").attr('checked', true);
        }
        //if ($("#marketsFlag").val() != "") {
        $("#Market").show();
        //}
    }
    else {
        $("#chkAllMarkets").attr('checked', false);
        $(".chkMarket").attr('checked', false);
        $("#Market").hide();
    }
    //    if (action == 'add') {
    //        if ($('#OddsType2:checked').length + $('#OddsType4:checked').length > 0) {
    //            $("#chkAllMarkets").attr('checked', false);
    //            $(".chkMarket").attr('checked', false);
    //            $("#Market").show();
    //        }
    //        else {
    //            $("#Market").hide();
    //        }
    //    }
    //    else {
    //        if ($('#OddsType2:checked').length + $('#OddsType4:checked').length > 0) {
    //            $("#Market").show();
    //        }
    //        else {
    //            $("#chkAllMarkets").attr('checked', false);
    //            $(".chkMarket").attr('checked', false);
    //            $("#Market").hide();
    //        }
    //    }
};

function addRow(IPFrom, IPTo) {
    //validate IP
    var valid = OddsFeedAdminManager.validateIP('add');

    //check maximum number of IP
    if (valid) {
        var numberOfIPs = OddsFeedAdminManager.MaximumNumberOfIPs;
        valid = OddsFeedAdminManager.validateNumberOfIP('add', numberOfIPs, 'IpTable');
    }

    //add new IP row
    if (valid) {
        var str = '';
        if (IPFrom == "" || IPFrom == undefined || IPTo == "" || IPTo == undefined) {
            str = "<tr><td><input name=\"FromIP\" type=\"text\" /></td><td><input name=\"ToIP\" type=\"text\" /></td> <td><span style=\"padding-left:18px\"></span><span title=\"Save\" class=\"action save\" onclick=\"SaveIPSetting(this)\"  ></span><span style=\"padding-left:30px\"></span><span title=\"Delete\" class=\"action del\" onclick=\"removeRow(this,'" + IPFrom + "','" + IPTo + "');\"></span></td></tr>";
        } else {
            str = "<tr><td><input name=\"FromIP\" type=\"text\" value=\"" + IPFrom.toString() + "\" /></td><td><input name=\"ToIP\" type=\"text\" value=\"" + IPTo.toString() + "\" /></td> <td><span style=\"padding-left:18px\"></span><span title=\"Save\" class=\"action save\" onclick=\"SaveIPSetting(this,'" + IPFrom + "','" + IPTo + "')\"  ></span><span style=\"padding-left:30px\"></span><span title=\"Delete\" class=\"action del\" onclick=\"removeRow(this,'" + IPFrom + "','" + IPTo + "');\"></span></td></tr>";
        }

        $("#IpTable > tbody").append(str);
        var h = $("#IpDiv").height();
        $("#IpDiv").height(h + 30);
    }
};

function removeRowTemporary(obj) {
    $(obj).closest("tr").remove();
    var h = $("#IpDiv").height();
    $("#IpDiv").height(h - 30);
    $("#IpTable").height(h - 30);
}

function removeRow(obj, IPFromOld, IPToOld) {
    if (IPFromOld == undefined || IPFromOld == "" || IPToOld == undefined || IPToOld == "") {
        $(obj).closest("tr").remove();
        var h = $("#IpDiv").height();
        $("#IpDiv").height(h - 30);
        $("#IpTable").height(h - 30);
        return;
    }
    else {
        var currentRow = $(obj).closest("tr");
        var columns = $(currentRow).children();

        IPFromOld = $(columns[0]).children()[0].value;
        IPToOld = $(columns[1]).children()[0].value;
    }

    //    //check validation
    //    var valid = OddsFeedAdminManager.validateUpdatingIP('remove', IPFromOld, IPToOld);
    //    if (!valid) {
    //        return;
    //    }

    confirm('Do you want to delete this IP range?', function () {
        $(obj).closest("tr").remove();
        var h = $("#IpDiv").height();
        $("#IpDiv").height(h - 30);
        $("#IpTable").height(h - 30);
        DoDeleteIPRange_EditSetting(IPFromOld, IPToOld);
    });
};

function DoDeleteIPRange_EditSetting(IPFromOld, IPToOld) {
    $(".result-message-box").addClass("hidden");
    var updateParams = {
        SystemId: $("#SystemID").val(),
        IPFromOld: IPFromOld,
        IPToOld: IPToOld
    };

    $.ajax({
        url: 'DeleteIPRange_EditSetting',
        type: 'POST',
        cache: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(updateParams),
        async: true,
        cache: false,
        success: function (result) {
            OddsFeedAdminManager.informActionResult({
                IsSuccess: result.IsSuccess,
                ResultMessage: result.ResultMessage
            }, $(".result-message-box"));
        },
        error: function (xhr, status, err) {
        }
    });
};

function DoUpdateIPRange_EditSetting(IPFromOld,
    IPToOld,
    IPFromNew,
    IPToNew) {
    var updateParams = {
        SystemId: $("#SystemID").val(),
        IpFrom: IPFromNew,
        IpTo: IPToNew,
        IpFromOld: IPFromOld,
        IpToOld: IPToOld
    };

    $.ajax({
        url: 'UpdateIPRange_EditSetting',
        type: 'POST',
        cache: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(updateParams),
        cache: false,
        success: function (message) {
            if (message != "Successfully!") {
                OddsFeedAdminManager.informActionResult({
                    IsSuccess: false,
                    ResultMessage: message
                }, $(".result-message-box"));
            }
            else {
                OddsFeedAdminManager.informActionResult({
                    IsSuccess: true,
                    ResultMessage: "The IP has been saved successfully!"
                }, $(".result-message-box"));
            }
        },
        error: function (xhr, status, err) {
        }
    });
};

function showingDomainList() {
    //left
    var leftCount = $('#ListDomain option').length;
    var leftShow = leftCount > 8 ? 8 : leftCount;
    $("#showingDomainListId").text("Displaying " + leftShow + " of " + leftCount);

    if ($.browser.msie) {
        var version = parseFloat($.browser.version);
        if (version < 9) {
            $(".domainsActionGroup .doubleright").css("margin-left", "48px");
            $(".domainsActionGroup .left").css("margin-left", "48px");
            $(".domainsActionGroup .doubleleft").css("margin-left", "48px");
        }

        if (version >= 9) {
            $(".domainsActionGroup .doubleleft").css("margin-left", "0px");
            $(".domainsActionGroup").css("margin-left", "25px");
        }
    }

    //right
    var rightCount = $('#AssignedDomainList option').length;
    var rightShow = rightCount > 8 ? 8 : rightCount;
    $("#showingAssignedDomainId").text("Displaying " + rightShow + " of " + rightCount);
};

function getDataFromTable(id, type) {
    var data = '';
    var fields = $(id).find('input[name=' + type + ']').serializeArray();

    $.each(fields, function (i, field) {
        if (field.value.length != 0)
            data += $.trim(field.value) + ',';
    });

    data = data.substr(0, data.length - 1);
    return data;
};

function SaveIPSetting(obj, IPFromOld, IPToOld) {
    var valid = true;
    var currentRow = $(obj).closest("tr");

    var columns = $(currentRow).children();
    var inputFrom = $(columns[0]).children()[0].value;
    var inputTo = $(columns[1]).children()[0].value;

    //check validation
    valid = OddsFeedAdminManager.validateUpdatingIP('save', inputFrom, inputTo);

    //check maximum number of IP
    if (valid) {
        var numberOfIPs = OddsFeedAdminManager.MaximumNumberOfIPs; // size of  (smallint)
        valid = OddsFeedAdminManager.validateNumberOfIP('save', numberOfIPs, 'IpTable');
    }

    if (!valid) return;

    DoUpdateIPRange_EditSetting(IPFromOld, IPToOld, inputFrom, inputTo);
    $(columns[2]).find(".action.save").attr("onclick", "SaveIPSetting(this,'" + inputFrom + "','" + inputTo + "');");
    $(columns[2]).find(".action.del").attr("onclick", "removeRow(this,'" + inputFrom + "','" + inputTo + "');");
};

function GetErrorMessage(errCode) {
    $.ajax({
        url: "GetMessage",
        type: "POST",
        data: "ErrCode=" + errCode,
        success: function (result) {
            OddsFeedAdminManager.informActionResult({
                IsSuccess: false,
                ResultMessage: result
            }, $(".result-message-box"));

            $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 100);
        }
    });
}

function CheckBoxListEvent(chkAll, chkItems, isCheckAll, callback) {
    var paras = arguments;

    InitCheckBoxList(chkAll, chkItems, isCheckAll);
    CheckItemsEvent(chkAll, chkItems, callback, paras);
    CheckAllListEvent(chkAll, chkItems, callback, paras);
}

function CheckBoxListEventWithConfirm(chkAll, chkItems, isCheckAll, callback) {
    var paras = arguments;

    InitCheckBoxList(chkAll, chkItems, isCheckAll);
    CheckItemsEvent(chkAll, chkItems, callback, paras);
    CheckAllListEventWithConfirm(chkAll, chkItems, callback, paras);
}

function InitCheckBoxList(chkAll, chkItems, isCheckAll) {
    if (isCheckAll != undefined) {
        var optionItems = $("." + chkItems);
        if (optionItems && optionItems.length) {
            $("." + chkItems).attr('checked', isCheckAll); // uncheck all League when load
            $('#' + chkAll).attr("checked", isCheckAll);
        } else {
            $('#' + chkAll).attr("checked", false);
        }
    } else {
        if ($('.' + chkItems + ':checked').length != 0 && ($('.' + chkItems + ':checked').length == $("." + chkItems).length)) {
            $('#' + chkAll).attr("checked", true);
        }
        else {
            $('#' + chkAll).attr("checked", false);
        }
    }
}

function CheckItemsEvent(chkAll, chkItems, callback, paras) {
    $("." + chkItems).click(function () {
        if ($('.' + chkItems + ':checked').length != 0 && ($('.' + chkItems + ':checked').length == $("." + chkItems).length)) {
            $('#' + chkAll).attr("checked", true);
        }
        else {
            $('#' + chkAll).attr("checked", false);
        }

        if (jQuery.isFunction(callback))
            callback(paras[4], paras[5], paras[6], paras[7]);
    });
}

function CheckAllListEvent(chkAll, chkItems, callback, paras) {
    $('#' + chkAll).click(function () {
        if ($('#' + chkAll).is(":checked")) {
            $("." + chkItems).each(function () { this.checked = true; });
        }
        else {
            $("." + chkItems).each(function () { this.checked = false; });
        }

        if (jQuery.isFunction(callback))
            callback(paras[4], paras[5], paras[6], paras[7]);
    });
}

function CheckAllListEventWithConfirm(chkAll, chkItems, callback, paras) {
    $('#' + chkAll).click(function () {
        var chkAllBtn = this;

        if ($('#' + chkAll).is(":checked")) {
            chkAllBtn.checked = false;
            confirm('YOU ARE SELECTING ALL SELECTIONS, SO THE ODDS SERVICES MAY RETURN DATA SLOWLY! Are you sure to continue?', function () {
                $("." + chkItems).each(function () { this.checked = true; });
                chkAllBtn.checked = true;

                if (jQuery.isFunction(callback))
                    callback(paras[4], paras[5], paras[6], paras[7]);
            });
        }
        else {
            $("." + chkItems).each(function () { this.checked = false; });

            if (jQuery.isFunction(callback))
                callback(paras[4], paras[5], paras[6], paras[7]);
        }
    });
}

//function GetLeagueOutright(sportItemList, countryItemList, leagueElement, outrightElement) {
//    var sports = GetDropDownCheckBoxListValue(sportItemList);

//    var countries = GetDropDownCheckBoxListValue(countryItemList);
//    var league = GetDropDownCheckBoxListValue('chkLeague');
//    $.ajax({
//        url: 'GetLeague',
//        type: 'POST',
//        data: "SportIDList=" + sports + "&CountryCodeList= " + countries + "&LeagueKeyword=" + $('#LeagueKeyword').val() + "&LeagueKeywordFlag=" + $('#LeagueKeywordFlag').val() + "&LeagueIDList=" + league + "&LeagueIDListFlag=" + $('#LeagueIDListFlag').val(),
//        dataType: 'html',
//        cache: false,
//        success: function (result) {
//            $('#' + leagueElement).html(result);
//            $('#LeagueKeyword').keyup(function () {
//                return LeagueFilter($(this).val(), 'LeagueIDList');
//            });

//            CheckBoxListEvent('chkAllLeagues', 'chkLeague');
//        },
//        error: function (xhr, status, err) {
//        }
//    });

//    $.ajax({
//        url: 'GetOutRight',
//        type: 'POST',
//        data: "SportIDList=" + sports + "&CountryCodeList= " + countries,
//        dataType: 'html',
//        cache: false,
//        success: function (result) {
//            $('#' + outrightElement).html(result);
//            $('#LeagueKeyword_O').keyup(function () {
//                return LeagueFilter($(this).val(), 'OutRightIDList');
//            });

//            CheckBoxListEvent('chkAllOutrights', 'chkOutright');
//        },
//        error: function (xhr, status, err) {
//        }
//    });
//}

function GetLeagueOutright(formID, leagueElement, outrightElement) {
    var sportIdList = GetIdList('chkMajorSport', 'chkMinorSport');
    $('#SportIDList').val(sportIdList);

    $('#CountryCodeList').val(GetDropDownCheckBoxListValue('chkCountry'));

    $.ajax({
        url: currentDomainURL + '/Licensee/GetLeague',
        type: 'POST',
        data: $('#' + formID).serialize(),
        dataType: 'html',
        cache: false,
        success: function (result) {
            $('#' + leagueElement).html(result);
            $('#LeagueKeyword').unbind("keyup");
            $('#LeagueKeyword').keyup(function () {
                return LeagueFilter($(this).val(), 'LeagueIDList');
            });

            $('#LeagueKeyword').keyup();

            CheckBoxListEvent('chkAllLeagues', 'chkLeague', true);
        },
        error: function (xhr, status, err) {
        }
    });

    $.ajax({
        url: currentDomainURL + '/Licensee/GetOutRight',
        type: 'POST',
        data: $('#' + formID).serialize(),
        dataType: 'html',
        cache: false,
        success: function (result) {
            $('#' + outrightElement).html(result);
            $('#LeagueKeyword_O').unbind("keyup");
            $('#LeagueKeyword_O').keyup(function () {
                return LeagueFilter($(this).val(), 'OutRightIDList');
            });

            $('#LeagueKeyword_O').keyup();

            CheckBoxListEvent('chkAllOutrights', 'chkOutright', true);
        },
        error: function (xhr, status, err) {
        }
    });
}

function LeagueFilter(LeagueKeyword, prefix) {
    $('label[for^="' + prefix + '"]').not($('label[for="' + prefix + 'Flag"]')).not($('label[for="' + prefix + '"]')).each(function () {
        if (LeagueKeyword == undefined || LeagueKeyword == "") {
            $(this).parent('div').css('color', 'black');
        } else {
            if ($(this).html().toLowerCase().indexOf(LeagueKeyword.toLowerCase()) != -1) {
                $(this).parent('div').css('color', 'red');
                //            $(this).parent('div').show();
            } else {
                $(this).parent('div').css('color', 'black');
                //            $(this).parent('div').hide();
            }
        }
    });
}

function GetDropDownCheckBoxListValue(className) {
    var value = "";
    //    if ($('.' + className).length == $('.' + className + ':checked').length) {
    //        return "all";
    //    }

    $('.' + className).each(function () {
        if (this.checked) {
            value += $(this).val() + ",";
        }
    });

    return value.substr(0, value.length - 1);
}

function CheckValidationStep3() {
    // Refresh error box
    $('#errorStep3').addClass('hidden');
    $("#errorStep3").html("")
    $(".error").removeClass('error');

    // Refresh values
    $('#OddsType').val(GetDropDownCheckBoxListValue('oddstype'));

    $('#LanguageList').val(GetDropDownCheckBoxListValue('language'));

    $('#SportIDList').val(GetIdList('chkMajorSport', 'chkMinorSport'));

    $('#CountryCodeList').val(GetDropDownCheckBoxListValue('chkCountry'));

    $('#LeagueIDList').val(GetDropDownCheckBoxListValue('chkLeague'));

    $('#OutRightIDList').val(GetDropDownCheckBoxListValue('chkOutright'));

    $('#BetTypeIDList').val(GetIdList('chkMajorMarket', 'chkMinorMarket'));

    // Validation
    if ($('#OddsType').val().length == 0) {
        $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_OddType + "</li></ul>")
        $('#errorStep3').removeClass('hidden');
        $('input[id^="OddsType"]').not('[id$="Flag"]').addClass('error');
        $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);
        return false;
    }
    else {//At least Deadball or Outright must be selected
        if ($('#OddsType').val().indexOf('2') == -1 && $('#OddsType').val().indexOf('8') == -1) {
            $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_Deadball_or_Outright + "</li></ul>");
            $('#errorStep3').removeClass('hidden');
            $('input[id^="OddsType"]').not('[id$="Flag"]').addClass('error');
            $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);
            return false;
        }
    }

    //Check number of IP is required
    if ($('#NumberOfIPs').val() == "") {
        $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_NumberOfIPs + "</li></ul>")
            .removeClass('hidden');
        $('#NumberOfIPs').addClass('error');
        $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);;
        return false;
    }

    //Check maximum number of IP (32767)
    var numberOfIPs = parseInt($('#NumberOfIPs').val()); //at step3
    if (numberOfIPs > OddsFeedAdminManager.MaximumNumberOfIPs) {
        $("#errorStep3").append("<ul><li>" + viewResources.MaximumNumberOfIPs.replace("{MaximumNumberOfIPs}", OddsFeedAdminManager.MaximumNumberOfIPs) + "</li></ul>")
            .removeClass('hidden');
        $('#NumberOfIPs').addClass('error');
        $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);;
        return false;
    }

    if ($('#LanguageList').val().length == 0) {
        $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_Language + "</li></ul>")
        $('#errorStep3').removeClass('hidden');
        $('input[id^="LanguageList"]').not('[id$="Flag"]').addClass('error');
        $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);
        return false;
    }

    if ($('#SportIDList').val().length == 0) {
        $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_Sport + "</li></ul>")
        $('#errorStep3').removeClass('hidden');
        $('#SportIDList').parent('div').addClass('error');
        $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);
        return false;
    }

    if ($('#CountryCodeList').val().length == 0) {
        $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_Country + "</li></ul>")
        $('#errorStep3').removeClass('hidden');
        $('#CountryCodeList').parent('div').addClass('error');
        $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);
        return false;
    }

    if ($('.chkLeague:checked').length == 0) {
        $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_League + "</li></ul>")
        $('#errorStep3').removeClass('hidden');
        $('#LeagueIDList').parent('div').addClass('error');
        $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);
        return false;
    }

    if ($('.chkOutright:checked').length == 0) {
        $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_Outright + "</li></ul>")
        $('#errorStep3').removeClass('hidden');
        $('#OutRightIDList').parent('div').addClass('error');
        $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);
        return false;
    }

    //If Deadball or Liveball is selected then check if at least a market(bettype) is selected
    if ($.inArray('2', $('#OddsType').val()) > -1 || $.inArray('4', $('#OddsType').val()) > -1) {
        if ($('#BetTypeIDList').val().length == 0) {
            $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_Market + "</li></ul>")
            $('#errorStep3').removeClass('hidden');
            $('#BetTypeIDList').parent('div').addClass('error');
            $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);
            return false;
        }
    }

    return true;
}

function OpenDialog(id, licenseeID) {
    $("#" + id).dialog("open");
}

function GetIdList(marjorClass, minorClass) {
    var minorIDList = GetDropDownCheckBoxListValue(minorClass);
    var majorIDList = GetDropDownCheckBoxListValue(marjorClass);
    var idList = majorIDList;

    if (minorIDList != '') {
        if (majorIDList != '')
            idList = majorIDList + ',' + minorIDList;
        else
            idList = minorIDList;
    }

    return idList;
}