// also supports: startTest(function(t) {
// also supports: startTest(function(t) {
StartTest(function (t) {
    t.diag("Sanity");

    t.ok(Ext, 'ExtJS is here');
    t.ok(Ext.Window, '.. indeed');

    t.ok(RegistrationApp.view.ViewportMain, 'My project is here RegistrationApp.view.ViewportMain');
  
    t.done();   // Optional, marks the correct exit point from the test
})