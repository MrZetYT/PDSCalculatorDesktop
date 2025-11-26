using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public class DischargeConcentrationService : IDischargeConcentrationService
    {
        private readonly IDischargeConcentrationRepository _repository;

        public DischargeConcentrationService(IDischargeConcentrationRepository repository)
        {
            _repository = repository;
        }

        public async Task<DischargeConcentration?> GetDischargeConcentrationByIdAsync(int id)
        {
            return await _repository.GetValueAsync(id);
        }

        public async Task<IEnumerable<DischargeConcentration>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<DischargeConcentration>> GetByDischargeIdAsync(int dischargeId)
        {
            return await _repository.GetByDischargeIdAsync(dischargeId);
        }

        public async Task<IEnumerable<DischargeConcentration>> GetBySubstanceIdAsync(int substanceId)
        {
            return await _repository.GetBySubstanceIdAsync(substanceId);
        }

        public async Task<DischargeConcentration?> GetLatestByDischargeAndSubstanceAsync(
            int dischargeId, int substanceId, DateTime beforeDate)
        {
            return await _repository.GetLatestByDischargeAndSubstanceAsync(
                dischargeId, substanceId, beforeDate);
        }

        public async Task<DischargeConcentration> CreateAsync(
            int dischargeId, int substanceId, double concentration, DateTime measurementDate)
        {
            if (concentration < 0)
                throw new ArgumentException("Концентрация не может быть отрицательной");

            var dischargeConcentration = new DischargeConcentration
            {
                DischargeId = dischargeId,
                SubstanceId = substanceId,
                Concentration = concentration,
                MeasurementDate = measurementDate
            };

            return await _repository.CreateAsync(dischargeConcentration);
        }

        public async Task<DischargeConcentration> UpdateAsync(
            int id, int dischargeId, int substanceId, double concentration, DateTime measurementDate)
        {
            if (concentration < 0)
                throw new ArgumentException("Концентрация не может быть отрицательной");

            var dischargeConcentration = await _repository.GetValueAsync(id)
                ?? throw new ArgumentException("Концентрация в выпуске не найдена");

            dischargeConcentration.DischargeId = dischargeId;
            dischargeConcentration.SubstanceId = substanceId;
            dischargeConcentration.Concentration = concentration;
            dischargeConcentration.MeasurementDate = measurementDate;

            return await _repository.UpdateAsync(dischargeConcentration);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
