Ext.define('SessionApp.store.override.SessionStore1', {
    override: 'SessionApp.store.SessionStore1',

    constructor: function () {

        this.data = this.CreateData();
        this.pageSize = 3;
        this.proxy = {
            type: 'memory',
            enablePaging: true
        };
        this.callParent(arguments);
    },
    
    CreateData: function () {

        // get meta data from breeze AND populate model associated with this store



        //var data = [
        //     { sessionId: 'test 1', sessionTitle: 'record1' },
        //     { name: 'test 2', text: 'record2' },
        //     { name: 'test 3', text: 'record3' },
        //     { name: 'test 4', text: 'record4' }
        //];
        data = [];
        if (typeof parent.SessionsDataGlobal != 'undefined') {
            console.log('parent.SessionsDataGlobal has value');
            Ext.Array.each(parent.SessionsDataGlobal, function (value) {
                data.push(value);
            });
        } else {
             console.log('parent.SessionsDataGlobal has no value');
        }
        return data;
    }
});