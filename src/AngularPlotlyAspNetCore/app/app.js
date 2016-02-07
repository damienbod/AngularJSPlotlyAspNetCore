(function () {
    var mainApp = angular.module("mainApp",
        [
            "ui.router", 
            "plotly"
        ]);

	mainApp.config(["$stateProvider", "$urlRouterProvider",
		function ($stateProvider, $urlRouterProvider) {
            	$urlRouterProvider.otherwise("/overview");

            	$stateProvider
                    .state("overview", {
                        url: "/overview",
                        templateUrl: "/templates/overview.html",
                        controller: "OverviewController",
                        resolve: {

                            SnakeDataService: "SnakeDataService",

                            geographicalRegions: ["SnakeDataService", function (SnakeDataService) {
                                return SnakeDataService.getGeographicalRegions();
                            }]
                        }

                    }).state("regionbarchart", {
                        url: "/regionbarchart/:region/:datapoint",
		                templateUrl: "/templates/regoinbarchart.html",
		                controller: "RegionBarChartController",
		                resolve: {

		                    SnakeDataService: "SnakeDataService",

		                    barChartData: ["SnakeDataService", "$stateParams", function (SnakeDataService, $stateParams) {
		                        return SnakeDataService.getRegionBarChartData($stateParams.region, $stateParams.datapoint);
		                }]
		        }
		    });

		    //    .state("details", {
		    //        url: "/details/:machineName",
		    //        templateUrl: "/templates/details.html",
		    //        controller: "DetailsController",
		    //        resolve: {

		    //            OeeDataService: "OeeDataService",

		    //            oeeMachineData: ["OeeDataService", "$stateParams", function (OeeDataService, $stateParams) {
		    //                return OeeDataService.getMachineOee($stateParams.machineName);
		    //        }]
		    //        }
		    //    })
            
		}
	]
    );

	mainApp.run(["$rootScope", function ($rootScope) {

		$rootScope.$on('$stateChangeError', function (event, toState, toParams, fromState, fromParams, error) {
			console.log(event);
			console.log(toState);
			console.log(toParams);
			console.log(fromState);
			console.log(fromParams);
			console.log(error);
		})

		$rootScope.$on('$stateNotFound', function (event, unfoundState, fromState, fromParams) {
			console.log(event);
			console.log(unfoundState);
			console.log(fromState);
			console.log(fromParams);
		})

	}]);

})();
