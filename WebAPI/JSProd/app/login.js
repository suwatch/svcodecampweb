Ext.ns('CodeCampSV');

CodeCampSV.loginsvcc1 = {

    init : function () {
        var submitLogin = function () {

            var loginForm = Ext.getCmp("frmLogin").getForm();

            if (loginForm.isValid()) {
                Ext.Msg.wait("Verifying...", "Authentication");
                
                loginForm.submit({
                    url: "LoginService.ashx/",
                    
                    success : function (form, action) {
                        var url, urlParams = {}, returnUrl;
                        url = window.location.href;
                        if ( url.indexOf("?") > -1 ){
                            urlParams = url.substr(url.indexOf("?")).slice(1);
                            urlParams = Ext.urlDecode(urlParams);
                        }
                        //if (urlParams.ReturnUrl) {
                       //     returnUrl = urlParams.ReturnUrl;
                        //} else {
                         //   returnUrl = "News.aspx";
                        //}
                        returnUrl = 'News.aspx';
                        window.location.href = returnUrl;
                    },
                    
                    failure : function (form, action) {
                        Ext.Msg.alert("Failure", action.result.msg);
                    }
                });
            }
        };

        var win = new Ext.Window({
            title       : "Login",
            iconCls     : "icon-lock",
            closable    : false,
            draggable   : false,
            resizable   : false,
            modal       : true,
            width       : 300,
            items       : {
                xtype       : "form",
                id          : "frmLogin",
                border      : false,
                bodyStyle   : "padding: 5px;",
                labelWidth  : 90,
                defaultType : "textfield",
                defaults    : {
                    allowBlank  : false,
                    width       : 180
                },
                items : [
                    {
                        fieldLabel  : "Username",
                        name        : "username",
                        invalidText : "Your username is required",
                        listeners   : {
                            render      : function (el) {
                                el.focus("", 1500);
                            },
                            specialkey  : function (f, e) {
                                if (e.getKey() === e.ENTER) {
                                    submitLogin();
                                }
                            }
                        }
                    },
                    {
                        fieldLabel  : "Password",
                        name        : "password",
                        inputType   : "password",
                        invalidText : "Your password is required",
                        listeners   : {
                            specialkey : function (f, e) {
                                if (e.getKey() === e.ENTER) {
                                    submitLogin();
                                }
                            }
                        }
                    },
                    {
                        fieldLabel  : "Remember Me",
                        name        : "RememberMe",
                        inputType   : "checkbox",
                        width       : "auto",
                        allowBlank  : true
                    }
                ]
            },
            buttons : [
                {
                    text    : "Login",
                    handler : submitLogin
                }
            ]
        }).show();
    }

//    init: function() {
//        var win = new Ext.Window({
//            title: 'Login',
//            
//            iconCls: 'icon-lock',
//            closable: false,
//            draggable: false,
//            resizable: false,
//            modal: true,
//            width: 350,
//            height: 150,
//            items: {
//                xtype: 'form',
//                id: 'frmLogin',
//                border: false,
//                bodyStyle: 'padding: 5px;',
//                defaultType: 'textfield',
//                defaults: {
//                    allowBlank: false,
//                    anchor: '100%'
//                },
//                items: [
//                    {
//                        fieldLabel: 'Username',
//                        name: 'username',
//                        invalidText: 'Your username is required'
//                    },
//                    {
//                        fieldLabel: 'Password',
//                        name: 'password',
//                        inputType: 'password',
//                        invalidText: 'Your password is required'
//                    },
//                    {
//                        fieldLabel: 'Remember Me',
//                        name: 'RememberMe',
//                        inputType: 'checkbox'
//                     }
//                ]
//            },
//            buttons: [
//                {
//                    text: 'Login',
//                    listeners: {
//                        click: {
//                            fn: function() {
//                                Ext.Msg.wait('Verifying...', 'Authentication');
//                                Ext.getCmp('frmLogin').getForm().submit({
//                                    url: 'LoginService.ashx/',
//                                    success: function(form, action) {
//                                        window.location = 'News.aspx';
//                                    },
//                                    failure: function(form, action) {
//                                        Ext.Msg.alert("Failure", action.result.msg);
//                                    }
//                                });
//                            }
//                        }
//                    }
//                }
//            ]
//        }).show();
//    }
};

//Ext.onReady(CodeCampSV.login.init);

//Ext.BLANK_IMAGE_URL = 'js/images/default/s.gif';
//Ext.QuickTips.init();