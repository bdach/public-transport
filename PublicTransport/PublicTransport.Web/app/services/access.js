(function () {
    var app = angular.module("myApp");

    app.factory("access", ["$q", "session", "utils", function ($q, session, utils) {
        var access = {
            OK: 200,
            UNAUTHORIZED: 401,
            FORBIDDEN: 403,

            error: function (error) {
                if (utils.getError() === error) {
                    return access.OK;
                }
                else {
                    return $q.reject(access.FORBIDDEN);
                }
            },
            
            isAnonymous: function () {
                if (utils.getError()) {
                    return $q.reject(access.FORBIDDEN);
                }
                else if (!session.isLoggedIn()) {
                    return access.OK;
                }
                else {
                    return $q.reject(access.FORBIDDEN);
                }
            },

            isAuthenticated: function () {
                if (utils.getError()) {
                    return $q.reject(access.FORBIDDEN);
                }
                else if (session.isLoggedIn()) {
                    return access.OK;
                }
                else {
                    return $q.reject(access.UNAUTHORIZED);
                }
            }
        };

        return access;
    }]);
})();
