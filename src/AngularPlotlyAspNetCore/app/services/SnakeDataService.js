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

		var getMachineOee = function (machineName) {
		    $log.info("SnakeDataService get MC1 data called: " + machineName);
		    $log.info(machineName);
			return $http.get("/api/OeeData/" + machineName)
			.then(function (response) {
				return response.data;
			});
		}

	    // http://localhost:31274/api/OeeData/LineData/mc1/oee/w
		var getBarChartData = function (machineName, dataPoints, perMonthWeekYear) {
		    $log.info("SnakeDataService getBarChartData: " + machineName);
		    $log.info(machineName);
		    return $http.get("/api/OeeData/LineData/" + machineName + "/" + dataPoints + "/" + perMonthWeekYear)
			.then(function (response) {
			    return response.data;
			});
		}

		return {
		    getGeographicalRegions: getGeographicalRegions,
		    getMachineOee: getMachineOee,
		    getBarChartData: getBarChartData
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
