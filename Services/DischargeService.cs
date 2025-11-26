using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories;
using PDSCalculatorDesktop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public class DischargeService : IDischargeService
    {
        private readonly IDischargeRepository _dischargeRepository;
        private readonly ITechnicalParametersRepository _technicalParametersRepository;

        public DischargeService(
            IDischargeRepository dischargeRepository,
            ITechnicalParametersRepository technicalParametersRepository)
        {
            _dischargeRepository = dischargeRepository;
            _technicalParametersRepository = technicalParametersRepository;
        }

        public async Task<Discharge?> GetDischargeByIdAsync(int id)
        {
            return await _dischargeRepository.GetByIdWithRelatedDataAsync(id);
        }

        public async Task<IEnumerable<Discharge>> GetAllDischargesAsync()
        {
            return await _dischargeRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Discharge>> GetByEnterpriseAndDateAsync(int enterpriseId, DateTime dateTime)
        {
            return await _dischargeRepository.GetByEnterpriseAndDateAsync(enterpriseId, dateTime);
        }

        public async Task<Discharge> CreateDischargeAsync(
            string code, string name, DateTime registrationDate, int enterpriseId, int controlPointId)
        {
            var discharge = new Discharge
            {
                Code = code,
                Name = name,
                RegistrationDate = registrationDate,
                EnterpriseId = enterpriseId,
                ControlPointId = controlPointId,
                Enterprise = null,
                ControlPoint = null
            };

            return await _dischargeRepository.CreateAsync(discharge);
        }

        public async Task<Discharge> UpdateDischargeAsync(
            int id, string code, string name, DateTime registrationDate, int enterpriseId, int controlPointId)
        {
            var discharge = await _dischargeRepository.GetValueAsync(id)
                ?? throw new ArgumentException("Выпуск не найден");

            discharge.Code = code;
            discharge.Name = name;
            discharge.RegistrationDate = registrationDate;
            discharge.EnterpriseId = enterpriseId;
            discharge.ControlPointId = controlPointId;

            return await _dischargeRepository.UpdateAsync(discharge);
        }

        public async Task<bool> DeleteDischargeAsync(int id)
        {
            return await _dischargeRepository.DeleteAsync(id);
        }

        public async Task<TechnicalParameters> AddTechnicalParametersAsync(
            int dischargeId, DateTime validFrom, double diameter, double flowRate,
            double waterFlowVelocity, double dischargeAngle, double distanceToWaterSurface,
            double distanceToShore, double distanceToControlPoint)
        {
            var technicalParameters = new TechnicalParameters
            {
                DischargeId = dischargeId,
                ValidFrom = validFrom,
                Diameter = diameter,
                FlowRate = flowRate,
                WaterFlowVelocity = waterFlowVelocity,
                DischargeAngle = dischargeAngle,
                DistanceToWaterSurface = distanceToWaterSurface,
                DistanceToShore = distanceToShore,
                DistanceToControlPoint = distanceToControlPoint,
                Discharge = null
            };

            return await _technicalParametersRepository.CreateAsync(technicalParameters);
        }

        public async Task<IEnumerable<TechnicalParameters>> GetTechnicalParametersHistoryAsync(int dischargeId)
        {
            return await _dischargeRepository.GetTechnicalParametersHistoryAsync(dischargeId);
        }

        public async Task<TechnicalParameters?> GetCurrentTechnicalParametersAsync(int dischargeId)
        {
            return await _dischargeRepository.GetCurrentTechnicalParametersAsync(dischargeId);
        }

        public async Task<IEnumerable<Discharge>> GetAllWithRelatedDataAsync()
        {
            return await _dischargeRepository.GetAllWithRelatedDataAsync();
        }
    }
}
