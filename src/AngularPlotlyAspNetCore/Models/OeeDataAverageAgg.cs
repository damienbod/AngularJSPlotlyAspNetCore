namespace AngularPlotlyAspNetCore.Models
{
    public class OeeDataAverageAgg
    {
        public double Availability { get; set; }
        public double Performance { get; set; }
        public double Quality { get; set; }
        public double Oee { get; set; }
        public long DataPoints { get; set; }

        public string MachineName { get; set; }

        public string FromTo { get; set; }
    }
}