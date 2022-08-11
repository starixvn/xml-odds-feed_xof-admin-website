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
        document.location = currentDomainURL + 'Affiliate/List';
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

    var openDiv7 = '<div class=\"grid7\">';
    var openDiv9 = '<div class=\"grid9\">';
    var openDiv = '<div class=\"row\">';
    var closeDiv = '</div>';

    // XML Odds Feed URL
    if (selectedDomains.length > 0) {
        var urlXMLMain = 'http://' + selectedDomains[0].replace('http://', '') + '/' + $('#virutalPath').val() + name + '/odds.xml';
        text = openDiv + openDiv7 + 'XML Odds Feed Main URL' + closeDiv + openDiv9 + "<a href='" + urlXMLMain + "'>" + urlXMLMain + '</a>' + closeDiv;

        if ($('#SportIDList27').prop('checked') || $('#SportIDList50').prop('checked')) {
            urlXMLMain = 'http://' + selectedDomains[0].replace('http://', '') + '/' + $('#virutalPath').val() + name + '/cricket/result.xml';
            text += openDiv + openDiv7 + 'XML Result Feed Main URL for Cricket' + closeDiv + openDiv9 + "<a href='" + urlXMLMain + "'>" + urlXMLMain + '</a>' + closeDiv;
        }

        var urlHelpMain = 'http://' + selectedDomains[0].replace('http://', '') + '/' + $('#virutalPath').val() + name + '/help.htm';
        text += openDiv + openDiv7 + 'How to implement' + closeDiv + openDiv9 + "<a href='" + urlHelpMain + "'>" + urlHelpMain + '</a>' + closeDiv + closeDiv;

        for (var i = 1; i < selectedDomains.length; i++) {
            var urlXML = 'http://' + selectedDomains[i].replace('http://', '') + '/' + $('#virutalPath').val() + name + '/odds.xml';
            text += openDiv + openDiv7 + 'XML Odds Feed URL (Backup)' + closeDiv + openDiv9 + "<a href='" + urlXML + "'>" + urlXML + '</a>' + closeDiv;

            if ($('#SportIDList27').prop('checked') || $('#SportIDList50').prop('checked')) {
                urlXML = 'http://' + selectedDomains[i].replace('http://', '') + '/' + $('#virutalPath').val() + name + '/cricket/result.xml';
                text += openDiv + openDiv7 + 'XML Result Feed Main URL for Cricket (Backup)' + closeDiv + openDiv9 + "<a href='" + urlXML + "'>" + urlXML + '</a>' + closeDiv;
            }

            var urlHelp = 'http://' + selectedDomains[i].replace('http://', '') + '/' + $('#virutalPath').val() + name + '/help.htm';
            text += openDiv + openDiv7 + 'How to implement (Backup)' + closeDiv + openDiv9 + "<a href='" + urlHelp + "'>" + urlHelp + '</a>' + closeDiv + closeDiv;
        }
    }

    document.getElementById('doneId').innerHTML = text;
}