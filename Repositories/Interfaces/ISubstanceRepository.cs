using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Repositories.Interfaces
{
    public interface ISubstanceRepository : IRepository<Substance>
    {
        Task<Substance?> GetByCodeAsync(string code);
    }
}
