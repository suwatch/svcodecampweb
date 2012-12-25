Logic.AutoSave = function(config) {
    Logic.AutoSave.superclass.constructor.call(this, config);
};

Ext.extend(Logic.AutoSave, Ext.Toolbar, {
    saveInterval : 30000,

    initComponent: function() {
        Logic.AutoSave.superclass.initComponent.call(this);

        this.on("render", function() { 
            //this.timeStamp = this.addText("&nbsp;");
            this.publishButton = this.addButton({
                text    : "Publish",
                iconCls : "icon-disk",
                handler : function() {
                    $C.events.fireEvent("viewsave");
                }
            });
        }, this);

        var task = {
            run : function() {
                if (this.rendered) {
                    //this.timeStamp.el.innerHTML = "Auto Saved: " + new Date().format("Y-m-d H:i:s") + "  ";
                }
                $C.events.fireEvent("viewsave");
            },
            interval : this.saveInterval,
            scope    : this
        };

        Ext.TaskMgr.start.defer(this.saveInterval, Ext.TaskMgr, [task]);
    }
});