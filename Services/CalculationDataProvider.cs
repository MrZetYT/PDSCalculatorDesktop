using Microsoft.EntityFrameworkCore;
using PDSCalculatorDesktop.Data;
using PDSCalculatorDesktop.Models.DTOs;
using PDSCalculatorDesktop.Repositories.Interfaces;
using System.CodeDom;

namespace PDSCalculatorDesktop.Services
{
    public class CalculationDataProvider : ICalculationDataProvider
    {
        private readonly IDischargeService _dischargeService;
        private readonly IBackgroundConcentrationService _backgroundConcentrationService;
        private readonly IDischargeConcentrationService _dischargeConcentrationService;
        private readonly ISubstanceService _substanceService;
        private readonly ITechnicalParametersRepository _technicalParametersRepository;

        private readonly ApplicationDbContext _context;

        public CalculationDataProvider(IDischargeService dischargeService,
            IBackgroundConcentrationService backgroundConcentrationService,
            IDischargeConcentrationService dischargeConcentrationService,
            ISubstanceService substanceService,
            ITechnicalParametersRepository technicalParametersRepository,
            ApplicationDbContext context)
        {
            _dischargeService = dischargeService;
            _backgroundConcentrationService = backgroundConcentrationService;
            _dischargeConcentrationService = dischargeConcentrationService;
            _substanceService = substanceService;
            _technicalParametersRepository = technicalParametersRepository;
            _context = context;
        }

        public async Task<CalculationInputData> GetCalculationDataAsync(
            int dischargeId,
            int substanceId,
            DateTime date)
        {
            var discharge = await _dischargeService.GetDischargeByIdAsync(dischargeId);
            if (discharge == null)
            {
                throw new InvalidOperationException($"Выпуск с ID {dischargeId} не найден");
            }
            var controlPointId = discharge.ControlPointId;

            var technicalParameters = await _technicalParametersRepository.GetActualAsync(dischargeId, date);
            if(technicalParameters == null)
            {
                throw new InvalidOperationException($"Технические параметры для выпуска {dischargeId} на дату {date} не найдены");
            }

            var backgroundConcentration = await _backgroundConcentrationService
                .GetLatestByControlPointAndSubstanceAsync(controlPointId, substanceId, date);
            if(backgroundConcentration == null)
            {
                throw new InvalidOperationException(
                    $"Фоновая концентрация для вещества {substanceId} в контрольном створе {controlPointId} на дату {date} не найдена");
            }

            var dischargeConcentration = await _dischargeConcentrationService
                .GetLatestByDischargeAndSubstanceAsync(dischargeId, substanceId, date);
            if(dischargeConcentration == null)
            {
                throw new InvalidOperationException(
                    $"Концентрация вещества {substanceId} в сточных водах выпуска {dischargeId} на дату {date} не найдена");
            }

            var substance = await _substanceService.GetSubstanceByIdAsync(substanceId);
            if(substance == null)
            {
                throw new InvalidOperationException(
                    $"Вещества {substanceId} не существует");
            }
            var knk = substance.KNK;

            var controlPoint = discharge.ControlPoint;
            if (controlPoint == null)
            {
                throw new InvalidOperationException($"У выпуска {dischargeId} нет контрольного створа");
            }
            var pdk = substance.WaterUseCharacteristics.FirstOrDefault(n=>n.WaterUseTypeId == controlPoint.WaterUseTypeId);
            if (pdk == null)
            {
                throw new InvalidOperationException(
                    $"Для вещества {substanceId} не найдена характеристика для типа водопользования контрольного створа {controlPointId}");
            }

            return new CalculationInputData
            {
                Substance = substance,
                TechnicalParameters = technicalParameters,
                PDK = pdk.PDK,
                KNK = knk,
                SubstanceConcentration = dischargeConcentration.Concentration,
                BackgroundConcentration = backgroundConcentration.Concentration
            };
        }
    }
}