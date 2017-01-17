(function () {
    var app = angular.module("myApp");

    app.controller("appController", ["$state", "session", "spinner", function ($state, session, spinner) {
        var ctrl = this;

        this.isBusy = function () {
            return spinner.visible;
        };

        this.session = function () {
            return {
                FullName: session.getFullName(),
                UserName: session.getUserName(),
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
