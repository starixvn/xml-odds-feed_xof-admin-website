var gridOptions
        , _pageIndex = 0
        , _pageSize = 5;
var gridOptions1
        , _pageIndex1 = 0
        , _pageSize1 = 5;
var gridOptions2
        , _pageIndex2 = 0
        , _pageSize2 = 5;

$(document).ready(function () {
    ShowLoginInfo();
    ShowCurrentOnlineUser();
    ShowRecentActivities();

    $(".lastreqname").hover(
        function () {
            //var obj = "<input id='tooltip' type='text' style='margin-top:-20px; margin-left:-20px; border: 0px solid #000000;' value='" + $("#lastreqname").val() + "' />";
            var obj = "<div id='tooltip' style='margin-top:-60px; margin-left:110px'>" + $("#lastreqname").val() + "</div>";
            $(this).append($(obj));
        },
        function () {
            $(this).find("#tooltip").remove();
        }
    );
});

// Show Login Info
function ShowLoginInfo() {
    $.ajax({
        url: currentDomainURL + "Dashboard/GetLoginInfo",
        type: "POST",
        data: "",
        success: function (result) {
            $("#GridLoginInfo").html(result);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //            alert(xhr);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

// Show Current Online User
function ShowCurrentOnlineUser() {
    // send back info to controllers
    var url = currentDomainURL + 'Dashboard/ShowCurrentOnlineUser?pageSize=' + _pageSize + '&pageIndex=' + _pageIndex;
    $.ajax({
        url: url,
        type: "POST",
        data: "",
        success: function (result) {
            result = result.replace(/\n/g, "<\/br>");
            result = eval(result);
            gridOptions =
            {
                columns: [
                    { HeaderText: "#", Width: 40, Name: "row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "UserID", Width: 10, Name: "userid", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Username", Width: 180, Name: "username", TdClass: "boldusername", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Logged In", Width: 250, Name: "lastlogin", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Last Request", Width: 250, Name: "reloadtime", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "IP", Width: 150, Name: "IP", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Role", Width: 150, Name: "rolename", HeaderAlign: "center", CellAlign: "left" }
                ],
                bodyRows: result[0].data,
                gridExpandHeight: 150,
                marginRight: 0,
                allowTruncate: true,
                showPager: true,
                showDistinctRow: false,
                customPager: true,
                fullHeight: false,
                gridHeight: 205,
                pagerOption: {
                    itemsPerPage: _pageSize,
                    currentPage: _pageIndex,
                    totalItem: result[1].totalItem,
                    itemsPerPageArray: [5, 10, 20, 50, 100],
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };
            $("#GridCurrentOnlineUser").fanexGrid(gridOptions);
            $("#GridCurrentOnlineUser").fanexGrid("hideColumns", gridOptions, [1]);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //            alert(xhr);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
    if (_pageSize != itemsPerPage) {
        page_index = 0;
    }

    _pageSize = itemsPerPage;
    _pageIndex = page_index;

    gridOptions.pagerOption.currentPage = page_index;
    ShowCurrentOnlineUser();

    return false;
}

// Show Recent Activities
function ShowRecentActivities() {
    // send back info to controllers

    if ($('input:radio[name=optActivities]:checked').val() == 1) {//get by this account
        var url = currentDomainURL + 'Dashboard/ShowRecentActivities?pageSize=' + _pageSize1 + '&pageIndex=' + _pageIndex1 + '&optionActivities=' + $('input:radio[name=optActivities]:checked').val();

        $.ajax({
            url: url,
            type: "POST",
            data: "",
            success: function (result) {
                result = result.replace(/\n/g, "<\/br>");
                result = eval(result);
                gridOptions1 =
            {
                columns: [
                    { HeaderText: "#", Width: 40, Name: "row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "UserId", Width: 20, Name: "userid", HeaderAlign: "center", CellAlign: "right" },                    
                    { HeaderText: "Action", Width: 745, Name: "detail", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Date", Width: 250, Name: "loggeddate", HeaderAlign: "center", CellAlign: "center" },
                ],
                bodyRows: result[0].data,
                gridExpandHeight: 150,
                allowTruncate: true,
                showPager: true,
                marginRight: 0,
                showDistinctRow: false,
                customPager: true,
                fullHeight: false,
                gridHeight: 205,
                pagerOption: {
                    itemsPerPage: _pageSize1,
                    currentPage: _pageIndex1,
                    totalItem: result[1].totalItem,
                    itemsPerPageArray: [5, 10, 20, 50, 100],
                    pagerSelectCallBack: ReRenderAfterPagedRecentActivities1
                },
                columnsNeedHide: [1],
                showColumnsShowHide: false,
                marginRight: 0
            };
                $("#GridRecentActivities").fanexGrid(gridOptions1);
                //$("#GridRecentActivities").fanexGrid("hideColumns", gridOptions1, [1]);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //            alert(xhr);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    }
    else { //get by licensee
        var url = currentDomainURL + 'Dashboard/ShowRecentActivities?pageSize='
            + _pageSize2 + '&pageIndex=' + _pageIndex2 + '&optionActivities=' + $('input:radio[name=optActivities]:checked').val();
        $.ajax({
            url: url,
            type: "POST",
            data: "",
            success: function (result) {
                result = result.replace(/\n/g, "<\/br>");
                result = OddsFeedAdminManager.formatFanexGridStringResult(result);

                //result = result.replace(/\n/g, "<\/br>");
                //result = eval(result);
                gridOptions2 =
            {
                columns: [
                    { HeaderText: "#", Width: 40, Name: "row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "UserId", Width: 20, Name: "userid", HeaderAlign: "center", CellAlign: "right" },
                    { HeaderText: "Username", Width: 180, Name: "username", TdClass: "boldusername", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Action", Width: 560, Name: "detail", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Date", Width: 250, Name: "loggeddate", HeaderAlign: "center", CellAlign: "center" },
                ],
                bodyRows: result[0].data,
                gridExpandHeight: 150,
                allowTruncate: true,
                showPager: true,
                marginRight: 0,
                showDistinctRow: false,
                customPager: true,
                fullHeight: false,
                gridHeight: 205,
                pagerOption: {
                    itemsPerPage: _pageSize2,
                    currentPage: _pageIndex2,
                    totalItem: result[1].totalItem,
                    itemsPerPageArray: [5, 10, 20, 50, 100],
                    pagerSelectCallBack: ReRenderAfterPagedRecentActivities2
                },
                columnsNeedHide: [1],
                showColumnsShowHide: false,
                marginRight: 0
            };
                $("#GridRecentActivities").fanexGrid(gridOptions2);
                //$("#GridRecentActivities").fanexGrid("hideColumns", gridOptions1, [1]);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //            alert(xhr);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    }
}

function ReRenderAfterPagedRecentActivities1(page_index, itemsPerPage, jq) {
    if (_pageSize1 != itemsPerPage) {
        page_index1 = 0;
    }

    _pageSize1 = itemsPerPage;
    _pageIndex1 = page_index;

    gridOptions1.pagerOption.currentPage = page_index;
    ShowRecentActivities();

    return false;
}

function ReRenderAfterPagedRecentActivities2(page_index, itemsPerPage, jq) {
    if (_pageSize2 != itemsPerPage) {
        page_index2 = 0;
    }

    _pageSize2 = itemsPerPage;
    _pageIndex2 = page_index;

    gridOptions2.pagerOption.currentPage = page_index;
    ShowRecentActivities();

    return false;
}

// view total affiliate
function ViewTotalAff() {
    window.location.href = currentDomainURL + "Affiliate/List";
}

// view total sub-account
function ViewTotalSubAcc() {
    window.location.href = currentDomainURL + "SubAccount/List";
}

// view all today's request
function ViewTotalReqToday() {
    window.location.href = currentDomainURL + "Affiliate/Requests";
}

// view all this month's requests
function ViewTotalReqThisMonth() {
    window.location.href = currentDomainURL + "Affiliate/Requests?curmonth=true";
}