/*
 * File: app/controller/RegisterSpeakerAttendee.js
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

Ext.define('RegistrationApp.controller.RegisterSpeakerAttendee', {
    extend: 'Ext.app.Controller',

    onBackButtonIdClick: function(button, e, options) {

        //console.log('back button in registerspeakerattendee controller');

        var tabWizardPanel = Ext.getCmp('TabWizardId')
        tabWizardPanel.setActiveTab(Ext.getCmp('TabWizardId').getTabIdByName('AttendeeSpeakerSponsorId'));

    },

    onContinueButtonIdClick: function(button, e, options) {

        //var continueEnabled = this.checkEnableContinue();



        var username = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias #username')[0].getValue();
        var password = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias #password')[0].getValue();
        var haveaccount = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias #haveaccount')[0].checked;
        var forgot = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias #forgot')[0].checked;
        var create = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias #create')[0].checked;


        //debugger;
        var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"Logging In..."});

        var tabPanel = Ext.ComponentQuery.query('tabWizardPanelAlias')[0];

        var attendeeFromFirstPage = Ext.ComponentQuery.query('AttendeeSpeakerOrSponsorAlias #rbAttendee')[0].checked;
        var speakerFromFirstPage = Ext.ComponentQuery.query('AttendeeSpeakerOrSponsorAlias #rbSpeaker')[0].checked;
        // (THIS IS SPEAKER OR ATTENDEE PAGE ONLY) var sponsorFromFirstPage = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias #rbSponsor')[0].checked;

        if (haveaccount === true && (username.length < 3 || password.length < 4)) {
            Ext.MessageBox.alert('Problem','username must be at least 3 characters and password must be at least 4 characters');
        }
        else {
            var tabWizardPanel = Ext.getCmp('TabWizardId');

            if (forgot === true) {
                tabWizardPanel.setActiveTab(tabWizardPanel.getTabIdByName('forgotusernameorpassword'));
            } else if (create === true) {
                // verify user entered does not exist


                if (username.length > 0) {
                    Ext.Ajax.request({ 
                        url:'/rpc/Account/CheckUsernameEmailExists', 
                        actionMethods:'POST', 
                        scope:this, 
                        params:{
                            Username: username,
                            Password: password
                        },
                        success: function(r,o) {



                            if (r.responseText.length > 2 && username.length > 0) {  // (this is because it returns "" instead of empty string)
                                Ext.Msg.alert('Error','Username exists, Try Again or use Forget Username or Password');
                            } else {
                                var thisPanel = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias')[0];
                                var retData = thisPanel.getForm().getValues();
                                var createPanel = Ext.ComponentQuery.query('createAccountAlias')[0];
                                createPanel.getForm().setValues({
                                    username: retData.username,
                                    password: retData.password
                                });
                                tabWizardPanel.setActiveTab(tabWizardPanel.getTabIdByName('createAccount'));
                            }
                        },
                        failure: function(r,o) {



                            Ext.Msg.alert('Error','Can not create username because it already exists');
                        }
                    });
                } else {
                    tabWizardPanel.setActiveTab(tabWizardPanel.getTabIdByName('createAccount'));
                }




                tabWizardPanel.setActiveTab(tabWizardPanel.getTabIdByName('createnewaccount'));
            } else if (haveaccount) {
                // try logging person in and if fails, put up message and leave them here,
                // otherwise redirect to home page

                //debugger;

                // attempt to login.  only move to next page if login is successful.
                // if successful, then get data and load it into form before moving
                myMask.show();

                Ext.Ajax.request({ 
                    url:'/rpc/Account/Login', 
                    actionMethods:'POST', 
                    scope:this, 
                    params:{
                        Username: username,
                        Password: password,
                        RememberMe: true
                    },
                    success: function(r, o) { 

                        //debugger;

                        //Ext.Msg.alert('Successful Login!');

                        var retData = Ext.JSON.decode(r.responseText);

                        if (attendeeFromFirstPage === false) {
                            // must be speaker
                            var speakerPanel = Ext.ComponentQuery.query('SpeakerAfterLoginAlias')[0];
                            speakerPanel.getForm().setValues(retData);


                            tabPanel.setActiveTab(tabPanel.getTabIdByName('SpeakerAfterLogin'));

                        } else {
                            // must be sponsor or attendee
                            var attendeePanel = Ext.ComponentQuery.query('AttendeeAfterLoginAlias')[0];
                            attendeePanel.getForm().setValues(retData);
                            tabPanel.setActiveTab(tabPanel.getTabIdByName('AttendeeAfterLogin'));

                        }
                        myMask.hide();

                    },
                    failure: function(r,o) {

                        Ext.Msg.alert('Failed Login!','Either Your password or username was not valid.  Please try again or choose forgot username or create account.');
                        myMask.hide();
                    }
                });
            }
        }
    },

    checkEnableContinue: function() {

        // we have username, password,haveaccount,forgot,create
        // here are the rules to follow
        //
        // if (username & password have data) enable
        // if (forgot checked or create new checke must have username)
        // if (create new account checked, need both username and password

        var continuebutton;

        var username = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias #username')[0].getValue();
        var password = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias #password')[0].getValue();
        var haveaccount = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias #haveaccount')[0].checked;
        var forgot = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias #forgot')[0].checked;
        var create = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias #create')[0].checked;

        //console.log('username: ' + username + ' password: ' + password);
        //console.log('haveaccount: ' + haveaccount + ' forgot: ' + forgot + ' create: ' + create);


        if ( (username.length > 0 && password.length > 0) ||
        forgot === true || 
        create === true) {
            continueButton = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias #continueButtonId')[0];
            if (continueButton.isDisabled()) {
                continueButton.enable();
            }
        } else {
            continueButton = Ext.ComponentQuery.query('AttendeeOrSpeakerAlias #continueButtonId')[0];
            if (!continueButton.isDisabled()) {
                continueButton.disable();
            }
        }

        var continueEnabledNow = !continueButton.isDisabled();

        //console.log('continueEnabled: ' + continueEnabledNow);

        return continueEnabledNow;



    },

    init: function(application) {
        this.control({
            "AttendeeOrSpeakerAlias #backButtonId": {
                click: this.onBackButtonIdClick
            },
            "AttendeeOrSpeakerAlias #continueButtonId": {
                click: this.onContinueButtonIdClick
            }
        });
    }

});
