<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" Inherits="MembershipExt"
    Title="Membership CodeCamp SFBA" Codebehind="MembershipExt.aspx.cs" %>

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
            read: 'MembershipHandlers/Users.ashx',
            create: 'Create.ashx',
            update: 'Save.ashx',
            destroy: 'Delete.ashx'
        }
    });

    ASPNETProfile.memberRecord = Ext.data.Record.create(
        [
            { name: 'PKID' },
            { name: 'Username', allowBlank: false },
            { name: 'UserFirstName', allowBlank: false },
            { name: 'UserLastName', allowBlank: false },
            { name: 'UserZipCode' },
            { name: 'Email', allowBlank: false },
            { name: 'IsApproved', type: 'bool' },
            { name: 'CreationDate', type: 'date', dateFormat: 'n/j/Y' },
            { name: 'LastLoginDate', type: 'date', dateFormat: 'n/j/Y' },
            { name: 'Comment' }
        ]);

    ASPNETProfile.memberReader = new Ext.data.JsonReader(
{
    totalProperty: 'total',   //   the property which contains the total dataset size (optional)
    idProperty: 'ProviderUserKey',
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
        autoLoad: false

    });

    Ext.onReady(function() {


        var pageSize = 25;


        function OnSearch(btn, ev) {
        
        
            ASPNETProfile.memberStore.load({
                params:
            {
                EmailContains: 'pkellner'
            }
            });
        }

        var grid = new Ext.grid.GridPanel({
            renderTo: 'MainDiv1',
            store: ASPNETProfile.memberStore,
            frame: true,
            title: 'GridPanel',
            height: 700,
            width: 700,
            loadMask: true,

            tbar: [
            {
                iconCls: './grayBtn', // does not seem to work???
                xtype: 'button',
                text: 'Search',
                handler: function(btn) {
                  var textFieldValue = this.ownerCt.getComponent('searchTextId').getValue();
                  ASPNETProfile.memberStore.load({
                    params:
                    {
                        EmailContains: textFieldValue
                    }
                  });
                }
            },
            {
                xtype: 'textfield',
                name: 'searchEmailField',
                itemId: 'searchTextId',
                emptyText: 'Search Email Here'
            }
            ],

            bbar: new Ext.PagingToolbar({
                pageSize: pageSize,
                store: ASPNETProfile.memberStore,
                displayInfo: true,
                displayMsg: 'Displaying topics {0} - {1} of {2}',
                emptyMsg: "No News Users to display"
            }),

            columns: [
                { header: 'UserFirstName', dataIndex: 'UserFirstName' },
                { header: 'UserLastName', dataIndex: 'UserLastName' },
                { header: 'Email', dataIndex: 'Email' },
                { header: 'Username', dataIndex: 'Username' },
                { header: 'UserZipCode', dataIndex: 'UserZipCode' },
                {
                    xtype: 'datecolumn',
                    format: 'm/d/Y',
                    header: 'CreationDate',
                    dataIndex: 'CreationDate'
                },
                 {
                     xtype: 'datecolumn',
                     format: 'm/d/Y',
                     header: 'LastLoginDate',
                     dataIndex: 'LastLoginDate'
                 }


            ]
        });



        ASPNETProfile.memberStore.load({
            params:
            {
                start: 0,
                limit: pageSize
            }
        });
    })
    
   
   


</script> 
 
</head>
<body>
    <form id="form1" runat="server">
    
        <asp:Button ID="ButtonAssignPresentersRolePresenter" runat="server" Text="Assign Role presenter to all presenters" OnClick="ButtonAssignPresentersRolePresenter_Click" />
        
        <br />
        
         <a href="Home.aspx">Home.aspx</a>
        
    <br />
        <div id='MainDiv1' >
        
           
       
        </div>
    </form>
</body>
</html>
