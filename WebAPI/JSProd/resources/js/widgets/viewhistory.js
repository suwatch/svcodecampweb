Logic.ViewHistory = function (config) {
    Logic.ViewHistory.superclass.constructor.call(this, config);
};

Ext.extend(Logic.ViewHistory, Ext.Toolbar, {
    allCapacity     : 10,
    displayCapacity : 5,
    historyItems    : [],

    initComponent   : function () {
        Logic.ViewHistory.superclass.initComponent.call(this);
        this.initHistory();

        $C.events.on("viewloaded", this.addHistoryItem, this);
        $C.events.on("loadselected", this.onLoadSelected, this);
        $C.events.on("contactselected", this.onContactSelected, this);

        Ext.History.on("change", this.onHistoryChange, this);

        this.menuButton = new Ext.Toolbar.Button({
            hidden: true,
            text: "More...",
            menu: new Ext.menu.Menu()
        });

        this.on("render", function () {
            this.addButton(this.menuButton);
        }, this);

        $C.events.on("ready", function () {
            var query = $C.getHrefHash();
            
            if (!Ext.isEmpty(query, false)) {
                this.onHistoryChange(query, true);
            }
        }, this);
    },

    initHistory: function () {
        Ext.DomHelper.append(Ext.getBody(), {
            id  : "history-form",
            tag : "form",
            cls : "x-hidden",
            children : [
                { 
                    tag  : "input", 
                    type : "hidden", 
                    id   : "x-history-field" 
                },
                { 
                    tag  : "iframe", 
                    id   : "x-history-frame" 
                }
            ]
        });
        Ext.History.init();
    },

    addHistoryToken : function (token) {
        if (this.historySuspended !== true) {
            this.currentToken = token;
            Ext.History.add(token, false);
        }
    },

    suspendHistory : function () {
        this.historySuspended = true;
    },

    resumeHistory : function () {
        this.historySuspended = false;
    },

    onHistoryChange : function (token, isQuery) {
        if (token && this.currentToken != token) {
            var parts = token.split(":");
            
            if (parts.length == 1) {
                parts.push($C.getViewIdByTitle(parts[0]));
                this.history_view(parts, isQuery);
            } else {
                this["history_" + parts[0]](parts, isQuery);
            }
        }
        
        this.currentToken = "";
    },

    //history handlers - 'history_' prefix is required
    history_tabs : function (tokens, grid, isQuery) {
        var selectRecord = function (tokens, grid, isQuery) {
            var initiator = tokens.length == 4 ? "history" : "browser";
            initiator = isQuery ? undefined : initiator;
            //really bad solution to pass to the history who is initiator
            //TODO: need to find better way
            grid.initiator = initiator;
            grid.getSelectionModel().selectRecords([grid.store.getById(tokens[1])]);
            grid.initiator = undefined;
        };

        var doRequest = function (tokens, grid) {
            var activePage = grid.bottomToolbar ? grid.bottomToolbar.getPageData().activePage : -1;
            var requiredPage = tokens[2];
            
            if (activePage >= 0 && activePage != requiredPage) {
                grid.store.on("load", function () {
                    selectRecord(tokens, grid, isQuery);
                }, this, { single: true });
                grid.bottomToolbar.changePage(requiredPage);
            } else {
                selectRecord(tokens, grid, isQuery);
            }
        };

        if (grid.store.proxy.activeRequest[Ext.data.Api.READ]) {
            grid.store.on("load", function () {
                doRequest(tokens, grid);
            }, this, { single: true });

            grid.store.on("loadexception", function () {
                doRequest(tokens, grid);
            }, this, { single: true });
        }
        else {
            doRequest(tokens, grid);
        }
    },

    history_Loads : function (tokens, isQuery) {
        this.getExplorerTabs().setActiveTab(1);
        this.history_tabs(tokens, this.getLoadsGrid(), isQuery);
    },

    history_Contacts : function (tokens, isQuery) {
        this.getExplorerTabs().setActiveTab(3);
        this.history_tabs(tokens, this.getContactsGrid(), isQuery);
    },

    history_view : function (tokens, isQuery) {
        $C.loadView({ id: tokens[1], initiator: isQuery ? undefined : "browser" });
    },
    //end history handlers

    getExplorerTabs : function () {
        return Ext.getCmp("dataexplorer.tabs");
    },

    getLoadsGrid : function () {
        return this.getExplorerTabs().tabs.loads;
    },

    getContactsGrid : function () {
        return this.getExplorerTabs().tabs.contacts;
    },

    onLoadSelected : function (args) {
        var v = $C.getViewById($C.defaultViews.loads.id);
        var page = this.getLoadsGrid().bottomToolbar.getPageData().activePage;
       
        this.addHistoryItem(v, page, args.record.json.Id, { initiator: this.getLoadsGrid().initiator });
    },

    onContactSelected : function (args) {
        var v = $C.getViewById($C.defaultViews.contacts.id);
        var page = this.getContactsGrid().bottomToolbar.getPageData().activePage;
        
        this.addHistoryItem(v, page, args.record.json.Id, { initiator: this.getContactsGrid().initiator });
    },

    removeHistoryButton : function (item) {
        if (this.rendered && item) {
            if (item.destroy) {
                item.destroy();
            }

            if (item.removeListeners) {
                item.removeListeners();
            }

            if (item.dom && item.dom.parentNode) {
                var td = item.dom.parentNode;
                if (!Ext.isEmpty(td)) {
                    td.removeChild(item.dom);
                    td.parentNode.removeChild(td);
                }
            }

            this.items.remove(item);
            this.doLayout();
        }
    },

    addHistoryButton : function (title, handler, pageId) {
        var buttonConfig = {
            text: title,
            viewId: this.currentViewId,
            subId: this.currentSubId,
            handler: handler,
            historyScope: this,
            allowDepress: false,
            pageId: pageId,
            toggleGroup: "historyItem",
            enableToggle: true
        }, newButton;

        if (this.items.getCount() > 0) {
            newButton = this.insertButton(0, buttonConfig);
        } else {
            newButton = this.addButton(buttonConfig);
        }
        
        newButton.on("render", function () { this.toggle(); }, newButton, { single: true });

        this.activeHistoryItem = { item: newButton, index: 0 };

        this.doLayout();
        
        return newButton;
    },

    addHistoryItem : function (view, pageId, subId, config) {
        if (view.id == "404" || (this.currentViewId == view.id && (Ext.isEmpty(subId) || this.currentSubId == subId))) {
            return;
        }

        if (!Ext.isEmpty(subId)) {
            this.currentSubId = subId;
        } else {
            this.currentSubId = undefined;
        }

        this.currentViewId = view.id;
        var title;

        //if ensure view then skip history because will be loaded record from grid
        if (!Ext.isEmpty(config) && config.ensureView === true) {
            return;
        }

        if (!Ext.isEmpty(config) && (config.initiator)) {
            var isBrowser = config.initiator == "browser";
            if (!isBrowser) {
                if (Ext.isEmpty(this.currentSubId)) {
                    this.addHistoryToken(view.parent);
                } else {
                    this.addHistoryToken(view.getTitle() + ":" + this.currentSubId + ":" + (pageId ? pageId : 1));
                }
            }

            title = Ext.isEmpty(this.currentSubId) ? view.getTitle() : (view.getTitle() + ":" + this.currentSubId);
            var found = false;
            var m_item;
            
            for (var i = 0; i < this.items.length; i++) {
                m_item = this.items.get(i);
                if (m_item.text == title && !found) {
                    if (isBrowser) {
                        m_item.toggle(true, true);
                        found = true;
                    }
                }
                else {
                    m_item.toggle(false, true);
                }
            }

            if (!this.menuButton.menu.items) {
                return;
            }

            for (var j = 0; j < this.menuButton.menu.items.length; j++) {
                m_item = this.menuButton.menu.items.get(j);
                if (m_item.text == title && !found) {
                    if (isBrowser) {
                        m_item.setChecked(true, true);
                        found = true;
                    }
                }
                else {
                    m_item.setChecked(false, true);
                }
            }
            return;
        }

        if (this.historyItems.length >= this.allCapacity) {
            this.menuButton.menu.remove(this.historyItems[this.historyItems.length - 1].button);
            this.historyItems.splice(this.historyItems.length - 1, 1);
        }

        if (this.historyItems.length >= this.displayCapacity) {
            this.menuButton.show();

            var btn = this.historyItems[this.displayCapacity - 1].button;

            var menuItemConfig = {
                text: btn.text,
                viewId: btn.viewId,
                subId: btn.subId,
                handler: btn.handler,
                pageId: btn.pageId,
                group: btn.toggleGroup,
                historyScope: btn.historyScope
            };

            var item;
            
            if (this.items.getCount() > 0) {
                item = this.menuButton.menu.insert(0, new Ext.menu.CheckItem(menuItemConfig));
            } else {
                item = this.menuButton.menu.addMenuItem(new Ext.menu.CheckItem(menuItemConfig));
            }
            
            this.historyItems[this.displayCapacity - 1].button = item;
            this.removeHistoryButton(btn);
        }

        var handler = Ext.emptyFn;

        if (Ext.isEmpty(this.currentSubId)) {
            title = view.getTitle();
            handler = function () {
                $C.loadView({ id: this.viewId, initiator: "history" });
            };
        } else {
            title = view.getTitle() + ":" + this.currentSubId;

            handler = function () {
                this.historyScope["history_" + view.parent]([view.parent, this.subId, this.pageId, true]);
            };
        }

        this.historyItems.splice(0, 0, {
            viewId: this.currentViewId,
            subId: this.currentSubId,
            pageId: pageId,
            button: this.addHistoryButton(title, handler, pageId)
        });

        if (Ext.isEmpty(this.currentSubId)) {
            this.addHistoryToken(view.parent);
        } else {
            this.addHistoryToken(view.getTitle() + ":" + this.currentSubId + ":" + (pageId ? pageId : 1));
        }
    }
});