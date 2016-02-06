using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using AngularPlotlyAspNetCore.Models;
using ElasticsearchCRUD;
using ElasticsearchCRUD.ContextSearch.SearchModel;
using ElasticsearchCRUD.ContextSearch.SearchModel.AggModel;
using ElasticsearchCRUD.ContextSearch.SearchModel.AggModel.Buckets;
using ElasticsearchCRUD.Model.SearchModel;
using ElasticsearchCRUD.Model.SearchModel.Aggregations;
using ElasticsearchCRUD.Model.SearchModel.Aggregations.RangeParam;
using ElasticsearchCRUD.Model.SearchModel.Queries;
using Newtonsoft.Json;

namespace AngularPlotlyAspNetCore.Providers
{    public class SnakeDataRepository : ISnakeDataRepository
    {
        private readonly IElasticsearchMappingResolver _elasticsearchMappingResolver = new ElasticsearchMappingResolver();

        //private string _connectionString = "http://localhost.fiddler:9200";
        private string _connectionString = "http://localhost:9200";

        public List<GeographicalRegion> GetGeographicalRegions()
        {
            List<GeographicalRegion> geographicalRegions = new List<GeographicalRegion>();
            var search = new Search
            {
                Aggs = new List<IAggs>
                {
                    new TermsBucketAggregation("getgeographicalregions", "geographicalregion")
                    {
                        Aggs = new List<IAggs>
                        {
                            new SumMetricAggregation("countCases", "numberofcaseshigh"),
                            new SumMetricAggregation("countDeaths", "numberofdeathshigh")
                        }
                    }
                }
            };

            using (var context = new ElasticsearchContext(_connectionString, _elasticsearchMappingResolver))
            {
                var items = context.Search<SnakeBites>(
                    search,
                    new SearchUrlParameters
                    {
                        SeachType = SeachType.count
                    });

                try
                {
                    var aggResult = items.PayloadResult.Aggregations.GetComplexValue<TermsBucketAggregationsResult>("getgeographicalregions");

                    foreach (var bucket in aggResult.Buckets)
                    {
                        var cases = Math.Round(bucket.GetSingleMetricSubAggregationValue<double>("countCases"), 2);
                        var deaths = Math.Round(bucket.GetSingleMetricSubAggregationValue<double>("countDeaths"), 2);
                        geographicalRegions.Add(
                            new GeographicalRegion {
                                Countries = bucket.DocCount,
                                Name = bucket.Key.ToString(),
                                NumberOfCasesHigh = cases,
                                NumberOfDeathsHigh = deaths,
                                DangerHigh =  (deaths > 1000)
                            });


                    }
                }
                catch (Exception)
                {
                   
                }
            }
                   
            return geographicalRegions;
        } 

        //public List<OeeDataAverageAgg> GetOeeForMachines(List<string> machineNames)
        //{
        //    string searchText = machineNames.Aggregate("", (current, term) => current + term.ToLower() + " ");

        //    List<OeeDataAverageAgg> results = new List<OeeDataAverageAgg>();
        //    var oeeDataAverageAgg = new OeeDataAverageAgg();
        //    var search = new Search
        //    {
        //        Query = new Query(new MatchQuery("machinename", searchText)),
        //        Aggs = new List<IAggs>
        //        {
        //            new AvgMetricAggregation("AverageAggAvailability", "availability"),
        //            new AvgMetricAggregation("AverageAggPerformance", "performance"),
        //            new AvgMetricAggregation("AverageAggQuality", "quality"),
        //            new AvgMetricAggregation("AverageAggOee", "oee")
        //        }
        //    };

        //    using (var context = new ElasticsearchContext(_connectionString, _elasticsearchMappingResolver))
        //    {
        //        var items = context.Search<SnakeBites>(
        //            search,
        //            new SearchUrlParameters
        //            {
        //                SeachType = SeachType.count
        //            });

        //        oeeDataAverageAgg.Availability = Math.Round(items.PayloadResult.Aggregations.GetSingleMetricAggregationValue<double>("AverageAggAvailability"), 2);
        //        oeeDataAverageAgg.Performance = Math.Round(items.PayloadResult.Aggregations.GetSingleMetricAggregationValue<double>("AverageAggPerformance"), 2);
        //        oeeDataAverageAgg.Quality = Math.Round(items.PayloadResult.Aggregations.GetSingleMetricAggregationValue<double>("AverageAggQuality"), 2);
        //        oeeDataAverageAgg.Oee = Math.Round(items.PayloadResult.Aggregations.GetSingleMetricAggregationValue<double>("AverageAggOee"), 2);

        //        oeeDataAverageAgg.DataPoints = items.PayloadResult.Hits.Total;
        //    }

        //    results.Add(oeeDataAverageAgg);
        //    return results;
        //}

        public GeographicalCountries GetBarChartDataForRegion(string region, string datapoint)
        {
            GeographicalCountries result = new GeographicalCountries { RegionName = region, Datapoint = datapoint};

            var search = new Search
            {
                Query = new Query(new MatchQuery("geographicalregion", region)),
                Aggs = new List<IAggs>
                {
                    new TermsBucketAggregation("termsCountry", "country")
                }
            };

            using (var context = new ElasticsearchContext(_connectionString, _elasticsearchMappingResolver))
            {
                var items = context.Search<SnakeBites>(search, new SearchUrlParameters { SeachType = SeachType.count });
                var aggResult = items.PayloadResult.Aggregations.GetComplexValue<TermsBucketAggregationsResult>("termsCountry");

                result.NumberOfCasesHighData = new BarTrace { Y = new List<double>()};
                result.NumberOfCasesLowData = new BarTrace {Y = new List<double>() };
                result.NumberOfDeathsHighData = new BarTrace {  Y = new List<double>() };
                result.NumberOfDeathsLowData = new BarTrace {  Y = new List<double>() };
                result.X = new List<string>();
                result.DataPointsCount = new List<double>();

                foreach (var bucket in aggResult.Buckets)
                {
                    //result.NumberOfCasesHighData.Y.Add(GetBucketValue(bucket., "AverageAggAvailability"));
                    //result.NumberOfCasesLowData.Y.Add(GetBucketValue(aggResult, "AverageAggPerformance"));
                    //result.NumberOfDeathsHighData.Y.Add(GetBucketValue(aggResult, "AverageAggQuality"));
                    //result.NumberOfDeathsLowData.Y.Add(GetBucketValue(aggResult, "AverageAggOee"));

                    result.X.Add(bucket.Key.ToString());
                    result.DataPointsCount.Add(bucket.DocCount);
                }
            }

            return result;
        }

        private double GetBucketValue(RangeBucket bucket, string key)
        {
            var res = bucket.GetSingleMetricSubAggregationValue<double?>(key);

            return (Math.Round(res.GetValueOrDefault(0.0), 2) * 100);
        }

        public void AddAllData()
        {
            var elasticsearchContext = new ElasticsearchContext("http://localhost:9200/", new ElasticsearchMappingResolver());

            elasticsearchContext.IndexCreate<SnakeBites>();

            Thread.Sleep(2000);

            List<SnakeBites> data = JsonConvert.DeserializeObject<List<SnakeBites>>(File.ReadAllText(@"C:\\git\\damienbod\\AngularPlotlyAspNetCore\\src\\AngularPlotlyAspNetCore\\snakeBitesData.json"));
            long counter = 1;
            foreach (var snakeCountry in data)
            {
                // create some documents
                counter++;
                elasticsearchContext.AddUpdateDocument(snakeCountry, counter);
            }

            elasticsearchContext.SaveChanges();


        }
    }

 
}
