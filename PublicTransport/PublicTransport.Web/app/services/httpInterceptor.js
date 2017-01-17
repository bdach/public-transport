(function () {
    var app = angular.module("myApp");

    app.factory("httpInterceptor", ["$q", "$injector", "eventAggregator", function ($q, $injector, eventAggregator) {
        return {
            responseError: function (rejection) {
                var notify = $injector.get("notify");
                var state = $injector.get("$state");

                if (rejection.status === 401 && !state.is("login")) {
                    var message = rejection.data.Message;
                    if (rejection.data.Message === "Authorization has been denied for this request.") {
                        message = "Please log in again";
                    }
                    notify.error(message, "Session expired");
                    $injector.get("session").clear();
                    state.go("login");
                }
                else if (rejection.status === 400 && !state.is("register") && !state.is("settings")) {
                    eventAggregator.trigger("event:hideLoadingSpinner");
                    var message = rejection.data.Message;
                    if (rejection.data.Message === "The request is invalid.") {
                        message = "Something went wrong";
                    }
                    notify.error(message, "Request failed");
                }
                else if (rejection.status === 500 || rejection.status === -1 || rejection.status === 403 || rejection.status === 404 || rejection.status === 405) {
                    $injector.get("utils").setError(true);
                    state.go("error");
                }

                return $q.reject(rejection);
            }
        }
    }]);
})();
