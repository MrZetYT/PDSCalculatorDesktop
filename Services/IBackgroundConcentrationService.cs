using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public interface IBackgroundConcentrationService
    {
        Task<BackgroundConcentration?> GetBackgroundConcentrationByIdAsync(int id);
        Task<IEnumerable<BackgroundConcentration>> GetAllAsync();
        Task<IEnumerable<BackgroundConcentration>> GetByControlPointIdAsync(int controlPointId);
        Task<IEnumerable<BackgroundConcentration>> GetBySubstanceIdAsync(int substanceId);
        Task<BackgroundConcentration?> GetLatestByControlPointAndSubstanceAsync(
            int controlPointId, int substanceId, DateTime beforeDate);
        Task<BackgroundConcentration> CreateAsync(
            int controlPointId, int substanceId, double concentration, DateTime measurementDate);
        Task<BackgroundConcentration> UpdateAsync(
            int id, int controlPointId, int substanceId, double concentration, DateTime measurementDate);
        Task<bool> DeleteAsync(int id);
    }
}
