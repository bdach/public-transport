(function () {
    var app = angular.module("myApp");

    app.controller("registerController", ["$scope", "$state", "notify", "session", "utils", function ($scope, $state, notify, session, utils) {
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

            notify.success("You can now log in to your account", "Account created");
            $state.go(utils.getToState());
        };
    }]);
})();
