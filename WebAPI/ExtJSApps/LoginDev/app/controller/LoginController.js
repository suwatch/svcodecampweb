/*
 * File: app/controller/LoginController.js
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

Ext.define('App.controller.LoginController', {
    extend: 'Ext.app.Controller',

    onButtonClick: function(button, e, options) {

        /*
        var form = Ext.getCmp("LoginId").getForm();

        if (form.isValid()) {
        form.submit({
        success: function(form, action) {
        Ext.Msg.alert('Success', action.result.message);
        },
        failure: function(form, action) {
        Ext.Msg.alert('Failed', action.result ? action.result.message : 'No response');
        }
        });
        }
        */


        var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"Logging In..."});
        myMask.show();

        var formValues = Ext.getCmp("LoginId").getForm().getValues();


        Ext.Ajax.request({ 
            url:'/rpc/Account/Login', 
            actionMethods:'POST', 
            scope:this, 
            params: formValues,
            success: function(r, o) { 
                myMask.hide();

                //debugger;

                //Ext.Msg.alert('Successful Login!');

                var retData = Ext.JSON.decode(r.responseText);

                Ext.Msg.alert('Success Login!','xxx');



            },
            failure: function(r,o) {
                myMask.hide();
                Ext.Msg.alert('Failed Login!','Either Your password or username was not valid.  Please try again.');

            }
        });




    },

    onButtonIdAfterRender: function(abstractcomponent, options) {
        var form = Ext.getCmp("LoginId");
        var that = this;
        form.mon(form.el, {
            keypress : function(e, t, opts) {
                if (e.getKey() == e.ENTER) {
                    that.doLogin();
                }
            }
        });

    },

    doLogin: function() {
        var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"Logging In..."});
        myMask.show();

        var formValues = Ext.getCmp("LoginId").getForm().getValues();


        Ext.Ajax.request({ 
            url:'/rpc/Account/Login', 
            actionMethods:'POST', 
            scope:this, 
            params: formValues,
            success: function(r, o) { 
                myMask.hide();
                var retData = Ext.JSON.decode(r.responseText);
                //Ext.Msg.alert('Success Login!');
                parent.window.location = '/Sponsor/'; 
            },
            failure: function(r,o) {
                myMask.hide();
                Ext.Msg.alert('Failed Login!','Either Your password or username was not valid.  Please try again.');

            }
        });
    },

    init: function(application) {
        this.control({
            "#buttonId": {
                click: this.onButtonClick,
                afterrender: this.onButtonIdAfterRender
            }
        });
    }

});
