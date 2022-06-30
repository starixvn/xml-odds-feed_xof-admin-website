$(document).ready(function () {
    //step1
    $('#step1 input[type="text"]').val('');
    $('#step1  textarea').val('');

    //step2
    $('#step2 input[type="text"]').val('');

    //step3
    $('input[isdisabled="disabled"]').attr('disabled', 'disabled');

    //cancel
    $('.cancel').click(function () {
        confirm('Are you sure to cancel adding new affiliate?', function () {
            document.location = currentDomainURL + '/Affiliate/List';
        });
    });
});
//--------------------------------------------------------------------------------------------------------------------

function AddAffilliate() {
    //step1
    var name = $.trim($('#txtName').val());
    var email = $.trim($('#txtEmail').val());
    var webSite = $.trim($('#txtWebsite').val());
    var note = $.trim($('#txtNote').val());
    var content = $.trim($('#txtContent').val());

    //step2
    var selectedOptions = $('#slAssignedDomain option');
    var selectedDomains = $.map(selectedOptions, function (option) {
        return option.value;
    }).join(',');

    var IPs = getDataFromTable('#IpTable');

    //step3
    var oddsTypes = '';
    $('.oddstype').each(function () {
        if (this.checked) {
            oddsTypes += $(this).val() + ',';
        }
    });

    var oddsStyle = $('#slOddsStyle').val();

    var languages = '';
    $('.language').each(function () {
        if (this.checked) {
            languages += $(this).val() + ',';
        }
    });

    var sports = '';
    $('.chkMajorSport').each(function () {
        if (this.checked) {
            sports += $(this).val() + ',';
        }
    });

    $('.chkMinorSport').each(function () {
        if (this.checked) {
            sports += $(this).val() + ',';
        }
    });

    var countries = '';
    $('.chkCountry').each(function () {
        if (this.checked) {
            countries += $(this).val() + ',';
        }
    });

    var markets = '';
    $('.chkMajorMarket').each(function () {
        if (this.checked) {
            markets += $(this).val() + ',';
        }
    });
    $('.chkMinorMarket').each(function () {
        if (this.checked) {
            markets += $(this).val() + ',';
        }
    });
    //for Account Manager
    var leagues = '';
    $('.chkLeague').each(function () {
        if (this.checked) {
            leagues += $(this).val() + ',';
        }
    });

    var outrights = '';
    $('.chkOutright').each(function () {
        if (this.checked) {
            outrights += $(this).val() + ',';
        }
    });

    var leagueKeyword = $('#LeagueKeyword').length > 0 ? $('#LeagueKeyword').val() : '';
    var leagueKeyword_O = $('#LeagueKeyword_O').length > 0 ? $('#LeagueKeyword_O').val() : '';

    var delayTimes = $('#DeadballDelayTimes').length > 0 ? ($('#DeadballDelayTimes').val() + ',' + $('#LiveDelayTimes').val() + ',' + $('#OutrightDelayTimes').val()) : '';
    var numberOfIPs = $('#NumberOfIPs').length > 0 ? $('#NumberOfIPs').val() : 0;
    
    //End: for Account Manager

    var addParams = {
        UserName: name,
        Email: email,
        Website: webSite,
        Note: note,
        Content: content,
        ContactPerson: name,
        Domains: selectedDomains,
        IpsFrom: IPs.join(','),
        IpsTo: IPs.join(','),
        OddsType: oddsTypes,
        OddsRegion: oddsStyle,
        LanguageList: languages,
        SportIDList: sports,
        BetTypeIDList: markets,
        CountryCodeList: countries,
        LeagueIDList: leagues,
        LeagueKeyword: leagueKeyword,
        LeagueKeyword_O: leagueKeyword_O,
        OddsDelayTimes: delayTimes,
        ResultDelayTimes: $('#ResultDelayTimes').length > 0 ? $('#ResultDelayTimes').val() : '',
        OutRightIDList: outrights,
        NumberOfIPs: numberOfIPs,
        OddsTypeFlag: $('#oddsTypeFlag').val() == 'value',
        OddsRegionFlag: $('#oddsStyleFlag').val() == 'value',
        LanguageListFlag: $('#languagesFlag').val() == 'value',
        SportIDListFlag: $('#sportsFlag').val() == 'value',
        CountryCodeListFlag: $('#countriesFlag').val() == 'value',
        BetTypeIDListFlag: $('#marketsFlag').val() == 'value'
    };

    $('body').addClass('wait');

    $.ajax({
        url: 'AddNew',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(addParams),
        cache: false,
        success: function (result) {
            $('body').removeClass('wait');

            var success = result != null && result.IsSuccess;
            if (success) {
                $('.result-message-box').addClass('hidden');
                $('div.step-active').attr('class', 'step-inactive');
                $('#panelStep4').attr('class', 'step-active');
                $('div.step').hide();
                $('#step4').show();
                showDone();
            } else {
                OddsFeedAdminManager.informActionResult(result, $('.result-message-box'));
            }
        },
        error: function (xhr, status, err) {
        }
    });
}