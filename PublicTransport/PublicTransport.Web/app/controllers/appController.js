(function () {
    var app = angular.module("myApp");

    app.controller("appController", ["$state", "spinner", function ($state, spinner) {
        var ctrl = this;

        this.isBusy = function () {
            return spinner.visible;
        };

        this.session = function () {
            return {
                UserName: "Jan Kowalski",
                Login: "jkow"
            };
        };

        this.logout = function () {
            $state.go("login");
        };
    }]);
})();
