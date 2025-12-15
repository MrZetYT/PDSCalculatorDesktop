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
    public class ControlPointRepository : Repository<ControlPoint>, IControlPointRepository
    {
        public ControlPointRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> HasDischargesAsync(int controlPointId)
        {
            return await _context.Discharges
                .AnyAsync(d => d.ControlPointId == controlPointId);
        }

        public async Task<IEnumerable<ControlPoint>> GetAllWithWaterUseTypeAsync()
        {
            return await _context.Set<ControlPoint>()
                .Include(cp => cp.WaterUseType)
                .ToListAsync();
        }

        public async Task<ControlPoint?> GetByIdWithWaterUseTypeAsync(int id)
        {
            return await _context.Set<ControlPoint>()
                .Include(cp => cp.WaterUseType)
                .FirstOrDefaultAsync(cp => cp.Id == id);
        }
    }
}
