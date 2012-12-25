Ext.override(Ext.form.NumberField, {
    getValue : function () {
        var value = this.fixPrecision(this.parseValue(Ext.form.NumberField.superclass.getValue.call(this)));
        
        return !Ext.isEmpty(value) ? value : null;
    }
});