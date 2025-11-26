using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Repositories.Interfaces
{
    public interface IControlPointRepository : IRepository<ControlPoint>
    {
        Task<ControlPoint?> GetByNumberAsync(string number);
        Task<bool> HasDischargesAsync(int controlPointId);
        Task<IEnumerable<ControlPoint>> GetAllWithWaterUseTypeAsync();
        Task<ControlPoint?> GetByIdWithWaterUseTypeAsync(int id);
    }
}
