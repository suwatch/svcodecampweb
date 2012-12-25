Logic.Footer = function (config) {
    Logic.Footer.superclass.constructor.call(this, Ext.apply(config || {}, {
            header : false,
            border : false
        })
    );
};

Ext.extend(Logic.Footer, Ext.Panel, {
    initComponent : function () {
        this.tbar = new Ext.Toolbar({
            items: [
                {
                    xtype         : "combo",
                    width         : 180,
                    store         : new Ext.data.SimpleStore({
                        fields    : [ "text", "value" ],
                        data      : [[ "Eric Rempel [erempel]", "Eric Rempel [erempel]"], ["Peter Kellner [pkellner]", "Peter Kellner [pkellner]"], ["BJ Heinly [bjheinly]", "BJ Heinly [bjheinly]" ]]
                    }),
                    valueField    : "value",
                    triggerAction : "all",
                    queryDelay    : 10,
                    mode          : "local",
                    hiddenName    : "cmbSearch_Value",
                    displayField  : "text"
                }, 
                {
                    xtype         : "tbfill"
                }, 
                {
                    text          : "Eric Rempel",
                    iconCls       : "icon-stopgreen"
                }, 
                {
                    text          : "Peter Kellner",
                    iconCls       : "icon-stopred"
                }, 
                {
                    text          : "Online Friends (57)"
                }
            ]
        });
    
        Logic.Footer.superclass.initComponent.call(this);
    }
});