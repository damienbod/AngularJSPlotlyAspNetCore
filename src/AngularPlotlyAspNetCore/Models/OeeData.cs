namespace AngularPlotlyAspNetCore.Models
{
    using System;

    public class OeeData
    {
        public DateTime CreatedTimestamp { get; set; }
        public double Availability { get; set; }
        public double Performance { get; set; }
        public double Quality { get; set; }
        public double Oee { get; set; }
        public string MachineName { get; set; }
    }
}
