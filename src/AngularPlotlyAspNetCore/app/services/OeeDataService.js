(function () {
	'use strict';

	function OeeDataService($http, $log) {

	    $log.info("OeeDataService called");

	    var getMachines = function () {
	        $log.info("OeeDataService getMachines called");
			return $http.get("/api/OeeData/Machines")
			.then(function (response) {
				return response.data;
			});
		}

		var getMachineOee = function (machineName) {
		    $log.info("OeeDataService get MC1 data called: " + machineName);
		    $log.info(machineName);
			return $http.get("/api/OeeData/" + machineName)
			.then(function (response) {
				return response.data;
			});
		}

	    // http://localhost:31274/api/OeeData/LineData/mc1/oee/w
		var getBarChartData = function (machineName, dataPoints, perMonthWeekYear) {
		    $log.info("OeeDataService getBarChartData: " + machineName);
		    $log.info(machineName);
		    return $http.get("/api/OeeData/LineData/" + machineName + "/" + dataPoints + "/" + perMonthWeekYear)
			.then(function (response) {
			    return response.data;
			});
		}

		return {
		    getMachines: getMachines,
		    getMachineOee: getMachineOee,
		    getBarChartData: getBarChartData
		}
	}

	var module = angular.module('mainApp');

	// this code can be used with uglify
	module.factory("OeeDataService",
		[
			"$http",
			"$log",
			OeeDataService
		]
	);

})();
