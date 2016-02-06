(function () {
	'use strict';

	var module = angular.module("mainApp");

	module.controller("OverviewController",
		[
			"$scope",
			"$log",
            "machines",
			OverviewController
		]
	);

	function OverviewController($scope, $log, machines) {
	    $log.info("OverviewController called");
	    $scope.message = "Overview";
	    $scope.machines = machines;


	}
})();
