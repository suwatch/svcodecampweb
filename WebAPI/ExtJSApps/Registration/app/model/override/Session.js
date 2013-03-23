Ext.define('RegistrationApp.model.override.Session', {
    override: 'RegistrationApp.model.Session',
    
    constructor:function() {
        
        var that = this;
        this.getProxy().on('exception', function(proxy, response, operation) {
            debugger;
            var errorMessage = Ext.JSON.decode(response.responseText).message;
            that.getProxy().errorString = errorMessage;
        });
        
        this.callParent(arguments);

        
    }
    
});