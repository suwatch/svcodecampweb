/*
 * File: app.js
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

Ext.Loader.setConfig({
    enabled: true
});

Ext.application({

    requires: [
        'Ext.ux.data.PagingMemoryProxy',
        'Ext.grid.Panel',
        'Ext.data.reader.Json',
        'Ext.toolbar.Paging'
    ],
    models: [
        'SessionModel'
    ],
    stores: [
        'SessionStore1'
    ],
    views: [
        'MainViewport'
    ],
    autoCreateViewport: true,
    name: 'SessionApp',

    launch: function() {

        //SessionApp.sessionData = [[103],[104],[105]];
    }

});
