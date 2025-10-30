using Microsoft.EntityFrameworkCore;
using PDSCalculatorDesktop.Data;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Repositories
{
    public class SubstanceRepository : Repository<Substance>, ISubstanceRepository
    {
        public SubstanceRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Substance?> GetByCodeAsync(string code)
        {
            return await _context.Set<Substance>().FirstOrDefaultAsync(n => n.Code == code);
        }
    }
}
