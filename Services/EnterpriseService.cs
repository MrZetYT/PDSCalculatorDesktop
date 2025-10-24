using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories;

namespace PDSCalculatorDesktop.Services
{
    public class EnterpriseService : IEnterpriseService
    {
        private readonly EnterpriseRepository _enterpriseRepository;

        public async Task<Enterprise?> GetEnterpriseByIdAsync(int id)
        {
            if (id <= 0) return null;
            return await _enterpriseRepository.GetValueAsync(id);
        }
    }
}
