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
        private readonly ISubstanceRepository _substanceRepository;

        public SubstanceService(ISubstanceRepository substanceRepository)
        {
            _substanceRepository = substanceRepository;
        }

        public async Task<Substance?> GetSubstanceByIdAsync(int id)
        {
            if (id <= 0) return null;
            return await _substanceRepository.GetValueAsync(id);
        }

        public async Task<IEnumerable<Substance>> GetAllSubstancesAsync()
        {
            return await _substanceRepository.GetAllAsync();
        }

        public async Task<Substance> CreateSubstanceAsync(string code,
                                                          string name,
                                                          string groupLFV,
                                                          HazardClass hazardClass)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Не введен код вещества.", nameof(code));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Не введено название вещества", nameof(name));
            }
            if (string.IsNullOrEmpty(groupLFV))
            {
                throw new ArgumentException("Не введена группа лимитирующего фактора вредности вещества", nameof(groupLFV));
            }

            if (await _substanceRepository.GetByCodeAsync(code) != null)
            {
                throw new ArgumentException("Вещество с таким кодом уже существует");
            }

            return await _substanceRepository.CreateAsync(new Substance() { 
                Code = code,
                Name = name,
                GroupLFV = groupLFV,
                HazardClass = hazardClass });
        }

        public async Task<Substance> UpdateSubstanceAsync(int id, string code, string name, string groupLFV, HazardClass hazardClass)
        {
            var substance = await _substanceRepository.GetValueAsync(id);

            if (substance == null)
            {
                throw new ArgumentException("Вещества с таким Id не существует", nameof(id));
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Не введен код вещества.", nameof(code));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Не введено название вещества", nameof(name));
            }
            if (string.IsNullOrEmpty(groupLFV))
            {
                throw new ArgumentException("Не введена группа лимитирующего фактора вредности вещества", nameof(groupLFV));
            }


            var existingSubstance = await _substanceRepository.GetByCodeAsync(code);
            if (existingSubstance != null && existingSubstance.Id != id)
            {
                throw new ArgumentException("Этот код вещества занят", nameof(code));
            }

            substance.Name = name;
            substance.Code = code;
            substance.GroupLFV = groupLFV;
            substance.HazardClass = hazardClass;

            await _substanceRepository.SaveChangesAsync();

            return substance;
        }

        public async Task<bool> DeleteSubstanceAsync(int id)
        {
            var substance = await _substanceRepository.GetValueAsync(id);
            if (substance == null)
            {
                throw new ArgumentException("Вещества с таким Id не существует", nameof(id));
            }

            return await _substanceRepository.DeleteAsync(id);
        }
    }
}
