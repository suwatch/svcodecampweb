Ext.form.Action.JsonSubmit = Ext.extend(Ext.form.Action.Submit, {
    type : "JsonSubmit",
    run  : function () {
        var o = this.options,
            method = this.getMethod(),
            isPost = method == "POST";
            
        if (o.clientValidation === false || this.form.isValid()) {
            Ext.Ajax.request(Ext.apply(this.createCallback(), {
                url      : this.getUrl(!isPost),
                method   : method,
                params   : isPost ? this.getParams() : null,
                isUpload : this.form.fileUpload
            }));
        } else if (o.clientValidation !== false) {
            this.failureType = Ext.form.Action.CLIENT_INVALID;
            this.form.afterAction(this, false);
        }
    },

    getParams : function () {
            var values = Ext.util.JSON.encode(Ext.apply(this.form.jsonData || {}, this.form.serialize() || {}));
            return Ext.apply({ data: values }, Ext.form.Action.JsonSubmit.superclass.getParams.apply(this, arguments));
    }
});

Ext.form.Action.ACTION_TYPES.JsonSubmit = Ext.form.Action.JsonSubmit;

Ext.form.JsonForm  = Ext.extend(Ext.form.BasicForm, {
    afterAction : function (action, success) {
        if (action.type == "load") {
            this.jsonData = action.result.data;
        }
        Ext.form.JsonForm.superclass.afterAction.call(this, action, success);
    },
    submit : function (options) {
        this.doAction("JsonSubmit", options);
        return this;
    }
});

Ext.JsonFormPanel = Ext.extend(Ext.FormPanel, {
    createForm : function () {
        delete this.initialConfig.listeners;
        return new Ext.form.JsonForm(null, this.initialConfig);
    }
});

Ext.form.JsonFormPanel = Ext.JsonFormPanel;

Ext.reg("jsonform", Ext.form.JsonFormPanel);

Ext.override(Ext.form.BasicForm, {
    serialize : function () {
        var o = {};
        
        this.items.each(function (f) {
            if (f.getValue) {
                o[f.getName()] = f.getValue();
            }
        });
        
        return o;
    }
});