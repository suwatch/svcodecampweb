/*
 * File: app/store/SessionPresenterStore.js
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

Ext.define('RegistrationApp.store.SessionPresenterStore', {
    extend: 'Ext.data.Store',

    requires: [
        'RegistrationApp.model.SessionPresenterModel'
    ],

    constructor: function(cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            model: 'RegistrationApp.model.SessionPresenterModel',
            storeId: 'MyStore2',
            proxy: {
                type: 'rest',
                url: '/rest/SessionPresenter'
            }
        }, cfg)]);
    }
});