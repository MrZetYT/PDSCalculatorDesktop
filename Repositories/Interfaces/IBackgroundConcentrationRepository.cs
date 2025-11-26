using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Repositories.Interfaces
{
    public interface IBackgroundConcentrationRepository : IRepository<BackgroundConcentration>
    {
        Task<IEnumerable<BackgroundConcentration>> GetByControlPointIdAsync(int controlPointId);
        Task<IEnumerable<BackgroundConcentration>> GetBySubstanceIdAsync(int substanceId);
        Task<BackgroundConcentration?> GetLatestByControlPointAndSubstanceAsync(
            int controlPointId, int substanceId, DateTime beforeDate);
    }
}
