/*
 * File: app/controller/OptInController.js
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

Ext.define('RegistrationApp.controller.OptInController', {
    extend: 'Ext.app.Controller',

    onBackButtonIdClick: function(button, e, options) {

    },

    onContinueButtonIdClick: function(button, e, options) {
        var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"Checking Logged In Status..."});

        myMask.show();
        var thisPanel = Ext.ComponentQuery.query('OptInAlias')[0];

        Ext.Ajax.request({ 
            url:'/api/Account/UpdateOptIn', 
            actionMethods:'POST', 
            scope:this, 
            params: thisPanel.getForm().getValues(),
            success: function(r, o) {  

                var retData = Ext.JSON.decode(r.responseText);

                // take to sponsor opt in next
                //Ext.Msg.alert('take to sponsor opt in page');
                myMask.hide();

                //debugger;
                //window.location = '../../Session#';


            },
            failure: function(r,o) {
                //tabPanel.setActiveTab(tabPanel.getTabIdByName('AttendeeSpeakerSponsorId'));
                Ext.Msg.alert("Update Record Failed");
                myMask.hide();
            } 
        });
    },

    init: function(application) {
        this.control({
            "OptInAlias #backButtonId": {
                click: this.onBackButtonIdClick
            },
            "OptInAlias #continueButtonId": {
                click: this.onContinueButtonIdClick
            }
        });
    }

});
