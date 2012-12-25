/**
 * @class Ext.ux.FieldSerializer
 * A Serializer class to provide custom field serialization of any Container
 * which container field widgets.
 * The serializer constructor can be passed a configuration object of custom converters. 
 * The 'item' object is passed as an arguments to each converter.
 * The Converter must return either a single 'value', or a 'value object' which 
 * contains 'name' & 'value' properties. 
 * If no converter is specified for the field, the serialized objects name will be
 * set the fields .name property and the value will be set using .getValue().
 * The context of 'this' within a converter is the instance of the 'item'.
<pre><code>
var serializer = new Ext.ux.plugins.FieldSerializer({
    converters : {
        simpleConverter : function (item) {
            return this.dateFormat("Y-m-d");
        },
        complexConverter : function (item) {
            return {
                name  : "bday",
                value : item.getValue().dateFormat("Y-m-d")
            };
        },
        anotherConverter : function () {
            return "id-" + arguments[0].someProperty;
        }
    }
});

var obj = serializer.serialize(Ext.getCmp("myForm"));

var jsonString = obj.toJsonString();
</code></pre>
 */
Ext.ns("Ext.ux");

Ext.ux.FieldSerializer = function (config) {
    return Ext.apply({
        converters : {},
        
        // By default the serializer will add an extra .toJsonString() function
        // onto the serialized object. Calling the .toJsonString() function will 
        // return an encoded json string representation of the object. 
        // The addition of toJsonString can be avoided by passing addJsonStringFn : false
        // in the FieldSerializer configuration object. 
        addJsonStringFn : true,
        
        // private
        doObject : function (container, obj) {
            if (container.items) {
                var result;
                container.items.each(function (item) {
                    // just return quickly if item should be ignored.
                    if (item.ignore === true) {
                        return;
                    }
                    
                    // if the item has a "name", "idemId" or "converter" config 
                    // option specified, jump in and start the conversion.
                    if (!Ext.isEmpty(item.name || item.itemId) || item.converter) {
                        result = this.doConvert(item);
                        
                        if (Ext.isObject(result)) {
                            obj[result.name] = result.value;
                        }
                        
                        return;
                    } 
                    
                    if (item.items) {
                        return this.doObject(item, obj);
                    }
                }, this);
            }
            return obj;
        },
        
        // private
        doArray : function (container, obj) {
            var name;
            container.items.each(function (item, index) {
                name = item.name || item.itemId;
                obj[index] = (!Ext.isEmpty(name) && item.items && this.isArray(name)) ? 
                    this.doArray(item, []) : this.doObject(item, {});
            }, this);
            
            return obj;
        },
        
        isArray : function (s) {
            return s.substr(s.length - 2, s.length) === "[]";
        },
        
        // private
        doConvert : function (item) {
            // simple item with no converter.
            if (!item.converter) {
                return this.doSimple(item);
            }
            
            // get the items 'name' or 'itemId' config property.
            var name = item.name || item.itemId, value;
            
            // if the converter is a function, then call.
            if (Ext.isFunction(item.converter)) {
                value = item.converter.call(item, [item]);
            }
            
            // if the converter is a string, then find in converters config.
            if (typeof item.converter == "string") {
                value = this.converters[item.converter] ? this.converters[item.converter].call(item, [item]) : null;
            }
            
            // if we have a valid name/value object, return.
            if (!Ext.isEmpty(value.name) && !Ext.isEmpty(value.value)) {
                return value;
            }
            
            // converters did not work out, so check again if this is a simple .getValue() field?
            if (Ext.isEmpty(value) && item.getValue) {
                return this.doSimple(item);
            }
            
            // return proper name/value object, or null.
            return !Ext.isEmpty(name) ? {
                name  : name,
                value : value
            } : null; 
        },
        
        doSimple : function (item) {
            var name = item.name || item.itemId, value;
            
            if (Ext.isEmpty(name)) {
                return null;
            }
            
            // if simple .getValue() field
            if (item.getValue) {
                return {
                    name  : name,
                    value : item.getValue()
                };
            }
            
            // if item has child items
            if (item.items) {
                // is this an array?
                if (this.isArray(name)) {
                    name = name.replace("[]", "");
                    value = this.doArray(item, []);
                } else {
                    value = this.doObject(item, {});
                }
                
                return {
                    name  : name,
                    value : value
                };
            }
            
            // fail
            return null;
        },
        
        serialize : function (container) {
            return Ext.apply(this.doObject(container, {}), this.addJsonStringFn === true ? { toJsonString : function () {
                delete this.toJsonString;
                return Ext.encode(this);
            }} : {});
        }
    }, config);
};