(function () {
    var app = angular.module("myApp");

    app.factory("session", ["$q", "$http", "utils", function ($q, $http, utils) {
        var deferred = $q.defer();

        var user = {
            FullName: "",
            UserName: "",
            Roles: [],
            LoggedIn: false
        };

        var clear = function () {
            user.FullName = "";
            user.UserName = "";
            user.Roles = [];
            user.LoggedIn = false;

            localStorage.removeItem("token");
            delete $http.defaults.headers.common["Authorization"];
        };

        var promise = function () {
            return deferred.promise;
        };

        var setup = function (token) {
            var header = "Bearer " + token;
            delete $http.defaults.headers.common["Authorization"];
            $http.defaults.headers.common["Authorization"] = header;
            localStorage["token"] = token;
        };

        var setUserData = function (data) {
            user.FullName = data.FullName;
            user.UserName = data.UserName;
            user.Token = data.Token;
            user.Roles = data.Roles;
            user.LoggedIn = true;
        };

        var restore = function () {
            if (localStorage["token"]) {
                var header = "Bearer " + localStorage["token"];
                delete $http.defaults.headers.common["Authorization"];
                $http.defaults.headers.common["Authorization"] = header;

                $http({
                    method: "GET",
                    url: utils.getApiBaseUrl() + "/Session"
                }).then(function (response) {
                    setup(response.data.Token);
                    setUserData(response.data);
                    deferred.resolve();
                }, function () {
                    delete $http.defaults.headers.common["Authorization"];
                    deferred.resolve();
                });
            }
            else {
                deferred.resolve();
            }
        };

        var getFullName = function () {
            return user.FullName;
        };

        var getUserName = function () {
            return user.UserName;
        };

        var isLoggedIn = function () {
            return user.LoggedIn;
        };

        return {
            clear: clear,
            promise: promise,
            restore: restore,
            setup: setup,
            setUserData: setUserData,
            getFullName: getFullName,
            getUserName: getUserName,
            isLoggedIn: isLoggedIn
        };
    }]);
})();
