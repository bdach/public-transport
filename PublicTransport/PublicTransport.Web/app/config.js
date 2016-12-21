(function () {
    var app = angular.module("myApp");

    app.config(function ($httpProvider, $stateProvider, $urlRouterProvider, toastrConfig, usSpinnerConfigProvider) {
        angular.extend(toastrConfig, {
            autoDismiss: true,
            closeButton: true,
            maxOpened: 3,
            newestOnTop: false,
            preventOpenDuplicates: true
        });
        usSpinnerConfigProvider.setDefaults({ color: "grey", lines: 11, length: 16, width: 10, radius: 20 });
        $httpProvider.interceptors.push("httpInterceptor");
        $urlRouterProvider.otherwise(function ($injector) {
            $injector.get("$state").go("index.home");
        });
        $stateProvider
            .state("login", {
                url: "/login",
                templateUrl: "pages/login-page.html",
                controller: "loginController",
                controllerAs: "loginCtrl",
                resolve: {
                    access: ["access", function (access) { return access.isAnonymous(); }]
                }
            })
            .state("register", {
                url: "/register",
                templateUrl: "pages/register-page.html",
                controller: "registerController",
                controllerAs: "registerCtrl",
                resolve: {
                    access: ["access", function (access) { return access.isAnonymous(); }]
                }
            })
            .state("error", {
                url: "/error",
                template: "<h1 style='margin-left: 20px;'>Oops... Something went wrong :(</h1>",
                resolve: {
                    access: ["access", function (access) { return access.error(true); }]
                }
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
                controllerAs: "homeCtrl",
                resolve: {
                    access: ["access", function (access) { return access.error(false); }]
                }
            })
            .state("index.timetable", {
                url: "/timetable",
                abstract: true,
                template: "<div ui-view></div>"
            })
            .state("index.timetable.route", {
                url: "/route",
                templateUrl: "pages/contents/timetable-route-content.html",
                controller: "timetableController",
                controllerAs: "timetableCtrl",
                resolve: {
                    access: ["access", function (access) { return access.error(false); }]
                }
            })
            .state("index.timetable.stop", {
                url: "/stop",
                templateUrl: "pages/contents/timetable-stop-content.html",
                controller: "timetableController",
                controllerAs: "timetableCtrl",
                resolve: {
                    access: ["access", function (access) { return access.error(false); }]
                }
            })
            .state("index.favourites", {
                url: "/favourites",
                templateUrl: "pages/contents/favourites-content.html",
                controller: "favouritesController",
                controllerAs: "favouritesCtrl",
                resolve: {
                    access: ["access", function (access) { return access.isAuthenticated(); }]
                }
            })
            .state("index.settings", {
                url: "/settings",
                templateUrl: "pages/contents/settings-content.html",
                controller: "settingsController",
                controllerAs: "settingsCtrl",
                resolve: {
                    access: ["access", function (access) { return access.isAuthenticated(); }]
                }
            });
    });
})();
