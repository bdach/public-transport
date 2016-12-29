(function() {
    var app = angular.module("myApp");

    app.controller("mapController", ["$http", "$uibModalInstance", "NgMap", "tripInfo", "utils", function($http, $uibModalInstance, NgMap, tripInfo, utils) {
        var ctrl = this;
        this.markers = [];
        this.origin = null;
        this.destination = null;
        this.waypoints = [];

        $http({
            method: "POST",
            url: utils.getApiBaseUrl() + "/trip/mapData",
            data: tripInfo
        }).then(function (response) {
            ctrl.markers = response.data;
            ctrl.origin = { location: { lat: ctrl.markers[0].Latitude, lng: ctrl.markers[0].Longtitude } };
            ctrl.destination = { location: { lat: ctrl.markers[ctrl.markers.length - 1].Latitude, lng: ctrl.markers[ctrl.markers.length - 1].Longtitude } }
            for (var i = 1; i < ctrl.markers.length - 1; i++) {
                ctrl.waypoints.push({ location: { lat: ctrl.markers[i].Latitude, lng: ctrl.markers[i].Longtitude } })
            }
        });

        this.text = "This is a modal window bound to a controller";

        this.close = function () {
            $uibModalInstance.dismiss("cancel");
        };
    }])
})();