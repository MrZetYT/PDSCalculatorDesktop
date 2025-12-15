using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public class SubstanceService : ISubstanceService
    {
        private readonly ISubstanceRepository _repository;

        public SubstanceService(ISubstanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<Substance?> GetSubstanceByIdAsync(int id)
        {
            return await _repository.GetByIdWithCharacteristicsAsync(id);
        }

        public async Task<IEnumerable<Substance>> GetAllSubstancesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Substance>> GetAllWithCharacteristicsAsync()
        {
            return await _repository.GetAllWithCharacteristicsAsync();
        }

        public async Task<Substance> CreateSubstanceAsync(string code, string name, double knk)
        {
            var substance = new Substance
            {
                Code = code,
                Name = name,
                KNK = knk
            };

            return await _repository.CreateAsync(substance);
        }

        public async Task<Substance> UpdateSubstanceAsync(int id, string code, string name, double knk)
        {
            var substance = await _repository.GetValueAsync(id)
                ?? throw new ArgumentException("Вещество не найдено");

            substance.Code = code;
            substance.Name = name;
            substance.KNK = knk;

            return await _repository.UpdateAsync(substance);
        }

        public async Task<bool> DeleteSubstanceAsync(int id)
        {
            var canDeleteResult = await _repository.CanDeleteSubstanceAsync(id);

            if (!canDeleteResult.CanDelete)
            {
                throw new InvalidOperationException(
                    $"{canDeleteResult.Reason}\n" +
                    $"Связанных концентраций в выпусках: {canDeleteResult.RelatedDischarges}\n" +
                    $"Связанных фоновых концентраций: {canDeleteResult.RelatedBackgroundConc}");
            }

            return await _repository.DeleteAsync(id);
        }
    }
}
