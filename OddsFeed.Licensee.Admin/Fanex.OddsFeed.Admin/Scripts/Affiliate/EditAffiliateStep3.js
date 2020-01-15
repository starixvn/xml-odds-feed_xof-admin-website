$(document).ready(function () {
    if ($("#isAccountManagerPermission").val() == "") {
        SetDefault();
    }

    //Sport checkboxes
    CheckBoxListEvent('chkAllMajorSports', 'chkMajorSport', undefined, GetLeagueOutright, 'formAffiliate', "League", 'Outrights');
    CheckBoxListEventWithConfirm('chkAllMinorSports', 'chkMinorSport', undefined, GetLeagueOutright, 'formAffiliate', "League", 'Outrights');
    //Country checkboxes
    CheckBoxListEvent('chkAllCountries', 'chkCountry', undefined, GetLeagueOutright, 'formAffiliate', "League", 'Outrights');
    //Market checkboxes
    CheckBoxListEvent('chkAllMajorMarkets', 'chkMajorMarket', undefined);
    CheckBoxListEventWithConfirm('chkAllMinorMarkets', 'chkMinorMarket', undefined);

    // Show Martket options if "Deadball or Live" is selected
    ShowHideMarket('edit');

    $(".oddstype").click(function () {
        if ($("#oddsTypeFlag").val() != "") {
            var id = this.id;
            if (id != 'OddsType8')//NOT Outright
            {
                ShowHideMarket('checkall');
            }
        }
        else {
            return false;
        }
    });

    if ($("#isAccountManagerPermission").val() != "") {
        //League
        CheckBoxListEvent('chkAllLeagues', 'chkLeague', undefined);
        $('#LeagueKeyword').keyup();

        //Outright
        CheckBoxListEvent('chkAllOutrights', 'chkOutright', undefined);
        $('#LeagueKeyword_O').keyup();
    }

    $('input[isdisabled="disabled"]').prop("disabled", true);
    $('input[isdisabled!="disabled"]').prop("disabled", false);
    //$('input[isdisabled="disabled"]').attr("disabled", "disabled");

    $("#editStep3Save").click(function () {
        $(".result-message-box").addClass("hidden");

        // Refresh values
        var updateParams = {
            PartnerID: $("#PartnerID").val(),
            OddsType: affiliateManager.GetDropDownCheckBoxListValue('oddstype'),
            OddsRegion: $("#slOddsStyle option:selected").val(),
            DeadballDelayTimes: $("#DeadballDelayTimes").length > 0 ? $("#DeadballDelayTimes").val() : "",
            LiveDelayTimes: $("#LiveDelayTimes").length > 0 ? $("#LiveDelayTimes").val() : "",
            OutrightDelayTimes: $("#OutrightDelayTimes").length > 0 ? $("#OutrightDelayTimes").val() : "",
            NumberOfIPs: $("#NumberOfIPs").val(),
            LanguageList: affiliateManager.GetDropDownCheckBoxListValue('language'),
            SportIDList: affiliateManager.GetIdList('chkMajorSport', 'chkMinorSport'),
            CountryCodeList: affiliateManager.GetDropDownCheckBoxListValue('chkCountry'),
            BetTypeIDList: affiliateManager.GetIdList('chkMajorMarket', 'chkMinorMarket'),
            LeagueIDList: $("#chkAllLeagues").length > 0 ? affiliateManager.GetDropDownCheckBoxListValue('chkLeague') : "",
            OutRightIDList: $("#chkAllOutrights").length > 0 ? affiliateManager.GetDropDownCheckBoxListValue('chkOutright') : "",
            LeagueKeyword: $("#LeagueKeyword").length > 0 ? $("#LeagueKeyword").val() : "",
            LeagueKeyword_O: $("#LeagueKeyword_O").length > 0 ? $("#LeagueKeyword_O").val() : "",
            OddsTypeFlag: $("#oddsTypeFlag").val() == "value",
            OddsRegionFlag: $("#oddsStyleFlag").val() == "value",
            LanguageListFlag: $("#languagesFlag").val() == "value",
            SportIDListFlag: $("#sportsFlag").val() == "value",
            CountryCodeListFlag: $("#countriesFlag").val() == "value",
            BetTypeIDListFlag: $("#marketsFlag").val() == "value"
        };

        var isValid = affiliateManager.CheckValidation_OddsProfile(updateParams);
        if (!isValid) {
            return;
        }
        $("body").addClass("wait");
        $('input:button[value="Save"]').attr('disabled', true);

        $.ajax({
            type: 'POST',
            url: 'EditProfile',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(updateParams),
            cache: false,
            beforeSend: function (xhr) {
            },
            success: function (result) {
                $('body').removeClass('wait');
                $('input:button[value="Save"]').attr('disabled', false);

                var success = result != null && result.IsSuccess == true;
                OddsFeedAdminManager.informActionResult(result, $("#editStep3 .result-message-box"));
                if (success) {
                    OddsFeedAdminManager.confirmAfterSuccess(currentDomainURL + "/Affiliate/List");
                }
            },
            error: function (xhr, status, err) {
            }
        });
    });
});
//--------------------------------------------------------------------------------------------------------------------

function SetDefault() {
    //oddsStyle
    $('#slOddsStyle option:not(:selected)').attr('disabled', $("#oddsStyleFlag").val() == "");
}