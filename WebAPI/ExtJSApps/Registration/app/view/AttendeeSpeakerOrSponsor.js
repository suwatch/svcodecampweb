/*
 * File: app/view/AttendeeSpeakerOrSponsor.js
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

Ext.define('RegistrationApp.view.AttendeeSpeakerOrSponsor', {
    extend: 'Ext.form.Panel',
    alias: 'widget.AttendeeSpeakerOrSponsorAlias',

    id: 'AttendeeSpeakerSponsorId',
    layout: {
        align: 'stretch',
        type: 'vbox'
    },
    bodyPadding: 20,
    title: 'Silicon Valley Code Camp Registration',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'radiogroup',
                    itemId: 'RadiobuttonGrp',
                    width: 400,
                    fieldLabel: '',
                    columns: 1,
                    items: [
                        {
                            xtype: 'radiofield',
                            itemId: 'rbAttendee',
                            name: 'attendeeType',
                            boxLabel: 'Attendee'
                        },
                        {
                            xtype: 'radiofield',
                            itemId: 'rbSpeaker',
                            name: 'attendeeType',
                            boxLabel: 'Speaker'
                        },
                        {
                            xtype: 'radiofield',
                            itemId: 'rbSponsor',
                            name: 'attendeeType',
                            boxLabel: 'Interested In Sponsoring'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    flex: 1,
                    dock: 'top',
                    itemId: 'ToolBarAttendeeSpeakerSponsor',
                    layout: {
                        pack: 'end',
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'tbseparator'
                        },
                        {
                            xtype: 'button',
                            itemId: 'continueButtonId',
                            text: 'Continue'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});