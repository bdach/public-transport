(function () {
    var app = angular.module("myApp");

    app.controller("timetableController", ["$http", "notify", "utils", function ($http, notify, utils) {
        var ctrl = this;

        this.stopFilter = {
            StopNameFilter: "",
            StreetNameFilter: "",
            CityNameFilter: "",
            ZoneNameFilter: "",
            ParentStationNameFilter: "",
            OnlyStations: false
        };

        this.filteredStops = [];
        this.selectedStop = null;
        this.selectedRoute = null;
        this.stopTimetable = [];

        this.filterStops = function() {
            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/Stop/Filter",
                data: ctrl.stopFilter
            }).then(function (response) {
                ctrl.filteredStops = response.data;
                notify.success("Stops filtered successfully", "Response received");
            });
        };

        this.fetchStopTimetable = function (id) {
            ctrl.selectedRoute = null;
            ctrl.stopTimetable = [];
            ctrl.selectedStop = ctrl.filteredStops.find(function(stop) { return stop.Id === id });
            $http({
                method: "GET",
                url: utils.getApiBaseUrl() + "/timetable/stop/" + id,
            }).then(function(response) {
                console.log(response.data);
                ctrl.stopTimetable = response.data;
                notify.success("Successfully fetched timetable", "Response received");
            });
        };

        this.selectRoute = function(key) {
            ctrl.selectedRoute = ctrl.stopTimetable.find(function(pair) { return pair.Key === key });
        }
    }]);
})();
