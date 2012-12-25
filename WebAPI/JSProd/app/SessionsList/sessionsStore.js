Ext.namespace('CCWeb');

CCWeb.sessionProxy = new Ext.data.HttpProxy({
    prettyUrls: false,
    api: {
        load: 'SessionsGet.ashx',
        save: 'Save.ashx',
        destroy: 'Delete.ashx'
    }
});

CCWeb.sessionRecord = Ext.data.Record.create(
        [
        { name: 'Id' },
        { name: 'CodeCampYearId' },
        { name: 'Attendeesid' },
        { name: 'UsersIdLoggedIn' },
        { name: 'SessionLevel_id' },
        { name: 'Username' },
        { name: 'Title' },
        { name: 'Description' },
        { name: 'CreateDate', type: 'date', dateFormat: 'n/j/Y' },
        { name: 'LectureRoomsId' },
        { name: 'SessionTimesId' },
        { name: 'InterestedCount' },
        { name: 'NotInterestedCount' },
        { name: 'WillAttendCount' },
        { name: 'InterestedBool' },
        { name: 'NotInterestedBool' },
        { name: 'WillAttendBool' },
        { name: 'WikiURL' },
        { name: 'SessionLevel' },
        { name: 'RoomNumber' },
        { name: 'SessionTime' },
        { name: 'PresenterName' },
        { name: 'PresenterURL' }
        ]);

CCWeb.sessionReader = new Ext.data.JsonReader(
{
    totalProperty: 'total',   //   the property which contains the total dataset size (optional)
    idProperty: 'Id',
    root: 'rows',
    successProperty: 'success'
},
        CCWeb.sessionRecord
        );

CCWeb.sessionWriter = new Ext.data.JsonWriter({
    returnJson: true,
    writeAllFields: true
});

CCWeb.sessionStore = new Ext.data.Store({
    id: 'user',
    root: 'records',
    proxy: CCWeb.sessionProxy,
    reader: CCWeb.sessionReader,
    writer: CCWeb.sessionWriter,
    paramsAsHash: true,
    batchSave: false,
    autoLoad: false

});