Ext.define('RegistrationApp.view.override.PasswordConfirm', {
	override: 'RegistrationApp.view.PasswordConfirm',
	vtype: 'password'
}, function() {
	Ext.apply(Ext.form.field.VTypes, {
		password : function(val, field) {
			if (field.initialPassField) {
				var container = field.up('form');
				if (!container) {
					container = field.up("container");
				}
				var pwd = container.down('#' + field.initialPassField);
				if (!pwd) {
					pwd = container.down('[name=' + initialPassField + ']');
				}
				return (val == pwd.getValue());
			}
			return true
		},
		passwordText : 'Password Confirmation',
	});
});
