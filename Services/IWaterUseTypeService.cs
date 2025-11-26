using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public interface IWaterUseTypeService
    {
        Task<WaterUseType?> GetWaterUseTypeByIdAsync(int id);
        Task<IEnumerable<WaterUseType>> GetAllWaterUseTypesAsync();
        Task<IEnumerable<WaterUseType>> GetAllWithCharacteristicsAsync();
        Task<WaterUseType> CreateWaterUseTypeAsync(string code, string name);
        Task<WaterUseType> UpdateWaterUseTypeAsync(int id, string code, string name);
        Task<bool> DeleteWaterUseTypeAsync(int id);
    }
}
