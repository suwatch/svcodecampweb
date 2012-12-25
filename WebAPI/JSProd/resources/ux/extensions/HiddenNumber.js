Ext.ux.HiddenNumber = Ext.extend(Ext.form.Hidden, {
    onRender : function(){
        Ext.ux.HiddenNumber.superclass.onRender.apply(this, arguments);
    },
    
    getValue : function () {
        return Ext.ux.HiddenNumber.superclass.getValue.call(this) * 1;
    },
    
    setValue : function (value) {
        Ext.ux.HiddenNumber.superclass.setValue.call(this, (value + "").replace(/[^0-9]/g, ""));
    }
});

Ext.reg("hiddennumber", Ext.ux.HiddenNumber);