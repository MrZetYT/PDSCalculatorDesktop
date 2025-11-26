using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDSCalculatorDesktop.Models;

namespace PDSCalculatorDesktop.Services
{
    public interface IDischargeService
    {
        Task<Discharge?> GetDischargeByIdAsync(int id);
        Task<IEnumerable<Discharge>> GetAllDischargesAsync();
        Task<IEnumerable<Discharge>> GetByEnterpriseAndDateAsync(int enterpriseId, DateTime dateTime);
        Task<Discharge> CreateDischargeAsync(
            string code, string name, DateTime registrationDate, int enterpriseId, int controlPointId);
        Task<Discharge> UpdateDischargeAsync(
            int id, string code, string name, DateTime registrationDate, int enterpriseId, int controlPointId);
        Task<bool> DeleteDischargeAsync(int id);
        Task<TechnicalParameters> AddTechnicalParametersAsync(
            int dischargeId, DateTime validFrom, double diameter, double flowRate,
            double waterFlowVelocity, double dischargeAngle, double distanceToWaterSurface,
            double distanceToShore, double distanceToControlPoint);
        Task<IEnumerable<TechnicalParameters>> GetTechnicalParametersHistoryAsync(int dischargeId);
        Task<TechnicalParameters?> GetCurrentTechnicalParametersAsync(int dischargeId);
        Task<IEnumerable<Discharge>> GetAllWithRelatedDataAsync();
    }
}
