using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Models
{
    public class Discharge
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required DateTime RegistrationAt { get; set; }
        public int EnterpriseId { get; set; }
        public required Enterprise Enterprise { get; set; }
        public ICollection<TechnicalParameters> TechnicalParameters { get; set; }
        public int ControlPointId { get; set; }
        public required ControlPoint ControlPoint { get; set; }
    }
}
