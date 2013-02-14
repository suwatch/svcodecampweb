Ext.define('RegistrationApp.Utils', {
    singleton: true,

    checkForLoggedInGoToAttendeeProfile: function() {

        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "Checking Logged In Status..." });
        // always check logged in status when get here
        myMask.show();
        // first check to see if person is already logged in.  If they are, then go edit details page as if attendee
        var tabPanel = Ext.ComponentQuery.query('tabWizardPanelAlias')[0];
        Ext.Ajax.request({
            url: '/api/Account/IsLoggedIn',
            actionMethods: 'POST',
            scope: this,
            params: {
                Username: '',
                Password: '',
                RememberMe: true
            },
            success: function(r, o) {
                console.log('is logged in from launch');
                // logged in, take the person to attendee edit page. NEED TO CHECK IF SPEAKER AND DO SPEAKER PAGE
                var retData = Ext.JSON.decode(r.responseText);
                var attendeePanel = Ext.ComponentQuery.query('AttendeeAfterLoginAlias')[0];
                attendeePanel.getForm().setValues({
                    FirstName: retData.UserFirstName,
                    LastName: retData.UserLastName
                });
                tabPanel.setActiveTab(tabPanel.getTabIdByName('AttendeeAfterLogin'));
                myMask.hide();
            },
            failure: function(r, o) {
                console.log('is NOT logged in from launch');
                // not logged in so take them to opening page
                tabPanel.setActiveTab(tabPanel.getTabIdByName('AttendeeSpeakerSponsorId'));
                myMask.hide();
            }
        });
    }
});