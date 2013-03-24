/*
 * File: app/view/SpeakerAfterLoginNotDup.js
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

Ext.define('RegistrationApp.view.SpeakerAfterLoginNotDup', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.SpeakerAfterLoginAlias2',

    id: 'speakerAfterLoginId',
    itemId: 'speakerAfterLogin',
    layout: {
        type: 'border'
    },
    title: 'Speaker After Login',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    flex: 2,
                    dock: 'top',
                    itemId: 'ToolBarAttendeeSpeaker',
                    layout: {
                        pack: 'end',
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'button',
                            itemId: 'logoutButtonId',
                            iconAlign: 'right',
                            text: 'Logout'
                        },
                        {
                            xtype: 'tbseparator'
                        },
                        {
                            xtype: 'button',
                            itemId: 'continueButtonId',
                            iconAlign: 'right',
                            text: 'Continue'
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'form',
                    flex: 3,
                    region: 'north',
                    split: true,
                    height: 150,
                    id: 'speakerAfterLoginProfileId',
                    itemId: '',
                    autoScroll: true,
                    defaults: {
                        anchor: '95%'
                    },
                    bodyPadding: 15,
                    title: 'Speaker After Login',
                    url: '/api/Register',
                    items: [
                        {
                            xtype: 'radiogroup',
                            name: 'CanSpeakDate',
                            id: 'CanSpeakRbGroupId',
                            padding: 5,
                            fieldLabel: '',
                            allowBlank: false,
                            blankText: 'You must choose which day(s) you can speak',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'radiofield',
                                    itemId: 'speakSaturdayAndSundayId',
                                    name: 'attendingDaysChoiceCurrentYear',
                                    boxLabel: 'Can Speak Saturday Or Sunday',
                                    checked: true,
                                    inputValue: 'AttendingSaturdaySunday'
                                },
                                {
                                    xtype: 'radiofield',
                                    itemId: 'speakSaturdayId',
                                    name: 'attendingDaysChoiceCurrentYear',
                                    inputId: 'speakSaturday',
                                    boxLabel: 'Can Speak Saturday',
                                    inputValue: 'AttendingSaturday'
                                },
                                {
                                    xtype: 'radiofield',
                                    itemId: 'speakSundayId',
                                    name: 'attendingDaysChoiceCurrentYear',
                                    boxLabel: 'Can Speak Sunday',
                                    inputValue: 'AttendingSunday'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'FirstName',
                            name: 'userFirstName',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Last Name',
                            name: 'userLastName',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Company',
                            name: 'company'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Principle Job',
                            name: 'principleJob'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Email',
                            name: 'email',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Phone Number',
                            name: 'phoneNumber'
                        },
                        {
                            xtype: 'combobox',
                            fieldLabel: 'Speaker Shirt Size',
                            name: 'shirtSize',
                            allowBlank: false,
                            displayField: 'shirtSize',
                            store: 'ShirtSizeStore',
                            valueField: 'shirtSize'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Web Site',
                            name: 'userWebsite'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Twitter Handle',
                            name: 'twitterHandle'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Facebook',
                            name: 'facebookId'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Google Plus',
                            name: 'googlePlusId'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'LinkedIn',
                            name: 'linkedInId'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Eventboard Email',
                            name: 'emailEventBoard'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'City',
                            name: 'city',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'State',
                            name: 'state',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Zipcode',
                            name: 'userZipCode',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            hidden: true,
                            fieldLabel: 'Attendees Id',
                            name: 'attendeesId',
                            allowBlank: false
                        },
                        {
                            xtype: 'checkboxfield',
                            fieldLabel: 'Volunteer',
                            name: 'volunteeredCurrentYear',
                            boxLabel: '(Volunteer To Help Day Of)',
                            inputValue: 'true',
                            uncheckedValue: 'false'
                        },
                        {
                            xtype: 'textareafield',
                            fieldLabel: 'Bio',
                            name: 'userBio',
                            allowBlank: false,
                            enforceMaxLength: true,
                            maxLength: 500,
                            minLength: 30
                        },
                        {
                            xtype: 'fieldset',
                            width: 150,
                            title: 'Reset Password (Leave blank to not change password)',
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Password',
                                    name: 'password1'
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Password Confirm',
                                    name: 'password2'
                                }
                            ]
                        },
                        {
                            xtype: 'label',
                            height: 14,
                            html: '<i>Press Continue on top right to submit sessions for approval</i>',
                            text: ''
                        }
                    ]
                },
                {
                    xtype: 'form',
                    flex: 3,
                    region: 'center',
                    split: true,
                    id: 'speakerPictureUploadFormId',
                    bodyPadding: 5,
                    animCollapse: true,
                    collapseDirection: 'bottom',
                    collapsible: true,
                    title: 'Speaker Picture Upload (Required For All Speakers Before Submitting Sessions)',
                    url: '/api/Account/FormData',
                    items: [
                        {
                            xtype: 'filefield',
                            border: false,
                            id: 'SpeakerPictureUploadXId',
                            itemId: 'SpeakerPictureUploadId',
                            fieldLabel: '',
                            hideLabel: true,
                            listeners: {
                                change: {
                                    fn: me.onSpeakerPictureUploadXIdChange,
                                    scope: me
                                }
                            }
                        },
                        {
                            xtype: 'panel',
                            border: false,
                            height: 300,
                            itemId: 'PicturePanelId',
                            items: [
                                {
                                    xtype: 'image',
                                    border: 1,
                                    id: 'SpeakerImageId',
                                    itemId: 'SpeakerImgId',
                                    src: '/image/none.jpg'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    onSpeakerPictureUploadXIdChange: function(filefield, value, eOpts) {
        // FOR SOME REASON THIS WILL NOT GET CALLED IN CONTROLLER

        var speakerForm = Ext.getCmp('speakerPictureUploadFormId');
        var imgId = Ext.getCmp('SpeakerImageId');
        if(speakerForm.isValid()){
            speakerForm.submit({
                url: '/rpc/Account/FormData',
                waitMsg: 'Uploading your photo...',
                success: function(fp, o) {
                    var attendeesId = o.result.attendeeId;
                    var imageLocation = '/attendeeimage/' + attendeesId + '.jpg?width=260&height=260&borderWidth=1&borderColor=black&scale=both';
                    var antiCachePart = (new Date()).getTime();
                    var newSrc = imageLocation + '&dc=' + antiCachePart;
                    imgId.setSrc(newSrc); 

                },
                failure: function(form, action){
                    //debugger;
                    if (action.failureType === Ext.form.action.Action.CONNECT_FAILURE) {
                        Ext.Msg.alert('Error',
                        'Status:'+action.response.status+': '+
                        action.response.statusText);
                    }
                    if (action.failureType === Ext.form.action.Action.SERVER_INVALID){
                        // server responded with success = false
                        Ext.Msg.alert('Invalid', action.result.errormsg);
                    }
                }
            });
        }
    }

});