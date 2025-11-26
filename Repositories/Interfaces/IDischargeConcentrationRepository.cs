using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Repositories.Interfaces
{
    public interface IDischargeConcentrationRepository : IRepository<DischargeConcentration>
    {
        Task<IEnumerable<DischargeConcentration>> GetByDischargeIdAsync(int dischargeId);
        Task<IEnumerable<DischargeConcentration>> GetBySubstanceIdAsync(int substanceId);
        Task<DischargeConcentration?> GetLatestByDischargeAndSubstanceAsync(
            int dischargeId, int substanceId, DateTime beforeDate);
    }
}
