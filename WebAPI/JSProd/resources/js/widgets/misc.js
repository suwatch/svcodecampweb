Ext.ns("Logic.widgets");

Logic.widgets.ViewButton = function (config) {
    Logic.widgets.ViewButton.superclass.constructor.call(this, config);
};

Ext.extend(Logic.widgets.ViewButton, Ext.Button, {
    initComponent : function () {
        Logic.widgets.ViewButton.superclass.initComponent.call(this);

        if (Ext.isEmpty(this.viewId)) {
            this.viewId = this.text.toLowerCase();
        }

        this.on("click", function () {
            var selectedTab, className;
            $C.loadView(this.viewId);
            if (this.ownerCt.selectecdTab !== this.text) {
                selectedTab = Ext.query(".headerTabsSelected");
                className = selectedTab[0].className;
                className = className.replace("headerTabsSelected", "");
                selectedTab[0].className = className;
                this.addClass("headerTabsSelected");
                this.ownerCt.selectecdTab = this.text;
            }
        });
    },
    onMouseOver : function(e){
        if(!this.disabled){
            var internal = e.within(this.el,  true);
            if(!internal){
                if(!this.monitoringMouseOver){
                    Ext.getDoc().on('mouseover', this.monitorMouseOver, this);
                    this.monitoringMouseOver = true;
                }
                this.fireEvent('mouseover', this, e);
            }
            if(this.isMenuTriggerOver(e, internal)){
                this.fireEvent('menutriggerover', this, this.menu, e);
            }
        }
    }
});

Ext.reg("viewbutton", Logic.widgets.ViewButton);