using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Models.DTOs;
using PDSCalculatorDesktop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public class CalculationDataProvider : ICalculationDataProvider
    {
        private readonly IDischargeRepository _dischargeRepository;
        private readonly IMeasurementRepository _measurementRepository;

        public CalculationDataProvider(IDischargeRepository dischargeRepository, IMeasurementRepository measurementRepository)
        {
            _dischargeRepository = dischargeRepository;
            _measurementRepository = measurementRepository;
        }

        public async Task<CalculationInputData> GetCalculationDataAsync(
            int dischargeId,
            int substanceId,
            DateTime date)
        {
            var technicalParameters = await _dischargeRepository.GetTechnicalParametersAsync(dischargeId);
            var validTechnicalParameters = technicalParameters.Where(n=>n.ValidFrom<=date).MaxBy(n=>n.ValidFrom);
            if (validTechnicalParameters == null)
                throw new InvalidOperationException(
                    $"Не найдены технические параметры для выпуска {dischargeId} на дату {date}");

            var discharge = await _dischargeRepository.GetByIdWithRelatedDataAsync(dischargeId);

            if (discharge?.ControlPoint == null)
                throw new InvalidOperationException(
                    $"У выпуска {dischargeId} не указан контрольный створ");

            var controlPointId = discharge.ControlPointId;

            var substanceConcentration = await _measurementRepository.GetByDischargeSubstanceAndTypeAsync(
                dischargeId, substanceId, MeasurementType.SubstanceConcentration, date);
            
            var backgroundConcentration= await _measurementRepository.GetByControlPointSubstanceAndTypeAsync(
                controlPointId, substanceId, MeasurementType.BackgroundConcentration, date);

            var PDK = await _measurementRepository.GetByControlPointSubstanceAndTypeAsync(
                controlPointId, substanceId, MeasurementType.PDK, date);

            var KNK = await _measurementRepository.GetByControlPointSubstanceAndTypeAsync(
                controlPointId, substanceId, MeasurementType.KNK, date);

            var substance = await _dischargeRepository.GetSubstancesWithConcentrationsAsync(dischargeId);
            var validSubstance = substance.FirstOrDefault(n => n.SubstanceId == substanceId);

            return new CalculationInputData
            {
                BackgroundConcentration = backgroundConcentration.Value,
                SubstanceConcentration = substanceConcentration.Value,
                PDK = PDK.Value,
                KNK = KNK.Value,
                Substance = validSubstance.Substance,
                TechnicalParameters = validTechnicalParameters
            };
        }
    }
}
