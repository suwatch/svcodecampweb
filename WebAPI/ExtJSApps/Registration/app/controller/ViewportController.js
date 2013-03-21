/*
 * File: app/controller/ViewportController.js
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

Ext.define('RegistrationApp.controller.ViewportController', {
    extend: 'Ext.app.Controller',

    onViewportAfterRender: function(component, eOpts) {
        var attendeeFromFirstPage = Ext.ComponentQuery.query('AttendeeSpeakerOrSponsorAlias #rbAttendee')[0];
        var speakerFromFirstPage = Ext.ComponentQuery.query('AttendeeSpeakerOrSponsorAlias #rbSpeaker')[0];

        var nextPanel;
        var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"Checking Logged In Status..."});
        // always check logged in status when get here
        myMask.show();
        // first check to see if person is already logged in.  If they are, then go edit details page as if attendee
        var tabPanel = Ext.ComponentQuery.query('tabWizardPanelAlias')[0];

        tabPanel.getTabBar().hide();
        tabPanel.componentLayout.childrenChanged = true;
        tabPanel.doComponentLayout();

        Ext.Ajax.request({ 
            url:'/rpc/Account/IsLoggedIn', 
            actionMethods:'POST', 
            scope:this, 
            params:{
                Username: '',
                Password: '',
                RememberMe: true
            },
            success: function(r, o) { 
                var retData = Ext.JSON.decode(r.responseText);
                tabPanel.updateAllPanelsWithData(retData);
                if (retData.hasSessionsCurrentYear === true) {
                    speakerFromFirstPage.checked = true;
                    // need to load sessions also for this attendee, the current attendee is the spaker

                    var sessionsBySpeakerStore = Ext.getCmp("sessionsBySpeakerGridPanelId").getStore();
                    sessionsBySpeakerStore.load({
                        params: {
                            option: 'byspeaker',
                            param1: retData.attendeesId,
                            param2: '-1',
                            param3: '-1'
                        },
                        callback: function(records,operation,success) {
                            var imgId = Ext.ComponentQuery.query('#SpeakerImgId')[0];
                            var imageLocation = '/attendeeimage/' + retData.attendeesId + '.jpg?width=260&height=260&borderWidth=1&borderColor=black&scale=both';
                            var antiCachePart = (new Date()).getTime();
                            var newSrc = imageLocation + '&dc=' + antiCachePart;
                            imgId.setSrc(newSrc); 
                            tabPanel.setActiveTab(tabPanel.getTabIdByName('SpeakerAfterLogin'));  
                            myMask.hide();
                        }
                    });
                } else {
                    attendeeFromFirstPage.checked = true;
                    tabPanel.setActiveTab(tabPanel.getTabIdByName('AttendeeAfterLogin'));
                    myMask.hide();
                }
            },
            failure: function(r,o) {
                console.log('is NOT logged in from viewportafterrender');
                // not logged in so take them to opening page
                tabPanel.setActiveTab(tabPanel.getTabIdByName('AttendeeSpeakerSponsorId'));
                myMask.hide();
            } 
        });  

        // create an array of titles so we can make sure the title the person enters is unique on validation
        var store = Ext.data.StoreManager.lookup('SessionTitlesStore');
        store.load({
            params: {
                option: 'justlowercasetitle',
                param1: '-1',
                param2: '-1',
                param3: '-1'
            },
            callback: function(records,operation,success) {
                console.log('lowercase titles found: ' + records.length);
            }
        });





    },

    init: function(application) {
        this.control({
            "viewport > panel": {
                afterrender: this.onViewportAfterRender
            }
        });
    }

});
