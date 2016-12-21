(function () {
    var app = angular.module("myApp");

    app.controller("appController", ["$state", "session", "spinner", function ($state, session, spinner) {
        var ctrl = this;

        this.isBusy = function () {
            return spinner.visible;
        };

        this.session = function () {
            return {
                UserName: session.getUserName(),
                Login: session.getLogin(),
                isLoggedIn: session.isLoggedIn()
            };
        };

        this.logout = function () {
            session.clear();
            if ($state.is("index.favourites") || $state.is("index.settings")) {
                $state.go("index.home");
            }
        };
    }]);
})();
