namespace PDSCalculatorDesktop.Models
{
    public class TechnicalParameters
    {
        public int Id { get; set; }

        public int DischargeId { get; set; }

        public DateTime ValidFrom { get; set; }

        public double Diameter { get; set; }

        public double FlowRate { get; set; }

        public double WaterFlowVelocity { get; set; }

        public double DischargeAngle { get; set; }

        public double DistanceToWaterSurface { get; set; }

        public double DistanceToShore { get; set; }

        public double DistanceToControlPoint { get; set; }

        public Discharge? Discharge { get; set; }
    }
}