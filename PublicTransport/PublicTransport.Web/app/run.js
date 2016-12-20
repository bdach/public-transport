(function () {
    var app = angular.module("myApp");

    app.run(function ($rootScope, $state, eventAggregator, spinner) {
        eventAggregator.on("event:showLoadingSpinner", function () {
            spinner.visible = true;
            spinner.show();
        });
        eventAggregator.on("event:hideLoadingSpinner", function () {
            spinner.visible = false;
            spinner.hide();
        });

        $rootScope.$on("$stateChangeError", function(event, toState, toParams, fromState, fromParams, error) {
            spinner.visible = false;
            spinner.hide();
        });

        $rootScope.$on("$stateChangeStart", function (event, toState, toParams, fromState, fromParams, options) {
            spinner.visible = true;
            spinner.show();
        });

        $rootScope.$on("$stateChangeSuccess", function (event, toState, toParams, fromState, fromParams, options) {
            spinner.visible = false;
            spinner.hide();
        });
    });
})();