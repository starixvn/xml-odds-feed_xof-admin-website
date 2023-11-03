$(document).ready(function () {
    //Process redirect to Afiiliate list.
    var successed = $("#successed").val();
    if (successed == "true") {
        confirm('Successfull!\nPress OK to continue', function () {
            document.location = currentDomainURL + "Affiliate/List";
        });
    }
});

//--------------------------------------------------------------------------------------------------------------------
function editActive(obj, affId) {
    if ($(obj).hasClass('ico-tick')) {
        confirm('Do you want to deactivate this affiliate ?', function () {
            $(obj).removeClass('ico-tick').addClass('ico-untick');

            $.ajax({
                url: currentDomainURL + 'Affiliate/EditActive?affId=' + affId + '&status=false',
                type: 'POST',
                dataType: 'html',
                cache: false,
                success: function (result) {
                    result = result.toLowerCase();
                    if (result == 'successfully!') {
                        document.location = currentDomainURL + "Affiliate/List";
                    }
                    else {
                        alert(result);
                    }
                }
            });
        });
    } else {
        confirm('Do you want to activate this affiliate ?', function () {
            $(obj).removeClass('ico-untick').addClass('ico-tick');

            $.ajax({
                url: 'EditActive?affId=' + affId + '&status=true',
                type: 'POST',
                dataType: 'html',
                cache: false,
                success: function (result) {
                    result = result.toLowerCase();
                    if (result == 'successfully!') {
                        document.location = currentDomainURL + "Affiliate/List";
                    } else {
                        alert(result);
                    }
                }
            });
        });
    }
}

function deleteAffiliate(affId, name) {
    confirm('Do you want to delete this affiliate (' + name + ') ?', function () {
        $.ajax({
            url: currentDomainURL + 'Affiliate/Delete?affId=' + affId,
            type: 'POST',
            dataType: 'html',
            cache: false,
            success: function (result) {
                //NAP.loadingBox.close();
                result = result.toLowerCase();

                if (result == 'successfully!') {
                    document.location = currentDomainURL + "Affiliate/List";
                }
                else {
                    alert(result);
                }
            }
        });
    });
}