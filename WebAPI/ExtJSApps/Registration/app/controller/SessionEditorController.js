/*
 * File: app/controller/SessionEditorController.js
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

Ext.define('RegistrationApp.controller.SessionEditorController', {
    extend: 'Ext.app.Controller',

    onSessionEditorPanelIdAfterRender: function(component, eOpts) {
        /*
        var speakerProfilePanel =  Ext.getCmp('speakerAfterLoginProfileId');
        var retData = speakerProfilePanel.getForm().getValues();

        //debugger;

        var tagList = Ext.getCmp("SessionTagsGridPanelId");
        var tagListStore = tagList.store;
        tagListStore.load({
            params: {
                sessionId: retData.attendeesId
            },
            callback: function(records,operation,success) {
                // get selection model of grid


                var sm = tagList.getSelectionModel();
                //sm.bindStore(tagListStore);


                var recs = [];
                Ext.each(records,function(rec) {
                    if (rec.get("taggedInSession") === true) {
                        recs.push(rec);
                    }
                });
                sm.select(recs);

                // would like to scroll to top now


            }
        });


        var sessionEditForm = Ext.getCmp("sessionFormPanelEditorId").getForm();
        sessionEditForm.setValues(this.sessionData);


        */
    },

    onButtonClick: function(button, e, eOpts) {

        Ext.create('RegistrationApp.view.WindowSession',{

        }).show();


        //SessionTagsGridPanelId


    },

    init: function(application) {
        this.control({
            "#SessionEditorPanelId": {
                afterrender: this.onSessionEditorPanelIdAfterRender
            },
            "#AddNewSessionButtonId": {
                click: this.onButtonClick
            }
        });
    }

});
