Ext.namespace('CodeCamp.MySchedule');

CodeCamp.MySchedule.SingleTimeSlotGrid = Ext.extend(Ext.grid.GridPanel, {
	// Properties
	title : 'Single Time Slot',
	viewConfig : {
		forceFit : true
	},

	// Overriden methods
	initComponent : function() {
		this.columns = [{
					dataIndex : 'Id',
					header : 'Id'
				}, {
					dataIndex : 'Starttimefriendly',
					header : 'Start Time'
				}, {
					dataIndex : 'Starttime',
					header : 'Start Time DateTime',
					renderer : Ext.util.Format.dateRenderer('m/d/Y')
				}];

		CodeCamp.MySchedule.SingleTimeSlotGrid.superclass.initComponent
				.call(this);
	}
		// Custom Methods
	});

CodeCamp.MySchedule.TimeSlotStore = Ext.extend(Ext.data.JsonStore, {
			constructor : function(config) {
				config = config || {};
				config.url = 'SessionJSONHandler.ashx?SpeakerData=true';
				config.root = '';
				config.fields = ['Starttimefriendly', 'Id', {
							name : 'StartTime',
							type : 'date',
							dateFormat : 'timestamp'
						}]; // fields go here...
				CodeCamp.MySchedule.TimeSlotStore.superclass.constructor.call(
						this, config);
			}
		});

CodeCamp.MySchedule.App = Ext.extend(Ext.Panel, {
			// Properties
			layout : 'border',

			// Overriden methods

			initComponent: function() {
			
			    var store1 = new CodeCamp.MySchedule.TimeSlotStore();
			
				this.grid = new CodeCamp.MySchedule.SingleTimeSlotGrid({
							itemId : 'grid',
							store : store1;
						});
				// three items here...
				this.items = [{
							region : 'center',
							height : 500,
							title : 'Content',
							layout : 'fit',
							items : [this.grid]
						}];
				CodeCamp.MySchedule.App.superclass.initComponent.call(this);
				// form.on('save', this.onFormSave, this);
				// var sm = this.grid.getSelectionModel();
				// event handlers have same event : eventname, function or
				// mehtod to run,scope to run it in, addional config
				// this.grid.getSelectionModel().on('rowselect',
				// this.onRowSelect,this,{buffer : 300});
				// this.detailTpl = Ext.Template.from('detailTpl');
			}

		});

Ext.reg('app', CodeCamp.MySchedule.App);

Ext.onReady(function() {
			var vp = new Ext.Viewport({
						layout : 'fit',
						items : [{
									xtype : 'app',
									height : 500
								}]
					});
		});
