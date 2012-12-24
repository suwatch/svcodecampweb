<%@ Page Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" 
      Inherits="SessionInterestChart" Title="Session Interest Chart" Codebehind="SessionInterestChart.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<!-- ExtJS --> 
    <link rel="stylesheet" type="text/css" href="JSProd/resources/extjs/4/resources/css/ext-all.css" />
    <script type="text/javascript" src="JSProd/resources/extjs/4/bootstrap.js"></script> 
    <script type="text/javascript" language="javascript">
        Ext.require([
            'Ext.form.*',
            'Ext.data.*',
            'Ext.chart.*',
            'Ext.grid.Panel',
            'Ext.layout.container.Column'
        ]);


        Ext.onReady(function () {

            Ext.define('Session', {
                extend: 'Ext.data.Model',
                fields: [
                    {name: 'Id', type: 'int'},
                    {name: 'Title', type: 'string'},
                    {name: 'Description', type: 'string'},
                    {name: 'InterestCountInt', type: 'int'},
                    {name: 'SessionPosted', type: 'int'},
                    {name: 'PresenterName', type: 'string'},
                    {name: 'PresenterURL', type: 'string'},
                    {name: 'SpeakerPictureUrl', type: 'string'},
                    {name: 'LoggedInUserInterested', type: 'bool'}
                ]
            });

            var rec = false,
                selectedStoreItem = false,
                loggedInUsername = '',
                selectItem = function(storeItem) {
                    selectedStoreItem = storeItem;
                    updateSessionDetailsPanel();
                    var sessionID = storeItem.get('Id'),
                        series = barChart.series.get(0),
                        i, items, l;

                    series.highlight = true;
                    series.unHighlightItem();
                    series.cleanHighlights();

                    for (i = 0, items = series.items, l = items.length; i < l; i++) {
                        if (sessionID == items[i].storeItem.get('Id')) {
                            series.highlightItem(items[i]);
                            break;
                        }
                    }
                    series.highlight = false;
                },
                greenDotImg = 'App_Themes/Gray2011/Images/green-dot.png',
                imInterestedRenderer = function (val) {
                    if (val) {
                        return '&nbsp;<img src="'+greenDotImg+'" />';
                    }
                },
                extInterestButtonHandler = function () {
                    this.disable();
                    var that = this;

                    var jsonData = {
                        success: true,
                        msg: 'ButtonName: Intersted userName:' + loggedInUsername + ' sessionId: ' + selectedStoreItem.data.Id + ' interestLevel: 2'
                    }
                    Ext.Ajax.request({
                        url: 'SessionInterest.ashx',
                        method: 'POST',
                        params: {
                            SessionId: selectedStoreItem.data.Id,
                            ButtonName: 'Interested',
                            UserName: loggedInUsername,
                            ChoiceNumber: 1
                        },
                        success: function (response) {
                            that.destroy();
                            Ext.get('interestButton').update('&nbsp;<img src="'+greenDotImg+'" /> &nbsp; <b>You are interested in this session.</b>');
                            selectedStoreItem.set({InterestCountInt: ++selectedStoreItem.data.InterestCountInt});
                            selectedStoreItem.set({LoggedInUserInterested: true});
                            var topSessionSelRecIdx = topSessionStore.find('Id', selectedStoreItem.data.Id);
                            if (topSessionSelRecIdx >= 0) {
                                var topSessionSelRec = topSessionStore.getAt(topSessionSelRecIdx);
                                topSessionSelRec.set({InterestCountInt: selectedStoreItem.data.InterestCountInt});
                                Ext.defer(selectItem, 1000, barChart, [selectedStoreItem]);
                            }

                        },
                        failure: function (response) {
                            Ext.Msg.alert('Error', 'There was an error updating your session interest');
                            this.enable();
                        }
                    })
                },
                extNotInterestedButtonHandler = function () {
                    this.disable();
                    var that = this;

                    var jsonData = {
                        success: true,
                        msg: 'ButtonName: Intersted userName:' + loggedInUsername + ' sessionId: ' + selectedStoreItem.data.Id + ' interestLevel: 2'
                    }
                    Ext.Ajax.request({
                        url: 'SessionInterest.ashx',
                        method: 'POST',
                        params: {
                            SessionId: selectedStoreItem.data.Id,
                            ButtonName: 'Not Interested',
                            UserName: loggedInUsername,
                            ChoiceNumber: 0
                        },
                        success: function (response) {
                            that.hide();

                            Ext.get('interestButton').update('');

                            that.destroy();

                            Ext.create('Ext.Button', {
                                text: 'Press Here To <br />Show Your Interest<br /> In This Session',
                                height: 60,
                                width: 120,
                                id: 'extInterstButton',
                                renderTo: Ext.get('interestButton'),
                                handler: extInterestButtonHandler
                            });

                            selectedStoreItem.set({InterestCountInt: --selectedStoreItem.data.InterestCountInt});
                            selectedStoreItem.set({LoggedInUserInterested: false});
                            selectedStoreItem.commit();

                            var topSessionSelRecIdx = topSessionStore.find('Id', selectedStoreItem.data.Id);

                            if (topSessionSelRecIdx >= 0) {
                                var topSessionSelRec = topSessionStore.getAt(topSessionSelRecIdx);
                                topSessionSelRec.set({InterestCountInt: selectedStoreItem.data.InterestCountInt});
                                Ext.defer(selectItem, 1000, barChart, [selectedStoreItem]);
                            }

                        },
                        failure: function (response) {
                            Ext.Msg.alert('Error', 'There was an error updating your session interest');
                            that.show();
                            this.enable();
                        }
                    })
                },
                updateSessionDetailsPanel = function () {
                    if (!selectedStoreItem) return;
                    var sessionDetailsPanel = Ext.getCmp('sessionDetails'), interestButton = Ext.getCmp('extInterstButton');
                    var sessionDetailsHtml = '<h2 style="color: #AF1D00; font: bold 16px Cambria,Georgia,Arial,Verdana,Tahoma">' + selectedStoreItem.data.Title + '</h1>';

                    if (interestButton) interestButton.destroy();

                    var presenterUrl = selectedStoreItem.data.PresenterURL;
                    if (presenterUrl.search('http') === -1 && presenterUrl != '') {
                        presenterUrl = 'http://' + presenterUrl;
                    }

                    sessionDetailsHtml += '<br /><table width="100%"><tr><td width="50%"><img src="'+selectedStoreItem.data.SpeakerPictureUrl+'" /></td><td style="vertical-align:top">';

                    var createButton = false;
                    if (loggedInUsername == "") {
                        sessionDetailsHtml +=  '<b>Please login to show your session interests.</b>';
                    } else {
                        if (selectedStoreItem.data.LoggedInUserInterested) {
                            sessionDetailsHtml +=  '&nbsp;<img src="'+greenDotImg+'" /> &nbsp; <b>You are interested in this session.</b><br /><span id="interestButton"></span>';
                        } else {
                            createButton = true;
                            sessionDetailsHtml +=  '<span id="interestButton"></span>';
                        }
                    }

                    sessionDetailsHtml += '</td></tr></table>'

                    sessionDetailsHtml += '<br />' + selectedStoreItem.data.PresenterName +'<br /><a href="'+ presenterUrl+'" target="_blank">'+ presenterUrl+'</a>';
                    sessionDetailsHtml += '<br /><br /> ' + selectedStoreItem.data.Description;
                    sessionDetailsPanel.update(sessionDetailsHtml);
                    Ext.defer(sessionDetailsPanel.doLayout, 500, sessionDetailsPanel, []);
                    if (loggedInUsername) {
                        if (createButton) {
                            Ext.create('Ext.Button', {
                                text: 'Press Here To <br />Show Your Interest<br /> In This Session',
                                height: 60,
                                width: 120,
                                id: 'extInterstButton',
                                renderTo: Ext.get('interestButton'),
                                handler: extInterestButtonHandler
                            });
                        } else {
                            Ext.create('Ext.Button', {
                                text: 'Remove Interest In Session',
                                id: 'extInterstButton',
                                renderTo: Ext.get('interestButton'),
                                handler: extNotInterestedButtonHandler
                            });
                        }
                    }

                },
                topSessionsStoreUpdater = function () {
                    var top25SessionsRecords = sessionsStore.getRange(0,24), top25SessionsArray = [];
                    for (var i = 0; i < top25SessionsRecords.length; i++) {
                        top25SessionsArray[top25SessionsArray.length] = [
                            top25SessionsRecords[i].data.Id,
                            top25SessionsRecords[i].data.Title,
                            top25SessionsRecords[i].data.Description,
                            top25SessionsRecords[i].data.InterestCountInt,
                            top25SessionsRecords[i].data.SessionPosted,
                            top25SessionsRecords[i].data.PresenterName,
                            top25SessionsRecords[i].data.PresenterURL,
                            top25SessionsRecords[i].data.SpeakerPictureUrl,
                            top25SessionsRecords[i].data.LoggedInUserInterested
                        ]
                    }
                    topSessionStore.loadData(top25SessionsArray);
                };

            var sessionsStore = new Ext.data.Store({
                model: 'Session',
                sorters: [
                    {
                        property : 'InterestCountInt',
                        direction: 'DESC'
                    }
                ],
                proxy: {
                    type: 'ajax',
                    url : 'GeneralHandlers/SessionsInterest.ashx',
                    limitParam: undefined,
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                },
                autoLoad: {
                    params: {
                        limit: 1000
                    }
                },
                sortOnLoad: false,
                listeners: {
                    load: function (store, records, success) {
                        if (success) {
                            loggedInUsername = store.proxy.reader.jsonData.loggedInUsername;
                            topSessionsStoreUpdater();
                            sessionsGrid.getSelectionModel().select(0);
                            Ext.defer(selectItem, 1000, this, [sessionsStore.getAt(0)]);
                        } else {
                            Ext.Msg.alert('Error', 'Sorry, there was an error loading Silicon Valley Code Camp Sessions.');
                        }
                    }
                }
            });

            var topSessionStore = new Ext.data.Store({
                model: 'Session',
                proxy: {
                    type: 'memory',
                    data: [],
                    reader: {
                        type: 'array'
                    }
                }
            });

            var barChart = Ext.create('Ext.chart.Chart', {
                flex: 1,
                shadow: true,
                animate: true,
                store: topSessionStore,
                axes: [{
                    type: 'Numeric',
                    position: 'left',
                    fields: ['InterestCountInt'],
                    minimum: 0,
                    label: {
                        font: '9px Arial'
                    },
                    hidden: true
                }, {
                    type: 'Category',
                    position: 'bottom',
                    fields: ['Title'],
                    hidden: true,
                    label: {
                        font: '9px Arial'
                    }
                }],
                series: [{
                    type: 'column',
                    axis: 'left',
                    highlight: true,
                    style: {
                        fill: '#456d9f'
                    },
                    highlightCfg: {
                        fill: '#a2b5ca'
                    },
                    label: {
                        display: 'InterestCountInt',
                        field: 'InterestCountInt',
                        color: '#000',
                        orientation: 'vertical',
                        'text-anchor': 'middle'
                    },
                    listeners: {
                        'itemmouseup': function(item) {
                             var series = barChart.series.get(0),
                                index = Ext.Array.indexOf(series.items, item),
                                selectionModel = sessionsGrid.getSelectionModel();

                                selectedStoreItem = item.storeItem;
                                selectionModel.select(index);
                        }
                    },
                    xField: 'Title',
                    yField: ['InterestCountInt']
                }]
            });

            // Grid for sessoins
            var sessionsGrid = Ext.create('Ext.grid.Panel', {
                id: 'sessionsGrid',
                store: sessionsStore,
                title:'Sessions (Randomized on First Load, Click Column Headers to Sort)',

                columns: [
                    {
                        width: 30,
                        sortable: true,
                        dataIndex: 'LoggedInUserInterested',
                        renderer: imInterestedRenderer
                    },
                    {
                        text   : 'Interest',
                        width : 65,
                        cls: 'headerWrap',
                        sortable : true,
                        dataIndex: 'InterestCountInt'
                    },
                    {
                        text : 'Session Title',
                        flex: 1,
                        sortable : true,
                        dataIndex: 'Title'
                    },
                    {
                        text   : 'Days Up',
                        width: 65,
                        sortable : true,
                        dataIndex: 'SessionPosted'
                    },
                    {
                        text   : 'Speaker Name',
                        width: 125,
                        sortable : true,
                        dataIndex: 'PresenterName'
                    }
                    
                ],

                listeners: {
                    selectionchange: function(model, records) {
                        var json, name, i, l, items, series, fields;
                        if (records[0]) {
                            var rec = records[0];
                            selectItem(rec);
                        }
                    },
                    sortchange: topSessionsStoreUpdater
                }
            });

            var sessionsPanel = Ext.create('Ext.panel.Panel', {
                title: 'Sessions By Interest (Top 25 Session From Sortable Grid Are Charted)                       Green Dot Indicates Your Interest',
                renderTo: Ext.get('sessionsTopChart'),
                frame: true,
                bodyPadding: 5,
                width: 870,
                height: 720,

                fieldDefaults: {
                    labelAlign: 'left',
                    msgTarget: 'side'
                },

                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },

                items: [
                    {
                        flex: 3,
                        border: false,
                        bodyStyle: 'background-color: transparent',
                        layout: 'border',
                        items: [{
                                region: 'center',
                                border: false,
                                margin: '0 3 0 0',
                                autoHeight: true,
                                layout: 'fit',
                                items: [sessionsGrid]
                            }, {
                                region: 'east',
                                title: 'Session Details',
                                collapsible: true,
                                width: 300,
                                autoScroll: true,
                                layout: 'fit',
                                items: [{
                                    id: 'sessionDetails',
                                    bodyPadding: 10,
                                    border: false,
                                    autoScroll: true,
                                    html: ''
                                }]
                            }]
                    },
                    {
                        height: 100,  // I really want to get rid of barchart but when I do, loading icon never disappears
                        layout: 'fit',
                        margin: '0 0 3 0',
                        items: [barChart]
                    }]
            });

        });
    </script>
    <div class="mainHeading">Session Interest Chart</div>

<div id="sessionsTopChart"></div>


</asp:Content>

