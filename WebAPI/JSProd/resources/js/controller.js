Logic.EventController = Ext.extend(Ext.util.Observable, {
    constructor : function () {
        Logic.EventController.superclass.constructor.apply(this, arguments);
    }
});

$C = Logic.Controller = {
    view   : null,
    vp     : null,
    events : null,
    // for storing ext windows
    windows: {},

    // mixed: view, id or config
    loadView : function (config) {
        if (typeof config === "string" || typeof config === "number") {
            this.onViewLoaded(this.getViewById(config));
            return;
        }

        if (Ext.isEmpty(config)) {
            this.onViewLoaded(this.views[0]);
            return;
        }

        if (config.id) {
            var view = this.getViewById(config.id);
            if (!Ext.isEmpty(view)) {
                this.onViewLoaded(view, config);
                return;
            }
        }

        if (config.url) {
            this.loadRemoteView(config);
            return;
        }

        this.onViewLoaded(config);
    },

    loadRemoteView : function (config) {
        if (config.url) {
            Ext.data.Connection.disableCaching = false;
            Ext.Ajax.request({
                url     : config.url,
                preload : config.preload,
                method  : "GET",
                scope   : this,
                success : function (response, options) {
                    if (!Ext.isEmpty(response.responseText)) {
                        var view = new Logic.View(eval("(" + response.responseText + ")"));
                        this.onViewLoaded(Ext.apply(view, { url: options.url }), { remote: true, preload: options.preload, callback: options.loadedCallback });
                    }
                },
                loadedCallback : config.callback,
                disableCaching : true
            });
            return;
        }
    },

    onViewLoaded : function (view, config) {
        if (view === null) {
            view = this.getViewById("404");
        }
        
        if (Ext.isEmpty(view.id) || view.id === -1) {
            return;
        }
        
        this.view = view;
        config = config || {};

        if (Ext.isEmpty(config.preload) || config.preload === false) {
            this.view.render(this.vp);
            this.events.fireEvent("viewloaded", this.view, undefined, undefined, config);
        }

        this.putToCache(this.view, config.remote);

        if (!Ext.isEmpty(config.callback)) {
            config.callback.fn.call(config.callback.scope || this, view);
        }        
    },

    getViewById : function (viewId) {
        if (!Ext.isEmpty(viewId)) {
            var v = this.views;
            
            for (var i = 0; i < v.length; i++) {
                if (v[i].id === viewId) {
                    return v[i];
                }
            }
        }
        
        return null;
    },

    viewIndexOf : function (view) {
        for (var i = 0; i < this.views.length; i++) {
            if (this.views[i].id === view.id) {
                return i;
            }
        }
        return -1;
    },

    getViewIdByTitle : function (title) {
        if (!Ext.isEmpty(title)) {
            var v = this.views;
            
            for (var i = 0; i < v.length; i++) {
                if (v[i].title == title) {
                    return v[i].id;
                }
            }
        }
        return -1;
    },

    putToCache : function (view, remote) {
        var i = this.viewIndexOf(view);
        
        if (i > -1) {
            if (remote) {
                //need remove previous rendered items because we loaded new instance of view (not from cache)
                if (this.views[i].init && this.views[i].init().destroy) {
                    this.views[i].init().destroy();
                }
            }
            
            this.views[i] = view;
        } else {
            this.views.push(view);
        }
    },

    getHrefHash : function () {
        var href = top.location.href,
            i = href.indexOf("#");

        return i >= 0 ? href.substr(i + 1) : null;
    },

    initialize : function () {
        this.events = new Logic.EventController();
        
        this.events.addEvents({
            "viewloaded"         : true,
            "beforeregionupdate" : true,
            "afterregionupdate"  : true,
            "viewsave"           : true,
            "loadselected"       : true,
            "contactselected"    : true,
            "ready"              : true
        });
        
        this.vp = new Logic.Viewport();
        
        this.vp.render(Ext.getBody());
        
        this.doPreload();
        
        this.loadView("home");
    },

    onReady : function (view) {
        if (--this.preloadCounter == 0) {
            var query = this.getHrefHash();

            if (Ext.isEmpty(query, false)) {
                //this.loadView();
            }

            this.events.fireEvent("ready");
        }
    },

    //view is object {(optional)id, (optional)url}
    //(optional)callback is object {fn,scope}
    ensureView : function (view, callback) {
        if (Ext.isEmpty(this.view) || this.view.id != view.id) {
            $C.loadView({
                id         : view.id,
                url        : view.url,
                ensureView : true,
                callback   : callback
            });
        } else if (!Ext.isEmpty(callback)) {
            callback.fn.call(callback.scope || this, this.view);
        }
    },

    doPreload : function () {
        this.preloadCounter = this.views.length;
        
        Ext.each(this.views, function (view) {
            this.loadView(Ext.apply(view, { 
                preload  : true, 
                callback : { 
                    fn    : this.onReady, 
                    scope : this
                } 
            }));
        }, this);
    },

    views : [
        new Logic.View({
            id    : "404",
            title : "Erorr",
            init  : function () {
                return new Ext.Panel({
                    title     : "Error",
                    html      : "An error has occurred",
                    iconCls   : "icon-error",
                    bodyStyle : "padding: 5px;"
                });
            }
        }),
        new Logic.View({
            id    : "home",
            title : "Home",
            init  : function () {
                return new Ext.Panel({
                    title     : "Home",
                    ctCls     : "contentTitle_ct",
                    html      : '<img src="resources/images/warehouse.jpg" width="386" height="513" />',
                    bodyStyle : "padding: 15px;"
                });
            },
            js    : ["widget1", "widget2", { url: "test.js"}],
            css   : ["widget1", "widget2", { url: "test.css"}]
        }),
        new Logic.View({ url : "app/colemanData/colemanData.js" }),
        new Logic.View({ url : "app/loads/loads.js" }),
        new Logic.View({ url : "app/loadsProNumber/loadsProNumber.js" }),
        new Logic.View({ url : "app/newproject/newproject.js" })
    ]
};

//    Sample Load
//
//    "LoadStatusName": "Draft",
//    "ShipmentTypeName": "Unknown",
//    "TenderedByCompanyName": null,
//    "CreatedByCompanyName": "COLEMAN CABLE, INC.",
//    "Cargos": null,
//    "IsNew": true,
//    "IsStar": false,
//    "LoadStatusId": 1,
//    "ShipmentTypeId": 0,
//    "TenderedByCompanyId": null,
//    "CreatedByCompanyId": 251,
//    "LoadReference": "1718267",
//    "DateCreated": new Date(1198972800000),
//    "Price": 697.0,
//    "PlanId": null,
//    "Id": 228671

Ext.ns("Logic.Models");

Logic.Models = {
    Load: Ext.data.Record.create([
        {
            name    : "Price", 
            type    : "int",
            convert : function (value, record) {
                return Ext.util.Format.usMoney(value);
            } 
        }
    ]),

    Contact: Ext.data.Record.create([])
};