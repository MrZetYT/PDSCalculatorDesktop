using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Repositories.Interfaces
{
    public interface IWaterUseTypeRepository : IRepository<WaterUseType>
    {
        Task<WaterUseType?> GetByCodeAsync(string code);
        Task<IEnumerable<WaterUseType>> GetAllWithCharacteristicsAsync();
    }
}
