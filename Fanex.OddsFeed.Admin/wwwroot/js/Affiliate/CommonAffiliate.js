$(document).ready(function () {
    $("#DateStart").attr("readonly", "true");
    $("#DateEnd").attr("readonly", "true");

    //step 2            
    $('#btnRight').click(function () {
        !$('#slDomain option:selected').remove().appendTo('#slAssignedDomain');
        showingDomainList();
    });

    $('#btnLeft').click(function () {
        !$('#slAssignedDomain option:selected').remove().appendTo('#slDomain');
        showingDomainList();
    });

    $('#btnDoubleRight').click(function () {
        !$('#slDomain option').remove().appendTo('#slAssignedDomain');
        showingDomainList();
    });

    $('#btnDoubleLeft').click(function () {
        !$('#slAssignedDomain option').remove().appendTo('#slDomain');
        showingDomainList();
    });

    $(".language").click(function () {
        return $("#languagesFlag").val() != "";
    });

    var sportsFlag = $("#sportsFlag").val();
    $("#chkAllSports").click(function () {
        if (sportsFlag) {
            if ($('#chkAllSports').is(":checked")) {
                $('.chkSport').each(function () { this.checked = true; });
            } else {
                $('.chkSport').each(function () { this.checked = false; });
            }
        } else {
            return sportsFlag != "";
        }
    });

    $(".chkSport").click(function () {
        if (sportsFlag) {
            if ($('.chkSport:checked').length == $('.chkSport').length) {
                $('#chkAllSports').attr("checked", true);
            } else {
                $('#chkAllSports').attr("checked", false);
            }
        } else {
            return sportsFlag != "";
        }
    });

    var countriesFlag = $("#countriesFlag").val();
    $("#chkAllCountries").click(function () {
        if (countriesFlag) {
            if ($('#chkAllCountries').is(":checked")) {
                $('.chkCountry').each(function () { this.checked = true; });
            } else {
                $('.chkCountry').each(function () { this.checked = false; });
            }
        } else {
            return countriesFlag != "";
        }
    });

    $(".chkCountry").click(function () {
        if (countriesFlag) {
            if ($('.chkCountry:checked').length == $('.chkCountry').length) {
                $('#chkAllCountries').attr("checked", true);
            } else {
                $('#chkAllCountries').attr("checked", false);
            }
        } else {
            return countriesFlag != "";
        }
    });

    var marketsFlag = $("#marketsFlag").val();
    $("#chkAllMarkets").click(function () {
        if (marketsFlag) {
            if ($('#chkAllMarkets').is(":checked")) {
                $('.chkMarket').each(function () { this.checked = true; });
            } else {
                $('.chkMarket').each(function () { this.checked = false; });
            }
        } else {
            return marketsFlag != "";
        }
    });

    $(".chkMarket").click(function () {
        if (marketsFlag) {
            if ($('.chkMarket:checked').length == $('.chkMarket').length) {
                $('#chkAllMarkets').attr("checked", true);
            } else {
                $('#chkAllMarkets').attr("checked", false);
            }
        } else {
            return marketsFlag != "";
        }
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

    } else {
        $("#chkAllMarkets").attr('checked', false);
        $(".chkMarket").attr('checked', false);
        $("#Market").hide();
    }
}

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
    } else {
        var currentRow = $(obj).closest("tr");
        var columns = $(currentRow).children();

        IPFromOld = $(columns[0]).children()[0].value;
        IPToOld = $(columns[1]).children()[0].value;
    }

    confirm('Do you want to delete this IP range?', function () {
        $(obj).closest("tr").remove();
        var h = $("#IpDiv").height();
        $("#IpDiv").height(h - 30);
        $("#IpTable").height(h - 30);

        DoDeleteIPRange_EditSetting(IPFromOld, IPToOld);
    });
}

function DoDeleteIPRange_EditSetting(IPFromOld, IPToOld) {
    $(".result-message-box").addClass("hidden");
    var updateParams = {
        PartnerId: $("#affID").val(),
        IPFromOld: IPFromOld,
        IPToOld: IPToOld
    };

    $.ajax({
        url: 'DeleteIPRange_EditSetting_Partner?PartnerId=' + updateParams.PartnerId + '&IPFromOld=' + updateParams.IPFromOld + '&IPToOld=' + updateParams.IPToOld,
        type: 'POST',
        cache: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: true,
        cache: false,
        success: function (result) {
            if (result && result.isSuccessful) {
                $(".result-message-box").empty()
                    .append("<ul><li>" + result.message + "</li></ul>")
                    .addClass("success-message-box")
                    .removeClass("error-message-box")
                    .removeClass("hidden");
            } else {
                $(".result-message-box").empty()
                    .append("<ul><li>" + result.message + "</li></ul>")
                    .addClass("error-message-box")
                    .removeClass("success-message-box")
                    .removeClass("hidden");
            }
        },
        error: function (xhr, status, err) {
        }
    });
}

function DoUpdateIPRange_EditSetting() {
    $("#editStep2 .result-message-box").addClass("hidden");
    $(".success").addClass("hidden");
    $("#errorStep2").addClass("hidden");
    $('#slAssignedDomain').removeClass('error');

    var valid = OddsFeedAdminManager.validateIP('save');
    if (!valid) {
        return;
    }

    valid = CheckValidation_AssignSetting();
    if (!valid) {
        return;
    }

    //step2
    var affID = $("#affID").val();
    var selectedOptions = $('#slAssignedDomain option');
    var selectedDomains = $.map(selectedOptions, function (option) {
        return option.value;
    }).join(',');
    var IPs = getDataFromTable("#IpTable");
    var updateParams = {
        affId: affID,
        domains: selectedDomains,
        Ips: IPs.join(",")
    };

    $.ajax({
        type: 'POST',
        url: 'EditSetting',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(updateParams),
        cache: false,
        beforeSend: function (xhr) {
        },
        success: function (result) {
            if (result && result.IsSuccessful) {
                $("#editStep2 .result-message-box").empty()
                    .append("<ul><li>The IP has been saved successfully!</li></ul>")
                    .addClass("success-message-box")
                    .removeClass("error-message-box")
                    .removeClass("hidden");
            }
        },
        error: function (xhr, status, err) {
        }
    });
}

function showingDomainList() {
    //left
    var leftCount = $('#slDomain option').length;
    var leftShow = leftCount > 8 ? 8 : leftCount;
    $("#showingDomainListId").text("Displaying " + leftShow + " of " + leftCount);

    //right
    var rightCount = $('#slAssignedDomain option').length;
    var rightShow = rightCount > 8 ? 8 : rightCount;
    $("#showingAssignedDomainId").text("Displaying " + rightShow + " of " + rightCount);
}

function getDataFromTable(id) {
    var data = Array();
    var fields = $(id).find('input').serializeArray();

    $.each(fields, function (i, field) {
        data[i] = $.trim(field.value);
    });

    return data;
}

var affiliateManager = {
    GetDropDownCheckBoxListValue: function (className) {
        var value = [];
        $('.' + className).each(function () {
            if (this.checked) {
                value.push($(this).val());
            }
        });

        return value.join(",");
    },

    CheckValidation_OddsProfile: function (updateParams) {
        // Refresh error box
        $('#errorStep3').addClass('hidden').empty();
        $(".error").removeClass('error');

        // Validation
        if (updateParams.OddsType.length == 0) {
            $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_OddType + "</li></ul>")
                .removeClass('hidden');
            $('input[id^="OddsType"]').not('[id$="Flag"]').addClass('error');
            $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);;
            return false;
        }

        //If End-User is AccManager
        if ($("#isAccountManagerPermission").val() == "value") {

            //Check number of IP is required
            if (updateParams.NumberOfIPs == "") {
                $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_NumberOfIPs + "</li></ul>")
                    .removeClass('hidden');
                $('#NumberOfIPs').addClass('error');
                $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);;
                return false;
            }

            if (!$.isNumeric(updateParams.NumberOfIPs)) {
                $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_NumberOfIPs + "</li></ul>")
                    .removeClass('hidden');
                $('#NumberOfIPs').addClass('error');
                $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);;
                return false;
            }

            //Check maximum number of IP          
            var numberOfIPs = parseInt(updateParams.NumberOfIPs); //at step3
            var maxNumOfIPs = OddsFeedAdminManager.getMaximumNumberOfIPs();
            if (numberOfIPs > maxNumOfIPs) {
                $("#errorStep3").append("<ul><li>" + viewResources.MaximumNumberOfIPs.replace("{MaximumNumberOfIPs}", maxNumOfIPs) + "</li></ul>")
                    .removeClass('hidden');
                $('#NumberOfIPs').addClass('error');
                $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);;
                return false;
            }

            //Check maximum number of IP with the number of IP of licensee
            var sum = $("#ActualNumberOfIps").length > 0
                ? parseInt($("#ActualNumberOfIps").val()) // at step 3 (edit aff)
                : OddsFeedAdminManager.getNumberOfIP('IpTable'); //at step2 (add aff)
            var numberOfIPs = parseInt(updateParams.NumberOfIPs); //at step3            
            if (numberOfIPs < sum) {
                $("#errorStep3").append("<ul><li>" + viewResources.Error_NumberOfIPs.replace("{IpCount}", sum) + "</li></ul>")
                    .removeClass('hidden');
                $('#NumberOfIPs').addClass('error');
                $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);;
                return false;
            }
        }

        if (updateParams.LanguageList.length == 0) {
            $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_Language + "</li></ul>")
                .removeClass('hidden');
            $('input[id^="slLanguage"]').not('[id$="Flag"]').addClass('error');
            $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);;
            return false;
        }

        if (updateParams.SportIDList.length == 0) {
            $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_Sport + "</li></ul>")
                .removeClass('hidden');
            $('.chkMinorSport').parent().parent().parent().addClass('error');
            $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);;
            return false;
        }

        if (updateParams.CountryCodeList.length == 0) {
            $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_Country + "</li></ul>")
                .removeClass('hidden');
            $('.chkCountry').parent().parent().addClass('error');
            $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);;
            return false;
        }

        //If Deadball or Liveball is selected then check if at least a market(bettype) is selected
        if ($.inArray('2', updateParams.OddsType) > -1 || $.inArray('4', updateParams.OddsType) > -1) {
            if (updateParams.BetTypeIDList.length == 0) {
                $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_Market + "</li></ul>")
                    .removeClass('hidden');
                $('.chkMajorMarket').parent().parent().parent().addClass('error');
                //$('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000); ;
                return false;
            }
        }

        //If End-User is AccManager
        if ($("#isAccountManagerPermission").val() == "value") {

            //League
            if (updateParams.LeagueIDList.length == 0) {
                $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_League + "</li></ul>")
                    .removeClass('hidden');
                $('.chkLeague').parent().parent().addClass('error');
                $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);;
                return false;
            }

            //Outright
            if (updateParams.OutRightIDList.length == 0) {
                $("#errorStep3").append("<ul><li>" + viewResources.Error_Require_Outright + "</li></ul>")
                    .removeClass('hidden');
                $('.chkOutright').parent().parent().addClass('error');
                $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 1000);;
                return false;
            }
        }

        return true;
    },
    GetIdList: function (marjorClass, minorClass) {
        var minorIDList = affiliateManager.GetDropDownCheckBoxListValue(minorClass);
        var majorIDList = affiliateManager.GetDropDownCheckBoxListValue(marjorClass);
        var idList = majorIDList;

        if (minorIDList != '') {
            if (majorIDList != '')
                idList = majorIDList + ',' + minorIDList;
            else
                idList = minorIDList;
        }

        return idList;
    }
}