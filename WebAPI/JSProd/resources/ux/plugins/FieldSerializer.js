/**
 * @class Ext.ux.plugins.FieldSerializer
 * A plugin to add a custom FieldSerializer object to Container controls.
 * The plugin constructor can be passed a configuration object of custom converters. 
 * The 'value', 'field' and 'root' objects are passed as arguments to each converter.
 * The Converter must return either a single 'value', or a 'value object' which 
 * contains 'name' & 'value' pair. 
 * If no converter is specified for the field, the serialized objects name will be
 * set the fields .name property and the value will be set using .getValue().
<pre><code>
plugins : [ new Ext.ux.plugins.FieldSerializer({
    converters : {
        simpleConverter : function (value, field, root) {
            return value.dateFormat("Y-m-d");
        },
        complexConverter : function (value, field, root) {
            return {
                name  : "bday",
                value : value.dateFormat("Y-m-d")
            };
        }
    }
})],
</code></pre>
 */
Ext.ns("Ext.ux", "Ext.ux.plugins");

Ext.ux.plugins.FieldSerializer = Ext.extend(Ext.Container, {
    cfg : {},
    
    constructor : function (config) {
        this.cfg = Ext.apply(this.cfg, config || {});
    },
    
    init : function (container) {
        var self = container;
        
        self.serializer = new Ext.ux.FieldSerializer(this.cfg);
        
        // a conveinience .serialize function is added to the container.
        // var obj = Ext.getCmp("myForm").serialize();
        self.serialize = function () {
            return self.serializer.serialize(this);
        };
    }
});