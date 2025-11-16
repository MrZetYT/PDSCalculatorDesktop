namespace PDSCalculatorDesktop.Models
{
    public class BackgroundConcentration
    {
        public int Id { get; set; }

        public int ControlPointId { get; set; }

        public int SubstanceId { get; set; }

        public double Concentration { get; set; }

        public DateTime MeasurementDate { get; set; }

        public required ControlPoint ControlPoint { get; set; }

        public required Substance Substance { get; set; }
    }
}