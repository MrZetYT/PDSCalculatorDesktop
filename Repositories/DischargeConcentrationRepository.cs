using Microsoft.EntityFrameworkCore;
using PDSCalculatorDesktop.Data;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;

namespace PDSCalculatorDesktop.Repositories
{
    public class DischargeConcentrationRepository : Repository<DischargeConcentration>, IDischargeConcentrationRepository
    {
        public DischargeConcentrationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<DischargeConcentration>> GetByDischargeIdAsync(int dischargeId)
        {
            return await _context.Set<DischargeConcentration>()
                .Where(dc => dc.DischargeId == dischargeId)
                .Include(dc => dc.Substance)
                .Include(dc => dc.Discharge)
                .OrderByDescending(dc => dc.MeasurementDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DischargeConcentration>> GetBySubstanceIdAsync(int substanceId)
        {
            return await _context.Set<DischargeConcentration>()
                .Where(dc => dc.SubstanceId == substanceId)
                .Include(dc => dc.Substance)
                .Include(dc => dc.Discharge)
                .OrderByDescending(dc => dc.MeasurementDate)
                .ToListAsync();
        }

        public async Task<DischargeConcentration?> GetLatestByDischargeAndSubstanceAsync(
            int dischargeId, int substanceId, DateTime beforeDate)
        {
            return await _context.Set<DischargeConcentration>()
                .Where(dc => dc.DischargeId == dischargeId
                    && dc.SubstanceId == substanceId
                    && dc.MeasurementDate <= beforeDate)
                .Include(dc => dc.Substance)
                .OrderByDescending(dc => dc.MeasurementDate)
                .FirstOrDefaultAsync();
        }
    }
}