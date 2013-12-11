$(document).ready(function () {
    //Process redirect to Afiiliate list.
    var successed = $("#successed").val();
    if (successed == "true") {
        confirm('Successfull!\nPress OK to continue', function () {
            document.location = currentDomainURL + "/Affiliate/List";
        });
    }
        
});

//--------------------------------------------------------------------------------------------------------------------

function editActive(obj, affId) {

    if ($(obj).hasClass('ico-tick')) {

        confirm('Do you want to deactivate this affiliate ?', function () {
            $(obj).removeClass('ico-tick').addClass('ico-untick');

            var url = 'EditActive?affId=' + affId + '&status=false';

            //todo
            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'html',
                cache: false,
                beforeSend: function (xhr) {
                },
                success: function (result) {
                    result = result.toLowerCase();

                    if (result == 'successfully!') {
                        document.location = currentDomainURL + "/Affiliate/List";
                    }
                    else {
                        alert(result);
                    }
                },
                error: function (xhr, status, err) {
                }
            });
        });
    }
    else {
        confirm('Do you want to activate this affiliate ?', function () {
            $(obj).removeClass('ico-untick').addClass('ico-tick');

            var url = 'EditActive?affId=' + affId + '&status=true';

            //todo
            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'html',
                cache: false,
                beforeSend: function (xhr) {
                },
                success: function (result) {
                    result = result.toLowerCase();

                    if (result == 'successfully!') {
                        document.location = currentDomainURL + "/Affiliate/List";
                    }
                    else {
                        alert(result);
                    }
                },
                error: function (xhr, status, err) {
                }
            });
        });
    }

};

function deleteAffiliate(affId, name ) {

    confirm('Do you want to delete this affiliate (' +name + ') ?', function () {

        var url = 'Delete?affId=' + affId

        //todo
        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                //NAP.loadingBox.show();
                //NAP.closePopup();
            },
            success: function (result) {
                //NAP.loadingBox.close();
                result = result.toLowerCase();

                if (result == 'successfully!') {
                    document.location = currentDomainURL + "/Affiliate/List";
                    //NAP.headlineMsgBox.showErr("Unknowned error detected!");                    
                }
                else {
                    alert(result);
                    //NAP.headlineMsgBox.showInfo("Added '" + username + "'. You will be redirected in 3s.", { closeBtn: false, modal: true });
                    //NAP.redirect('User/Manage?SiteId=0', 3000);
                }
            },
            error: function (xhr, status, err) {
                //NAP.handleAjaxError(xhr, status, err);
            }
        });
    });
}