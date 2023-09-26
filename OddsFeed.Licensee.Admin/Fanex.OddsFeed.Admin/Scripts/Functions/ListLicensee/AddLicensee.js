$(document).ready(function () {
    //step1
    $('#step1 input[type="text"]').val('');
    $('#step1  textarea').val('');

    //step2
    $('#step2 input[type="text"]').val('');

    //step3
    $('#step3 input[type="checkbox"]').prop('checked', false);

    //cancel
    $('.cancel').click(function () {
        confirm('Are you sure to cancel adding new licensee?', function () {
            document.location = currentDomainURL + 'Licensee/ListLicensee';
        });
    });


});
//--------------------------------------------------------------------------------------------------------------------

function AddLicensee() {
    //step2
    var selectedDomains = $.map($('#AssignedDomainList option'), function (option) {
        return option.value;
    }).join(',');

    $('#DomainList').val(selectedDomains);

    // IP
    $('#ListIPFrom').val(getDataFromTable('#IpTable', 'FromIP'));
    $('#ListIPTo').val(getDataFromTable('#IpTable', 'ToIP'));

    //step3
    $('#OddsType').val(GetDropDownCheckBoxListValue('oddstype'));

    $('#LanguageList').val(GetDropDownCheckBoxListValue('language'));

    $('#SportIDList').val(GetIdList('chkAllMajorSports', 'chkAllMinorSports', 'chkMajorSport', 'chkMinorSport'));

    $('#CountryCodeList').val(GetDropDownCheckBoxListValue('chkCountry'));

    $('#LeagueIDList').val(GetDropDownCheckBoxListValue('chkLeague'));

    $('#OutRightIDList').val(GetDropDownCheckBoxListValue('chkOutright'));

    $('#BetTypeIDList').val(GetIdList('chkAllMajorMarkets', 'chkAllMinorMarkets', 'chkMajorMarket', 'chkMinorMarket'));

    // Refresh values
    var updateParams = {
        SystemName: $('#SystemName').val(),
        ContactEmail: $('#ContactEmail').val(),
        DomainList: $('#DomainList').val(),
        ListIPFrom: $('#ListIPFrom').val(),
        ListIPTo: $('#ListIPTo').val(),
        OddsType: $('#OddsType').val(),
        OddsRegion: $('#OddsRegion option:selected').val(),
        DeadballDelayTimes: $('#DeadballDelayTimes').val(),
        LiveDelayTimes: $('#LiveDelayTimes').val(),
        OutrightDelayTimes: $('#OutrightDelayTimes').val(),
        ResultDelayTimes: $('#ResultDelayTimes').val(),
        NumberOfIPs: $('#NumberOfIPs').val(),
        LanguageList: $('#LanguageList').val(),
        SportIDList: $('#chkAllSports').prop('checked') ? 'all' : $('#SportIDList').val(),
        CountryCodeList: $('#chkAllCountries').prop('checked') ? 'all' : $('#CountryCodeList').val(),
        LeagueIDList: $('#chkAllLeagues').prop('checked') ? 'all' : $('#LeagueIDList').val(),
        OutRightIDList: $('#chkAllOutrights').prop('checked') ? 'all' : $('#OutRightIDList').val(),
        BetTypeIDList: $('#chkAllMarkets').prop('checked') ? 'all' : $('#BetTypeIDList').val(),
        LeagueKeyword: $('#LeagueKeyword').val(),
        LeagueKeyword_O: $('#LeagueKeyword_O').val(),
        OddsTypeFlag: $('#OddsTypeFlag').prop('checked'),
        OddsRegionFlag: $('#OddsRegionFlag').prop('checked'),
        LanguageListFlag: $('#LanguageListFlag').prop('checked'),
        SportIDListFlag: $('#SportIDListFlag').prop('checked'),
        CountryCodeListFlag: $('#CountryCodeListFlag').prop('checked'),
        BettypeIDListFlag: $('#BettypeIDListFlag').prop('checked'),
        LicenseeID: $('#LicenseeID option:selected').val()
    };

    if (CheckValidationStep3()) {
        $('body').addClass('wait');

        $.ajax({
            url: 'CreateLicensee',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(updateParams),
            cache: false,
            success: function (errCode) {
                $('body').removeClass('wait');

                if (errCode != 0) {
                    GetErrorMessage(errCode);
                } else {
                    OddsFeedAdminManager.informActionResult({
                        IsSuccess: true,
                        ResultMessage: "The licensee's information has been added successfully!"
                    }, $('.result-message-box'));
                    OddsFeedAdminManager.confirmAfterSuccess(currentDomainURL + 'Licensee/ListLicensee');
                }
            },
            error: function (xhr, status, err) {
            }
        });
    }
};