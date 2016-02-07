(function () {
	'use strict';

	var module = angular.module('mainApp');

	module.controller('RegionBarChartController',
		[
			'$scope',
			'$log',
            'barChartData',
			RegionBarChartController
		]
	);

	function RegionBarChartController($scope, $log, barChartData) {
	    $log.info("RegionBarChartController called");
	    $scope.message = "RegionBarChartController";

	    $scope.barChartData = barChartData;
	    

	    $scope.RegionName = $scope.barChartData.RegionName;

	    $scope.layout = {
	        title: $scope.barChartData.RegionName + ": Number of snake bite deaths" ,
	        height: 500,
	        width: 1200
	    };

	   
	    function getYDatafromDatPoint() {
	        return $scope.barChartData.NumberOfDeathsHighData.Y;
	    }

	    $scope.data = [
          {
              x: $scope.barChartData.X,
              y: getYDatafromDatPoint(),
              name: $scope.barChartData.Datapoint,
              type: 'bar',
              orientation :'v'
          }
	    ];

	    $log.info($scope.data);

	}


})();
