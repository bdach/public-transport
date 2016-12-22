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

        this.filterStops = function() {
            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/Stop/Filter",
                data: ctrl.stopFilter
            }).then(function (response) {
                console.log(response.data);
                ctrl.filteredStops = response.data;
                notify.success("Stops filtered successfully", "Response received");
            });
        };
    }]);
})();
