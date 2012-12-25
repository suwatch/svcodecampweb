Ext.onReady(function () {
    var panelRegister = new Ext.Panel({
        id      : 'formRegister',
        layout  : 'form',
        autoHeight  : true,
        width       : 400,
        bodyCssClass: 'formRegister',
        border      : false,
        labelAlign  : 'right',
        labelWidth  : 110,
        defaults    : {
            border  : false
        },
        items   : [{
            html    : '<h3>What day(s) are you attending?</h3>'
        }, {
            xtype   : 'checkboxgroup',
            items   : [
                { boxLabel : 'Saturday', name : 'CheckBoxSaturday', checked : true },
                { boxLabel : 'Sunday', name : 'CheckBoxSunday', checked : true }
            ]
        }, {
            html    : '<h3>Are you also speaking?</h3>'
        }, {
            xtype       : 'combo',
            width       : 50,
            triggerAction: 'all',
            editable    : false,
            lazyRender  :true,
            mode        : 'local',
            value       : false,
            store: new Ext.data.ArrayStore({
                id      : 0,
                fields  : [
                    'value',
                    'displayText'
                ],
                data: [[false, 'No'], [true, 'Yes']]
            }),
            valueField  : 'value',
            displayField: 'displayText'
        }, {
            html    : '<span class="smallText">Note that we can not guarantee which day you will be assigned to speak on, ' +
                      'regardless of your attending choices above (Upon return, only shows Yes if actually signed up for ' +
                      'sessions).</span>'
        }, {
            html    : '<h3>Who are you?</h3>'
        }, {
            xtype       : 'textfield',
            fieldLabel  : 'First Name',
            name        : 'TextBoxFirstName',
            width       : 180,
            allowBlank  : false
        }, {
            xtype       : 'textfield',
            fieldLabel  : 'Last Name',
            name        : 'TextBoxLastName',
            width       : 180,
            allowBlank  : false
        }, {
            xtype       : 'textfield',
            fieldLabel  : 'Email',
            name        : 'TextBoxEmail',
            width       : 180,
            allowBlank  : false
        }, {
            xtype       : 'textfield',
            fieldLabel  : 'Website or blog',
            name        : 'TextBoxWebsite',
            width       : 180
        }, {
            xtype       : 'fileuploadfield',
            fieldLabel  : 'Picture of Self',
            name        : 'FileUpload1',
            width       : 220
        }, {
            xtype       : 'textfield',
            fieldLabel  : 'Location ZIP code',
            name        : 'TextBoxZipCode',
            width       : 80
        }, {
            html        : "<h3>What's your story? </h3><span class=\"smallText\">Take a moment to enter a few words about yourself below:</span>"
        }, {
            xtype       : 'textarea',
            name        : 'TextBoxBio',
            width       : 250,
            height      : 80
        }, {
            html        : '<h3>Sign Up!</h3>'
        }, {
            xtype       : 'textfield',
            name        : 'TextBoxUserName',
            fieldLabel  : 'Create Username',
            allowBlank  : false,
            width       : 120
        }, {
            xtype       : 'textfield',
            fieldLabel  : 'Create Password',
            name        : 'TextBoxPassword',
            inputType   : 'password',
            allowBlank  : false,
            width       : 120
        }, {
            html        : '<h3>Wanna help us?</h3>'
        }, {
            xtype       : 'checkbox',
            name        : 'CheckBoxVolunteer',
            id          : 'CheckBoxVolunteer',
            boxLabel    : "Yes, I'd like to volunteer!" 
        }, {
            html        : '<span class="smallText" style="padding-left: 110px">Best Phone Number to reach me:<span>'
        }, {
            xtype       : 'textfield',
            name        : 'TextBoxPhoneNumber',
            id          : 'TextBoxPhoneNumber',
            width       : 120
        }, {
            html        : '<span class="smallText" style="padding-left: 110px">e.g. (650) 555-1212<span>'
        }, {
            xtype       : 'button',
            text        : 'Register'
        }]
    });
    panelRegister.render('divFormRegister');
});