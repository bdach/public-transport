(function () {
    var app = angular.module("myApp");

    app.controller("loginController", ["$scope", "$state", function ($scope, $state) {
        var ctrl = this;

        this.user = {
            login: "",
            password: ""
        };

        this.processLogin = function () {
            if (!$scope.loginForm.$valid) {
                $scope.loginForm.attempted = true;
                return;
            }
            else {
                $scope.loginForm.attempted = false;
            }

            $state.go("index.home");
        };
    }]);
})();
