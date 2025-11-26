using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PDSCalculatorDesktop.Data;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;

namespace PDSCalculatorDesktop.Repositories
{
    public class EnterpriseRepository : Repository<Enterprise>, IEnterpriseRepository
    {
        public EnterpriseRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Enterprise?> GetByCodeAsync(string code)
        {
            return await _context.Set<Enterprise>()
                .FirstOrDefaultAsync(e => e.Code == code);
        }

        public async Task<bool> HasDischargesAsync(int enterpriseId)
        {
            return await _context.Discharges
                .AnyAsync(d => d.EnterpriseId == enterpriseId);
        }
    }
}
