$(document).ready(function () {
    //Process redirect to Afiiliate list.
    var successed = $("#successed").val();
    if (successed == "true") {
        confirm('Successfull!\nPress OK to continue', function () {
            document.location = "/OddsFeedAdmin/Affiliate/List";
        });
    }

});
//--------------------------------------------------------------------------------------------------------------------

$(document).click(function (event) {

    if (!$(event.target).hasClass('btn-action')) {
        if (!$(event.target).hasClass('actions')) {
            $(".actions").hide();
        }
    }    
});

