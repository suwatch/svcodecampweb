Ext.ns("Logic");

Logic.View = function (config) {
    Ext.apply(this, config || {});

    // TODO: bubble to EventController
    // this.addEvents("beforeupdate", "afterupdate");

    this.render = function (container) {
        if (!Ext.isEmpty(container)) {
            if (container.updateRegion) {
                // this.fireEvent("beforeupdate", this);
                if (this.preRender) {
                    this.preRender();
                }

                if (this.init) {
                    var item = Ext.apply(this.init(), { logicView : this });
                    this.oldItem = item;
                    this.init = function () { return this.oldItem; };
                    container.updateRegion(this, "center", item);
                }

                if (this.postRender) {
                    this.postRender();
                }
                
                //this.fireEvent("afterupdate", this);
            }
            
            this.rendered = true;
            
            $C.events.on("viewsave", this.onViewSave, this);
        }
    };

    this.onViewSave = function () {
        if (this.save) {
            this.save();
        }
    };

    this.reload = function () {
        if (this.url) {
            $C.loadView({ url : this.url });
        }
    };

    this.getTitle = function () {
        return this.title;
    };
};

// IView
Ext.extend(Logic.View, Ext.util.Observable, {
    preRender:    null,
    render:       null,
    postRender:   null,
    getTitle:     null,
    init:         null,
    js:           null,
    css:          null,
    update:       null,
    setID:        null,
    title:        "",
    setTitle:     null,
    getTitle:     null,
    rendered:     false
});