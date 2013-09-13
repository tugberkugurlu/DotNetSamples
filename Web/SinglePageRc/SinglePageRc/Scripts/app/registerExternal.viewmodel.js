function RegisterExternalViewModel(app, dataModel) {
    // Private state
    var self = this;

    // Data
    self.loginProvider = ko.observable();
    self.userName = ko.observable(null);

    // Other UI state
    self.errors = ko.observableArray();
    self.registering = ko.observable(false);
    self.externalAccessToken = null;
    self.state = null;
    self.loginUrl = null;

    // data-bind click
    self.register = function () {
        self.errors.removeAll();
        self.registering(true);
        dataModel.registerExternal(self.externalAccessToken, {
            userName: self.userName()
        }).done(function (data) {
            sessionStorage["state"] = self.state;
            // IE doesn't reliably persist sessionStorage when navigating to another URL. Move sessionStorage
            // temporarily to localStorage to work around this problem.
            app.archiveSessionStorageToLocalStorage();
            window.location = self.loginUrl;
        }).failJSON(function (data) {
            var errors;

            self.registering(false);
            errors = dataModel.toErrorsArray(data);

            if (errors) {
                self.errors(errors);
            } else {
                self.errors.push("An unknown error occurred.");
            }
        });
    };
}

app.addViewModel({
    name: "RegisterExternal",
    bindingMemberName: "registerExternal",
    factory: RegisterExternalViewModel,
    navigatorFactory: function (app) {
        return function (userName, loginProvider, externalAccessToken, loginUrl, state) {
            app.errors.removeAll();
            app.view(app.Views.RegisterExternal);
            app.registerExternal().userName(userName);
            app.registerExternal().loginProvider(loginProvider);
            app.registerExternal().externalAccessToken = externalAccessToken;
            app.registerExternal().loginUrl = loginUrl;
            app.registerExternal().state = state;
        };
    }
});
