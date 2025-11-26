using Microsoft.EntityFrameworkCore;
using PDSCalculatorDesktop.Data;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;

namespace PDSCalculatorDesktop.Repositories
{
    public class BackgroundConcentrationRepository : Repository<BackgroundConcentration>, IBackgroundConcentrationRepository
    {
        public BackgroundConcentrationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<BackgroundConcentration>> GetByControlPointIdAsync(int controlPointId)
        {
            return await _context.Set<BackgroundConcentration>()
                .Where(bc => bc.ControlPointId == controlPointId)
                .Include(bc => bc.Substance)
                .Include(bc => bc.ControlPoint)
                .OrderByDescending(bc => bc.MeasurementDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BackgroundConcentration>> GetBySubstanceIdAsync(int substanceId)
        {
            return await _context.Set<BackgroundConcentration>()
                .Where(bc => bc.SubstanceId == substanceId)
                .Include(bc => bc.Substance)
                .Include(bc => bc.ControlPoint)
                .OrderByDescending(bc => bc.MeasurementDate)
                .ToListAsync();
        }

        public async Task<BackgroundConcentration?> GetLatestByControlPointAndSubstanceAsync(
            int controlPointId, int substanceId, DateTime beforeDate)
        {
            return await _context.Set<BackgroundConcentration>()
                .Where(bc => bc.ControlPointId == controlPointId
                    && bc.SubstanceId == substanceId
                    && bc.MeasurementDate <= beforeDate)
                .Include(bc => bc.Substance)
                .OrderByDescending(bc => bc.MeasurementDate)
                .FirstOrDefaultAsync();
        }
    }
}
