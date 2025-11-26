using Microsoft.EntityFrameworkCore;
using PDSCalculatorDesktop.Data;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;

namespace PDSCalculatorDesktop.Repositories
{
    public class WaterUseTypeRepository : Repository<WaterUseType>, IWaterUseTypeRepository
    {
        public WaterUseTypeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<WaterUseType?> GetByCodeAsync(string code)
        {
            return await _context.Set<WaterUseType>()
                .FirstOrDefaultAsync(wut => wut.Code == code);
        }

        public async Task<IEnumerable<WaterUseType>> GetAllWithCharacteristicsAsync()
        {
            return await _context.Set<WaterUseType>()
                .Include(wut => wut.SubstanceCharacteristics)
                .ThenInclude(swc => swc.Substance)
                .ToListAsync();
        }
    }
}