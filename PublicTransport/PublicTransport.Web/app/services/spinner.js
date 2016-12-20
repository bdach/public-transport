(function () {
    var app = angular.module("myApp");

    app.factory("spinner", ["usSpinnerService", function (usSpinnerService) {
        var visible = false;

        var show = function () {
            usSpinnerService.spin("loading");
        };

        var hide = function () {
            usSpinnerService.stop("loading");
        };

        return {
            visible: visible,
            show: show,
            hide: hide
        };
    }]);
})();
