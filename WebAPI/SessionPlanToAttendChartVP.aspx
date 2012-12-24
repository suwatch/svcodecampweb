<%@ Page Language="C#"  AutoEventWireup="true" 
      Inherits="SessionPlanToAttendChartVP" Title="Session Plan To Attend Chart Standalone" Codebehind="SessionPlanToAttendChartVP.aspx.cs" %>


<!DOCTYPE html >

<html>


<head id="Head1" runat="server">
    <title>Session Plan To Attend Chart Standalone</title>
    <link rel="stylesheet" type="text/css" href="JSProd/resources/extjs/4/resources/css/ext-all.css" />
    <meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; minimum-scale=1.0; user-scalable=0;" />
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
                    { name: 'Id', type: 'int' },
                    { name: 'Title', type: 'string' },
                    { name: 'Description', type: 'string' },
                    { name: 'SessionTime', type: 'string' },
                    { name: 'PlanAheadCountInt', type: 'int' },
                    { name: 'SessionPosted', type: 'int' },
                    { name: 'PresenterName', type: 'string' },
                    { name: 'PresenterURL', type: 'string' },
                    { name: 'SpeakerPictureUrl', type: 'string' },
                    { name: 'LoggedInUserPlanToAttend', type: 'bool' }
                ]
            });

            Ext.define('SessionTime', {
                extend: 'Ext.data.Model',
                fields: [
                    { name: 'Id', type: 'int' },
                    { name: 'StartTimeFriendly', type: 'string' }
                ]
            });

            var rec = false,
                selectedStoreItem = false,
                loggedInUsername = '',
                selectItem = function (storeItem) {
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
                        return '&nbsp;<img src="' + greenDotImg + '" />';
                    }
                },
                extInterestButtonHandler = function () {
                    this.disable();
                    var that = this;

                    var jsonData = {
                        success: true,
                        msg: 'ButtonName: Intersted userName:' + loggedInUsername + ' sessionId: ' + selectedStoreItem.data.Id + ' interestLevel: 2'
                    };
                    Ext.Ajax.request({
                        url: 'SessionInterest.ashx',
                        method: 'POST',
                        params: {
                            SessionId: selectedStoreItem.data.Id,
                            ButtonName: 'Interested',
                            UserName: loggedInUsername,
                            ChoiceNumber: 2
                        },
                        success: function (response) {
                            that.destroy();

                            var previousPlanToAttend = sessionsStore.findRecord('LoggedInUserPlanToAttend', true);
                            if (previousPlanToAttend) {
                                previousPlanToAttend.set({ LoggedInUserPlanToAttend: false });

                                //previousPlanToAttend.set({PlanAheadCountInt: selectedStoreItem.data.PlanAheadCountInt--});
                                //           previousPlanToAttend.set({ PlanAheadCountInt: selectedStoreItem.data.PlanAheadCountInt++ });

                                previousPlanToAttend.commit();
                            }

                            Ext.get('interestButton').update('<div class="status"><img src="' + greenDotImg + '" /> &nbsp; <b>You plan to attend this session.</b></div>');
                            //selectedStoreItem.set({PlanAheadCountInt: ++selectedStoreItem.data.PlanAheadCountInt});
                            selectedStoreItem.set({ LoggedInUserPlanToAttend: true });
                            var topSessionSelRecIdx = topSessionStore.find('Id', selectedStoreItem.data.Id);
                            if (topSessionSelRecIdx >= 0) {
                                var topSessionSelRec = topSessionStore.getAt(topSessionSelRecIdx);
                                //topSessionSelRec.set({PlanAheadCountInt: selectedStoreItem.data.PlanAheadCountInt});
                                Ext.defer(selectItem, 1000, barChart, [selectedStoreItem]);
                            }

                        },
                        failure: function (response) {
                            Ext.Msg.alert('Error', 'There was an error updating your plan to attend session.');
                            this.enable();
                        }
                    });
                },
                extNotInterestedButtonHandler = function () {
                    this.disable();
                    var that = this;

                    var jsonData = {
                        success: true,
                        msg: 'ButtonName: Intersted userName:' + loggedInUsername + ' sessionId: ' + selectedStoreItem.data.Id + ' interestLevel: 2'
                    };
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
                                text: 'Press Here To <br />Plan To Attend<br /> This Session',
                                height: 60,
                                width: 120,
                                id: 'extInterstButton',
                                renderTo: Ext.get('interestButton'),
                                handler: extInterestButtonHandler
                            });

                            // commented out pgk
                            //selectedStoreItem.set({PlanAheadCountInt: --selectedStoreItem.data.PlanAheadCountInt});


                            selectedStoreItem.set({ LoggedInUserPlanToAttend: false });
                            selectedStoreItem.commit();

                            // commented out pgk
                            var topSessionSelRecIdx = topSessionStore.find('Id', selectedStoreItem.data.Id);

                            if (topSessionSelRecIdx >= 0) {
                                var topSessionSelRec = topSessionStore.getAt(topSessionSelRecIdx);


                                //topSessionSelRec.set({PlanAheadCountInt: selectedStoreItem.data.PlanAheadCountInt});

                                Ext.defer(selectItem, 1000, barChart, [selectedStoreItem]);
                            }

                        },
                        failure: function (response) {
                            Ext.Msg.alert('Error', 'There was an error updating your session interest');
                            that.show();
                            this.enable();
                        }
                    });
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

                    sessionDetailsHtml += '<br /><table width="100%"><tr><td width="50%"><img src="' + selectedStoreItem.data.SpeakerPictureUrl + '" /></td><td style="vertical-align:top; text-align: center">';

                    var createButton = false;
                    if (loggedInUsername == "") {
                        sessionDetailsHtml += '<div class="status"><b>Please login to show your plan to attend sessions.</b></div>';
                    } else {
                        if (selectedStoreItem.data.LoggedInUserPlanToAttend) {
                            sessionDetailsHtml += '<div class="status"><img src="' + greenDotImg + '" /> &nbsp; <b>You plan to attend this session.</b></div><span id="interestButton"></span>';
                        } else {
                            createButton = true;
                            sessionDetailsHtml += '<span id="interestButton"></span><span> Only one allowed<br />per time slot </span>';
                        }
                    }

                    sessionDetailsHtml += '</td></tr></table>'

                    sessionDetailsHtml += '<br />' + selectedStoreItem.data.PresenterName + '<br /><a href="' + presenterUrl + '" target="_blank">' + presenterUrl + '</a>';
                    sessionDetailsHtml += '<br /><br /> ' + selectedStoreItem.data.Description;
                    sessionDetailsPanel.update(sessionDetailsHtml);
                    Ext.defer(sessionDetailsPanel.doLayout, 500, sessionDetailsPanel, []);
                    if (loggedInUsername) {
                        if (createButton) {
                            Ext.create('Ext.Button', {
                                text: 'Press Here To <br />Plan To Attend<br /> This Session <br />',
                                height: 60,
                                width: 120,
                                id: 'extInterstButton',
                                renderTo: Ext.get('interestButton'),
                                handler: extInterestButtonHandler
                            });
                        } else {
                            Ext.create('Ext.Button', {
                                text: 'Remove Plan to Attend This Session',
                                id: 'extInterstButton',
                                renderTo: Ext.get('interestButton'),
                                handler: extNotInterestedButtonHandler
                            });
                        }
                    }

                },
                topSessionsStoreUpdater = function () {
                    var top25SessionsRecords = sessionsStore.getRange(0, 24), top25SessionsArray = [];
                    for (var i = 0; i < top25SessionsRecords.length; i++) {
                        top25SessionsArray[top25SessionsArray.length] = [
                            top25SessionsRecords[i].data.Id,
                            top25SessionsRecords[i].data.Title,
                            top25SessionsRecords[i].data.Description,
                            top25SessionsRecords[i].data.SessionTime,
                            top25SessionsRecords[i].data.PlanAheadCountInt,
                            top25SessionsRecords[i].data.SessionPosted,
                            top25SessionsRecords[i].data.PresenterName,
                            top25SessionsRecords[i].data.PresenterURL,
                            top25SessionsRecords[i].data.SpeakerPictureUrl,
                            top25SessionsRecords[i].data.LoggedInUserPlanToAttend
                        ];
                    }
                    topSessionStore.loadData(top25SessionsArray);
                };

            var sessionsStore = new Ext.data.Store({
                model: 'Session',
                sorters: [
                        {
                            property: 'PlanAheadCountInt',
                            direction: 'DESC'
                        }
                    ],
                proxy: {
                    type: 'ajax',
                    url: 'GeneralHandlers/SessionsPlanToAttend.ashx',
                    limitParam: undefined,
                    reader: {
                        type: 'json',
                        root: 'rows'
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
                    fields: ['PlanAheadCountInt'],
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
                        display: 'PlanAheadCountInt',
                        field: 'PlanAheadCountInt',
                        color: '#000',
                        orientation: 'vertical',
                        'text-anchor': 'middle'
                    },
                    listeners: {
                        'itemmouseup': function (item) {
                            var series = barChart.series.get(0),
                                index = Ext.Array.indexOf(series.items, item),
                                selectionModel = sessionsGrid.getSelectionModel();

                            selectedStoreItem = item.storeItem;
                            selectionModel.select(index);
                        }
                    },
                    xField: 'Title',
                    yField: ['PlanAheadCountInt']
                }]
            });

            // Grid for sessions
            var sessionsGrid = Ext.create('Ext.grid.Panel', {
                id: 'sessionsGrid',
                store: sessionsStore,
                title: 'Sessions (Randomized on First Load, Click Column Headers to Sort)',

                columns: [
                    {
                        width: 30,
                        sortable: true,
                        dataIndex: 'LoggedInUserPlanToAttend',
                        renderer: imInterestedRenderer
                    },
                    {
                        text: 'Attend',
                        width: 65,
                        cls: 'headerWrap',
                        sortable: true,
                        dataIndex: 'PlanAheadCountInt'
                    },
                    {
                        text: 'Session Title',
                        flex: 1,
                        sortable: true,
                        dataIndex: 'Title'
                    },
                    {
                        text: 'Speaker Name',
                        width: 125,
                        sortable: true,
                        dataIndex: 'PresenterName'
                    }

                ],

                listeners: {
                    selectionchange: function (model, records) {
                        var json, name, i, l, items, series, fields;
                        if (records[0]) {
                            var rec = records[0];
                            selectItem(rec);
                        }
                    },
                    sortchange: topSessionsStoreUpdater
                }
            });

            // renderTo: Ext.get('sessionsTopChart'),

            var sessionsPanel = Ext.create('Ext.panel.Panel', {
                title: 'Plan Your Sessions to Attend at Code Camp',
                region: 'center',
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
                        height: 200,
                        layout: 'fit',
                        margin: '0 0 3 0',
                        collapsible: true,
                        collapsed: true,
                        title: 'Bar Chart Showing Number Of Attendees Planning to Attend each Session',
                        items: [barChart]
                    },
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
                    }]
            });

            Ext.create('Ext.container.Viewport', {
                layout: 'border',
                items: [{
                    region: 'north',
                    padding: '3px 5px',
                    frame: false,
                    bodyStyle: {background: 'none'},
                    border: false,
                    html: '<div style="float: right"><a href="SessionPlanToAttendChart.aspx" >Back To Full Code Camp Site</a></div><div id="sessionsChartSchedule"><ul class="days" id="sessionDays"><li id="sessionDaySaturday" class="selected">Saturday</li><li id="sessionDaySunday">Sunday</li></ul><div style="clear: both"></div><ul class="times" id="sessionTimesSaturday"><li id="Saturday1" class="selected">9:45 AM</li><li id="28">11:15 AM</li><li id="29">1:45 PM</li><li id="30">3:30 PM</li><li id="31">5:00 PM</li></ul><ul class="times hide" id="sessionTimesSunday"><li id="Sunday1">9:15 AM</li><li id="33">10:45 AM</li><li id="34">1:15 PM</li><li id="35">2:45 PM</li></ul></div>'
                },
                    sessionsPanel

                ]
            });



            var sessionsChartSchedule = Ext.get('sessionsChartSchedule'), defaultsLoaded = false, elId, sessionRecord, sessionDay = 'Saturday', sessionTime, prevSessionDay = Ext.get('sessionDaySaturday'), prevSessionTime = Ext.get('Saturday1'), allSessions = false;


            var sessionsTimesStore = new Ext.data.Store({
                model: 'SessionTime',
                proxy: {
                    type: 'ajax',
                    url: 'GeneralHandlers/SessionTimes.ashx',
                    reader: {
                        type: 'json',
                        successProperty: 'success',
                        idProperty: 'Id',
                        root: 'rows'
                    }
                },
                autoLoad: true,
                listeners: {
                    load: function () {
                        if (!defaultsLoaded) {
                            updateInterestData('Saturday', '9:45 AM');
                            defaultsLoaded = true;
                        }
                    }
                }
            });

            sessionsChartSchedule.on({
                click: function (e, el) {
                    elId = el.id;
                    if (el.tagName == 'LI') {
                        // Schedules by Day
                        if (elId.indexOf('sessionDay') != -1) {

                            if (prevSessionTime) {
                                prevSessionTime.removeCls('selected');
                            }

                            // All Sessions
                            if (elId.indexOf('All') != -1) {
                                allSessions = true;
                                Ext.get('sessionTimesSaturday').addCls('hide');
                                Ext.get('sessionTimesSunday').addCls('hide');
                                updateInterestData('All');

                            } else {
                                // Saturday or Sunday
                                allSessions = false;
                                sessionDay = elId.substr(10);

                                if (prevSessionDay) {
                                    prevSessionDay.removeCls('selected');
                                }

                                if (sessionDay == 'Saturday') {
                                    Ext.get('sessionTimesSunday').addCls('hide');
                                    Ext.get('sessionTimesSaturday').removeCls('hide');
                                    prevSessionDay = Ext.get('sessionDaySaturday');

                                } else {
                                    Ext.get('sessionTimesSunday').removeCls('hide');
                                    Ext.get('sessionTimesSaturday').addCls('hide');
                                    prevSessionDay = Ext.get('sessionDaySunday');

                                }

                                prevSessionTime = Ext.get(sessionDay + '1');
                                prevSessionTime.addCls('selected');
                                updateInterestData(sessionDay, prevSessionTime.dom.innerHTML);

                            }

                            if (prevSessionDay) {
                                prevSessionDay.removeCls('selected');
                            }
                            prevSessionDay = Ext.get(elId);

                        } else {
                            // Time Schedules
                            if (prevSessionTime) {
                                prevSessionTime.removeCls('selected');
                            }

                            prevSessionTime = Ext.get(elId);

                            if (prevSessionTime.dom.innerHTML == 'All Times') {
                                updateInterestData('All');
                            } else {
                                updateInterestData(sessionDay, prevSessionTime.dom.innerHTML);
                            }


                        }

                        Ext.get(elId).addCls('selected');

                    }

                }
            });

            var updateInterestData = function (day, time) {
                if (day == 'All') {
                    sessionsStore.load({ params: {} });
                } else {
                    var sessionTimeRec = sessionsTimesStore.findRecord('StartTimeFriendly', time + ' ' + day);
                    if (sessionTimeRec.data) {
                        sessionsStore.load({ params: { SessionTimesId: sessionTimeRec.data.Id} });
                    }
                }
            };

        });
    </script>

    </head>
    <body>
    <div>

     </div>


        <script type="text/javascript">
           var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
           document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
        </script>
        <script type="text/javascript">
            try {
                var pageTracker = _gat._getTracker("UA-648264-3");
                pageTracker._trackPageview();
            } catch (err) { }</script>

    </body>
</html>

