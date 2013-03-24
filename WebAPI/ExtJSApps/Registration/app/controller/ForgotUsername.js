/*
 * File: app/controller/ForgotUsername.js
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

Ext.define('RegistrationApp.controller.ForgotUsername', {
    extend: 'Ext.app.Controller',

    onBackButtonIdClick: function(button, e, eOpts) {
        var tabWizardPanel = Ext.getCmp('TabWizardId')
        tabWizardPanel.setActiveTab(tabWizardPanel.getTabIdByName('attendeeorspeaker'));

    },

    onContinueButtonIdClick: function(button, e, eOpts) {
        var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"Checking..."});
        myMask.show();

        var values = button.up().up().getForm().getValues(); // really just username and email

        // need to call ajax method to send themt here username and pwd
        var exceptionHandler = function(conn, response, options) {
            var errorMessage = Ext.JSON.decode(response.responseText).message;
            Ext.MessageBox.show({
                title: 'Error Message',
                msg: errorMessage,
                icon: Ext.MessageBox.ERROR,
                buttons: Ext.Msg.OK
            });
        };

        Ext.Ajax.on('requestexception',exceptionHandler);
        Ext.Ajax.request({ 
            url:'/rpc/Account/ForgotPassword', 
            actionMethods:'POST', 
            scope:this, 
            params: values,
            success: function(response, o) {  
                myMask.hide();
                var retData = Ext.JSON.decode(response.responseText);
                Ext.Ajax.un('requestexception',exceptionHandler);

                var messageStr1 = 
                "We have sent a temporary password to your email address on file: " + 
                retData.email + " for username:" + retData.username;
                Ext.MessageBox.show({
                    title: '',
                    msg: messageStr1,
                    icon: Ext.MessageBox.INFO,
                    buttons: Ext.Msg.OK
                });

                var tabPanel = Ext.ComponentQuery.query('tabWizardPanelAlias')[0];
                tabPanel.setActiveTab(tabPanel.getTabIdByName('attendeeorspeaker'));
            },
            failure: function(r,o) {
                myMask.hide();
                Ext.Ajax.un('requestexception',exceptionHandler);
            } 
        });

    },

    init: function(application) {
        this.control({
            "ForgotUsernameAlias #backButtonId": {
                click: this.onBackButtonIdClick
            },
            "ForgotUsernameAlias #continueButtonId": {
                click: this.onContinueButtonIdClick
            }
        });
    }

});
