namespace AngularPlotlyAspNetCore.Models
{
    using System;

    public class SnakeBites
    {
        public string GeographicalRegion { get; set; }
        public string Country { get; set; }
        public double NumberOfCasesLow { get; set; }
        public double NumberOfCasesHigh { get; set; }
        public double NumberOfDeathsLow { get; set; }
        public double NumberOfDeathsHigh { get; set; }

    }
}
