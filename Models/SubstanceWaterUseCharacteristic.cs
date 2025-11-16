namespace PDSCalculatorDesktop.Models
{
    public enum HazardClass
    {
        ExtremelyHazardous = 1,
        HighlyHazardous = 2,
        Hazardous = 3,
        ModeratelyHazardous = 4
    }

    public class SubstanceWaterUseCharacteristic
    {
        public int Id { get; set; }

        public int SubstanceId { get; set; }

        public int WaterUseTypeId { get; set; }

        public required string GroupLFV { get; set; }

        public HazardClass HazardClass { get; set; }

        public double PDK { get; set; }

        public required Substance Substance { get; set; }

        public required WaterUseType WaterUseType { get; set; }
    }
}