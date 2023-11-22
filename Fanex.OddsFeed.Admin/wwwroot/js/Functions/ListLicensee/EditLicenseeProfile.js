$(document).ready(function () {
    $('#errorStep3').addClass('hidden');

    OddsFeedAdminManager.numericTextBox($('#NumberOfIPs'));
    // begin: check permission to enable/disable editing oddstype, oddstyle.....
    CheckBoxListEvent('chkAllLeagues', 'chkLeague');
    CheckBoxListEvent('chkAllOutrights', 'chkOutright');
    // Sport checkboxes
    CheckBoxListEvent('chkAllMajorSports', 'chkMajorSport', undefined, GetLeagueOutright, 'formEditLicenseeProfile', 'League', 'Outrights');
    CheckBoxListEventWithConfirm('chkAllMinorSports', 'chkMinorSport', undefined, GetLeagueOutright, 'formEditLicenseeProfile', 'League', 'Outrights');

    // Country checkboxes
    CheckBoxListEvent('chkAllCountries', 'chkCountry', undefined, GetLeagueOutright, 'formEditLicenseeProfile', 'League', 'Outrights');

    // Market checkboxes
    CheckBoxListEvent('chkAllMajorMarkets', 'chkMajorMarket', undefined);
    CheckBoxListEventWithConfirm('chkAllMinorMarkets', 'chkMinorMarket', undefined);

    // Show Martket options if "Deadball or Live" is selected
    ShowHideMarket('edit');

    $('.oddstype').click(function () {
        var id = this.id;
        if (id != 'OddsType8')//NOT Outright
        {
            ShowHideMarket('checkall');
        }
    });

    $('#LeagueKeyword').keyup();
    $('#LeagueKeyword_O').keyup();

    $('#step3Previous').click(function () {
        $('div.step-active').attr('class', 'step-inactive');
        $('#panelStep2').attr('class', 'step-active');

        $('div.step').hide();
        $('#step2').show();
    });

    $('#step3Done').click(function () {
        $('.result-message-box').addClass('hidden');

        if (!$('#ApplyAllCurrentAffiliates').prop('checked')) {
            UpdateLicenseeProfile(false);
        } else {
            var alertMsg = 'Warning! This will make all the current settings of all affiliates changed also and it will affect their output XML. Are you sure?';
            confirm(alertMsg.fontcolor('red'), function () {
                UpdateLicenseeProfile(true);
            });
        }
    });

    $('#step3Cancel').click(function () {
        confirm('Are you sure to cancel edit licensee?', function () {
            document.location = currentDomainURL + 'Licensee/ListLicensee';
        });
    });
});

function UpdateLicenseeProfile(isForceUpdate) {
    // Refresh values
    var updateParams = {
        SystemID: parseInt($('#SystemID').val()),
        OddsType: GetDropDownCheckBoxListValue('oddstype'),
        OddsRegion: $('#OddsRegion option:selected').val(),
        DeadballDelayTimes: $('#DeadballDelayTimes').val(),
        LiveDelayTimes: $('#LiveDelayTimes').val(),
        OutrightDelayTimes: $('#OutrightDelayTimes').val(),
        ResultDelayTimes: $('#ResultDelayTimes').val(),
        NumberOfIPs: $('#NumberOfIPs').val(),
        LanguageList: GetDropDownCheckBoxListValue('language'),
        SportIDList: GetIdList('chkAllMajorSports', 'chkAllMinorSports', 'chkMajorSport', 'chkMinorSport'),
        CountryCodeList: $('#chkAllCountries').prop('checked') ? 'all' : GetDropDownCheckBoxListValue('chkCountry'),
        LeagueIDList: $('#chkAllLeagues').prop('checked') ? 'all' : GetDropDownCheckBoxListValue('chkLeague'),
        OutRightIDList: $('#chkAllOutrights').prop('checked') ? 'all' : GetDropDownCheckBoxListValue('chkOutright'),
        BetTypeIDList: GetIdList('chkAllMajorMarkets', 'chkAllMinorMarkets', 'chkMajorMarket', 'chkMinorMarket'),
        LeagueKeyword: $('#LeagueKeyword').val(),
        LeagueKeyword_O: $('#LeagueKeyword_O').val(),
        ApplyAllCurrentAffiliates: $('#ApplyAllCurrentAffiliates').prop('checked'),
        OddsTypeFlag: $('#OddsTypeFlag').prop('checked'),
        OddsRegionFlag: $('#OddsRegionFlag').prop('checked'),
        LanguageListFlag: $('#LanguageListFlag').prop('checked'),
        SportIDListFlag: $('#SportIDListFlag').prop('checked'),
        CountryCodeListFlag: $('#CountryCodeListFlag').prop('checked'),
        BettypeIDListFlag: $('#BettypeIDListFlag').prop('checked'),
        IsForceUpdate: isForceUpdate
    };

    if (CheckValidationStep3()) {
        $('body').addClass('wait');
        $('input:button[value="Save"]').attr('disabled', true);

        $.ajax({
            url: 'EditProfilePost',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(updateParams),
            cache: false,
            success: function (obj) {
                $('body').removeClass('wait');
                $('input:button[value="Save"]').attr('disabled', false);

                if (obj.errCode != 0) {
                    if (obj.errCode == 10701) {//some affiliates under this licensee have the number of selections greater than the current licensee's profile
                        var alertMsg = "You are removing some selections that affiliates under this licensee are available. Are you sure you want to remove these selections for all affiliates' odds profile also?";
                        confirmUpdateMissingProfile(alertMsg.fontcolor('red'), 'LICENSEE ADMIN', updateParams.SystemID, obj);
                    }
                    else {
                        GetErrorMessage(errCode);
                    }
                }
                else {
                    OddsFeedAdminManager.informActionResult({
                        IsSuccess: true,
                        ResultMessage: "The licensee's information has been updated successfully!"
                    }, $('.result-message-box'));
                    OddsFeedAdminManager.confirmAfterSuccess(currentDomainURL + 'Licensee/ListLicensee');

                    $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 100);
                }
            },
            error: function (xhr, status, err) {
            }
        });
    }
}

function confirmUpdateMissingProfile(dialogText, dialogTitle, systemID, obj) {
    $('<div style="padding: 10px; ; word-wrap: break-word;">' + dialogText + '</div>').dialog({
        dialogClass: 'no-close',
        draggable: false,
        modal: true,
        resizable: false,
        width: 'auto',
        title: dialogTitle || 'Confirm',
        minHeight: 75,
        buttons: [
            {
                text: 'Yes',
                click: function () {
                    $(this).dialog('close');
                    UpdateLicenseeProfile(true);
                }
            },
            {
                text: 'No',
                click: function () {
                    $(this).dialog('close');
                }
            },
            {
                text: 'Show missing selections',
                width: '200',
                click: function () {
                    $(this).dialog('close');

                    AddMissingProfile(obj);
                }
            }
        ]
    });
};

function AddMissingProfile(missingPartnersProfile) {
    //Odds Type
    var oddsType = missingPartnersProfile.AffsOddsType;
    var arrayOddsType = oddsType.split(',');
    $('.oddstype').each(function () {
        this.checked = arrayOddsType.indexOf($(this).val()) != -1;
    });

    //Language
    var language = missingPartnersProfile.AffsLanguageList;
    var arrayLanguage = language.split(',');
    $('.language').each(function () {
        this.checked = arrayLanguage.indexOf($(this).val()) != -1;
    });

    //Sport
    var sport = missingPartnersProfile.AffsSportIDList;
    if (sport == 'all') {
        $('#chkAllMajorSports').attr('checked', true);
        $('#chkAllMinorSports').attr('checked', true);
        $('.chkMajorSport').attr('checked', true);
        $('.chkMinorSport').attr('checked', true);
    }
    else {
        var arraySport = sport.split(',');
        $('.chkMajorSport').each(function () {
            this.checked = arraySport.indexOf($(this).val()) != -1;
        });

        $('.chkMinorSport').each(function () {
            this.checked = arraySport.indexOf($(this).val()) != -1;
        });

        //Sport checkboxes
        CheckBoxListEvent('chkAllMajorSports', 'chkMajorSport', undefined);
        CheckBoxListEvent('chkAllMinorSports', 'chkMinorSport', undefined);

    }

    //Country
    var country = missingPartnersProfile.AffsCountryCodeList;
    if (country == 'all') {
        $('#chkAllCountries').attr('checked', true);
        $('.chkCountry').attr('checked', true);
    }
    else {
        var arrayCountry = country.split(',');
        $('.chkCountry').each(function () {
            this.checked = arrayCountry.indexOf($(this).val()) != -1;
        });

        //Country checkboxes
        CheckBoxListEvent('chkAllCountries', 'chkCountry', undefined);
    }

    //Get League & Outright base on Sport & Country
    //League & Outright default check all
    GetLeagueOutright('formEditLicenseeProfile', 'League', 'Outrights');

    //Market
    var bettype = missingPartnersProfile.AffsBettypeIDList;
    if (bettype == 'all') {
        $('#chkAllMajorMarkets').attr('checked', true);
        $('#chkAllMinorMarkets').attr('checked', true);

        $('.chkMajorMarket').attr('checked', true);
        $('.chkMinorMarket').attr('checked', true);
    }
    else {
        var arrayBettype = bettype.split(',');
        $('.chkMajorMarket').each(function () {
            this.checked = arrayBettype.indexOf($(this).val()) != -1;
        });

        $('.chkMinorMarket').each(function () {
            this.checked = arrayBettype.indexOf($(this).val()) != -1;
        });

        //Market checkboxes
        CheckBoxListEvent('chkAllMajorMarkets', 'chkMajorMarket', undefined);
        CheckBoxListEvent('chkAllMinorMarkets', 'chkMinorMarket', undefined);
    }

    //Refresh
    ShowHideMarket('edit');
}