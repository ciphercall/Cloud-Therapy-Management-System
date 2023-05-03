
$(function () {
    $("#menugrid").jqGrid({
        url: "/ASL_MENU/GetTodoLists",
        editurl: '/ASL_MENU/Edit',

        postData: {
            ModId: function () { return jQuery("#txtModId").val(); },
            MenuType: function () { return jQuery("#ddlMtype option:selected").val(); },
        },
        editData: {
            ModId: $('#txtModId').val(),
            MenuType: $('#ddlMtype option:selected').val()
        },

        datatype: 'json',
        mtype: 'Get',
        colNames: ['Action', 'Id', 'Serial', 'Menu Id', 'Menu Name','Action Name','Controller Name'],
        colModel: [
            {
                name: "Edit Actions",
                width: 40,
                formatter: "actions",
                formatoptions: {
                    keys: false,
                    editbutton: true,
                    delbutton: true,
                    editOptions: {},
                    addOptions: {},
                    delOptions: {
                        url: "/ASL_MENU/Delete"
                    },
                    afterSave: function (rowid) {
                        $('#pager').show();
                        alert("record saved!");
                    },
                    afterRestore: function (rowid) {
                        $('#pager').show();
                        return false;
                    }
                }
            },
            { key: true, hidden: true, name: 'Id', index: 'Id', editable: true },
            { key: false, name: 'SERIAL', index: 'SERIAL', editable: true, width: 50 },
            { key: false, name: 'MENUID', index: 'MENUID', editoptions: { readonly: "readonly" }, editable: true, width: 50 },
            { key: false, name: 'MENUNM', index: 'MENUNM', editable: true, width: 100 },
            { key: false, name: 'ACTIONNAME', index: 'ACTIONNAME', editable: true, width: 100 },
            { key: false, name: 'CONTROLLERNAME', index: 'CONTROLLERNAME', editable: true, width: 100 }],
        pager: jQuery('#pager'),
        onSelectRow: editRow,
        rowNum: 15,
        rowList: [15, 20, 30, 40],
        height: '100%',
        viewrecords: true,
        caption: 'Menu List',
        emptyrecords: 'No records to display',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        autowidth: true,
        multiselect: false
        
    }).navGrid('#pager', { edit: false, add: true, del: false, search: false, refresh: true },
        {
            // edit options
            zIndex: 100,
            url: '/ASL_MENU/Edit',
            editData: {
                ModId: function () { return $('#txtModId').val(); },
                MenuType: function () { return $('#ddlMtype option:selected').val(); }
            },
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            // add options
            zIndex: 100,
            url: "/ASL_MENU/Create",

            editData: {
                ModId: function () { return $('#txtModId').val(); },
                MenuType: function () { return $('#ddlMtype option:selected').val(); }
            },
            closeOnEscape: true,
            closeAfterAdd: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            // delete options
            zIndex: 100,
            url: "/ASL_MENU/Delete",
            closeOnEscape: true,
            closeAfterDelete: true,
            recreateForm: true,
            msg: "Are you sure you want to delete this task?",
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        });
    var lastSelection;

    function editRow(id) {
        if (id && id !== lastSelection) {
            var grid = $("#menugrid");
            grid.restoreRow(lastSelection);
            grid.editRow(id, true);
            lastSelection = id;
        }
    }

});