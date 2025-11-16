namespace PDSCalculatorDesktop.Models
{
    public class WaterUseType
    {
        public int Id { get; set; }

        public required string Code { get; set; }

        public required string Name { get; set; }

        public ICollection<ControlPoint> ControlPoints { get; set; } = new List<ControlPoint>();

        public ICollection<SubstanceWaterUseCharacteristic> SubstanceCharacteristics { get; set; } = new List<SubstanceWaterUseCharacteristic>();
    }
}