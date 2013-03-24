/*
 * File: app.js
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


Ext.Loader.setConfig({
    enabled: true
});

Ext.application({

    requires: [
        'Ext.data.proxy.Rest'
    ],
    models: [
        'Session',
        'TagsModel',
        'SessionPresenterModel',
        'SessionLevelModel',
        'SessionTitleModel'
    ],
    stores: [
        'ShirtSizeStore',
        'SessionStore',
        'TagStore',
        'SessionPresenterStore',
        'SessionLevelStore',
        'SessionTitlesStore'
    ],
    views: [
        'ViewportMain',
        'AttendeeSpeakerOrSponsor',
        'AttendeeOrSpeaker',
        'ForgotUsernameOrPassword',
        'sponsor',
        'TabWizardPanel',
        'AttendeeAfterLogin',
        'createAccount',
        'OptIn',
        'PasswordConfirm',
        'SpeakerAfterLoginNotDup',
        'SpeakerSessionUpdate'
    ],
    autoCreateViewport: true,
    controllers: [
        'RegisterSpeakerAttendeeSponsorController',
        'RegisterSpeakerAttendee',
        'ForgotUsername',
        'SponsorController',
        'AttendeeAfterLoginController',
        'SpeakerAfterLoginController',
        'CreateAccountController',
        'OptInController',
        'SessionEditorController',
        'ViewportController',
        'SpeakerSessionUpdateController'
    ],
    name: 'RegistrationApp',

    launch: function() {

    }

});
