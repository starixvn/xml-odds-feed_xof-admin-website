$(document).ready(function () {
    $("#slSystem").select2();
    $('span.select2-selection__rendered').css('text-align', 'left');
    $("#slSystem").change(function (e) {
        const systemId = $("#slSystem").val();
        const selectParams = {
            systemid: systemId,
            systemname: $("#slSystem option:selected").html()
        };

        const url = currentDomainURL + "Base/Home?" + $.param(selectParams);
        document.location = url;
    });

    const currentUrl = $('#currentUrl').val();
    let selectNavItem = $("#" + currentUrl);
    if (selectNavItem != undefined) {
        selectNavItem.addClass("selected");
    }

    //show/hide navigation secion
    $("div.togglable_section").bind("click", function () {
        let currentSection = $(this);
        let divItems = currentSection.next("div.navItem");
        if (currentSection.hasClass("folded")) {
            currentSection.removeClass("folded");
            divItems.show(200);
        } else {

            currentSection.addClass("folded");
            divItems.hide(200);
        }
    });

    //add style for first item section
    $("div.togglable_section:first-child").children(":first").addClass("first");

    $('.el[data-hashovermenu=True], .el[data-hashovermenu=data-hashovermenu]')
        .hover(function () {
            $(this).find('.hoveraction').fadeIn(400);
        }, function () {
            $(this).find('.hoveraction').hide();
        });
});