var _affId = 0;
var _affName = "";
var gridOptions,
    _pageIndex = 0,
    _pageSize = 10;

$(document).ready(function () {
    var successed = $("#successed").val();
    if (successed == "true") {
        confirm('Successfull!\nPress OK to continue', function () {
            document.location =currentDomainURL + "Affiliate/List";
        });
    }

    $("#Username").Watermark("Username");

    $("#Username").autocomplete({
        source: [],
        minLength: 1
    });

    $("#Username").keyup(function (e) {
        if (e.keyCode == $.ui.keyCode.ENTER) {
            Search();
        }
    });
    
    $("#DomainName").keyup(function (e) {
        if (e.keyCode == $.ui.keyCode.ENTER) {
            Search();
        }
    });

    Search();
});

//--------------------------------------------------------------------------------------------------------------------
function OnSearch() {
    _pageIndex = 0;
    _pageSize = 10;

    Search();
};

function renderBodyRows(affiliates) {
    var rows = [];
    var totalRows = affiliates.length;
    for (var i = 0; i < totalRows; i++) {
        var affiliate = affiliates[i];
        var colactive = '';
        if ($("#IsActivateSystem").val() !== '') {
            if (affiliate.isAllowed) {
                colactive = "<img title='Deactivate this account' class='ico-tick' alt='' onclick='editActive(this," + affiliate.id + ")'/>";
            } else {
                colactive = "<img title='Activate this account' class='ico-untick' alt='' onclick='editActive(this," + affiliate.id + ")'/>";
            }
        }
        else {
            colactive = "<img class='ico-untick-deactivate' />";
        }

        var row = {
            Columns: [
                { Name: 'Row', Value: affiliate.row },
                { Name: 'Username', Value: affiliate.name },
                {
                    Name: 'Action', Value: "<div class='btn-action' onclick='javascript:ShowAction("
                        + JSON.stringify({ id: affiliate.id, name: affiliate.name }) + ");'></div>"
                },
                { Name: 'IsAllowed', Value: colactive },
                { Name: 'Site', Value: affiliate.site },
                { Name: 'CreatedDate', Value: affiliate.createdDate },
                { Name: 'LastUpdate', Value: affiliate.lastUpdate },
                { Name: 'Creator', Value: affiliate.creator },
                { Name: 'ContactPerson', Value: affiliate.contactPerson },
                { Name: 'ContactEmail', Value: affiliate.contactEmail }
            ]
        };

        rows.push(row);
    }

    return rows;
}

function Search() {
    $('#PageSize').val(_pageSize);
    $('#PageIndex').val(_pageIndex);
    
    var searchParams = {
        UserName: $("#Username").val(),
        IsActive: $("#isActive option:selected").val(),
        PageSize: _pageSize,
        PageIndex: _pageIndex
    };

    $.ajax({
        url: "GetAffiliateList",
        type: 'POST',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(searchParams),
        cache: false,
        success: function (result) {
            gridOptions =
            {
                columns: [
                    { HeaderText: "#", Width: 40, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Username", Width: 120, Name: "Username", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    { HeaderText: "Action", Width: 50, Name: "Action", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Active", Width: 70, Name: "IsAllowed", HeaderAlign: "center", CellAlign: "center", CssClass: "header-mt", Sort: true },
                    { HeaderText: "Site", Width: 170, Name: "Site", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    {
                        HeaderText: "Created Date", Width: 120, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center", CssClass: "header-mt",
                        Sort: true, FormatDateTime: 'DD MMM YYYY HH:mm:ss', CustomSortFunction: function (col1Value, col2Value, sortDirection) {
                            if (sortDirection === 'asc') {
                                return moment(col1Value).diff(col2Value);
                            } else {
                                return moment(col2Value).diff(col1Value);
                            }
                        }                    },
                    {
                        HeaderText: "Last Update", Width: 120, Name: "LastUpdate", HeaderAlign: "center", CellAlign: "center", CssClass: "header-mt",
                        Sort: true, FormatDateTime: 'DD MMM YYYY HH:mm:ss', CustomSortFunction: function (col1Value, col2Value, sortDirection) {
                            if (sortDirection === 'asc') {
                                return moment(col1Value).diff(col2Value);
                            } else {
                                return moment(col2Value).diff(col1Value);
                            }
                        }                    },
                    { HeaderText: "Creator", Width: 120, Name: "Creator", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true, CssClass: "header-mt", Sort: true },
                    { HeaderText: "Contact Person", Width: 120, Name: "ContactPerson", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true, CssClass: "header-mt", Sort: true },
                    { HeaderText: "Contact Email", Width: 170, Name: "ContactEmail", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true }
                ],
                bodyRows: renderBodyRows(result.data),
                hideYBackGroundScroller: true,
                gridExpandHeight: 250,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: _pageSize,
                    currentPage: _pageIndex,
                    totalItem: result.totalItems,
                    itemsPerPageArray: [10, 20, 50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            $("#SearchResult").fanexGrid(gridOptions);
        }
    });
}

function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
    if (itemsPerPage != $('#PageSize').val()) {
        $('#PageIndex').val(0);
        _pageIndex = 0;
    } else {
        $('#PageIndex').val(page_index);
        _pageIndex = page_index
    }

    $('#PageSize').val(itemsPerPage);
    _pageSize = itemsPerPage;

    gridOptions.pagerOption.currentPage = page_index;
    Search();

    return false;
}

function ShowAction(affiliate) {
    _affId = affiliate.id;
    _affName = affiliate.name;

    $.contextMenu({
        selector: '.btn-action',
        items: {
            deleteAffiliate: {
                name: "Delete this affiliate",
                height: 25,
                className: "iconDelAff",
                callback: function (key, opt) {
                    deleteAffiliate(_affId, _affName);
                }
            },
            editSetting: {
                name: "Edit authentication settings",
                height: 25,
                className: "iconEditSetting",
                callback: function (key, opt) {
                    var updateParam = $.param({
                        id: _affId,
                        name: _affName
                    });

                    window.location = currentDomainURL + "Affiliate/EditSetting?" + updateParam;
                }
            },
            editInfo: {
                name: "Edit information",
                height: 25,
                className: "iconEditInfo",
                callback: function (key, opt) {
                    var updateParam = $.param({
                        userName: _affName,
                    });

                    window.location = currentDomainURL + "Affiliate/EditAffiliateInfo/" + _affId+ "?" + updateParam;
                }
            },
            editProfile: {
                name: "Edit profile",
                className: "iconOddProfile",
                callback: function (key, opt) {
                    var updateParam = $.param({
                        name: _affName
                    });

                    window.location = currentDomainURL + "Affiliate/EditProfile/" + _affId + "?" + updateParam;
                }
            },
            viewXMLURLs: {
                name: "View XML URLs",
                className: "iconViewXMLURLs",
                callback: function (key, opt) {
                    var updateParam = $.param({
                        name: _affName
                    });

                    window.location = currentDomainURL + "Affiliate/ViewXMLURLs_Edit/" + _affId + "?" + updateParam;
                }
            }
        },
        trigger: "left",
        ignoreRightClick: true,
        autoHide: true
    });
}

function StartAutoSuggesttion() {
    var temp = $("#Username").val()
    var url = 'ListExistedAff?username=' + temp;

    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'html',
        cache: false,
        success: function (result) {
            var sourceAdmin = result.split(',');

            $("#Username").autocomplete({
                source: sourceAdmin,
                minLength: 1
            });
        },
        error: function (xhr, status, err) {
        }
    });
}