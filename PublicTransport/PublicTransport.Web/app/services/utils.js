(function () {
    var app = angular.module("myApp");

    app.factory("utils", function () {
        var apiBaseUrl = configuration.apiBaseUrl;
        var error = false;
        var toState = "index.home";
        var route = null;
        var stop = null;

        var getApiBaseUrl = function () {
            return apiBaseUrl;
        };

        var getError = function () {
            return error;
        };

        var setError = function (value) {
            error = value;
        };

        var getToState = function () {
            return toState;
        };

        var setToState = function (value) {
            toState = value;
        };

        var setRoute = function (value) {
            route = value;
        };

        var getRoute = function () {
            return route;
        };

        var setStop = function (value) {
            stop = value;
        };

        var getStop = function () {
            return stop;
        };

        return {
            getApiBaseUrl: getApiBaseUrl,
            getError: getError,
            setError: setError,
            getToState: getToState,
            setToState: setToState,
            setRoute: setRoute,
            getRoute: getRoute,
            setStop: setStop,
            getStop: getStop
        };
    });
})();
