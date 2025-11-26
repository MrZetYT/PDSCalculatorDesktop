using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public class ControlPointService : IControlPointService
    {
        private readonly IControlPointRepository _repository;

        public ControlPointService(IControlPointRepository repository)
        {
            _repository = repository;
        }

        public async Task<ControlPoint?> GetControlPointByIdAsync(int id)
        {
            return await _repository.GetByIdWithWaterUseTypeAsync(id);
        }

        public async Task<IEnumerable<ControlPoint>> GetAllControlPointsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<ControlPoint>> GetAllWithWaterUseTypeAsync()
        {
            return await _repository.GetAllWithWaterUseTypeAsync();
        }

        public async Task<ControlPoint> CreateControlPointAsync(string number, string name, int waterUseTypeId)
        {
            var controlPoint = new ControlPoint
            {
                Number = number,
                Name = name,
                WaterUseTypeId = waterUseTypeId,
                WaterUseType = null
            };

            return await _repository.CreateAsync(controlPoint);
        }

        public async Task<ControlPoint> UpdateControlPointAsync(int id, string number, string name, int waterUseTypeId)
        {
            var controlPoint = await _repository.GetValueAsync(id)
                ?? throw new ArgumentException("Контрольный створ не найден");

            controlPoint.Number = number;
            controlPoint.Name = name;
            controlPoint.WaterUseTypeId = waterUseTypeId;

            return await _repository.UpdateAsync(controlPoint);
        }

        public async Task<bool> DeleteControlPointAsync(int id)
        {
            var hasDischarges = await _repository.HasDischargesAsync(id);
            if (hasDischarges)
                throw new InvalidOperationException("Невозможно удалить контрольный створ с выпусками");

            return await _repository.DeleteAsync(id);
        }
    }
}
