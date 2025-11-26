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
            return await _context.Set<Substance>()
                .FirstOrDefaultAsync(s => s.Code == code);
        }

        public async Task<IEnumerable<Substance>> GetAllWithCharacteristicsAsync()
        {
            return await _context.Set<Substance>()
                .Include(s => s.WaterUseCharacteristics)
                .ThenInclude(swc => swc.WaterUseType)
                .ToListAsync();
        }

        public async Task<Substance?> GetByIdWithCharacteristicsAsync(int id)
        {
            return await _context.Set<Substance>()
                .Include(s => s.WaterUseCharacteristics)
                .ThenInclude(swc => swc.WaterUseType)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
