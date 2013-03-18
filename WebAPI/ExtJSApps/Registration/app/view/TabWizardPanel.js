/*
 * File: app/view/TabWizardPanel.js
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

Ext.define('RegistrationApp.view.TabWizardPanel', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.tabWizardPanelAlias',

    requires: [
        'RegistrationApp.view.AttendeeSpeakerOrSponsor',
        'RegistrationApp.view.AttendeeOrSpeaker',
        'RegistrationApp.view.ForgotUsernameOrPassword',
        'RegistrationApp.view.sponsor',
        'RegistrationApp.view.AttendeeAfterLogin',
        'RegistrationApp.view.SpeakerAfterLoginNotDup',
        'RegistrationApp.view.SpeakerSessionUpdate',
        'RegistrationApp.view.createAccount',
        'RegistrationApp.view.OptIn',
        'Ext.form.RadioGroup',
        'Ext.form.field.Radio',
        'Ext.form.Label'
    ],

    height: 500,
    id: 'TabWizardId',
    itemId: '',
    activeTab: 0,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'AttendeeSpeakerOrSponsorAlias'
                },
                {
                    xtype: 'AttendeeOrSpeakerAlias'
                },
                {
                    xtype: 'ForgotUsernameAlias'
                },
                {
                    xtype: 'sponsorAlias'
                },
                {
                    xtype: 'AttendeeAfterLoginAlias'
                },
                {
                    xtype: 'SpeakerAfterLoginAlias2'
                },
                {
                    xtype: 'SpeakerSessionUpdateAlias',
                    title: 'Speaker Session Update'
                },
                {
                    xtype: 'createAccountAlias'
                },
                {
                    xtype: 'OptInAlias'
                }
            ]
        });

        me.callParent(arguments);
    },

    getTabIdByName: function(stepName) {
        //console.log('getTabIdByName:stepName(top): ' + stepName);



        var tabId = -1;
        if (stepName === 'AttendeeSpeakerSponsorId') {
            tabId = 0;
        } else if (stepName === 'attendeeorspeaker') {
            tabId = 1;
        } else if (stepName === 'forgotusernameorpassword') {
            tabId = 2;
        } else if (stepName === 'Sponsor') {
            tabId = 3;
        }
        else if (stepName === 'AttendeeAfterLogin') {
            tabId = 4;
        }
        else if (stepName === 'SpeakerAfterLogin') {
            tabId = 5;
        }
        else if (stepName === 'SpeakerSessionUpdate') {
            tabId = 6;
        }
        else if (stepName === 'createAccount') {
            tabId = 7;
        }
        else if (stepName === 'optIn') {
            tabId = 8;
        }
        else {
            console.log('getTabIdByName called with no match ' + stepName);
        }

        //console.log('getTabIdByName:stepName(bottom): ' + tabId);


        return tabId;
    },

    updateAllPanelsWithData: function(retData) {
        // Update all form pages with data coming back from server


        var speakerProfilePanel =  Ext.getCmp('speakerAfterLoginProfileId');
        speakerProfilePanel.getForm().setValues(retData);

        var attendeePanel = Ext.ComponentQuery.query('AttendeeAfterLoginAlias')[0];
        attendeePanel.getForm().setValues(retData);

        var optInPanel = Ext.ComponentQuery.query('OptInAlias')[0];
        optInPanel.getForm().setValues(retData);
    }

});