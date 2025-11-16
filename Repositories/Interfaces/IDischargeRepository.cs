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
        Task<IEnumerable<Discharge>> GetByEnterpriseAndDateAsync(int id, DateTime dateTime);

        Task<IEnumerable<Measurement>> GetSubstancesWithConcentrationsAsync(int dischargeId);

        Task<IEnumerable<Measurement>> GetNormativeIndicatorsByDischargeAsync(int dischargeId);

        Task<bool> HasTechnicalParametersAsync(int dischargeId);

        Task<IEnumerable<TechnicalParameters>> GetTechnicalParametersAsync(int dischargeId);

        Task<Discharge?> GetByIdWithRelatedDataAsync(int id);
        Task<IEnumerable<Discharge>> GetAllWithRelatedDataAsync();
    }
}
