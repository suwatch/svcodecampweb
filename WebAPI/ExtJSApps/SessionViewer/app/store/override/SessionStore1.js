Ext.define('SessionApp.store.override.SessionStore1', {
    override: 'SessionApp.store.SessionStore1',

    constructor: function () {

        this.data = this.CreateData();
        this.callParent(arguments);
    },
    CreateData: function () {

        data = [[1101, 'Ed'], [1102, 'Aaron']];
        return data;

    }



});