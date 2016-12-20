(function () {
    var app = angular.module("myApp");

    app.factory("utils", function () {
        var apiBaseUrl = "http://localhost:49878/api";
        var error = false;
        var toState = "index.home";

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

        return {
            getApiBaseUrl: getApiBaseUrl,
            getError: getError,
            setError: setError,
            getToState: getToState,
            setToState: setToState
        };
    });
})();
