Logic.Viewport = function (config) {

    var northDock = new Ext.Container({
        region : "center",
        border : false,
        layout : "fit"
    });
    
    var north = new Ext.Panel({
        region : "north",
        height : 102,
        split  : true,
        border : false,
        layout : "border",
        items  : [ new Logic.Header({ }), northDock ]
    });

    north.on("render", function () {
        this.getLayout().north.splitEl.hide();
    }, this, { 
        single : true, 
        delay  : 100 
    });
    
    var center = new Ext.Container({
        region : "center",
        border : false,
        layout : "fit"
    });
    
    var south = new Logic.Footer({ region : "south" });

//  Commented West panel by request of Peter
//    var west = new Ext.Panel({
//        region       : "west",
//        cls          : "westPanel_ct",
//        split        : false,
//        collapsible  : true,
//        collapsed    : true,
//        border       : false,
//        title        : "&nbsp;",
//        width        : 285,
//        layout       : "fit",
//        items        : [ new Logic.DataExplorer.ExplorerTabs({ id : "dataexplorer.tabs" }) ]
//    });
    
    Logic.Viewport.superclass.constructor.call(this, {
        id          : "vp",
        layout      : "border",
        items       : [ north, center]
    });
};

Ext.extend(Logic.Viewport, Ext.Viewport, {
    updateRegions: function (config) {
        // update all regions
    },

    updateRegion : function (view, region, config) {
        var cmp = config;

        if (!config) {
            cmp = region;
            region = "center";
        }

        var panel = this.layout[region].panel,
            oldItem = panel.items.items[0];

        $C.events.fireEvent("beforeregionupdate", view, region, oldItem, cmp);

        panel.remove(oldItem, false);

        if (oldItem) {
            oldItem.hide();
        }

        panel.add(cmp);
        if (cmp.rendered || panel.rendered) {
            cmp.show();
        }

        panel.doLayout();

        $C.events.fireEvent("afterregionupdate", view, region, cmp);
    }
});