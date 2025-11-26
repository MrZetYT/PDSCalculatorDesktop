using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDSCalculatorDesktop.Models;

namespace PDSCalculatorDesktop.Repositories.Interfaces
{
    public interface IDischargeRepository : IRepository<Discharge>
    {
        Task<IEnumerable<Discharge>> GetByEnterpriseAndDateAsync(int enterpriseId, DateTime dateTime);
        Task<IEnumerable<DischargeConcentration>> GetSubstanceConcentrationsAsync(int dischargeId);
        Task<IEnumerable<BackgroundConcentration>> GetNormativeIndicatorsAsync(int dischargeId);
        Task<IEnumerable<TechnicalParameters>> GetTechnicalParametersHistoryAsync(int dischargeId);
        Task<TechnicalParameters?> GetCurrentTechnicalParametersAsync(int dischargeId);
        Task<IEnumerable<Discharge>> GetAllWithRelatedDataAsync();
        Task<Discharge?> GetByIdWithRelatedDataAsync(int id);
    }
}
