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
            var oeeDataAverageAgg = new OeeDataAverageAgg();
            var search = new Search
            {
                Aggs = new List<IAggs>
                {
                    new TermsBucketAggregation("getgeographicalregions", "geographicalregion")
                    {
                        Aggs = new List<IAggs>
                        {
                            new ValueCountMetricAggregation("countCases", "numberofcaseshigh"),
                            new ValueCountMetricAggregation("countDeaths", "numberofdeathshigh")
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
                        geographicalRegions.Add(new GeographicalRegion { Countries = bucket.DocCount, Name = bucket.Key.ToString() });
                    }
                    oeeDataAverageAgg.DataPoints = items.PayloadResult.Hits.Total;
                }
                catch (Exception)
                {
                   
                }
            }
                   
            return geographicalRegions;
        } 

        public OeeDataAverageAgg GetOeeForAll()
        {
            var oeeDataAverageAgg = new OeeDataAverageAgg();
            var search = new Search
            {
                Aggs = new List<IAggs>
                {
                    new AvgMetricAggregation("AverageAggAvailability", "availability"),
                    new AvgMetricAggregation("AverageAggPerformance", "performance"),
                    new AvgMetricAggregation("AverageAggQuality", "quality"),
                    new AvgMetricAggregation("AverageAggOee", "oee")
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

                oeeDataAverageAgg.Availability = Math.Round(items.PayloadResult.Aggregations.GetSingleMetricAggregationValue<double>("AverageAggAvailability"), 2);
                oeeDataAverageAgg.Performance = Math.Round(items.PayloadResult.Aggregations.GetSingleMetricAggregationValue<double>("AverageAggPerformance"), 2);
                oeeDataAverageAgg.Quality = Math.Round(items.PayloadResult.Aggregations.GetSingleMetricAggregationValue<double>("AverageAggQuality"), 2);
                oeeDataAverageAgg.Oee = Math.Round(items.PayloadResult.Aggregations.GetSingleMetricAggregationValue<double>("AverageAggOee"), 2);

                oeeDataAverageAgg.DataPoints = items.PayloadResult.Hits.Total;
            }

            return oeeDataAverageAgg;
        }

        public List<OeeDataAverageAgg> GetOeeForMachines(List<string> machineNames)
        {
            string searchText = machineNames.Aggregate("", (current, term) => current + term.ToLower() + " ");

            List<OeeDataAverageAgg> results = new List<OeeDataAverageAgg>();
            var oeeDataAverageAgg = new OeeDataAverageAgg();
            var search = new Search
            {
                Query = new Query(new MatchQuery("machinename", searchText)),
                Aggs = new List<IAggs>
                {
                    new AvgMetricAggregation("AverageAggAvailability", "availability"),
                    new AvgMetricAggregation("AverageAggPerformance", "performance"),
                    new AvgMetricAggregation("AverageAggQuality", "quality"),
                    new AvgMetricAggregation("AverageAggOee", "oee")
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

                oeeDataAverageAgg.Availability = Math.Round(items.PayloadResult.Aggregations.GetSingleMetricAggregationValue<double>("AverageAggAvailability"), 2);
                oeeDataAverageAgg.Performance = Math.Round(items.PayloadResult.Aggregations.GetSingleMetricAggregationValue<double>("AverageAggPerformance"), 2);
                oeeDataAverageAgg.Quality = Math.Round(items.PayloadResult.Aggregations.GetSingleMetricAggregationValue<double>("AverageAggQuality"), 2);
                oeeDataAverageAgg.Oee = Math.Round(items.PayloadResult.Aggregations.GetSingleMetricAggregationValue<double>("AverageAggOee"), 2);

                oeeDataAverageAgg.DataPoints = items.PayloadResult.Hits.Total;
            }

            results.Add(oeeDataAverageAgg);
            return results;
        }

        public OeeDataProUnit GetLineDataForMachine(string machineName, string datapoint, string proYearMonthDay)
        {
            // start at 2015.01.01, just for demo
            OeeDataProUnit result = new OeeDataProUnit { MachineName = machineName, Datapoint = datapoint, ProDayMonthYear = proYearMonthDay};

            List<RangeAggregationParameter<string>> rangeDef = new List<RangeAggregationParameter<string>>();
            if (proYearMonthDay == "M")
            {
                rangeDef.Add(  new ToRangeAggregationParameter<string>("now-12M/M"));
                for (int i = 12; i >1; i--)
                {
                    string to = "now-" + (i - 1) + "M/M";
                    string from = "now-" + i + "M/M";
                    rangeDef.Add(new ToFromRangeAggregationParameter<string>(to, from));
                }
                rangeDef.Add(new FromRangeAggregationParameter<string>("now"));
            }
            else if (proYearMonthDay == "W")
            {
                rangeDef.Add(new ToRangeAggregationParameter<string>("now-365d/d"));
                for (int i = 365; i > 1; i--)
                {
                    
                    string to = "now-" + (i - 7) + "d/d";
                    string from = "now-" + i + "d/d";
                    rangeDef.Add(new ToFromRangeAggregationParameter<string>(to, from));
                    i--; i--; i--; i--; i--; i--; 
                }
                rangeDef.Add(new FromRangeAggregationParameter<string>("now"));
            }
            else if (proYearMonthDay == "D")
            {
                rangeDef.Add(new ToRangeAggregationParameter<string>("now-100d/d"));
                for (int i = 100; i > 1; i--)
                {
                    string to = "now-" + (i - 1) + "d/d";
                    string from = "now-" + i + "d/d";
                    rangeDef.Add(new ToFromRangeAggregationParameter<string>(to, from));
                }
                rangeDef.Add(new FromRangeAggregationParameter<string>("now"));
            }

            var search = new Search
            {
                Query = new Query(new MatchQuery("machinename", machineName.ToLower())),
                Aggs = new List<IAggs>
                {
                    new DateRangeBucketAggregation("testRangesBucketAggregation", "createdtimestamp", "MM-yyy-dd", rangeDef)
                        {
                            Aggs = new List<IAggs>
                            {
                                new AvgMetricAggregation("AverageAggAvailability", "availability"),
                                new AvgMetricAggregation("AverageAggPerformance", "performance"),
                                new AvgMetricAggregation("AverageAggQuality", "quality"),
                                new AvgMetricAggregation("AverageAggOee", "oee")
                            }
                        }
                }
            };

            using (var context = new ElasticsearchContext(_connectionString, _elasticsearchMappingResolver))
            {
                var items = context.Search<SnakeBites>(search, new SearchUrlParameters { SeachType = SeachType.count });
                var aggResult = items.PayloadResult.Aggregations.GetComplexValue<RangesBucketAggregationsResult>("testRangesBucketAggregation");

                result.AvailabilityData = new BarTrace { Y = new List<double>()};
                result.OeeTracesData = new BarTrace {Y = new List<double>() };
                result.PerformanceData = new BarTrace {  Y = new List<double>() };
                result.QualityData = new BarTrace {  Y = new List<double>() };
                result.X = new List<string>();
                result.DataPointsCount = new List<double>();

                foreach (var bucket in aggResult.Buckets)
                {
                    result.AvailabilityData.Y.Add(GetBucketValue(bucket, "AverageAggAvailability"));
                    result.PerformanceData.Y.Add(GetBucketValue(bucket, "AverageAggPerformance"));
                    result.QualityData.Y.Add(GetBucketValue(bucket, "AverageAggQuality"));
                    result.OeeTracesData.Y.Add(GetBucketValue(bucket, "AverageAggOee"));

                    result.X.Add(bucket.FromAsString + " " + bucket.ToAsString);
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
