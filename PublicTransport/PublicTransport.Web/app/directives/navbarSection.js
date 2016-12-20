(function () {
    var app = angular.module("myApp");

    app.directive("navbarSection", function () {
        return {
            restrict: "E",
            templateUrl: "pages/sections/navbar-section.html"
        };
    });
})();
