/*
 * File: app/controller/UserInvoiceArticleController.js
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

Ext.define('App.controller.UserInvoiceArticleController', {
    extend: 'Ext.app.Controller',

    onReLoadGridButtonClick: function(button, e, options) {

        this.refreshData();
    },

    onGridIdAfterRender: function(abstractcomponent, options) {
        var form = Ext.getCmp("UserInvoiceArticleTabId");
        var that = this;
        form.mon(form.el, {
            keypress : function(e, t, opts) {
                if (e.getKey() == e.ENTER) {
                    that.refreshData();
                }
            }}
            );
    },

    refreshData: function() {

        var numberDays =  Ext.getCmp('numberDaysId').getValue();
        var searchUsername =  Ext.getCmp('searchNameId').getValue();
        var store = Ext.getCmp('gridId').store;
        store.load({
            params: {
                usernameMatch: searchUsername,
                daysToGoBack: numberDays,
                maxUsersToReturn: '100'
            }
        });

    },

    init: function(application) {
        this.control({
            "#reLoadGridButton": {
                click: this.onReLoadGridButtonClick
            },
            "#gridId": {
                afterrender: this.onGridIdAfterRender
            }
        });
    }

});