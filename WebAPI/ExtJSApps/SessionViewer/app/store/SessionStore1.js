/*
 * File: app/store/SessionStore1.js
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

Ext.define('SessionApp.store.SessionStore1', {
    extend: 'Ext.data.Store',

    requires: [
        'SessionApp.store.override.SessionStore1',
        'Ext.data.writer.Json',
        'SessionApp.model.SessionModel'
    ],

    constructor: function(cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            storeId: 'MyArrayStore1',
            model: 'SessionApp.model.SessionModel',
            pageSize: 10
        }, cfg)]);
    }
});