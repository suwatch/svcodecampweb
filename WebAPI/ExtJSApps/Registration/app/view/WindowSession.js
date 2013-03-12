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

    height: 630,
    width: 836,
    layout: {
        type: 'fit'
    },
    title: 'Session Editor',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'button',
                            text: 'Save'
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'panel',
                    id: 'SessionEditorPanelId',
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
                            title: 'Session Tags',
                            store: 'TagStore',
                            viewConfig: {

                            },
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    width: 160,
                                    dataIndex: 'tagName',
                                    flex: 1,
                                    text: 'TagName'
                                },
                                {
                                    xtype: 'booleancolumn',
                                    width: 60,
                                    dataIndex: 'taggedInSession',
                                    text: 'Tagged'
                                }
                            ],
                            selModel: Ext.create('Ext.selection.CheckboxModel', {
                                listeners: {
                                    selectionchange: {
                                        fn: me.onCheckboxselectionmodelSelectionChange,
                                        scope: me
                                    }
                                }
                            })
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    onCheckboxselectionmodelSelectionChange: function(model, selected, options) {
        // all records that are selected are passed in here.  we need to run through the store
        // itself and verify that is what we think we should have.
        var tagsSelected1 = [];
        Ext.each(selected,function(rec) {
            tagsSelected1.push(rec.getData().tagName); 
        });


        // find record in store and update it
        var tagList = Ext.getCmp("SessionTagsGridPanelId");
        var tagListStore = tagList.store;

        tagListStore.each(function(storeRec) {
            var storeRecTagName = storeRec.getData().tagName;
            var storeRecTaggedInSession = storeRec.getData().taggedInSession;
            var gridPanelTagged = Ext.Array.contains(tagsSelected1,storeRecTagName);

            if (storeRecTaggedInSession != gridPanelTagged) {
                storeRec.set("taggedInSession",gridPanelTagged);
            }
        });

    }

});