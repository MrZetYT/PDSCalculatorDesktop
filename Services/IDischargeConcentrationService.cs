using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public interface IDischargeConcentrationService
    {
        Task<DischargeConcentration?> GetDischargeConcentrationByIdAsync(int id);
        Task<IEnumerable<DischargeConcentration>> GetAllAsync();
        Task<IEnumerable<DischargeConcentration>> GetByDischargeIdAsync(int dischargeId);
        Task<IEnumerable<DischargeConcentration>> GetBySubstanceIdAsync(int substanceId);
        Task<DischargeConcentration?> GetLatestByDischargeAndSubstanceAsync(
            int dischargeId, int substanceId, DateTime beforeDate);
        Task<DischargeConcentration> CreateAsync(
            int dischargeId, int substanceId, double concentration, DateTime measurementDate);
        Task<DischargeConcentration> UpdateAsync(
            int id, int dischargeId, int substanceId, double concentration, DateTime measurementDate);
        Task<bool> DeleteAsync(int id);
    }
}
