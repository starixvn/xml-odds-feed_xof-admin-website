$(document).ready(function () {
    $("#slSystem").select2();
    $('span.select2-selection__rendered').css('text-align', 'left');
    $("#slSystem").change(function (e) {
        var systemId = $("#slSystem").val();
        console.log(systemId)
        var selectParams = {
            systemid: systemId,
            systemname: $("#slSystem option:selected").html()
        };

        var url = currentDomainURL + "Base/Home?" + $.param(selectParams);
        document.location = url;

    });
});
//--------------------------------------------------------------------------------------------------------------------