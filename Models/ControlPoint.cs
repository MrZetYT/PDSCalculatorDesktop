using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Models
{
    public class ControlPoint
    {
        public int Id {  get; set; }
        public required string Number { get; set; }
        public required string Name { get; set; }
        public double Distance { get; set; }
        public ICollection<Discharge> Discharges { get; set; } = new List<Discharge>();
    }
}
