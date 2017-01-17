(function () {
    var app = angular.module("myApp");

    app.controller("loginController", ["$http", "$scope", "$state", "eventAggregator", "notify", "session", "utils", function ($http, $scope, $state, eventAggregator, notify, session, utils) {
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

            eventAggregator.trigger("event:showLoadingSpinner");
            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/Token",
                data: ctrl.user
            }).then(function (response) {
                session.setup(response.data.Token);
                session.setUserData(response.data);
                notify.success("You have logged in to your account", "Login successful");
                $state.go(utils.getToState());
            }, function () {
                eventAggregator.trigger("event:hideLoadingSpinner");
                notify.error("Invalid username or password", "Login failed");
            });
        };
    }]);
})();
