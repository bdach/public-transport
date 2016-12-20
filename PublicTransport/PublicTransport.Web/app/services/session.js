(function () {
    var app = angular.module("myApp");

    app.factory("session", function () {
        var user = {
            UserName: "",
            Login: "",
            LoggedIn: false,
            Roles: {}
        };

        var clear = function () {
            user.UserName = "";
            user.Login = "";
            user.LoggedIn = false;
            user.Roles = {};
        };

        var setUserData = function (data) {
            user.UserName = data.UserName;
            user.Login = data.Login;
            user.LoggedIn = true;
            user.Roles = data.Roles;
        };

        var getLogin = function () {
            return user.Login;
        };

        var getUserName = function () {
            return user.UserName;
        };

        var isLoggedIn = function () {
            return user.LoggedIn;
        };

        return {
            clear: clear,
            setUserData: setUserData,
            getLogin: getLogin,
            getUserName: getUserName,
            isLoggedIn: isLoggedIn
        };
    });
})();
