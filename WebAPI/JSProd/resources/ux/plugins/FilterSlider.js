/**
 * @class Ext.ux.FilterSlider
 * @extends Ext.ux.RangeSlider
 * Just a customized Slider or RangeSlider with text defaulting to above
 * the slider as well as some custom CSS
<pre><code>
new Ext.ux.FilterSlider({
	renderTo  : Ext.getBody()           ,
	msg       : 'From {0} to {1} hours' ,
	width     : 200                     ,
	lowValue  : 0                       ,
	minValue  : 0                       ,
	highValue : 100                     ,
	maxValue  : 100                     ,
	spacing   : 6
});
</code></pre>
 */
Ext.ux.FilterSlider = Ext.extend(Ext.BoxComponent, {
	/**
	 * @cfg {String} spacing The message template to show with the slider.  Use {0} and {1} for replacement values.
	 */
	msg : '{0}' ,

	/**
	 * @cfg {Boolean} pluralize Pluralize the message when appropriate.  Defaults to false.
	 */
	pluralize : false ,

	/**
	 * @cfg {String} sliderType The type of slider to use.  Possible options are
	 *  'Slider' and 'RangeSlider'.  Defaults to 'Slider'
	 */
	type        : 'Slider' , 
	isFormField : true     , 

	onRender : function(){
		this.autoEl = {
			cls : 'x-filter-slider ',
			cn  : [{cls:'x-filter-slider-msg'}, {cls:'x-filter-slider-slider'}]
		};

		Ext.ux.FilterSlider.superclass.onRender.apply(this, arguments);

		this.msgBox    = this.el.first();
		this.sliderBox = this.msgBox.next();

		var config = Ext.apply(Ext.apply({}, this.initialConfig), {
			renderTo : this.sliderBox
		});

		switch(this.type){
			case 'Slider':
				this.slider = new Ext.Slider(config);
				this.slider.on('change', this.changeLowValue, this);

				this.lowValue = this.slider.value;

				break;

			case 'RangeSlider':
				this.slider = new Ext.ux.RangeSlider(config);
				this.slider.on('lowchange' , this.changeLowValue , this);
				this.slider.on('highchange', this.changeHighValue, this);

				this.lowValue  = this.slider.lowSlider .value;
				this.highValue = this.slider.highSlider.value;

				break;
		}

		this.msgTemplate = new Ext.Template(this.msg);
		this.msgTemplate.compile();

		this.changeMsg();
	},

	changeLowValue : function(s, v){
		this.lowValue = v;
		this.changeMsg();
	},

	changeHighValue : function(s, v){
		this.highValue = v;
		this.changeMsg();
	},

	changeMsg: function(){

		this.msgBox.update(this.msgTemplate.apply({0:this.lowValue, 1:this.highValue})[this.pluralize?'pluralize':'trim'](this.highValue||this.lowValue));
	},

	getValue : function(){
		return this.slider.getValue();
	},

	/**
	 * Overridden and disabled. The editor element does not support standard valid/invalid marking. @hide
	 * @method
	 */
	markInvalid : Ext.emptyFn,
	/**
	 * Overridden and disabled. The editor element does not support standard valid/invalid marking. @hide
	 * @method
	 */
	clearInvalid : Ext.emptyFn,

	// private
	initValue : Ext.emptyFn
});
Ext.reg('ux.FilterSlider', Ext.ux.FilterSlider);