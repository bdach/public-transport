(function () {
    var app = angular.module("myApp");

    app.factory("notify", ["toastr", function (toastr) {
        var success = function (text, title) {
            toastr.success(text, title);
        };

        var error = function (text, title) {
            toastr.error(text, title);
        };

        var warning = function (text, title) {
            toastr.warning(text, title);
        };

        var info = function (text, title) {
            toastr.info(text, title);
        };

        return {
            success: success,
            error: error,
            warning: warning,
            info: info
        };
    }]);
})();
