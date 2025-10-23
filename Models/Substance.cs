using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Models
{
    public enum HazardClass
    {
        ExtremelyHazardous = 1,
        HighlyHazardous = 2,
        Hazardous = 3,
        ModeratelyHazardous = 4
    }
    public class Substance
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string GroupLFV { get; set; }
        public required HazardClass HazardClass { get; set; }
    }
}
