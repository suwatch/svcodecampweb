ASPWeb.facilityGrid = function() {

    var pageSize = 18;

    //Ext.Msg.alert('in ASPWeb.facilityGrid. same as newsGrid but for facility');

    // {"CompanyAddressTypeId":null,"CompanyAddressTypeName":null,"Name":null,"Notes":null,"CompanyId":null,"CompanyName":null,"ActiveFlag":null,"AddressId":null,"Address":{"Line1":null,"Line2":null,"City":null,"State":null,"Zipcode":null,"Country":null,"Province":null,"Note":null,"AssociatedPhoneResult":null,"Id":0},"Id":0}
    var FacilityRecord = Ext.data.Record.create([
        { name: 'Id' },
        { name: 'CodeCampYearId' },
        { name: 'Attendeesid' },
        { name: 'SessionLevel_id' },
        { name: 'Username' },
        { name: 'Title' },
        { name: 'Description' },
        { name: 'CreateDate' },
        { name: 'LectureRoomsId' },
        { name: 'SessionTimesId' }
    ]);

    //Ext.Msg.alert('1');

    var reader = new Ext.data.JsonReader(
    {                             // The metadata property, with configuration options:
        totalProperty: "total",   //   the property which contains the total dataset size (optional)
        root: "rows",
        idProperty: "Id"
    },
            FacilityRecord  // Ext.data.Record constructor that provides mapping for JSON object
            );

    var storeFacility = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            api: {
                load: '../Data/Facility/Get'
            }
        }),
        id: 'facilityEntry',
        reader: reader,
        autoLoad: false,
        paramsAsHash: true
    });

    storeFacility.baseParams.query = Ext.util.JSON.encode({});

    //create a container for the edit button
    function renderBtn(val, p, record) {
        var contentId = Ext.id();
        createGridButton.defer(1, this, [val, contentId, record]);
        return('<div id="' + contentId + '"></div>');
    }

    //create the edit button
    function createGridButton(value, contentid, record) {
        new Ext.Button({
            text: 'Edit',
            handler : function() {
                editWin(value, record);
            }
        }).render(document.body, contentid);
    }

    function editWin(value, record) {

        //console.info(value);

        if (!facilityEditWindow) {
            ASPWeb.facilityEdit();
            facilityEditWindow = Ext.getCmp('facilityEditWindow');
        }

        if (facilityEditWindow.hidden) {
            facilityEditWindow.show();
        }

        if (!facilityForm) {
            facilityForm = Ext.getCmp('facilityForm').getForm();
        }
        //show the facility

        //reset the facility form
        facilityForm.reset();
        //set the record values on the facility form
        facilityForm.setValues({
            Id: value,
            Name: record.data.Name,
            CompanyName: record.data.CompanyName,
            Line1: record.data['Address.Line1'],
            Line2: record.data['Address.Line2'],
            City: record.data['Address.City'],
            State: record.data['Address.State'],
            Zipcode: record.data['Address.Zipcode'],
            Country: record.data['Address.Country'],
            Province: record.data['Address.Province']
        });

    }

    //Get the facility record
    //Used for load the edit window with data
    

    //Queries the grid
    function queryGrid() {
        //Get the search field value
        var facilitySearchField = SearchFacilityBar.getComponent('searchFacilityField').getValue();
        var searchQuery = {};

        if (facilitySearchField !== '') {
            searchQuery = {
                Search: facilitySearchField
            };
        }
        storeFacility.baseParams.query = Ext.util.JSON.encode(searchQuery);
        storeFacility.load({ params: { start: 0, limit: pageSize} });
    }

    var SearchFacilityBar = new Ext.Toolbar({
        style: {
            background: 'white none ',
            border: 'none'
        },
        items: [{
            xtype: 'textfield',
            name: 'searchFacilityField',
            id: 'searchFacilityField',
            itemId: 'searchFacilityField',
            fieldLabel: '',
            width: 250,
            listeners:{
                specialkey: function(f, e) {
                    if (e.getKey() == e.ENTER) {
                        queryGrid();
                    }
                }
            }
        }, {
            xtype: 'tbspacer',
            width: 3
        }, {
            xtype: 'button',
            cls: 'grayBtn',
            width: 65,
            text: 'Search',
            handler: queryGrid
        }]
    });

    var grid1 = new Ext.grid.GridPanel({
        frame: false,
        store: storeFacility,
        tbar: [{
            text: 'Refresh',
            xtype: 'tbbutton',
            handler: function(btn) {
                storeFacility.load();
            }
        }],
        cm: new Ext.grid.ColumnModel({
            columns: [
                { header: "", width: 40, menuDisabled:true, lazyRender: true, sortable: false, fixed:true, renderer: renderBtn },
                { header: "Name", width: 400, sortable: true, autoExpandColumn: true, dataIndex: 'Name' }
            ]
        }),
        viewConfig: {
            forceFit: true
        },
        height: 400,
        loadMask: true,
        collapsible: false,
        animCollapse: false,
        iconCls: 'icon-grid',
        bbar: new Ext.PagingToolbar({
            pageSize: pageSize,
            store: storeFacility,
            displayInfo: true,
            displayMsg: 'Displaying topics {0} - {1} of {2}',
            emptyMsg: "No Facilities Items to display"
        })
    });

    //        Ext.log('4');
    storeFacility.load({ params: { start: 0, limit: pageSize} });
    //console.dir(storeFacility);

    var ct = new Ext.Panel({
        renderTo: 'FACILITYLISTID',
        border: false,
        title: '',
        height: 500,
        layout: 'fit',
        tbar: SearchFacilityBar,
        items: [
            grid1
        ]
    });


};
