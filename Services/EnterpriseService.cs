using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories;
using PDSCalculatorDesktop.Repositories.Interfaces;

namespace PDSCalculatorDesktop.Services
{
    public class EnterpriseService : IEnterpriseService
    {
        private readonly IEnterpriseRepository _enterpriseRepository;
        
        public EnterpriseService(IEnterpriseRepository enterpriseRepository)
        {
            _enterpriseRepository = enterpriseRepository;
        }

        public async Task<Enterprise?> GetEnterpriseByIdAsync(int id)
        {
            if (id <= 0) return null;
            return await _enterpriseRepository.GetValueAsync(id);
        }

        public async Task<IEnumerable<Enterprise>> GetAllEnterprisesAsync()
        {
            return await _enterpriseRepository.GetAllAsync();
        }

        public async Task<Enterprise> CreateEnterpriseAsync(string code, string name)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Не введен код предприятия.", nameof(code));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Не введено название предприятия", nameof(name));
            }

            if (await _enterpriseRepository.GetByCodeAsync(code) != null)
            {
                throw new ArgumentException("Предприятие с таким кодом уже существует");
            }

            return await _enterpriseRepository.CreateAsync(new Enterprise() { Code = code, Name = name });
        }

        public async Task<Enterprise> UpdateEnterpriseAsync(int id, string code, string name)
        {
            var enterprise = await _enterpriseRepository.GetValueAsync(id);

            if (enterprise == null)
            {
                throw new ArgumentException("Предприятия с таким Id не существует", nameof(id));
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Не введен код предприятия.", nameof(code));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Не введено название предприятия", nameof(name));
            }


            var existingEnterprise = await _enterpriseRepository.GetByCodeAsync(code);
            if (existingEnterprise != null && existingEnterprise.Id != id)
            {
                throw new ArgumentException("Этот код предприятия занят", nameof(code));
            }

            enterprise.Name = name;
            enterprise.Code = code;

            await _enterpriseRepository.SaveChangesAsync();

            return enterprise;
        }

        public async Task<bool> DeleteEnterpriseAsync(int id)
        {
            var enterprise = await _enterpriseRepository.GetValueAsync(id);
            if(enterprise == null)
            {
                throw new ArgumentException("Предприятия с таким Id не существует", nameof(id));
            }

            if(await _enterpriseRepository.HasDischargesAsync(id))
            {
                throw new InvalidOperationException("Невозможно удалить предприятие. У него есть свои выпуски");
            }

            return await _enterpriseRepository.DeleteAsync(id);
        }
    }
}
