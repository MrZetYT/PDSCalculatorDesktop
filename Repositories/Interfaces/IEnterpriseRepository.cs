using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Repositories.Interfaces
{
    public interface IEnterpriseRepository : IRepository<Enterprise>
    {
        Task<Enterprise?> GetByCodeAsync(string code);

        Task<bool> HasDischargesAsync(int enterpriseId);
    }
}
