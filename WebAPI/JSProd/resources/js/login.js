Ext.ns("Logic");

Logic.login = {
    init: function () {
        var win = new Ext.Window({
            title     : "Login",
            layout    : "fit",
            iconCls   : "icon-lock",
            closable  : false,
            draggable : false,
            resizable : false,
            modal     : false,
            width     : 300,
            height    : 150,
            items     : {
                xtype       : "form",
                id          : "frmLogin",
                border      : false,
                bodyStyle   : "padding: 5px;",
                labelWidth  : 90,
                width       : 280,
                defaultType : "textfield",
                defaults    : {
                    allowBlank  : false,
                    width   : 180
                },
                items       : [
                    { 
                        fieldLabel  : "Username",
                        name        : "username",
                        invalidText : "Your username is required"
                    }, 
                    { 
                        fieldLabel  : "Password",
                        name        : "password",
                        inputType   : "password",
                        invalidText : "Your password is required"
                    },
                    {
                        fieldLabel  : "Remember Me",
                        name        : "RememberMe",
                        inputType   : "checkbox",
                        width       : "auto"
                    }                       
                ]
            },
            buttons : [
                {
                    text       : "Login",
                    listeners  : {
                        click  : {
                            fn : function () {
                                Ext.Msg.wait("Verifying...", "Authentication");
                                Ext.getCmp("frmLogin").getForm().submit({
                                    url     : "/Account/Login/",
                                    success : function (form, action) {
                                        window.location.href = "/default.html";
                                    },
                                    failure : function (form, action) {
                                        Ext.Msg.alert("Failure", action.result.msg);
                                    }
                                });
                            }
                        }
                    }
                }
            ]
        }).show();
    }
};

Ext.BLANK_IMAGE_URL = "/resources/extjs/resources/images/default/s.gif";
Ext.QuickTips.init();
Ext.onReady(Logic.login.init);