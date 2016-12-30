(function () {
    var app = angular.module("myApp");

    app.run(function ($rootScope, $state, access, eventAggregator, notify, session, spinner, utils) {
        eventAggregator.on("event:showLoadingSpinner", function () {
            spinner.visible = true;
            spinner.show();
        });
        eventAggregator.on("event:hideLoadingSpinner", function () {
            spinner.visible = false;
            spinner.hide();
        });

        $rootScope.$on("$stateChangeError", function (event, toState, toParams, fromState, fromParams, error) {
            spinner.visible = false;
            spinner.hide();

            if (error === access.UNAUTHORIZED) {
                notify.error("You need to be logged in to view this page", "Access denied");
                $state.go("login");
            }
            else if (error === access.FORBIDDEN) {
                if (fromState.name !== "login" && fromState.name !== "register" && fromState.name !== "error" && fromState.name !== "") {
                    utils.setToState(fromState.name);
                }
                else {
                    $state.go("index.home");
                }
            }
        });

        session.restore();

        $rootScope.$on("$stateChangeStart", function (event, toState, toParams, fromState, fromParams, options) {
            spinner.visible = true;
            spinner.show();

            if (toState.name !== "login" && toState.name !== "register" && toState.name !== "error") {
                utils.setToState(toState.name);
            }
        });

        $rootScope.$on("$stateChangeSuccess", function (event, toState, toParams, fromState, fromParams, options) {
            spinner.visible = false;
            spinner.hide();
        });
    });
})();