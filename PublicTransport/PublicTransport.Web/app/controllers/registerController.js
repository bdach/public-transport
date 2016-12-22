(function () {
    var app = angular.module("myApp");

    app.controller("registerController", ["$http", "$scope", "$state", "notify", "session", "utils", function ($http, $scope, $state, notify, session, utils) {
        var ctrl = this;

        this.user = {
            Name: "",
            Surname: "",
            UserName: "",
            Password: "",
            PasswordRepeat: ""
        };

        this.processRegister = function () {
            if (!$scope.registerForm.$valid) {
                $scope.registerForm.attempted = true;
                notify.error("Please make sure fields are filled properly", "Validation error");
                return;
            }
            else {
                $scope.registerForm.attempted = false;
            }

            if (ctrl.user.Password !== ctrl.user.PasswordRepeat) {
                $scope.registerForm.password.$invalid = true;
                $scope.registerForm.passwordRepeat.$invalid = true;
                notify.error("Provided passwords don't match", "Validation error");
                return;
            }

            var userDto = {
                FullName: ctrl.user.Name + " " + ctrl.user.Surname,
                UserName: ctrl.user.UserName,
                Password: ctrl.user.Password
            };

            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/user/create",
                data: userDto
            }).then(function () {
                notify.success("You can now log in to your account", "Account created");
                $state.go(utils.getToState());
            }, function (response) {
                notify.error(response.data.Message, "Request failed");
                if (response.data.Message === "Provided username is already taken") {
                    $scope.registerForm.username.$invalid = true;
                }
            });

        };
    }]);
})();
