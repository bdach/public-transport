(function () {
    var app = angular.module("myApp");

    app.factory("session", function () {
        var user = {
            FullName: "",
            UserName: "",
            Token: "",
            Roles: [],
            LoggedIn: false
        };

        var clear = function () {
            user.FullName = "";
            user.UserName = "";
            user.Token = "";
            user.Roles = [];
            user.LoggedIn = false;
        };

        var setUserData = function (data) {
            user.FullName = data.FullName;
            user.UserName = data.UserName;
            user.Token = data.Token;
            user.Roles = data.Roles;
            user.LoggedIn = true;
        };

        var getFullName = function () {
            return user.FullName;
        };

        var getUserName = function () {
            return user.UserName;
        };

        var getToken = function () {
            return user.Token;
        };

        var isLoggedIn = function () {
            return user.LoggedIn;
        };

        return {
            clear: clear,
            setUserData: setUserData,
            getFullName: getFullName,
            getUserName: getUserName,
            getToken: getToken,
            isLoggedIn: isLoggedIn
        };
    });
})();
