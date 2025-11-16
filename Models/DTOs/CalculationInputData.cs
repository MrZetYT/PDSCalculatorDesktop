using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Models.DTOs
{
    public class CalculationInputData
    {
        public required TechnicalParameters TechnicalParameters { get; set; }
        public double BackgroundConcentration { get; set; }
        public double SubstanceConcentration { get; set; }
        public double PDK { get; set; }
        public double KNK { get; set; }
        public required Substance Substance { get; set; }
    }
}
