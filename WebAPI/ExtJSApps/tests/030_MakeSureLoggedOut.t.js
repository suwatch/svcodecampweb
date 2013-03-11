// also supports: startTest(function(t) {
StartTest(function (t) {

    // force logout
    Ext.Ajax.request({
        url: '/rpc/Account/LogOut',
        actionMethods: 'POST',
        scope: this,
        params: {
            Username: '',
            Password: '',
            RememberMe: true
        },
        success: function (r, o) {
            t.ok(Ext, 'logout succeeded');
            t.done();   // Optional, marks the correct exit point from the test
        },
        failure: function (r, o) {
            t.ok(Ext, 'logout method failed, should not happen, even if already logged out');
        }

    });


})