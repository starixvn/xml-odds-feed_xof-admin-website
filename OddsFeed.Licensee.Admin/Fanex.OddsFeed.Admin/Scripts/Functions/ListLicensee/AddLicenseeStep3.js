$(document).ready(function () {
    $("#errorStep3").addClass("hidden");
    //Sport checkboxes
    CheckBoxListEvent('chkAllSports', 'chkSport', undefined, GetLeagueOutright, 'formLicensee', "League", 'Outrights');

    //    //Country checkboxes
    CheckBoxListEvent('chkAllCountries', 'chkCountry', undefined, GetLeagueOutright, 'formLicensee', "League", 'Outrights');

    //    //Market checkboxes
    CheckBoxListEvent('chkAllMarkets', 'chkMarket', false);

    //hide market options
    $("#Market").hide();

    // Show Martket options if "Deadball or Live" is selected
    $(".oddstype").click(function () {
        var id = this.id;
        if (id != 'OddsType8')//NOT Outright
        {
            ShowHideMarket('checkall');
        } 
    });

    $("#step3Previous").click(function () {
        $("div.step-active").attr("class", "step-inactive");
        $("#panelStep2").attr("class", "step-active");

        $(".result-message-box").addClass("hidden");
        $("div.step").hide();
        $("#step2").show();
    });

    $("#step3Done").click(function () {
        AddLicensee();
    });
});