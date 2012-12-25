Logic.Header = function (config) {
    var topToolbar = new Ext.Toolbar({
        items : [
            { xtype : "tbfill" },
            {
                xtype        : "trigger",
                emptyText    : "Name, Place, Load, ID's",
                triggerClass : "x-form-search-trigger",
                width        : 180
            }
        ]
    });

    var tabToolbar = new Ext.Toolbar({
        ctCls : "headerTabs_container",
        style : {
            border : "none"
        },
        defaults : {
            xtype   : "viewbutton",
            listeners: {
                mouseover: function (button, e) {
                    if (!button.el.hasClass("headerTabsSelected")) {
                        this.addClass("headerTabsOver");
                    }
                },
                mouseout: function (button, e) {
                    this.removeClass("headerTabsOver");
                }
            }
        },
        items: [
            {   text    : "Home",
                listeners : {
                    afterRender: function () {
                        tabToolbar.selectecdTab = this.text;
                        this.addClass("headerTabsSelected");
                    },
                    mouseover: function (button, e) {
                        if (!button.el.hasClass("headerTabsSelected")) {
                            this.addClass("headerTabsOver");
                        }
                    },
                    mouseout: function (button, e) {
                        this.removeClass("headerTabsOver");
                    }
                }
            },
            { 
                text    : "Coleman Data" ,
                viewId  : "colemandata"
            },
            { 
                text    : "Loads",
                viewId  : "loadsFilter"
            }, 
            { 
                text    : "Status 90",
                viewId  : "loadsProNumber"
            },
            {
                text    : "New",
                viewId  : "newproject",
                iconCls : "icon-add"
            }, 
            {
                xtype   : "tbfill"
            },
            {
                xtype       : "button",
                text        : "Report Issues",
                cls         : "x-btn-blue-text",
                listeners   : {
                    scope   : this,
                    click   : function () {
                        if (!$C.windows.featureRequest) {
                            $C.windows.featureRequest = new Logic.FeatureRequests.Form();
                        }
                        $C.windows.featureRequest.resetForm();
                        $C.windows.featureRequest.show();
                    }
                }
            },
            { xtype : "tbspacer", width : 10 },
            {
                xtype     : "button",
                text      : "Logout",
                listeners : {
                    click : {
                        fn : function () {
                            Ext.Msg.wait("Logging out", "One moment...");
                            window.location.href = "/Data/Account/LogOff/";
                        }
                    }
                }
            }
        ],
        
        onViewLoaded : function (view) {
            var select;
            
            for (var i = 0; i < this.items.getCount(); i++) {
                var item = this.items.get(i);
                
                if (item.viewId) {
                    item.un("mouseout", this.onButtonMouseOut, this);
                    item.getEl().removeClass("x-btn-selected");
                    
                    if (item.viewId == view.parent) {
                        select = item;
                    }
                }
            }
            
            if (!Ext.isEmpty(select)) {
                select.on("mouseout", this.onButtonMouseOut, this);
                select.getEl().addClass("x-btn-selected");
            }
        },
        
        onButtonMouseOut: function (button, e) {
            button.getEl().addClass("x-btn-selected");
        }
    });

    $C.events.on("viewloaded", tabToolbar.onViewLoaded, tabToolbar);

    var historyBar = new Ext.Toolbar({
        ctCls : "historyBar_container",
        style : {
            border       : "none",
            borderBottom : "1px solid #003366"
        },
        items : [
            new Ext.Toolbar.TextItem(
                {
                    xtype : "tbtext",
                    text  : "Recent History..."
                }), 
                " ",
                new Logic.ViewHistory({
                    id  : "ViewHistory1",
                    cls : "inner-toolbar"
                }), 
                {
                    xtype : "tbfill"
                }
//                , 
//                new Logic.AutoSave({
//                    id  : "AutoSave1",
//                    cls : "inner-toolbar"
//                })
        ]
    });
        
    var tabs = new Ext.Panel({
        id     : "pnlNorth",
        border : false,
        tbar   : topToolbar,
        bbar   : tabToolbar
    });
    
    config = Ext.apply(config || {}, {
        autoHeight : true,
        border     : false,
        bbar       : historyBar,        
        region     : "north",
        layout     : "fit",
        items      : [ tabs ]
    });

    Logic.Header.superclass.constructor.call(this, config);
};

Ext.extend(Logic.Header, Ext.Panel, { });