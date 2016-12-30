(function () {
    var app = angular.module("myApp");

    app.filter("containsId", function () {
        return function (array, id) {
            return array.filter(function (elem) { return elem.Id === id }).length !== 0;
        };
    });

    app.controller("favouritesController", ["$http", "$state", "notify", "session", "utils", function ($http, $state, notify, session, utils) {
        var ctrl = this;

        this.displayedRoutes = [];
        this.filteredRoutes = [];
        this.storedRoutes = [];
        this.routeFilter = {
            AgencyNameFilter: "",
            LongNameFilter: "",
            ShortNameFilter: "",
            RouteTypeFilter: null
        };

        var updateRoutes = function (response) {
            ctrl.routeChanges = {};
            ctrl.displayedRoutes = response.data;
            ctrl.storedRoutes = response.data.slice();
        };

        var updateStops = function (response) {
            ctrl.stopChanges = {};
            ctrl.displayedStops = response.data;
            ctrl.storedStops = response.data.slice();
        };

        var init = function () {
            $http({
                method: "GET",
                url: utils.getApiBaseUrl() + "/User/FavouriteRoutes"
            }).then(updateRoutes).then(function () {
                $http({
                    method: "GET",
                    url: utils.getApiBaseUrl() + "/User/FavouriteStops"
                }).then(updateStops);
            });
        };

        init();

        this.removeRoute = function (routeId) {
            ctrl.displayedRoutes = ctrl.displayedRoutes.filter(function (route) { return route.Id !== routeId });
            ctrl.routeChanges[routeId] = false;
        };

        this.addRoute = function (route) {
            ctrl.displayedRoutes.push(route);
            ctrl.routeChanges[route.Id] = true;
        };

        this.filterRoutes = function () {
            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/Route/Filter",
                data: ctrl.routeFilter
            }).then(function (response) {
                ctrl.filteredRoutes = response.data;
                notify.success("Routes filtered successfully", "Response received");
            });
        };

        this.undoRoutes = function () {
            ctrl.displayedRoutes = ctrl.storedRoutes.slice();
        };

        this.saveRoutes = function () {
            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/User/FavouriteRoutes",
                data: {
                    UserName: session.getUserName(),
                    Changes: ctrl.routeChanges
                }
            }).then(function (response) {
                updateRoutes(response);
                notify.success("Changes saved successfully");
            });
        };

        this.showRouteTimetable = function (route) {
            utils.setRoute(route);
            $state.go("index.timetable.route");
        };

        this.displayedStops = [];
        this.filteredStops = [];
        this.storedStops = [];
        this.stopFilter = {
            StopNameFilter: "",
            StreetNameFilter: "",
            CityNameFilter: "",
            ZoneNameFilter: "",
            ParentStationNameFilter: "",
            OnlyStations: false
        };

        this.removeStop = function (stopId) {
            ctrl.displayedStops = ctrl.displayedStops.filter(function (stop) { return stop.Id !== stopId });
            ctrl.stopChanges[stopId] = false;
        };

        this.addStop = function (stop) {
            ctrl.displayedStops.push(stop);
            ctrl.stopChanges[stop.Id] = true;
        }

        this.filterStops = function () {
            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/Stop/Filter",
                data: ctrl.stopFilter
            }).then(function (response) {
                ctrl.filteredStops = response.data;
                notify.success("Stops filtered successfully", "Response received");
            });
        };

        this.undoStops = function () {
            ctrl.displayedStops = ctrl.storedStops.slice();
        };

        this.saveStops = function () {
            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/User/FavouriteStops",
                data: {
                    UserName: session.getUserName(),
                    Changes: ctrl.stopChanges
                }
            }).then(function (response) {
                updateStops(response);
                notify.success("Changes saved successfully");
            });
        };

        this.showStopTimetable = function (stop) {
            utils.setStop(stop);
            $state.go("index.timetable.stop");
        };
    }]);
})();
