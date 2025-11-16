namespace PDSCalculatorDesktop.Models
{
    public class Discharge
    {
        public int Id { get; set; }

        public required string Code { get; set; }

        public required string Name { get; set; }

        public DateTime RegistrationDate { get; set; }

        public int EnterpriseId { get; set; }

        public int ControlPointId { get; set; }

        public required Enterprise Enterprise { get; set; }

        public required ControlPoint ControlPoint { get; set; }

        public ICollection<TechnicalParameters> TechnicalParameters { get; set; } = new List<TechnicalParameters>();

        public ICollection<DischargeConcentration> DischargeConcentrations { get; set; } = new List<DischargeConcentration>();
    }
}