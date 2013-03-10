/*
 * File: app/controller/SpeakerAfterLoginController.js
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

Ext.define('RegistrationApp.controller.SpeakerAfterLoginController', {
    extend: 'Ext.app.Controller',

    onContinueButtonIdClick: function(button, e, options) {
        var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"Updating..."});
        // always check logged in status when get here
        myMask.show();

        var tabPanel = Ext.ComponentQuery.query('tabWizardPanelAlias')[0];
        //var thisPanel = Ext.ComponentQuery.query('SpeakerAfterLoginAlias')[0];
        var thisPanel = Ext.getCmp("speakerAfterLoginProfileId");


        Ext.Ajax.request({ 
            url:'/rpc/Account/UpdateSpeaker', 
            actionMethods:'POST', 
            scope:this, 
            params: thisPanel.getForm().getValues(),
            success: function(r, o) {  

                //var retData = Ext.JSON.decode(r.responseText);


                tabPanel.setActiveTab(tabPanel.getTabIdByName('SpeakerPicture'))
                //Ext.Msg.alert('take to sponsor opt in page');


                myMask.hide();
            },
            failure: function(r,o) {
                tabPanel.setActiveTab(tabPanel.getTabIdByName('AttendeeSpeakerSponsorId'));
                myMask.hide();
            } 
        });

    },

    onLogoutButtonIdClick: function(button, e, options) {
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
            "SpeakerAfterLoginAlias #continueButtonId": {
                click: this.onContinueButtonIdClick
            },
            "#logoutButtonId": {
                click: this.onLogoutButtonIdClick
            }
        });
    }

});
