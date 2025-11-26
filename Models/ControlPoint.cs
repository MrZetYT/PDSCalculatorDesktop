namespace PDSCalculatorDesktop.Models
{
    public class ControlPoint
    {
        public int Id { get; set; }

        public required string Number { get; set; }

        public required string Name { get; set; }

        public int WaterUseTypeId { get; set; }

        public WaterUseType? WaterUseType { get; set; }

        public ICollection<Discharge> Discharges { get; set; } = new List<Discharge>();

        public ICollection<BackgroundConcentration> BackgroundConcentrations { get; set; } = new List<BackgroundConcentration>();
    }
}