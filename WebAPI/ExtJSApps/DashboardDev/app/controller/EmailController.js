/*
 * File: app/controller/EmailController.js
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

Ext.define('App.controller.EmailController', {
    extend: 'Ext.app.Controller',

    onPreviewLetterButtonIdClick: function(button, e, options) {
        var formValues = Ext.getCmp("mailGeneratorFormPanelId").getForm().getValues();

        var myMask = new Ext.LoadMask(Ext.getBody(), 
        {msg:"Generating Email Preview..."});
        var task = new Ext.util.DelayedTask(function(){
            myMask.show();
        });

        task.delay(500);
        Ext.Ajax.request({ 
            url:'/rpc/Email/EmailPreview', 
            params: formValues,
            method: 'POST',
            success: function(r, o) { 
                task.cancel();
                if (myMask.isVisible) {
                    myMask.hide();
                }
                Ext.Msg.alert('Success to ' + formValues.previewEmailSend);
            },
            failure: function(r,o) {
                task.cancel();
                if (myMask.isVisible) {
                    myMask.hide();
                }  

                Ext.Msg.alert("Login Failed. Please Try Again");
            }
        });

    },

    onGenerateEmailButtonIdClick: function(button, e, options) {
        var formValues = Ext.getCmp("mailGeneratorFormPanelId").getForm().getValues();

        var myMask = new Ext.LoadMask(Ext.getBody(), 
        {msg:"Generating Email To DB..."});
        var task = new Ext.util.DelayedTask(function(){
            myMask.show();
        });

        task.delay(500);
        Ext.Ajax.request({ 
            url:'/rpc/Email/EmailGenerate', 
            params: formValues,
            method: 'POST',
            success: function(r, o) { 
                task.cancel();
                if (myMask.isVisible) {
                    myMask.hide();
                }
                Ext.Msg.alert('Success generated ' + formValues.previewEmailSend);
            },
            failure: function(r,o) {
                task.cancel();
                if (myMask.isVisible) {
                    myMask.hide();
                }  

                Ext.Msg.alert("Login Failed. Please Try Again");
            }
        });

    },

    onLoadAttendeesButtonIdClick: function(button, e, options) {

        gridPanel = Ext.getCmp("attendeesListGridPanelId");
        var store = gridPanel.store;
        var form = Ext.getCmp("mailGeneratorFormPanelId").getForm();

        var cntLabel = Ext.getCmp("attendeeListToolBarCntId");
        cntLabel.setText('Total: *');

        store.load({
            scope: this,
            callback: function(records,operation,success) {
                cntLabel.setText('Total: ' + records.length);
            },
            params: form.getValues()
        });
    },

    init: function(application) {
        this.control({
            "#previewLetterButtonId": {
                click: this.onPreviewLetterButtonIdClick
            },
            "#GenerateEmailButtonId": {
                click: this.onGenerateEmailButtonIdClick
            },
            "#loadAttendeesButtonId": {
                click: this.onLoadAttendeesButtonIdClick
            }
        });
    }

});