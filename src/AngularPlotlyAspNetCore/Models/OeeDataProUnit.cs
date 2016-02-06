namespace AngularPlotlyAspNetCore.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json.Linq;

    public class OeeDataProUnit
    {
        public BarTrace QualityData { get; set; }

        public BarTrace OeeTracesData { get; set; }

        public BarTrace PerformanceData { get; set; }

        public BarTrace AvailabilityData { get; set; }

        public string MachineName { get; set; }

        public string ProDayMonthYear { get; set; }

        public string Datapoint { get; set; }

        public List<double> DataPointsCount { get; set; }
   
        public List<string> X { get; set; }

    }
}

