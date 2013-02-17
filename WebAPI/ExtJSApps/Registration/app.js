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
    views: [
        'ViewportMain',
        'AttendeeSpeakerOrSponsor',
        'AttendeeOrSpeaker',
        'ForgotUsernameOrPassword',
        'sponsor',
        'TabWizardPanel',
        'AttendeeAfterLogin',
        'SpeakerAfterLogin',
        'createAccount',
        'SpeakerPicture',
        'OptIn'
    ],
    autoCreateViewport: true,
    name: 'RegistrationApp',
    controllers: [
        'RegisterSpeakerAttendeeSponsorController',
        'RegisterSpeakerAttendee',
        'ForgotUsername',
        'SponsorController',
        'AttendeeAfterLoginController',
        'SpeakerAfterLoginController',
        'CreateAccountController',
        'SpeakerPictureController',
        'OptInController'
    ],

    launch: function() {
        console.log('top of launch event in Application object');

        //RegistrationApp.view.override.ViewportMain.checkForLoggedInGoToAttendeeProfile();

        //debugger;

        //RegistrationApp.Utils.checkForLoggedInGoToAttendeeProfile();


        /* THIS DOES NOT WORK COMPILED.  IN  TAB CONSTRUCTOR AT THE MOMENT
        var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"Checking Logged In Status..."});
        // always check logged in status when get here
        myMask.show();
        // first check to see if person is already logged in.  If they are, then go edit details page as if attendee
        var tabPanel = Ext.ComponentQuery.query('tabWizardPanelAlias')[0];
        Ext.Ajax.request({ 
            url:'/api/Account/IsLoggedIn', 
            actionMethods:'POST', 
            scope:this, 
            params:{
                Username: '',
                Password: '',
                RememberMe: true
            },
            success: function(r, o) {  
                console.log('is logged in from launch');
                // logged in, take the person to attendee edit page. NEED TO CHECK IF SPEAKER AND DO SPEAKER PAGE
                var retData = Ext.JSON.decode(r.responseText);
                var attendeePanel = Ext.ComponentQuery.query('AttendeeAfterLoginAlias')[0];
                attendeePanel.getForm().setValues({
                    FirstName: retData.UserFirstName,
                    LastName: retData.UserLastName
                });
                tabPanel.setActiveTab(tabPanel.getTabIdByName('AttendeeAfterLogin'));
                myMask.hide();
            },
            failure: function(r,o) {
                console.log('is NOT logged in from launch');
                // not logged in so take them to opening page
                tabPanel.setActiveTab(tabPanel.getTabIdByName('AttendeeSpeakerSponsorId'));
                myMask.hide();
            } 
        });

        */



        /*
        var token = window.location.hash;

        if (token === '#login') {




        }
        else {


        }
        */

    }

});
