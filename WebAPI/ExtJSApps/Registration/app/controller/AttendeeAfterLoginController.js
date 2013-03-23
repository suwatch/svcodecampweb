/*
 * File: app/controller/AttendeeAfterLoginController.js
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

Ext.define('RegistrationApp.controller.AttendeeAfterLoginController', {
    extend: 'Ext.app.Controller',

    onContinueButtonIdClick: function(button, e, eOpts) {


        var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"Checking Logged In Status..."});
        // always check logged in status when get here
        myMask.show();
        // first check to see if person is already logged in.  If they are, then go edit details page as if attendee
        var tabPanel = Ext.ComponentQuery.query('AttendeeAfterLoginAlias')[0];
        var values = tabPanel.getValues();


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
            url:'/rpc/Account/UpdateAttendee', 
            actionMethods:'POST', 
            scope:this, 
            params: values,
            success: function(r, o) {  

                var tabPanel = Ext.ComponentQuery.query('tabWizardPanelAlias')[0];
                tabPanel.setActiveTab(tabPanel.getTabIdByName('optIn'));
                myMask.hide();
                Ext.Ajax.un('requestexception',exceptionHandler);
            },
            failure: function(r,o) {
                /*
                if (this.errorString) {
                Ext.Msg.alert(this.errorString);
                } else {
                Ext.Msg.alert("Error Saving Attendee Record.");
                }
                */
                myMask.hide();
                Ext.Ajax.un('requestexception',exceptionHandler);
            } 
        });

    },

    onLogoutButtonIdClick: function(button, e, eOpts) {
        var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"Logging Out..."});
        myMask.show();

        Ext.Ajax.request({ 
            url:'/rpc/Account/LogOut', 
            actionMethods:'POST', 
            scope:this, 
            params:{
                Username: '',
                Password: '',
                RememberMe: true
            },
            success: function(r, o) { 
                var tabPanel = Ext.ComponentQuery.query('tabWizardPanelAlias')[0];
                tabPanel.setActiveTab(tabPanel.getTabIdByName('attendeeorspeaker'));
                console.log('setting tab to attendeeorspeaker  logout succeeded');
                myMask.hide();
            },
            failure: function(r,o) {
                var tabPanel = Ext.ComponentQuery.query('tabWizardPanelAlias')[0];
                tabPanel.setActiveTab(tabPanel.getTabIdByName('attendeeorspeaker'));
                console.log('setting tab to attendeeorspeaker. logout failed');
                myMask.hide();
            } 

        });
    },

    init: function(application) {
        this.control({
            "AttendeeAfterLoginAlias #continueButtonId": {
                click: this.onContinueButtonIdClick
            },
            "AttendeeAfterLoginAlias #logoutButtonId": {
                click: this.onLogoutButtonIdClick
            }
        });
    }

});
