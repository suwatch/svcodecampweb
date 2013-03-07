/*
 * File: app/view/SpeakerPicture.js
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

Ext.define('RegistrationApp.view.SpeakerPicture', {
    extend: 'Ext.form.Panel',
    alias: 'widget.SpeakerPictureAlias',

    bodyPadding: 20,
    title: 'Speaker Picture Upload',
    url: '/api/Account/FormData',

    initComponent: function() {
        var me = this;

        me.initialConfig = Ext.apply({
            url: '/api/Account/FormData'
        }, me.initialConfig);

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'label',
                    border: false,
                    height: 140,
                    text: 'Speaker Picture Required Before Submitting Presentation'
                },
                {
                    xtype: 'filefield',
                    border: false,
                    height: 50,
                    itemId: 'SpeakerPictureUploadId',
                    fieldLabel: '',
                    labelWidth: 300
                },
                {
                    xtype: 'panel',
                    border: false,
                    itemId: 'PicturePanelId',
                    items: [
                        {
                            xtype: 'image',
                            border: 1,
                            height: 300,
                            itemId: 'SpeakerImgId',
                            width: 300
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    itemId: 'ToolBarForgotUsername',
                    layout: {
                        pack: 'end',
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'button',
                            disabled: false,
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
            ]
        });

        me.callParent(arguments);
    }

});