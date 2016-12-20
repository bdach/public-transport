(function () {
    var app = angular.module("myApp");

    app.config(function ($stateProvider, $urlRouterProvider, usSpinnerConfigProvider) {
        usSpinnerConfigProvider.setDefaults({ color: "grey", lines: 11, length: 16, width: 10, radius: 20 });
        $urlRouterProvider.otherwise(function ($injector) {
            $injector.get("$state").go("index.home");
        });
        $stateProvider
            .state("login", {
                url: "/login",
                templateUrl: "pages/login-page.html",
                controller: "loginController",
                controllerAs: "loginCtrl"
            })
            .state("error", {
                url: "/error",
                template: "<h1 style='margin-left: 20px;'>Oops... Something went wrong :(</h1>"
            })
            .state("index", {
                url: "/index",
                abstract: true,
                templateUrl: "pages/index-page.html"
            })
            .state("index.home", {
                url: "/home",
                templateUrl: "pages/contents/home-content.html",
                controller: "homeController",
                controllerAs: "homeCtrl"
            })
            .state("index.timetable", {
                url: "/timetable",
                abstract: true,
                template: "<div ui-view></div>"
            })
            .state("index.timetable.route", {
                url: "/route",
                templateUrl: "pages/contents/route-timetable-content.html",
                controller: "timetableController",
                controllerAs: "timetableCtrl"
            })
            .state("index.timetable.stop", {
                url: "/stop",
                templateUrl: "pages/contents/stop-timetable-content.html",
                controller: "timetableController",
                controllerAs: "timetableCtrl"
            })
            .state("index.favourites", {
                url: "/favourites",
                templateUrl: "pages/contents/favourites-content.html",
                controller: "favouritesController",
                controllerAs: "favouritesCtrl"
            })
            .state("index.settings", {
                url: "/settings",
                templateUrl: "pages/contents/settings-content.html",
                controller: "settingsController",
                controllerAs: "settingsCtrl"
            });
    });
})();
