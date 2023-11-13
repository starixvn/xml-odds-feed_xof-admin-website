var gridOptions,
    _pageIndex = 0,
    _pageSize = 5;
var gridOptions1,
    _pageIndex1 = 0,
    _pageSize1 = 5;
var gridOptions2,
    _pageIndex2 = 0,
    _pageSize2 = 5;

$(document).ready(function () {
    ShowLoginInfo();
    ShowCurrentOnlineUser();
    ShowRecentActivities();

    $(".lastreqname").hover(
        function () {
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
        }
    });
}

function renderUserOnlineBodyRows(users) {
    var rows = [];
    var totalRows = users.length;
    for (var i = 0; i < totalRows; i++) {
        var user = users[i];
        var row = {
            Columns: [
                { Name: 'Row', Value: user.row },
                { Name: 'Username', Value: user.username },
                { Name: 'LastLogin', Value: user.lastLogin },
                { Name: 'ReloadTime', Value: user.reloadTime },
                { Name: 'IP', Value: user.ip },
                { Name: 'RoleName', Value: user.roleName }
            ]
        };

        rows.push(row);
    }

    return rows;
}

// Show Current Online User
function ShowCurrentOnlineUser() {
    var url = currentDomainURL + 'Dashboard/ShowCurrentOnlineUser?pageSize=' + _pageSize + '&pageIndex=' + _pageIndex;
    $.ajax({
        url: url,
        type: "POST",
        data: "",
        success: function (result) {
            gridOptions =
            {
                columns: [
                    { HeaderText: "#", Width: 40, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Username", Width: 200, Name: "Username", TdClass: "boldusername", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Logged In", Width: 150, Name: "LastLogin", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Last Request", Width: 150, Name: "ReloadTime", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "IP", Width: 150, Name: "IP", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Role", Width: 200, Name: "RoleName", HeaderAlign: "center", CellAlign: "left" }
                ],
                bodyRows: renderUserOnlineBodyRows(result.data),
                gridExpandHeight: 150,
                marginRight: 0,
                showPager: true,
                customPager: true,
                fullHeight: false,
                gridHeight: 205,
                pagerOption: {
                    itemsPerPage: _pageSize,
                    currentPage: _pageIndex,
                    totalItem: result.totalItems,
                    itemsPerPageArray: [5, 10, 20, 50, 100],
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };
            $("#GridCurrentOnlineUser").fanexGrid(gridOptions);
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
function renderActivityBodyRows(records, isLicensee) {
    var rows = [];
    var totalRows = records.length;
    for (var i = 0; i < totalRows; i++) {
        var record = records[i];
        var row = {
            Columns: [{ Name: 'row', Value: record.row }]
        };

        if (isLicensee) {
            row.Columns.push({ Name: 'username', Value: record.username });
        }

        row.Columns.push({ Name: 'detail', Value: record.detail });
        row.Columns.push({ Name: 'loggeddate', Value: record.loggedDate });

        rows.push(row);
    }

    return rows;
}

function ShowRecentActivities() {
    if ($('input:radio[name=optActivities]:checked').val() == 1) {
        var url = currentDomainURL + 'Dashboard/ShowRecentActivities?pageSize=' + _pageSize1
            + '&pageIndex=' + _pageIndex1
            + '&optionActivities=' + $('input:radio[name=optActivities]:checked').val();

        $.ajax({
            url: url,
            type: "POST",
            data: "",
            success: function (result) {
                gridOptions1 =
                {
                    columns: [
                        { HeaderText: "#", Width: 40, Name: "row", HeaderAlign: "center", CellAlign: "center" },
                        { HeaderText: "Action", Width: 700, Name: "detail", HeaderAlign: "center", CellAlign: "left" },
                        { HeaderText: "Date", Width: 150, Name: "loggeddate", HeaderAlign: "center", CellAlign: "center" },
                    ],
                    bodyRows: renderActivityBodyRows(result.data, false),
                    gridExpandHeight: 150,
                    showPager: true,
                    customPager: true,
                    fullHeight: false,
                    gridHeight: 205,
                    pagerOption: {
                        itemsPerPage: _pageSize1,
                        currentPage: _pageIndex1,
                        totalItem: result.totalItems,
                        itemsPerPageArray: [5, 10, 20, 50, 100],
                        pagerSelectCallBack: ReRenderAfterPagedRecentActivities1
                    }
                };
                $("#GridRecentActivities").fanexGrid(gridOptions1);
            }
        });
    } else { //get by licensee
        var url = currentDomainURL + 'Dashboard/ShowRecentActivities?pageSize=' + _pageSize2
            + '&pageIndex=' + _pageIndex2
            + '&optionActivities=' + $('input:radio[name=optActivities]:checked').val();
        $.ajax({
            url: url,
            type: "POST",
            data: "",
            success: function (result) {
                gridOptions2 =
                {
                    columns: [
                        { HeaderText: "#", Width: 40, Name: "row", HeaderAlign: "center", CellAlign: "center" },
                        { HeaderText: "Username", Width: 200, Name: "username", TdClass: "boldusername", HeaderAlign: "center", CellAlign: "left" },
                        { HeaderText: "Action", Width: 500, Name: "detail", HeaderAlign: "center", CellAlign: "left" },
                        { HeaderText: "Date", Width: 150, Name: "loggeddate", HeaderAlign: "center", CellAlign: "center" },
                    ],
                    bodyRows: renderActivityBodyRows(result.data, true),
                    gridExpandHeight: 150,
                    showPager: true,
                    customPager: true,
                    fullHeight: false,
                    gridHeight: 205,
                    pagerOption: {
                        itemsPerPage: _pageSize2,
                        currentPage: _pageIndex2,
                        totalItem: result.totalItems,
                        itemsPerPageArray: [5, 10, 20, 50, 100],
                        pagerSelectCallBack: ReRenderAfterPagedRecentActivities2
                    }
                };
                $("#GridRecentActivities").fanexGrid(gridOptions2);
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
    window.location = currentDomainURL + "Affiliate/List";
}

// view total sub-account
function ViewTotalSubAcc() {
    window.location = currentDomainURL + "SubAccount/List";
}

// view all today's request
function ViewTotalReqToday() {
    window.location = currentDomainURL + "Affiliate/Requests";
}

// view all this month's requests
function ViewTotalReqThisMonth() {
    window.location = currentDomainURL + "Affiliate/Requests?curmonth=true";
}