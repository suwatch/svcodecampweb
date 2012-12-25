Ext.ns("Logic.DataExplorer");

/*---Loads Tab------------------*/
Logic.DataExplorer.Loads = function (config) {
    Logic.DataExplorer.Loads.superclass.constructor.call(this, config);
};

Ext.extend(Logic.DataExplorer.Loads, Ext.grid.GridPanel, {
    header         : false,
    border         : false,
    split          : true,
    trackMouseOver : true,
    stripeRows     : true,
    hideHeaders    : true,
    loadMask       : true,
    enableDragDrop : true,
    initComponent  : function () {
        this.store = new Ext.data.Store({
            autoLoad : true,
            reader   : new Ext.data.JsonReader({
                fields        : [ { name : "Id"} ],
                id            : "Id",
                totalProperty : "total",
                root          : "rows"
            }),
            proxy    : new Ext.data.HttpProxy({
                url : "/Data/Load/Get/"
            }),
            baseParams : {
                query : "{ limit : 15, start : 0 }"
            }
        });

        this.selModel = new Ext.grid.CheckboxSelectionModel({
            singleSelect : true,
            listeners    : {
//                rowselect: {
//                    fn: function (el, rowIndex, record) {
//                        $C.ensureView(
//                            { 
//                                id : $C.defaultViews.loads.id 
//                            },
//                            { 
//                                fn: function () {
//                                    $C.events.fireEvent("loadselected", {
//                                        el       : el,
//                                        rowIndex : rowIndex,
//                                        record   : record
//                                    });
//                                },
//                                scope : this
//                            }
//                        );                        
//                    }
//                }
            }
        });

        this.colModel = new Ext.grid.ColumnModel({
            columns : [
                this.selModel,
                {
                    dataIndex : "Id"
                },
                {
                    id        : "Cargos",
                    dataIndex : "Cargos",
                    renderer  : {
                        fn : function (value, metaData, record, rowIndex, colIndex, store) {
                            var data = record.json.Cargos[0];

                            if (!Ext.isEmpty(data)) {
                                return String.format("{0} {1}", data.AddressOrigin.City, data.AddressOrigin.State);
                            }
                        }
                    }
                }
            ]
        });

        this.bbar = new Ext.PagingToolbar({
            store    : this.store,
            pageSize : 15
        });

        Logic.DataExplorer.Loads.superclass.initComponent.call(this);
    }
});
/*---End Loads Tab------------------*/


/*---Contacts Tab------------------*/
Logic.DataExplorer.Contacts = function (config) {
    Logic.DataExplorer.Contacts.superclass.constructor.call(this, config);
};

Ext.extend(Logic.DataExplorer.Contacts, Ext.grid.GridPanel, {
    header         : false,
    border         : false,
    trackMouseOver : true,
    stripeRows     : true,
    hideHeaders    : true,
    //autoExpandColumn : "Id",
    loadMask       : true,

    initComponent  : function () {
        this.store = new Ext.data.Store({
            autoLoad : true,
            reader   : new Ext.data.JsonReader({
                fields        : [ { name : "Id" } ],
                id            : "Id",
                totalProperty : "total",
                root          : "rows"
            }),
            proxy    : new Ext.data.HttpProxy({
                url : "/Data/Company/Get/"
            }),
            baseParams : {
                limit : 15
            }
        });

        this.selModel = new Ext.grid.CheckboxSelectionModel({
            singleSelect : true,
            listeners    : {
                rowselect : {
                    fn: function (el, rowIndex, record) {
                        $C.ensureView(
                            { 
                                id : $C.defaultViews.contacts.id 
                            },
                            {
                                fn : function () {
                                $C.events.fireEvent("contactselected", {
                                        el       : el,
                                        rowIndex : rowIndex,
                                        record   : record
                                    });
                                },
                                scop : this
                            }
                        );
                    }
                }
            }
        });

        this.colModel = new Ext.grid.ColumnModel({
            columns : [
                this.selModel,
                { dataIndex : "Id" },
                { dataIndex : "Name" }
            ]
        });

        this.bbar = new Ext.PagingToolbar({
            store    : this.store,
            pageSize : 15
        })

        Logic.DataExplorer.Contacts.superclass.initComponent.call(this);
    }
});
/*---End Contacts Tab------------------*/

/*---Explorer Tabs------------------*/
Logic.DataExplorer.ExplorerTabs = function (config) {
    this.tabs = {};
    
    config = Ext.apply(config || {}, {
        activeTab : 0,
        items     : [
            {
                title : "Loads",
                items : [ new Logic.DataExplorer.Loads() ]
            }
        ]
    });

    Logic.DataExplorer.ExplorerTabs.superclass.constructor.call(this, config);
};

Ext.extend(Logic.DataExplorer.ExplorerTabs, Ext.TabPanel, {
    ctCls    : "exploreTabs_ct",
    border   : false,    
    defaults : {
        layout : "fit"
    },
    
    currentRegion : "west",   
    
    shiftTabs : function () {
        var tabs, layout = $C.vp.getLayout(), north = layout.north.panel, west = layout.west.panel;
        switch (this.currentRegion) {
            case "north":
                this.prevSize = north.getSize().height;
                tabs = north.items.items[1].items.items[0];
                north.items.items[1].remove(tabs, false);
                north.setHeight(128);  
                west.show();
                west.add(tabs);                
                layout.north.splitEl.hide();
                
                this.currentRegion = "west";        
                break;
            case "west":
            default:
                tabs = west.items.items[0];
                west.remove(tabs, false);
                west.hide();
                
                north.items.items[1].add(tabs);
                
                layout.north.splitEl.show();                
                north.setHeight(this.prevSize || 250);  
                this.currentRegion = "north";        
                break;
        }    
        
        $C.vp.doLayout();    
    },
        
    initComponent : function () {
        this.tbar = new Ext.Toolbar({
            ctCls : "dataExplorerBtns_ct",
            items : [
                { text  : "Book" },
                { text  : "Map" },
                { text  : "Share" },
                { text  : "Similar" },
                { text  : "Invite" },
                { text  : "Tag" },
                "->",
                {
                    xtype : "splitbutton",
                    text  : "More...",
                    menu  : new Ext.menu.Menu({
                        items: [
                            {
                                text      : "Shift",
                                listeners : {
                                    click : {
                                        fn : function () {
                                            this.shiftTabs();
                                        },
                                        
                                        scope: this
                                    }
                                }
                            }
                        ]
                    })
                }
             ]
        });
    
        Logic.DataExplorer.ExplorerTabs.superclass.initComponent.call(this);
    }
});
/*---Explorer Tabs------------------*/