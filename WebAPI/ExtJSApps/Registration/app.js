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
        'SpeakerAfterLogin'
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
        'CreateAccountController'
    ],

    launch: function() {
        //var tabPanel = Ext.getCmp('TabWizardId');
        //tabPanel.hideTabTitles();


        var token = window.location.hash;
        debugger;
        if (token === '#login') {

            /*Ext.define('SVCodeCamp.Data', {
            singleton: true,
            startPage: 'login'
            });  
            */

            var tabPanel = Ext.ComponentQuery.query('tabWizardPanelAlias')[0];
            tabPanel.setActiveTab(tabPanel.getTabIdByName('attendeeorspeaker'));
        }
    }

});
