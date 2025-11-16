namespace PDSCalculatorDesktop.Models
{
    public class Substance
    {
        public int Id { get; set; }

        public required string Code { get; set; }

        public required string Name { get; set; }

        public double KNK { get; set; }

        public ICollection<SubstanceWaterUseCharacteristic> WaterUseCharacteristics { get; set; } = new List<SubstanceWaterUseCharacteristic>();

        public ICollection<BackgroundConcentration> BackgroundConcentrations { get; set; } = new List<BackgroundConcentration>();

        public ICollection<DischargeConcentration> DischargeConcentrations { get; set; } = new List<DischargeConcentration>();
    }
}