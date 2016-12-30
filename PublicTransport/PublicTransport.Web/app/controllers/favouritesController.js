(function () {
    var app = angular.module("myApp");

    app.filter("containsId", function() {
        return function (array, id) {
            return array.filter(function(elem) { return elem.Id === id }).length !== 0;
        };
    });

    app.controller("favouritesController", ["$http", "notify", "session", "utils", function ($http, notify, session, utils) {
        var ctrl = this;

        // this can be split into 2 controllers once thread safety is ensured

        // ROUTES ---------------------

        this.storedRoutes = [];
        this.displayedRoutes = [];
        this.routeFilter = {
            AgencyNameFilter: "",
            LongNameFilter: "",
            ShortNameFilter: "",
            RouteTypeFilter: null
        }
        this.filteredRoutes = [];

        var updateRoutes = function (response) {
            ctrl.routeChanges = {};
            ctrl.displayedRoutes = response.data;
            ctrl.storedRoutes = response.data.slice(); // keep a copy
        };

        var updateStops = function (response) {
            ctrl.stopChanges = {};
            console.log(response.data);
            ctrl.displayedStops = response.data;
            ctrl.storedStops = response.data.slice(); // keep a copy
        };


        // THIS IS VERY UGLY
        // FIX THIS
        $http({
            method: "GET",
            url: utils.getApiBaseUrl() + "/user/favouriteroutes/",
            headers: {
                "Authorization": "Bearer " + session.getToken()
            }
        }).then(updateRoutes)
        .then(function() {
            $http({
                method: "GET",
                url: utils.getApiBaseUrl() + "/user/favouritestops/",
                headers: {
                    "Authorization": "Bearer " + session.getToken()
                }
            }).then(updateStops);
        })

        this.removeRoute = function (routeId) {
            ctrl.displayedRoutes = ctrl.displayedRoutes.filter(function (route) { return route.Id !== routeId });
            ctrl.routeChanges[routeId] = false;
        };

        this.addRoute = function (route) {
            ctrl.displayedRoutes.push(route);
            ctrl.stopChanges[route.Id] = true;
        }

        this.filterRoutes = function () {
            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/Route/Filter",
                data: ctrl.routeFilter
            }).then(function (response) {
                ctrl.filteredRoutes = response.data;
                notify.success("Successfully filtered routes", "Response received");
            });
        };

        this.undoRoutes = function () {
            ctrl.displayedRoutes = ctrl.storedRoutes.slice();
        };

        this.saveRoutes = function () {
            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/user/favouriteroutes",
                data: {
                    "UserName": session.getUserName(),
                    "Changes": ctrl.routeChanges
                },
                headers: {
                    "Authorization": "Bearer " + session.getToken()
                }
            }).then(function (response) {
                updateRoutes(response);
                notify.success("Changes saved successfully");
            });
        };

        // STOPS ----------------------

        this.storedStops = [];
        this.displayedStops = [];
        this.stopFilter = {
            StopNameFilter: "",
            StreetNameFilter: "",
            CityNameFilter: "",
            ZoneNameFilter: "",
            ParentStationNameFilter: "",
            OnlyStations: false
        };
        this.filteredStops = [];

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
                notify.success("Successfully filtered stops", "Response received");
            });
        };

        this.undoStops = function () {
            ctrl.displayedStops = ctrl.storedStops.slice();
        };

        this.saveStops = function () {
            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/user/favouritestops",
                data: {
                    "UserName": session.getUserName(),
                    "Changes": ctrl.stopChanges
                },
                headers: {
                    "Authorization": "Bearer " + session.getToken()
                }
            }).then(function (response) {
                updateStops(response);
                notify.success("Changes saved successfully");
            });
        };
    }]);
})();
