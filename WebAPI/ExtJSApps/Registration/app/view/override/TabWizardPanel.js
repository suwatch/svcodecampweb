Ext.define('RegistrationApp.view.override.TabWizardPanel', {
    override: 'RegistrationApp.view.TabWizardPanel',
    
    
     constructor: function () {
        this.callParent(arguments);
        this.hideTabTitles();
         
        /////////////////////////// below is patch, this should really be in open event someplace else
        console.log('checking log in status from constructor of RegistrationApp.view.override.TabWizardPanel');
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
                console.log('is logged in from launch');
                
              
                
                
                //debugger;
                // logged in, take the person to attendee edit page. NEED TO CHECK IF SPEAKER AND DO SPEAKER PAGE
                var retData = Ext.JSON.decode(r.responseText);
                if (retData.hasSessionsCurrentYear === true) {
                    nextPanel = Ext.ComponentQuery.query('SpeakerAfterLoginAlias')[0];
                    nextPanel.getForm().setValues(retData);
                    tabPanel.setActiveTab(tabPanel.getTabIdByName('SpeakerAfterLogin'));
                } else {
                    nextPanel = Ext.ComponentQuery.query('AttendeeAfterLoginAlias')[0];
                    nextPanel.getForm().setValues(retData);
                    tabPanel.setActiveTab(tabPanel.getTabIdByName('AttendeeAfterLogin'));
                }
                
                //debugger;
                // set picture string
                var imgId = Ext.ComponentQuery.query('#SpeakerImgId')[0];
                var imageLocation = '/attendeeimage/' + retData.attendeesId + '.jpg?width=300';
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