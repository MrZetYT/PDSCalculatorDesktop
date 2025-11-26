namespace PDSCalculatorDesktop.Models
{
    public class DischargeConcentration
    {
        public int Id { get; set; }

        public int DischargeId { get; set; }

        public int SubstanceId { get; set; }

        public double Concentration { get; set; }

        public DateTime MeasurementDate { get; set; }

        public Discharge? Discharge { get; set; }

        public Substance? Substance { get; set; }
    }
}