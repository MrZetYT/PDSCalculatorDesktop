namespace PDSCalculatorDesktop.Models
{
    public class Enterprise
    {
        public int Id { get; set; }

        public required string Code { get; set; }

        public required string Name { get; set; }

        public ICollection<Discharge> Discharges { get; set; } = new List<Discharge>();
    }
}