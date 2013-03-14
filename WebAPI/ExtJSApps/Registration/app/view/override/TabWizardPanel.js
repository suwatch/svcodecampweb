Ext.define('RegistrationApp.view.override.TabWizardPanel', {
    override: 'RegistrationApp.view.TabWizardPanel',
    
    
     constructor: function () {
        this.callParent(arguments);
        this.hideTabTitles();
         
        /////////////////////////// below is patch, this should really be in open event someplace else
        console.log('checking log in status from constructor of RegistrationApp.view.override.TabWizardPanel');
         
        var attendeeFromFirstPage = Ext.ComponentQuery.query('AttendeeSpeakerOrSponsorAlias #rbAttendee')[0];
        var speakerFromFirstPage = Ext.ComponentQuery.query('AttendeeSpeakerOrSponsorAlias #rbSpeaker')[0];
         
        var nextPanel;
        var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"Checking Logged In Status..."});
        // always check logged in status when get here
        myMask.show();
        // first check to see if person is already logged in.  If they are, then go edit details page as if attendee
        var tabPanel = Ext.ComponentQuery.query('tabWizardPanelAlias')[0];
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
                    // need to load sessions also for this attendee (who is speaker)
                    var sessionsBySpeakerStore = Ext.getCmp("sessionsBySpeakerGridPanelId").store;
                    sessionsBySpeakerStore.load({
                        params: {
                            option: 'byspeaker',
                            param1: retData.attendeesId,
                            param2: '-1',
                            param3: '-1'
                        },
                        callback: function(records,operation,success) {
                            tabPanel.setActiveTab(tabPanel.getTabIdByName('SpeakerAfterLogin'));  
                        }
                    });
                } else {
                    attendeeFromFirstPage.checked = true;
                    tabPanel.setActiveTab(tabPanel.getTabIdByName('AttendeeAfterLogin'));
                }
                
                var imgId = Ext.ComponentQuery.query('#SpeakerImgId')[0];
                var imageLocation = '/attendeeimage/' + retData.attendeesId + '.jpg?width=175';
                var antiCachePart = (new Date()).getTime();
                var newSrc = imageLocation + '?dc=' + antiCachePart;
                imgId.setSrc(newSrc); 
                
                myMask.hide();
            },
            failure: function(r,o) {
                console.log('is NOT logged in from launch');
                // not logged in so take them to opening page
                tabPanel.setActiveTab(tabPanel.getTabIdByName('AttendeeSpeakerSponsorId'));
                myMask.hide();
            } 
        });    
        /////////////////////////// above is patch
         
     }
});