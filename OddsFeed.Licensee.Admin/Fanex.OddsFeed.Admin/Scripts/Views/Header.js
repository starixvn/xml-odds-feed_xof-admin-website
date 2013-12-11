$(document).ready(function () {
    $("#slSystem").change(function (e) {
        var systemId = $("#slSystem").val();

        var selectParams = {
            systemid: systemId,
            systemname: $("#slSystem option:selected").html()
        };

        var url = currentDomainURL + "/Base/Home?" + $.param(selectParams);
        document.location = url;

    });
});
//--------------------------------------------------------------------------------------------------------------------