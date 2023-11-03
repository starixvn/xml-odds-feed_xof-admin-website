$(document).ready(function () {
    // begin: check permission to enable/disable editing oddstype, oddstyle.....
    if ($('#isAccountManagerPermission').val() == '') {
        SetDefault();
    }

    // Sport checkboxes
    CheckBoxListEvent('chkAllMajorSports', 'chkMajorSport', undefined, GetLeagueOutright, 'formAffiliate', 'League', 'Outrights');
    CheckBoxListEventWithConfirm('chkAllMinorSports', 'chkMinorSport', undefined, GetLeagueOutright, 'formAffiliate', 'League', 'Outrights');

    // Country checkboxes
    CheckBoxListEvent('chkAllCountries', 'chkCountry', undefined, GetLeagueOutright, 'formAffiliate', 'League', 'Outrights');

    // Market checkboxes
    CheckBoxListEvent('chkAllMajorMarkets', 'chkMajorMarket', undefined);
    CheckBoxListEventWithConfirm('chkAllMinorMarkets', 'chkMinorMarket', undefined);

    // Show Martket options if 'Deadball or Live' is selected
    ShowHideMarket('add');
    $('.oddstype').click(function () {

        if ($('#oddsTypeFlag').val() != '') {
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

    if ($('#isAccountManagerPermission').val() != '') {
        // League
        CheckBoxListEvent('chkAllLeagues', 'chkLeague', undefined);
        $('#LeagueKeyword').keyup();

        // Outright
        CheckBoxListEvent('chkAllOutrights', 'chkOutright', undefined);
        $('#LeagueKeyword_O').keyup();
    }

    $('#step3Previous').click(function () {
        $('div.step-active').attr('class', 'step-inactive');
        $('#panelStep2').attr('class', 'step-active');

        $('div.step').hide();
        $('#step2').show();
    });

    $('#step3Next').click(function () {
        var addParams = {
            OddsType: affiliateManager.GetDropDownCheckBoxListValue('oddstype'),
            NumberOfIPs: $('#NumberOfIPs').val(),
            LanguageList: affiliateManager.GetDropDownCheckBoxListValue('language'),
            SportIDList: affiliateManager.GetIdList('chkMajorSport', 'chkMinorSport'),
            CountryCodeList: affiliateManager.GetDropDownCheckBoxListValue('chkCountry'),
            LeagueIDList: affiliateManager.GetDropDownCheckBoxListValue('chkLeague'),
            OutRightIDList: affiliateManager.GetDropDownCheckBoxListValue('chkOutright'),
            BetTypeIDList: affiliateManager.GetIdList('chkMajorMarket', 'chkMinorMarket')
        };

        var isValid = affiliateManager.CheckValidation_OddsProfile(addParams);
        if (!isValid) {
            return;
        }

        AddAffilliate();

    });
});

function SetDefault() {
    // oddstype
    $('.oddstype').attr('checked', true); //check all oddstypes when adding new aff

    // oddsStyle
    $('#slOddsStyle option:not(:selected)').attr('disabled', $('#oddsStyleFlag').val() == '');

    // languages
    $('.language').attr('checked', true); //check all languages when adding new aff

    // Sport checkboxes
    $('.chkMajorSport').attr('checked', true); //check all sports when adding new aff
    $('#chkMajorAllSports').attr('checked', true);

    // Country checkboxes
    $('.chkCountry').attr('checked', true); //check all countries when adding new aff
    $('#chkAllCountries').attr('checked', true);

    // Market checkboxes
    $('.chkMajorMarket').attr('checked', true); //check all markets when adding new aff
    $('#chkMajorAllMarkets').attr('checked', true);
}