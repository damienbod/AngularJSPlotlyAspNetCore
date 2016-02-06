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

		                OeeDataService: "SnakeDataService",

		                machines: ["SnakeDataService",  function (SnakeDataService) {
		                    return SnakeDataService.getGeographicalRegions();
		                }]
		            }

		        })
		        .state("details", {
		            url: "/details/:machineName",
		            templateUrl: "/templates/details.html",
		            controller: "DetailsController",
		            resolve: {

		                OeeDataService: "OeeDataService",

		                oeeMachineData: ["OeeDataService", "$stateParams", function (OeeDataService, $stateParams) {
		                    return OeeDataService.getMachineOee($stateParams.machineName);
		            }]
		            }
		        })
            .state("linechart", {
                url: "/linechart/:machineName/:datapoint/:perMonthWeekYear",
                templateUrl: "/templates/linechart.html",
                controller: "LineChartController",
                resolve: {

                    OeeDataService: "OeeDataService",

                    barChartData: ["OeeDataService", "$stateParams", function (OeeDataService, $stateParams) {
                        return OeeDataService.getBarChartData($stateParams.machineName, $stateParams.datapoint, $stateParams.perMonthWeekYear);
                    }]
                }
            });
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
