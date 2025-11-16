using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Models
{
    public class PDSCalculationResult
    {
        public int Id { get; set; }
        public int DischargeId { get; set; }
        public int SubstanceId { get; set; }
        public DateTime CalculationDate { get; set; }
        public double IndividualPDS { get; set; }
        public double FinalPDS { get; set; }
        public string? Notes { get; set; }
    }
}
