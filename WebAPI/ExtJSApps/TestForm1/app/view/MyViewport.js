/*
 * File: app/view/MyViewport.js
 *
 * This file was generated by Sencha Architect version 2.2.0.
 * http://www.sencha.com/products/architect/
 *
 * This file requires use of the Ext JS 4.2.x library, under independent license.
 * License of Sencha Architect does not include license for Ext JS 4.2.x. For more
 * details see http://www.sencha.com/license or contact license@sencha.com.
 *
 * This file will be auto-generated each and everytime you save your project.
 *
 * Do NOT hand edit this file.
 */

Ext.define('MyApp.view.MyViewport', {
    extend: 'Ext.container.Viewport',

    id: 'myViewPortId',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'gridpanel',
                    title: 'My Grid Panel',
                    store: 'MyStore',
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'tagName',
                            text: 'TagName'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'tagSelected',
                            text: 'TagSelected'
                        }
                    ],
                    listeners: {
                        afterrender: {
                            fn: me.onGridpanelAfterRender,
                            scope: me
                        }
                    },
                    selModel: Ext.create('Ext.selection.CheckboxModel', {

                    })
                }
            ]
        });

        me.callParent(arguments);
    },

    onGridpanelAfterRender: function(component, eOpts) {

        component.getStore().loadData(
        [
        { tagName: 'tag1',tagSelected: true },
        { tagName: 'tag2',tagSelected: false },
        { tagName: 'tag3',tagSelected: true }
        ]
        );
    }

});