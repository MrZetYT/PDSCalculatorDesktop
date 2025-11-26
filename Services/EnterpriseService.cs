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
        private readonly IEnterpriseRepository _repository;

        public EnterpriseService(IEnterpriseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Enterprise?> GetEnterpriseByIdAsync(int id)
        {
            return await _repository.GetValueAsync(id);
        }

        public async Task<IEnumerable<Enterprise>> GetAllEnterprisesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Enterprise> CreateEnterpriseAsync(string code, string name)
        {
            var enterprise = new Enterprise
            {
                Code = code,
                Name = name
            };

            return await _repository.CreateAsync(enterprise);
        }

        public async Task<Enterprise> UpdateEnterpriseAsync(int id, string code, string name)
        {
            var enterprise = await _repository.GetValueAsync(id)
                ?? throw new ArgumentException("Предприятие не найдено");

            enterprise.Code = code;
            enterprise.Name = name;

            return await _repository.UpdateAsync(enterprise);
        }

        public async Task<bool> DeleteEnterpriseAsync(int id)
        {
            var hasDischarges = await _repository.HasDischargesAsync(id);
            if (hasDischarges)
                throw new InvalidOperationException("Невозможно удалить предприятие с выпусками");

            return await _repository.DeleteAsync(id);
        }
    }
}
