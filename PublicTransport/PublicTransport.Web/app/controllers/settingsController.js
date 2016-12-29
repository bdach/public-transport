(function () {
    var app = angular.module("myApp");

    app.controller("settingsController", ["$http", "$scope", "eventAggregator", "notify", "session", "utils", function ($http, $scope, eventAggregator, notify, session, utils) {
        var ctrl = this;

        this.user = {
            OldPassword: "",
            NewPassword: "",
            NewPasswordRepeat: ""
        };

        this.processPasswordChange = function () {
            if (!$scope.settingsForm.$valid) {
                $scope.settingsForm.attempted = true;
                notify.error("Please make sure fields are filled properly", "Validation error");
                return;
            }
            else {
                $scope.settingsForm.attempted = false;
            }

            if (ctrl.user.NewPassword !== ctrl.user.NewPasswordRepeat) {
                $scope.settingsForm.newPassword.$invalid = true;
                $scope.settingsForm.newPasswordRepeat.$invalid = true;
                notify.error("Provided passwords don't match", "Validation error");
                return;
            }

            var userDto = {
                UserName: session.getUserName(),
                OldPassword: ctrl.user.OldPassword,
                NewPassword: ctrl.user.NewPassword
            };

            eventAggregator.trigger("event:showLoadingSpinner");
            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/User/ChangePassword",
                data: userDto,
                headers: {
                    "Authorization" : "Bearer " + session.getToken()
                }
            }).then(function () {
                eventAggregator.trigger("event:hideLoadingSpinner");
                notify.success("Password changed successfully", "Changes saved");
            }, function (response) {
                eventAggregator.trigger("event:hideLoadingSpinner");
                notify.error(response.data.Message, "Request failed");
                if (response.data.Message === "Provided old password is invalid") {
                    $scope.settingsForm.oldPassword.$invalid = true;
                }
            });

        };
    }]);
})();
