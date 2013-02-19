{
    "xdsVersion": "2.1.0",
    "frameworkVersion": "ext41",
    "internals": {
        "type": "textfield",
        "reference": {
            "name": "items",
            "type": "array"
        },
        "codeClass": null,
        "userConfig": {
            "designer|userClassName": "PasswordConfirm",
            "designer|userAlias": "passwordconfirm",
            "inputType": "password",
            "fieldLabel": null,
            "initialPassField": ""
        },
        "customConfigs": [
            {
                "group": "(Custom Properties)",
                "name": "initialPassField",
                "type": "string",
                "basic": true,
                "alternates": [
                    {
                        "type": "array"
                    },
                    {
                        "type": "boolean"
                    },
                    {
                        "type": "number"
                    },
                    {
                        "type": "object"
                    }
                ],
                "isCustomConfig": true,
                "obscure": false
            }
        ]
    },
    "linkedNodes": {},
    "boundStores": {},
    "overrideClass": "Ext.define('VCI.view.override.PasswordConfirm', {\n\toverride: 'VCI.view.PasswordConfirm',\n\tvtype: 'password'\n}, function() {\n\tExt.apply(Ext.form.field.VTypes, {\n\t\tpassword : function(val, field) {\n\t\t\tif (field.initialPassField) {\n\t\t\t\tvar container = field.up('form');\n\t\t\t\tif (!container) {\n\t\t\t\t\tcontainer = field.up(\"container\");\n\t\t\t\t}\n\t\t\t\tvar pwd = container.down('#' + field.initialPassField);\n\t\t\t\tif (!pwd) {\n\t\t\t\t\tpwd = container.down('[name=' + initialPassField + ']');\n\t\t\t\t}\n\t\t\t\treturn (val == pwd.getValue());\n\t\t\t}\n\t\t\treturn true;\n\t\t},\n\t\tpasswordText : 'La contraseña no coincide',\n\t});\n});",
    "id": "ucmpPasswordConfirm",
    "name": "PasswordConfirm",
    "category": {
        "id": "xdcForm Fields",
        "name": "Form Fields"
    }
}