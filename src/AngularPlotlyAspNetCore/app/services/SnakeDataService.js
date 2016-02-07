(function () {
	'use strict';

	function SnakeDataService($http, $log) {

	    $log.info("SnakeDataService called");

	    var getGeographicalRegions = function () {
	        $log.info("SnakeDataService GetGeographicalRegions called");
	        return $http.get("/api/SnakeData/GeographicalRegions")
			.then(function (response) {
				return response.data;
			});
		}

		var getRegionBarChartData = function (region) {
		    $log.info("SnakeDataService getRegionBarChartData: " + region);
		    $log.info(region);
		    return $http.get("/api/SnakeData/RegionBarChart/" + region )
			.then(function (response) {
			    return response.data;
			});
		}

		return {
		    getGeographicalRegions: getGeographicalRegions,
		    getRegionBarChartData: getRegionBarChartData
		}
	}

	var module = angular.module('mainApp');

	// this code can be used with uglify
	module.factory("SnakeDataService",
		[
			"$http",
			"$log",
			SnakeDataService
		]
	);

})();
