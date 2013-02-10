Ext.application({
    
	 requires: [
       'Ext.ux.data.PagingMemoryProxy',
        'Ext.grid.Panel',
        'Ext.data.reader.Json',
        'Ext.toolbar.Paging'
    ],
    models: [
        'SessionModel'
    ],
    stores: [
        'SessionStore1'
    ],
    views: [
        'MainViewport'
    ],
    autoCreateViewport: true,
    name: 'SessionApp',

    launch: function() {

        //SessionApp.sessionData = [[103],[104],[105]];
    }
	
});
