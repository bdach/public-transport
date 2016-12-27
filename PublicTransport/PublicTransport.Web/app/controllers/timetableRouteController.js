(function () {
    var app = angular.module("myApp");

    app.controller("timetableRouteController", ["$http", "notify", "utils", function($http, notify, utils) {
        var ctrl = this;

        this.routeFilter = {
            AgencyNameFilter: "",
            LongNameFilter: "",
            ShortNameFilter: "",
            RouteTypeFilter: null
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
                notify.success("Successfully filtered routes", "Response received");
            });
        };

        this.fetchRouteTimetable = function (id) {
            ctrl.selectedStop = null;
            ctrl.routeTimetable = [];
            ctrl.selectedRoute = ctrl.filteredRoutes.find(function (route) { return route.Id === id });
            $http({
                method: "GET",
                url: utils.getApiBaseUrl() + "/Timetable/Route/" + id
            }).then(function(response) {
                ctrl.routeTimetable = response.data;
            });
        };

        this.selectStop = function(key) {
            ctrl.selectedStop = ctrl.routeTimetable.find(function(pair) { return pair.Key === key });
        }
    }]);
})();