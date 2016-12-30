(function () {
    var app = angular.module("myApp");

    app.controller("homeController", ["$http", "$scope", "notify", "utils", "$uibModal", function ($http, $scope, notify, utils, $uibModal) {
        var ctrl = this;

        this.originStop = null;
        this.destinationStop = null;
        this.searchResults = null;

        this.stopFilter = {
            StopNameFilter: "",
            StreetNameFilter: "",
            CityNameFilter: "",
            ZoneNameFilter: "",
            ParentStationNameFilter: "",
            OnlyStations: false
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
                    OriginStopIdFilter: ctrl.originStop.Id,
                    DestinationStopIdFilter: ctrl.destinationStop.Id
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

        this.showMap = function (trip) {
            $uibModal.open({
                animation: true,
                templateUrl: "pages/contents/map-modal-content.html",
                controller: "mapController",
                controllerAs: "mapCtrl",
                resolve: {
                    tripInfo: {
                        TripId: trip.Id,
                        OriginSequenceNumber: trip.OriginStop.SequenceNumber,
                        DestinationSequenceNumber: trip.DestinationStop.SequenceNumber
                    }
                },
                size: "lg"
            });
        };
    }]);
})();