/*
 * File: app/controller/CreateAccountController.js
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

Ext.define('RegistrationApp.controller.CreateAccountController', {
    extend: 'Ext.app.Controller',

    onBackButtonIdClick: function(button, e, eOpts) {
        var tabWizardPanel = Ext.getCmp('TabWizardId');
        tabWizardPanel.setActiveTab(Ext.getCmp('TabWizardId').getTabIdByName('attendeeorspeaker'));
    },

    onContinueButtonIdClick: function(button, e, eOpts) {
        // verify that password matches
        // verify username does not exist
        // verify email does not exist
        // Then, Create user and take to either speaker or attendee details page

        var localValues =  Ext.ComponentQuery.query('createAccountAlias')[0].getForm().getValues();


        //if (localValues.password === localValues.passwordConfirm) {

        // hopefully will get validation to work on form for this later (matching passwords)
        // CreateUser

        var tabPanel = Ext.ComponentQuery.query('tabWizardPanelAlias')[0];

        var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"Creating New Account..."});
        myMask.show();

        localValues.recaptchaChallengeField = Recaptcha.get_challenge();
        localValues.recaptchaResponseField =Recaptcha.get_response();

        Ext.Ajax.on('requestexception', function (conn, response, options) {
            myMask.hide();
            if (response.status != 200) {
                var errorData = Ext.JSON.decode(response.responseText);
                Ext.Msg.alert('Creating User Failed',errorData.message);
            }
        });

        Ext.Ajax.request({ 
            url:'/rpc/Account/CreateUser', 
            actionMethods:'POST', 
            scope:this, 
            params: localValues,
            success: function(r, o) { 
                //debugger;
                var retData = Ext.JSON.decode(r.responseText);
                tabPanel.updateAllPanelsWithData(retData);

                // need to figure out if speaker or attendee selected
                // var attendeeFromFirstPage = Ext.ComponentQuery.query('AttendeeSpeakerOrSponsorAlias #rbAttendee')[0].checked;
                var speakerFromFirstPage = Ext.ComponentQuery.query('AttendeeSpeakerOrSponsorAlias #rbSpeaker')[0].checked;
                if (speakerFromFirstPage === true) {
                    tabPanel.setActiveTab(tabPanel.getTabIdByName('SpeakerAfterLogin'));
                } else {
                    tabPanel.setActiveTab(tabPanel.getTabIdByName('AttendeeAfterLogin'));
                }
                myMask.hide();
            },
            failure: function(r,o) {
                // handled in exception now
                //debugger;
                //Ext.Msg.alert('Creating User Failed','');
                console.log('Creating User Failed ' + r);
                //myMask.hide();
            }


        });
    },

    onPanelAfterRender: function(component, eOpts) {
        Recaptcha.create("6LcrXN4SAAAAAG4gTUSUCzyfaFE4-yJOIXq86PdW",
        Ext.getDom('recaptcha'),
        {
            theme: "clean",
            callback: Recaptcha.focus_response_field
        }
        );
    },

    init: function(application) {
        this.control({
            "createAccountAlias #backButtonId": {
                click: this.onBackButtonIdClick
            },
            "createAccountAlias #continueButtonId": {
                click: this.onContinueButtonIdClick
            },
            "createAccountAlias #reCaptcha": {
                afterrender: this.onPanelAfterRender
            }
        });
    }

});
