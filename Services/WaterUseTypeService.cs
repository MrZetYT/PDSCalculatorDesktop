using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public class WaterUseTypeService : IWaterUseTypeService
    {
        private readonly IWaterUseTypeRepository _repository;

        public WaterUseTypeService(IWaterUseTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<WaterUseType?> GetWaterUseTypeByIdAsync(int id)
        {
            return await _repository.GetValueAsync(id);
        }

        public async Task<IEnumerable<WaterUseType>> GetAllWaterUseTypesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<WaterUseType>> GetAllWithCharacteristicsAsync()
        {
            return await _repository.GetAllWithCharacteristicsAsync();
        }

        public async Task<WaterUseType> CreateWaterUseTypeAsync(string code, string name)
        {
            var waterUseType = new WaterUseType
            {
                Code = code,
                Name = name
            };

            return await _repository.CreateAsync(waterUseType);
        }

        public async Task<WaterUseType> UpdateWaterUseTypeAsync(int id, string code, string name)
        {
            var waterUseType = await _repository.GetValueAsync(id)
                ?? throw new ArgumentException("Тип водопользования не найден");

            waterUseType.Code = code;
            waterUseType.Name = name;

            return await _repository.UpdateAsync(waterUseType);
        }

        public async Task<bool> DeleteWaterUseTypeAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
