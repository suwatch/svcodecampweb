var Harness = Siesta.Harness.Browser.ExtJS;

Harness.configure({
    title       : 'Awesome Test Suite',

    preload     : [
        '../RegistrationProd/resources/theme/app.css',
        '../RegistrationProd/all-classes.js'
    ]
});

Harness.start(
    '010_sanity.t.js',
    '020_opening.t.js',
    '030_MakeSureLoggedOut.t.js'
);
