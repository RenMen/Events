﻿$(function () {

    $("#Template").data("kendoDropDownList").wrapper.hide();// call for hide kendo dropdown call
    $("#Filter").data("kendoDropDownList").wrapper.hide();
    $("#grdInvitee").data('kendoGrid').wrapper.hide();
    $("#windowSelected").kendoWindow({
        width: "400px",
        title: "Sent to Selected Invitees",
        visible: false,
        actions: [
            //"Pin",
           // "Minimize",
           // "Maximize",
            "Close"
        ],
        //close: onClose
    });
    $("#windowAll").kendoWindow({
        width: "400px",
        title: "Sent to All Invitees",
        visible: false,
        actions: [
            //"Pin",
            // "Minimize",
            // "Maximize",
            "Close"
        ],
        //close: onClose
    });

});

function PostSelectedIDs() {
    var grid = $("#grdInvitee").data("kendoGrid");
    var dataItem = [];
    //var a = "test";
    grid.select().each(function () {
        //dataItem.push(grid.dataItem($(this)));
        // dataItem.push( grid.dataItem($(this)).toJSON());
        dataItem.push(grid.dataItem($(this)).Id);
    });
    if (dataItem.length > 0) {

        //console.log(JSON.stringify(dataItem));
        $.ajax({
            method: "POST",
            url: "/Invitee/SendMessage",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataItem)//JSON.stringify({dataItem })
        }).done(function (data) {
            console.log(data);
        });
    }
    else {
        var myWindow = $("#windowSelected");
        myWindow.data("kendoWindow").center().open();
    }

}
function PostAllIDs() {
    var grid = $("#grdInvitee").data("kendoGrid");
    var dataItem = [];
      
    var data = grid.dataSource.data();
    var totalNumber = data.length;

    for (var i = 0; i < totalNumber; i++) {
        var currentDataItem = data[i];
       // dataItem[i] = currentDataItem.Id;
        dataItem.push(currentDataItem.Id);
    }

    
    //var a = "test";
    //grid.dataSource.data().each(function () {
    //    //dataItem.push(grid.dataItem($(this)));
    //    // dataItem.push( grid.dataItem($(this)).toJSON());
    //    dataItem.push(grid.dataItem($(this)).Id);
    //});
    if (dataItem.length > 0) {

        //console.log(JSON.stringify(dataItem));
        $.ajax({
            method: "POST",
            url: "/Invitee/SendMessage",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataItem)//JSON.stringify({dataItem })
        }).done(function (data) {
            console.log(data);
        });
    }
    else {
        var myWindow = $("#windowAll");
        myWindow.data("kendoWindow").center().open();
    }

}

function onGridRowSelection(arg) {
    //var selected = $.map(this.select(), function (item) {
    //    return $(item).text();
    //});
    var selectedRows = this.select();
    $("#selectedCount").html(selectedRows.length);

}

function getTemplateParameters(e) {

}


function additionalData(e) {
    var EventID = $("#Event").data("kendoDropDownList").value();
    var InvType = $("#Filter").data("kendoDropDownList").value();
    if (InvType === '') { InvType = 0;}

    return { eid: EventID, InvTypeID: InvType }; // send the event id value as part of the Read request
}
function onEventChange() {
    var eid = this.value();
    if (eid === '') {
        eid = null;
    }
    $("#newInvitee").attr('href', '/Invitee/Create?eid=null');
    $("#uploadInvitee").attr('href', '/Upload?eid=null');

    //Reset the grid datasource and hide
    $("#grdInvitee").data('kendoGrid').dataSource.data([]);
    $("#grdInvitee").data('kendoGrid').wrapper.hide();

    //reset the select of related dropdownlist and hide
    var Filterdropdownlist = $("#Filter").data("kendoDropDownList");
    Filterdropdownlist.text(Filterdropdownlist.options.optionLabel);
    Filterdropdownlist._oldIndex = 0;
    Filterdropdownlist.wrapper.hide();

    var Templatedropdownlist = $("#Template").data("kendoDropDownList");
    Templatedropdownlist.text(Templatedropdownlist.options.optionLabel);
    Templatedropdownlist._oldIndex = 0;
    Templatedropdownlist.wrapper.show();

    //$("#Template").data("kendoDropDownList").wrapper.show();
}
function onTemplateChange() {
    var eid = this.value();
    if (eid !== '') {

        $("#Filter").data("kendoDropDownList").wrapper.show();

    }
}
 
function onFilterChange() {
    var id = this.value();
    var eid = $("#Event").data("kendoDropDownList").value();
    if (id !== '') {
        $("#grdInvitee").data('kendoGrid').wrapper.show();
        $("#newInvitee").attr('href', '/Invitee/Create?eid=' + eid);
        $("#uploadInvitee").attr('href', '/Upload?eid=' + eid);
        var grid = $("#grdInvitee").data("kendoGrid");
        grid.dataSource.read(); // rebind the Grid's DataSource
        //$.get('/Invitee/ReadInvitees', { eid: eid }, function (data) { // create a control to read all invitees who have no records in intimation table or all invitees  based on filter selection
        //    var grid = $("#grdInvitee").data("kendoGrid");
        //    grid.dataSource.read(); // rebind the Grid's DataSource
        //})
    }
    else {
        $("#grdInvitee").data('kendoGrid').dataSource.data([]);
        $("#grdInvitee").data('kendoGrid').wrapper.hide();
    }
}