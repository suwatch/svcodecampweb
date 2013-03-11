/*
 * File: app/view/WindowSession.js
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

Ext.define('RegistrationApp.view.WindowSession', {
    extend: 'Ext.window.Window',

    height: 517,
    width: 578,
    layout: {
        type: 'fit'
    },
    title: 'Session Editor',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    id: 'MainPanel',
                    layout: {
                        type: 'border'
                    },
                    items: [
                        {
                            xtype: 'form',
                            flex: 6,
                            region: 'center',
                            id: 'sessionFormPanelEditorId',
                            bodyPadding: 10,
                            items: [
                                {
                                    xtype: 'fieldset',
                                    itemId: 'FieldSetSessionEditorItemId',
                                    title: 'Basic Session Information',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            anchor: '100%',
                                            width: 150,
                                            name: 'title',
                                            fieldLabel: 'Title'
                                        },
                                        {
                                            xtype: 'textfield',
                                            anchor: '100%',
                                            width: 150,
                                            name: 'title',
                                            fieldLabel: 'Hash Tags For Twitter'
                                        },
                                        {
                                            xtype: 'combobox',
                                            anchor: '100%',
                                            fieldLabel: 'Session Level'
                                        },
                                        {
                                            xtype: 'textareafield',
                                            anchor: '100%',
                                            height: 100,
                                            fieldLabel: 'Session Description (just text, no html please)'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'gridpanel',
                            flex: 2,
                            region: 'east',
                            id: 'SessionTagsGridPanelId',
                            width: 150,
                            autoScroll: true,
                            title: 'My Grid Panel',
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

                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});