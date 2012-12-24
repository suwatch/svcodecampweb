Ext.namespace('Demo');

Demo.ProductGrid = Ext.extend(Ext.grid.GridPanel, {
	// Properties
	title : 'Product Inventory',
	viewConfig : {
		forceFit : true
	},

	// Overriden methods
	initComponent : function() {
		this.columns = [{
					dataIndex : 'productSku',
					header : 'Product SKU'
				}, {
					dataIndex : 'price',
					header : 'Price',
					renderer : Ext.util.Format.usMoney
				}, {
					dataIndex : 'expires',
					header : 'Expires',
					renderer : Ext.util.Format.dateRenderer('m/d/Y')
				}];
		Demo.ProductGrid.superclass.initComponent.call(this);
	}
		// Custom Methods
	});

Ext.onReady(function() {

			var loadHash = [];

			var onLoad = function() {
				loadHash.push(0);
				// console.log(loadHash.length);
				if (loadHash.length >= 2) {
					storeSpeaker.each(function(r) {
								r.data.sessions = [];
								storeSession.filter('Username', r
												.get('UserName'));
								storeSession.each(function(rec) {
											r.data.sessions.push({
														title : rec
																.get('Title')
													})
										});
							});

					var tpl = Ext.Template.from('detailtpl');

					var myContentIdWidth = Ext.get('ContentId').getWidth();

					var grid = new Ext.grid.GridPanel({
								store : storeSpeaker,
								columns : [{
											header : 'UserName',
											dataIndex : 'UserName',
											width : myContentIdWidth,
											sortable : true,
											renderer : function(value, meta,
													rec) {
												return tpl
														.applyTemplate(rec.data);

											}
										}],
								height : 300,
								width: myContentIdWidth
							});
					// console.log('grid');

					var p = panel.items.itemAt(0);
					p.add(grid);
					p.doLayout();

					Ext.Element.get('loading').remove();
				}
			}

			var storeSpeaker = new Ext.data.JsonStore({
						storeId : 'storeSpeakerId',
						url : 'SessionJSONHandler.ashx?SpeakerData=true',
						root : '',
						listeners : {
							load : onLoad,
							single : true
						},
						fields : ['FirstName', 'LastName', 'UserWebSite',
								'UserName', 'ZipCode', 'CreateDate', 'UserBio',
								'PKID1']
					});
			storeSpeaker.load();

			var storeTag = new Ext.data.JsonStore({
						storeId : 'storeTagId',
						url : 'SessionJSONHandler.ashx?TagData=true',
						root : '',
						listeners : {
							load : onLoad,
							single : true
						},
						fields : ['Tagname', 'Id', 'Sessionid']
					});
			storeSpeaker.load();

			var storeSession = new Ext.data.JsonStore({
						storeId : 'storeIdSession',
						url : 'SessionJSONHandler.ashx?SessionData=true',
						root : '',
						listeners : {
							load : onLoad,
							single : true
						},
						fields : ['Username', 'Title']
					});
			storeSession.load();

			// store does not show up in help

			var myContentIdHeight = Ext.get('ContentId').getHeight();

			var panel = new Ext.Panel({
						renderTo : 'myDiv',
						layout : 'border',
						items : [{
									layout : 'fit',
									region : 'center'
								}],
						title : 'Speakers',
						frame : true,
						autoWidth : true,
						height : myContentIdHeight
					});

			var iii = 0;

		});