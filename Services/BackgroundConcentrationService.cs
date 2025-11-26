using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public class BackgroundConcentrationService : IBackgroundConcentrationService
    {
        private readonly IBackgroundConcentrationRepository _repository;

        public BackgroundConcentrationService(IBackgroundConcentrationRepository repository)
        {
            _repository = repository;
        }

        public async Task<BackgroundConcentration?> GetBackgroundConcentrationByIdAsync(int id)
        {
            return await _repository.GetValueAsync(id);
        }

        public async Task<IEnumerable<BackgroundConcentration>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<BackgroundConcentration>> GetByControlPointIdAsync(int controlPointId)
        {
            return await _repository.GetByControlPointIdAsync(controlPointId);
        }

        public async Task<IEnumerable<BackgroundConcentration>> GetBySubstanceIdAsync(int substanceId)
        {
            return await _repository.GetBySubstanceIdAsync(substanceId);
        }

        public async Task<BackgroundConcentration?> GetLatestByControlPointAndSubstanceAsync(
            int controlPointId, int substanceId, DateTime beforeDate)
        {
            return await _repository.GetLatestByControlPointAndSubstanceAsync(
                controlPointId, substanceId, beforeDate);
        }

        public async Task<BackgroundConcentration> CreateAsync(
            int controlPointId, int substanceId, double concentration, DateTime measurementDate)
        {
            if (concentration < 0)
                throw new ArgumentException("Концентрация не может быть отрицательной");

            var backgroundConcentration = new BackgroundConcentration
            {
                ControlPointId = controlPointId,
                SubstanceId = substanceId,
                Concentration = concentration,
                MeasurementDate = measurementDate
            };

            return await _repository.CreateAsync(backgroundConcentration);
        }

        public async Task<BackgroundConcentration> UpdateAsync(
            int id, int controlPointId, int substanceId, double concentration, DateTime measurementDate)
        {
            if (concentration < 0)
                throw new ArgumentException("Концентрация не может быть отрицательной");

            var backgroundConcentration = await _repository.GetValueAsync(id)
                ?? throw new ArgumentException("Фоновая концентрация не найдена");

            backgroundConcentration.ControlPointId = controlPointId;
            backgroundConcentration.SubstanceId = substanceId;
            backgroundConcentration.Concentration = concentration;
            backgroundConcentration.MeasurementDate = measurementDate;

            return await _repository.UpdateAsync(backgroundConcentration);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
