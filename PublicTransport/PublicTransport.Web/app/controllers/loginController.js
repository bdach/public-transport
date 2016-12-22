(function () {
    var app = angular.module("myApp");

    app.controller("loginController", ["$scope", "$state", "notify", "session", "utils", function ($scope, $state, notify, session, utils) {
        var ctrl = this;

        this.user = {
            UserName: "",
            Password: ""
        };

        this.processLogin = function () {
            if (!$scope.loginForm.$valid) {
                $scope.loginForm.attempted = true;
                notify.error("Please make sure fields are filled properly", "Validation error");
                return;
            }
            else {
                $scope.loginForm.attempted = false;
            }

            session.setUserData({ FullName: "Sample Name", UserName: ctrl.user.UserName, Roles: [] });
            $state.go(utils.getToState());
        };
    }]);
})();
