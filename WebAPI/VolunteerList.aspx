<%@ Page Title="" Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" Inherits="VolunteerList" Codebehind="VolunteerList.aspx.cs" %>

 
<asp:Content ID="SublinksSessions" ContentPlaceHolderID="Sublinks" runat="server">

   
 <!-- CSS -->
     <link href="JSProd/resources/extjs/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    
    <!-- Base -->
    <script src="JSProd/resources/extjs/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="JSProd/resources/extjs/ext-all.js" type="text/javascript"></script>

    <asp:Menu ID="subMenu" runat="server" DataSourceID="SiteMapProgram" SkinID="subMenu">
    </asp:Menu>
    <%--The next line should be here, but seems to work without because it is in master page--%>
    <asp:SiteMapDataSource ID="SiteMapProgram" runat="server" ShowStartingNode="False"
        StartingNodeUrl="~/Program.aspx" />
</asp:Content>


<asp:Content ID="SessionsContent" ContentPlaceHolderID="MainContent" runat="server">

    
  
  
    
    
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
 
 <div id='MainDiv1' ></div>


</asp:Content>

