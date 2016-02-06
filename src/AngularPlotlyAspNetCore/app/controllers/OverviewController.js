(function () {
	'use strict';

	var module = angular.module("mainApp");

	module.controller("OverviewController",
		[
			"$scope",
			"$log",
            "geographicalRegions",
			OverviewController
		]
	);

	function OverviewController($scope, $log, geographicalRegions) {
	    $log.info("OverviewController called");
	    $scope.message = "Overview";
	    $scope.geographicalRegions = geographicalRegions;


	}
})();
