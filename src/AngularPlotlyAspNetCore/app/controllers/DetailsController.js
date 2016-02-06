(function () {
	'use strict';

	var module = angular.module('mainApp');

	module.controller('DetailsController',
		[
			'$scope',
			'$log',
            'oeeMachineData',
			DetailsController
		]
	);

	function DetailsController($scope, $log, oeeMachineData) {
	    $log.info("DetailsController called");
	    $scope.message = "Animal Details";

	    $scope.oeeMachineData = oeeMachineData;
	    $log.info(oeeMachineData);

	    function createOeeArray() {
	        var data1 = 1 - $scope.oeeMachineData.Oee;
	        var oeePieData = [];
            oeePieData.push($scope.oeeMachineData.Oee);
	        oeePieData.push(data1);
	       
	        return oeePieData;
	    }

	    function createAvailabilityArray() {
	        var data1 = 1 - $scope.oeeMachineData.Availability;
	        var oeePieData = [];
	        oeePieData.push($scope.oeeMachineData.Availability);
	        oeePieData.push(data1);

	        return oeePieData;
	    }

	    function createPerformanceArray() {
	        var data1 = 1 - $scope.oeeMachineData.Performance;
	        var oeePieData = [];
	        oeePieData.push($scope.oeeMachineData.Performance);
	        oeePieData.push(data1);

	        return oeePieData;
	    }

	    function createQualityArray() {
	        var data1 = 1 - $scope.oeeMachineData.Quality;
	        var oeePieData = [];
	        oeePieData.push($scope.oeeMachineData.Quality);
	        oeePieData.push(data1);

	        return oeePieData;
	    }

	    
	    //$scope.data = [{
	    //    x: [1, 2, 3, 4, 5],
	    //    y: [1, 2, 4, 8, 16]
	    //}];
	    //$scope.layout = { height: 600, width: 1000, title: 'foobar' };
	    //$scope.options = { showLink: false, displayLogo: false };

	    var ultimateColors = [
          ['rgb(0, 153, 0)', 'rgb(224,224,224)'],
          ['rgb(209, 239, 12)', 'rgb(224,224,224)'],
          ['rgb(255, 51, 51)', 'rgb(224,224,224)']
	    ];

	    $scope.options = { showLink: false, displayLogo: false, displayModeBar: false };

	    $scope.dataoee = [
	        {
	            values: createOeeArray(),
	            labels: ['', ''],
	            marker: {
	                colors: ultimateColors[0]
	            },
	            name: '',
	            hoverinfo: 'label+percent+name',
	            hole: .4,
	            type: 'pie'
	        }
	    ];

	    $scope.dataQuality = [
	        {
	            values: createQualityArray(),
	            labels: ['', ''],
	            marker: {
	                colors: ultimateColors[0]
	            },
	            name: '',
	            hoverinfo: 'label+percent+name',
	            hole: .4,
	            type: 'pie'
	        }
	    ];

	    $scope.dataPerformance = [
          {
              values: createPerformanceArray(),
              labels: ['', ''],
              marker: {
                  colors: ultimateColors[0]
              },
              name: '',
              hoverinfo: 'label+percent+name',
              hole: .4,
              type: 'pie'
          }
	    ];

	    $scope.dataAvailability = [
         {
             values: createAvailabilityArray(),
             labels: ['', ''],
             marker: {
                 colors: ultimateColors[0]
             },
             name: '',
             hoverinfo: 'label+percent+name',
             hole: .4,
             type: 'pie'
         }
	    ];

	    $scope.layoutoee = {
	        title: '',
	        showlegend: false,
	        height: 390,
	        width: 390
	    };

	    //$scope.data = [
        //    {
        //        values: createOeeArray(),
        //        labels: ['OEE'],
        //        marker: {
        //            colors: ultimateColors[0]
        //        },
        //        domain: {
        //            x: [0, .30],
        //            y: [0, 1]
        //        },
        //        name: 'OEE',
        //        hoverinfo: 'label+percent+name',
        //        hole: .4,
        //        type: 'pie'
        //    },
        //    {
        //        values: [.89, 0.11],
        //        labels: ['Availability'],
        //        domain: {
        //            x: [0.33, 0.53],
        //            y: [0, 0.6]
        //        },
        //        marker: {
        //            colors: ultimateColors[0]
        //        },
        //        name: 'Availability',
        //        hoverinfo: 'label+percent+name',
        //        hole: .4,
        //        type: 'pie'
        //    },
        //    {
        //        values: [.69, 0.31],
        //        labels: ['Quality'],
        //        marker: {
        //            colors: ultimateColors[0]
        //        },
        //        domain: {
        //            x: [0.56, .76],
        //            y: [0, 0.6]
        //        },
        //        name: 'Quality',
        //        hoverinfo: 'label+percent+name',
        //        hole: .4,
        //        type: 'pie'
        //    },
        //    {
        //        values: [.69, 0.31],
        //        labels: ['Performance'],
        //        marker: {
        //            colors: ultimateColors[1]
        //        },
        //        domain: {
        //            x: [0.79, .99],
        //            y: [0, 0.6]
        //        },
        //        name: 'Performance',
        //        hoverinfo: 'label+percent+name',
        //        hole: .4,
        //        type: 'pie'
        //    }

	    //];

	    //$scope.layout = {
	    //    title: 'OEE:' + $scope.oeeMachineData.MachineName,
	    //    annotations: [
        //        {
        //            font: {
        //                size: 20
        //            },
        //            showarrow: false,
        //            text: 'OEE',
        //            x: 0.12,
        //            y: 0.5
        //        },
        //        {
        //            font: {
        //                size: 10
        //            },
        //            showarrow: false,
        //            text: 'Availability',
        //            x: 0.43,
        //            y: 0.28
        //        }, {
        //            font: {
        //                size: 10
        //            },
        //            showarrow: false,
        //            text: 'Quality',
        //            x: 0.68,
        //            y: 0.28
        //        }, {
        //            font: {
        //                size: 10
        //            },
        //            showarrow: false,
        //            text: 'Performance',
        //            x: 0.93,
        //            y: 0.28
        //        }
	    //    ],
	    //    height: 600,
	    //    width: 1200
	    //};

	}


})();
