/*
 * File: app/store/StoreEmailSelection.js
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

Ext.define('App.store.StoreEmailSelection', {
    extend: 'Ext.data.Store',

    requires: [
        'App.model.ModelEmailSelection'
    ],

    constructor: function(cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            storeId: 'StoreEmailSelection',
            model: 'App.model.ModelEmailSelection',
            proxy: {
                type: 'rest',
                url: '/rpc/Email/UsersBySql'
            }
        }, cfg)]);
    }
});