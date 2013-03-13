/*
 * File: app/view/AttendeeOrSpeaker.js
 *
 * This file was generated by Sencha Architect version 2.2.0.
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

Ext.define('RegistrationApp.view.AttendeeOrSpeaker', {
    extend: 'Ext.form.Panel',
    alias: 'widget.AttendeeOrSpeakerAlias',

    id: 'attendeeOrSpeakerId',
    itemId: 'AttendeeOrSpeaker',
    bodyPadding: 20,
    title: 'Attendee Or Speaker',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    itemId: 'ToolBarAttendeeSpeaker',
                    layout: {
                        pack: 'end',
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'button',
                            itemId: 'backButtonId',
                            iconAlign: 'right',
                            text: 'Back'
                        },
                        {
                            xtype: 'tbseparator'
                        },
                        {
                            xtype: 'button',
                            formBind: true,
                            disabled: true,
                            itemId: 'continueButtonId',
                            iconAlign: 'right',
                            text: 'Continue'
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'textfield',
                    itemId: 'username',
                    fieldLabel: 'Username',
                    inputId: 'username',
                    emptyText: 'Username'
                },
                {
                    xtype: 'textfield',
                    itemId: 'password',
                    fieldLabel: 'Password',
                    inputId: 'password',
                    inputType: 'password',
                    emptyText: 'password'
                },
                {
                    xtype: 'radiogroup',
                    itemId: 'RadioGroupLoginActions',
                    width: 400,
                    fieldLabel: 'Actions',
                    allowBlank: false,
                    items: [
                        {
                            xtype: 'radiofield',
                            itemId: 'haveaccount',
                            labelWidth: 70,
                            name: 'LoginActions',
                            boxLabel: 'I Have a Code Camp Account Already',
                            checked: true
                        },
                        {
                            xtype: 'radiofield',
                            itemId: 'forgot',
                            labelWidth: 70,
                            name: 'LoginActions',
                            boxLabel: 'Forgot Username or Password'
                        },
                        {
                            xtype: 'radiofield',
                            itemId: 'create',
                            labelWidth: 70,
                            name: 'LoginActions',
                            boxLabel: 'Create a new Account for me'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});