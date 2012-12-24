<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" Inherits="SessionRoomUpdateExt"
    Title="Membership CodeCamp SFBA" Codebehind="SessionRoomUpdateExt.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Membership With ExtJS</title>
    
    <link href="JSProd/resources/extjs/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <link type="text/css" href="JSProd/resources/css/app.basic.css" rel="stylesheet" />
  
    
    <!-- Base -->
    <script src="JSProd/resources/extjs/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="JSProd/resources/extjs/ext-all.js" type="text/javascript"></script>
  
    
    
<script type="text/javascript">
    Ext.BLANK_IMAGE_URL = 'JSProd/resources/extjs/resources/images/default/s.gif';
    Ext.QuickTips.init();

    Ext.namespace('ASPNETProfile');

    ASPNETProfile.memberProxy = new Ext.data.HttpProxy({
        prettyUrls: false,
        api: {
            read: 'GeneralHandlers/Sessions.ashx',
            //create: 'Create.ashx',
            update: 'GeneralHandlers/SessionsSave.ashx',
            //destroy: 'Delete.ashx'
        }
    });

    ASPNETProfile.memberRecord = Ext.data.Record.create(
        [
            { name: 'Title' },
            { name: 'PlanAheadCount' },
            { name: 'PlanAheadCountInt',  },
            { name: 'LectureRoomsId', allowBlank: false },
            { name: 'SessionTimesId', allowBlank: false },
            { name: 'CodeCampYearId', allowBlank: false },
            { name: 'RoomNumber', allowBlank: false },
            { name: 'RoomNumberNew', allowBlank: false },
            { name: 'SessionTime', allowBlank: false },
            { name: 'TitleWithPlanAttend', allowsBlank: false },
            { name: 'Id' }
        ]);

    ASPNETProfile.memberReader = new Ext.data.JsonReader(
{
    totalProperty: 'total',   //   the property which contains the total dataset size (optional)
    idProperty: 'Id',
    root: 'rows',
    successProperty: 'success'
},
        ASPNETProfile.memberRecord
        );

    ASPNETProfile.memberWriter = new Ext.data.JsonWriter({
        returnJson: true,
        writeAllFields: true
    });

    ASPNETProfile.memberStore = new Ext.data.Store({
        id: 'user',
        root: 'records',
        proxy: ASPNETProfile.memberProxy,
        reader: ASPNETProfile.memberReader,
        writer: ASPNETProfile.memberWriter,
        paramsAsHash: true,
        batchSave: false,
        autoLoad: false,
        autoSave: false,
         listeners: {
           
            write : function(store, action, result, res, rs) {
                App.setAlert(res.success, res.message); // <-- show user-feedback for all write actions
            },
            exception : function(proxy, type, action, options, res, arg) {
              if (type === 'response') {
                
                if (Ext.util.JSON.decode(res.responseText).success == true)
                {
                    var message = Ext.util.JSON.decode(res.responseText).message;
                    Ext.Msg.show({
                        title: 'Success is true',
                        msg: message,
                        icon: Ext.MessageBox.INFO,
                        buttons: Ext.Msg.OK
                    });
                    ASPNETProfile.memberStore.commitChanges();  
                }
                else
                {
                   var message = Ext.util.JSON.decode(res.responseText).message;
                   Ext.Msg.show({
                        title: 'Success is false',
                        msg: message,
                        icon: Ext.MessageBox.INFO,
                        buttons: Ext.Msg.OK
                    });
                }
            }
        }
    }

    });

    Ext.onReady(function() {

        var query = {};
        function OnSearch(btn, ev) {
        
        
            ASPNETProfile.memberStore.load({
                params:
            {
                EmailContains: 'pkellner'
            }
            });
        }

        var grid = new Ext.grid.EditorGridPanel({
            renderTo: 'MainDiv1',
            store: ASPNETProfile.memberStore,
            frame: true,
            title: 'GridPanel',
            height: 800,
            width: 1100,
            loadMask: true,

            tbar: [
            {
                iconCls: './grayBtn', // does not seem to work???
                xtype: 'button',
                text: 'SessionTimesId',
                handler: function(btn) {
                    var textFieldValue = this.ownerCt.getComponent('searchTextId').getValue();
                    var query = {}; query.SessionTimesId = textFieldValue;
                    ASPNETProfile.memberStore.load({
                        params:
                    {
                        query: Ext.util.JSON.encode(query)
                    }
                    });
                }
            },
            {
                xtype: 'textfield',
                name: 'searchEmailField',
                itemId: 'searchTextId',
                emptyText: 'Search Email Here'
            },
            {
                ref: '../saveBtn',
                iconCls: 'icon-user-save',
                text: 'Save Changes',
                handler: function() {
                    ASPNETProfile.memberStore.save();
                }
            }
            ],

            columns: [
                { header: 'PA Cnt', dataIndex: 'PlanAheadCountInt', sortable: true},
                { header: 'Title', dataIndex: 'TitleWithPlanAttend', width: 400 },
                { header: 'RoomNumber', dataIndex: 'RoomNumber' },
                 { header: 'RoomNumberNew', dataIndex: 'RoomNumberNew', editor: { xtype: 'textfield', allowBlank: false} },
                { header: 'SessionTime', dataIndex: 'SessionTime' },
                { header: 'LectureRoomsId', dataIndex: 'LectureRoomsId' },
                { header: 'SessionTimesId', dataIndex: 'SessionTimesId' },
                { header: 'CodeCampYearId', dataIndex: 'CodeCampYearId' },
                { header: 'Id', dataIndex: 'Id' }
            ]
        });

        //query.CodeCampYearId = 4;  (built in already)      
        ASPNETProfile.memberStore.load({
            params:
            {
                //query:  Ext.util.JSON.encode(query)
            }
        });
    })
    
   
   


</script> 
 
</head>
<body>
    <form id="form1" runat="server">
        <br />
         <a href="Home.aspx">Home.aspx</a>
        <br /><br />
        <div id='MainDiv1' >   
        </div>
    </form>
</body>
</html>
