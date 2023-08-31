const addAffiliateStep4 = (function ($) {
    function getDomain(domain) {
        const name = $.trim($('#txtName').val());
        return 'http://' + domain.replace('http://', '').replace('https://', '')
            + '/' + $('#virutalPath').val() + name;
    }

    function showDone() {
        let text = '';

        let selectedOptions = $('#slAssignedDomain option');
        let selectedDomains = $.map(selectedOptions, function (option) {
            return option.text;
        });

        let openDiv7 = '<div class=\"grid7\">';
        let openDiv9 = '<div class=\"grid9\">';
        let openDiv = '<div class=\"row\">';
        let closeDiv = '</div>';

        // XML Odds Feed URL
        if (selectedDomains.length > 0) {
            let urlXMLMain = getDomain(selectedDomains[0]) + '/odds.xml';
            text = openDiv + openDiv7 + 'XML Odds Feed Main URL' + closeDiv + openDiv9 + "<a href='" + urlXMLMain + "'>" + urlXMLMain + '</a>' + closeDiv;

            if ($('#SportIDList27').prop('checked') || $('#SportIDList50').prop('checked')) {
                urlXMLMain = getDomain(selectedDomains[0]) + '/cricket/result.xml';
                text += openDiv + openDiv7 + 'XML Result Feed Main URL for Cricket' + closeDiv + openDiv9 + "<a href='" + urlXMLMain + "'>" + urlXMLMain + '</a>' + closeDiv;
            }

            let urlHelpMain = getDomain(selectedDomains[0]) + '/help.htm';
            text += openDiv + openDiv7 + 'How to implement' + closeDiv + openDiv9 + "<a href='" + urlHelpMain + "'>" + urlHelpMain + '</a>' + closeDiv + closeDiv;

            for (let i = 1; i < selectedDomains.length; i++) {
                let urlXML = getDomain(selectedDomains[i]) + '/odds.xml';
                text += openDiv + openDiv7 + 'XML Odds Feed URL (Backup)' + closeDiv + openDiv9 + "<a href='" + urlXML + "'>" + urlXML + '</a>' + closeDiv;

                if ($('#SportIDList27').prop('checked') || $('#SportIDList50').prop('checked')) {
                    urlXML = getDomain(selectedDomains[i]) + '/cricket/result.xml';
                    text += openDiv + openDiv7 + 'XML Result Feed Main URL for Cricket (Backup)' + closeDiv + openDiv9 + "<a href='" + urlXML + "'>" + urlXML + '</a>' + closeDiv;
                }

                let urlHelp = getDomain(selectedDomains[i]) + '/help.htm';
                text += openDiv + openDiv7 + 'How to implement (Backup)' + closeDiv + openDiv9 + "<a href='" + urlHelp + "'>" + urlHelp + '</a>' + closeDiv + closeDiv;
            }
        }

        document.getElementById('doneId').innerHTML = text;
    }

    return {
        showDone
    };
})(jQuery);


$(document).ready(function () {
    // step 4
    $('#step4Done').click(function () {
        document.location = currentDomainURL + 'Affiliate/List';
    });
});