(function() {
    var app = angular.module("myApp");

    app.controller("routeTimetableController", ["$http", "notify", "utils", function($http, notify, utils) {
        var ctrl = this;

        this.routeFilter = {
            "AgencyNameFilter": "",
            "LongNameFilter": "",
            "ShortNameFilter": "",
            "RouteTypeFilter": null
        }

        this.filteredRoutes = [];
        this.selectedRoute = null;
        this.selectedStop = null;
        this.routeTimetable = [];

        this.filterRoutes = function() {
            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/Route/Filter",
                data: ctrl.routeFilter
            }).then(function (response) {
                ctrl.filteredRoutes = response.data;
                notify.success("Routes filtered successfully", "Response received");
            });
        };

        this.fetchRouteTimetable = function (id) {
            ctrl.selectedStop = null;
            ctrl.routeTimetable = null;
            ctrl.selectedRoute = ctrl.filteredRoutes.find(function (route) { return route.Id === id });
            $http({
                method: "GET",
                url: utils.getApiBaseUrl() + "/timetable/route/" + id
            }).then(function(response) {
                console.log(response.data);
                ctrl.routeTimetable = response.data;
                notify.success("Successfully fetched timetable", "Response received");
            });
        };

        this.selectStop = function(key) {
            ctrl.selectedStop = ctrl.routeTimetable.find(function(pair) { return pair.Key === key });
        }
    }]);
})();