using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using PDSCalculatorDesktop.Data;
using System.Globalization;

namespace PDSCalculatorDesktop.Services
{
    public class PDSCalculationService : IPDSCalculationService
    {
        private readonly ApplicationDbContext _context;

        public PDSCalculationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stage1Result>> CalculateStage1Async(
            int dischargeId,
            DateTime calculationDate)
        {
            var utcDate = calculationDate.ToUniversalTime();

            var substances = await _context.DischargeConcentrations
                .Where(dc => dc.DischargeId == dischargeId && dc.MeasurementDate <= utcDate)
                .Select(dc => dc.SubstanceId)
                .Distinct()
                .ToListAsync();

            var results = new List<Stage1Result>();

            foreach (var substanceId in substances)
            {
                var result = await _context.Database
                    .SqlQuery<Stage1Result>($@"
                        SELECT * FROM calculate_individual_pds_only({dischargeId}, {substanceId}, {utcDate})")
                    .FirstOrDefaultAsync();

                if (result != null)
                {
                    results.Add(result);
                }
            }

            return results;
        }

        public async Task<IEnumerable<PDSCalculationResult>> CalculateFinalPDSAsync(
            int dischargeId,
            DateTime calculationDate)
        {
            var utcDate = calculationDate.ToUniversalTime();

            var results = await _context.Database
                .SqlQuery<FinalPDSResult>($@"
                    SELECT * FROM calculate_final_pds_for_discharge({dischargeId}, {utcDate})")
                .ToListAsync();

            return results.Select(r => new PDSCalculationResult
            {
                DischargeId = dischargeId,
                SubstanceId = r.SubstanceId,
                CalculationDate = calculationDate,
                IndividualPDS = r.IndividualPDS,
                FinalPDS = r.FinalPDS,
                Notes = FormatNotes(r)
            });
        }

        public async Task<IEnumerable<PDSCalculationResult>> GetCalculationHistoryAsync(int dischargeId)
        {
            return await Task.FromResult(Enumerable.Empty<PDSCalculationResult>());
        }

        private string FormatNotes(FinalPDSResult r)
        {
            return $"Код: {r.SubstanceCode}, " +
                   $"Название: {r.SubstanceName}, " +
                   $"Факт. конц.: {r.ActualConcentration.ToString("F4", CultureInfo.InvariantCulture)} мг/дм³, " +
                   $"Фон: {r.BackgroundConcentration.ToString("F4", CultureInfo.InvariantCulture)} мг/дм³, " +
                   $"ПДК: {r.PDK.ToString("F4", CultureInfo.InvariantCulture)} мг/дм³, " +
                   $"КНК: {r.KNK.ToString("F3", CultureInfo.InvariantCulture)}, " +
                   $"Кратность разб.: {r.DilutionRatio.ToString("F2", CultureInfo.InvariantCulture)}, " +
                   $"Индив. ПДС: {r.IndividualPDS.ToString("F4", CultureInfo.InvariantCulture)}, " +
                   $"Группа ЛФВ: {r.GroupLFV}, " +
                   $"Класс опасности: {r.HazardClass}, " +
                   $"Сумма группы: {r.GroupSum.ToString("F4", CultureInfo.InvariantCulture)}, " +
                   $"Коэфф. коррекции: {r.CorrectionFactor.ToString("F4", CultureInfo.InvariantCulture)}, " +
                   $"Макс. масса (т/год): {r.MaxAllowedMassPerYear.ToString("F6", CultureInfo.InvariantCulture)}, " +
                   $"Превышение: {(r.IsExceeded ? $"{r.ExcessPercent.ToString("F2", CultureInfo.InvariantCulture)}%" : "нет")}";
        }
    }

    public class Stage1Result
    {
        public int SubstanceId { get; set; }
        public string SubstanceCode { get; set; } = string.Empty;
        public string SubstanceName { get; set; } = string.Empty;
        public double ActualConcentration { get; set; }
        public double BackgroundConcentration { get; set; }
        public double PDK { get; set; }
        public double KNK { get; set; }
        public double DilutionRatio { get; set; }
        public double IndividualPDS { get; set; }
        public string GroupLFV { get; set; } = string.Empty;
        public int HazardClass { get; set; }
    }

    public class FinalPDSResult
    {
        public int SubstanceId { get; set; }
        public string SubstanceCode { get; set; } = string.Empty;
        public string SubstanceName { get; set; } = string.Empty;
        public double ActualConcentration { get; set; }
        public double BackgroundConcentration { get; set; }
        public double PDK { get; set; }
        public double KNK { get; set; }
        public double DilutionRatio { get; set; }
        public double IndividualPDS { get; set; }
        public string GroupLFV { get; set; } = string.Empty;
        public int HazardClass { get; set; }
        public double GroupSum { get; set; }
        public double CorrectionFactor { get; set; }
        public double FinalPDS { get; set; }
        public double MaxAllowedMassPerYear { get; set; }
        public bool IsExceeded { get; set; }
        public double ExcessPercent { get; set; }
    }
}