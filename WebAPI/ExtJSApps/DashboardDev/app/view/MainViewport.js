/*
 * File: app/view/MainViewport.js
 *
 * This file was generated by Sencha Architect version 2.1.0.
 * http://www.sencha.com/products/architect/
 *
 * This file requires use of the Ext JS 4.1.x library, under independent license.
 * License of Sencha Architect does not include license for Ext JS 4.1.x. For more
 * details see http://www.sencha.com/license or contact license@sencha.com.
 *
 * This file will be auto-generated each and everytime you save your project.
 *
 * Do NOT hand edit this file.
 */

Ext.define('App.view.MainViewport', {
    extend: 'Ext.container.Viewport',

    layout: {
        type: 'border'
    },

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 5,
                    region: 'center',
                    items: [
                        {
                            xtype: 'form',
                            id: 'mailGeneratorFormPanelId',
                            padding: '5 5 5 5',
                            autoScroll: true,
                            bodyPadding: '5 5 5 5',
                            title: 'Mail Generator',
                            dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    dock: 'top',
                                    items: [
                                        {
                                            xtype: 'button',
                                            id: 'GenerateEmailButtonId',
                                            text: 'Generate'
                                        },
                                        {
                                            xtype: 'tbseparator'
                                        },
                                        {
                                            xtype: 'button',
                                            id: 'previewLetterButtonId',
                                            text: 'Preview Letter'
                                        },
                                        {
                                            xtype: 'textfield',
                                            id: 'previewEmailToId',
                                            width: 332,
                                            name: 'previewEmailSend',
                                            value: 'peter@peterkellner.net',
                                            fieldLabel: 'Send Preview To'
                                        }
                                    ]
                                }
                            ],
                            items: [
                                {
                                    xtype: 'textfield',
                                    anchor: '100%',
                                    name: 'emailUrl',
                                    value: 'http://pkellner.site44.com/',
                                    fieldLabel: 'URL Of Letter',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    anchor: '100%',
                                    name: 'subject',
                                    value: 'Announcing SV Code Camp Version 8! October 5th and 6th, 2013',
                                    fieldLabel: 'Subject (Required)',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    anchor: '100%',
                                    name: 'mailBatchLabel',
                                    fieldLabel: 'Mail Batch Label',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'htmleditor',
                                    anchor: '100%',
                                    height: 89,
                                    margin: '10 0 0 0',
                                    style: 'background-color: white;',
                                    name: 'subjectHtml',
                                    fieldLabel: 'Subject HTML (optional)'
                                },
                                {
                                    xtype: 'htmleditor',
                                    anchor: '100%',
                                    height: 150,
                                    margin: '15 0 0 0',
                                    style: 'background-color: white;',
                                    name: 'emailHtml',
                                    fieldLabel: 'Content (HTML)'
                                },
                                {
                                    xtype: 'textareafield',
                                    anchor: '100%',
                                    height: 158,
                                    margin: '30 0 0 0',
                                    name: 'sqlFilter',
                                    value: 'select id from attendees where id > 900 and id < 1000',
                                    fieldLabel: 'Sql Statement',
                                    allowBlank: false
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            title: 'Admin Users',
                            items: [
                                {
                                    xtype: 'gridpanel',
                                    id: 'GridPanelAdminUsersId',
                                    store: 'StoreAdminUsers',
                                    viewConfig: {

                                    },
                                    columns: [
                                        {
                                            xtype: 'gridcolumn',
                                            width: 181,
                                            dataIndex: 'username',
                                            text: 'Username'
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'roles',
                                            flex: 1,
                                            text: 'Roles'
                                        }
                                    ],
                                    dockedItems: [
                                        {
                                            xtype: 'toolbar',
                                            dock: 'top',
                                            items: [
                                                {
                                                    xtype: 'button',
                                                    id: 'ReLoadAdminUsersButton',
                                                    text: 'ReLoadGrid'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'gridpanel',
                    flex: 2,
                    region: 'east',
                    id: 'attendeesListGridPanelId',
                    width: 150,
                    autoScroll: true,
                    title: 'Attendee List',
                    store: 'StoreEmailSelection',
                    viewConfig: {
                        autoScroll: false
                    },
                    dockedItems: [
                        {
                            xtype: 'toolbar',
                            dock: 'top',
                            items: [
                                {
                                    xtype: 'button',
                                    id: 'loadAttendeesButtonId',
                                    text: 'Load Attendees'
                                },
                                {
                                    xtype: 'tbfill'
                                },
                                {
                                    xtype: 'label',
                                    margins: '0 10 0 0',
                                    id: 'attendeeListToolBarCntId',
                                    text: 'Cnt'
                                }
                            ]
                        }
                    ],
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            width: 41,
                            dataIndex: 'id',
                            text: 'Id'
                        },
                        {
                            xtype: 'gridcolumn',
                            width: 150,
                            dataIndex: 'email',
                            text: 'Email'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'userFirstName',
                            text: 'UserFirstName'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'userLastName',
                            text: 'UserLastName'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'username',
                            text: 'Username'
                        }
                    ],
                    plugins: [
                        Ext.create('Ext.grid.plugin.CellEditing', {
                            ptype: 'cellediting'
                        })
                    ]
                },
                {
                    xtype: 'gridpanel',
                    region: 'south',
                    height: 200,
                    title: 'Email Batches',
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'string',
                            text: 'String'
                        },
                        {
                            xtype: 'numbercolumn',
                            dataIndex: 'number',
                            text: 'Number'
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'date',
                            text: 'Date'
                        },
                        {
                            xtype: 'booleancolumn',
                            dataIndex: 'bool',
                            text: 'Boolean'
                        }
                    ],
                    viewConfig: {
                        height: 400,
                        autoScroll: true
                    }
                }
            ]
        });

        me.callParent(arguments);
    }

});