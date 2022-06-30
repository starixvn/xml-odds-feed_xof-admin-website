$(document).ready(function () {
    // step 4
    $('#step4Previous').click(function () {
        $('div.step-active').attr('class', 'step-inactive');
        $('#panelStep3').attr('class', 'step-active');

        $('.result-message-box').addClass('hidden');
        $('div.step').hide();
        $('#step3').show();
    });

    $('#step4Done').click(function () {
        document.location = currentDomainURL + '/Affiliate/List';
    });
});

// ---------------------------------
function showDone() {
    var text = '';

    var name = $.trim($('#txtName').val());

    var selectedOptions = $('#slAssignedDomain option');
    var selectedDomains = $.map(selectedOptions, function (option) {
        return option.text;
    });

    var openDiv5 = '<div class=\"grid7\">';
    var openDiv = '<div class=\"row\">';
    var closeDiv = closeDiv5 = '</div>';

    // XML Odds Feed URL
    if (selectedDomains.length > 0) {
        var urlXMLMain = 'http://' + selectedDomains[0].replace('http://', '') + '/' + $('#virutalPath').val() + name + '/odds.xml';
        text = openDiv + openDiv5 + 'XML Odds Feed Main URL' + closeDiv5 + "<a href='" + urlXMLMain + "'>" + urlXMLMain + '</a>' + closeDiv;

        urlXMLMain = 'http://' + selectedDomains[0].replace('http://', '') + '/' + $('#virutalPath').val() + name + '/cricket/result.xml';
        text = openDiv + openDiv5 + 'XML Result Feed Main URL for Cricket' + closeDiv5 + "<a href='" + urlXMLMain + "'>" + urlXMLMain + '</a>' + closeDiv;

        for (var i = 1; i < selectedDomains.length; i++) {
            var urlXML = 'http://' + selectedDomains[i].replace('http://', '') + '/' + $('#virutalPath').val() + name + '/odds.xml';
            text += openDiv + openDiv5 + 'XML Odds Feed URL (Backup)' + closeDiv5 + "<a href='" + urlXML + "'>" + urlXML + '</a>' + closeDiv;

            urlXML = 'http://' + selectedDomains[i].replace('http://', '') + '/' + $('#virutalPath').val() + name + '/cricket/result.xml';
            text += openDiv + openDiv5 + 'XML Result Feed Main URL for Cricket (Backup)' + closeDiv5 + "<a href='" + urlXML + "'>" + urlXML + '</a>' + closeDiv;
        }
    }

    // Help URL
    if (selectedDomains.length > 0) {
        var urlHelpMain = 'http://' + selectedDomains[0].replace('http://', '') + '/' + $('#virutalPath').val() + name + '/help.htm';
        text += openDiv + openDiv5 + 'How to implement' + closeDiv5 + "<a href='" + urlHelpMain + "'>" + urlHelpMain + '</a>' + closeDiv;

        for (var i = 1; i < selectedDomains.length; i++) {
            var urlHelp = 'http://' + selectedDomains[i].replace('http://', '') + '/' + $('#virutalPath').val() + name + '/help.htm';
            text += openDiv + openDiv5 + 'How to implement (Backup)' + closeDiv5 + "<a href='" + urlHelp + "'>" + urlHelp + '</a>' + closeDiv;
        }
    }

    document.getElementById('doneId').innerHTML = text;
}