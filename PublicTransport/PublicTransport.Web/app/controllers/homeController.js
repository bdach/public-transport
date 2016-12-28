﻿(function () {
    var app = angular.module("myApp");

    app.controller("homeController", ["$http", "$scope", "notify", "utils", function ($http, $scope, notify, utils) {
        var ctrl = this;

        this.originStop = null;
        this.destinationStop = null;
        this.searchResults = null;

        this.stopFilter = {
            "StopNameFilter": "",
            "StreetNameFilter": "",
            "CityNameFilter": "",
            "ZoneNameFilter": "",
            "ParentStationNameFilter": "",
            "OnlyStations": false
        };

        this.filterStops = function (name) {
            ctrl.stopFilter["StopNameFilter"] = name;
            return $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/Stop/Filter",
                data: ctrl.stopFilter
            }).then(function (response) {
                return response.data;
            });
        };

        this.searchTrips = function () {
            $scope.searchForm.attempted = !$scope.searchForm.$valid;
            if (!$scope.searchForm.$valid) return;
            $http({
                method: "POST",
                url: utils.getApiBaseUrl() + "/Trip/Search",
                data: {
                    "OriginStopIdFilter": ctrl.originStop.Id,
                    "DestinationStopIdFilter": ctrl.destinationStop.Id
                }
            }).then(function (response) {
                ctrl.searchResults = response.data;
                if (ctrl.searchResults.length === 0) {
                    notify.info("No trips matching the search criteria were found.", "No results found");
                } else {
                    notify.success("Found " + ctrl.searchResults.length + " trips matching the search criteria");
                }
            });
        };
    }]);
})();