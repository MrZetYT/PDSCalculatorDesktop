using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PDSCalculatorDesktop.Data;
using PDSCalculatorDesktop.Models;

namespace PDSCalculatorDesktop.Repositories
{
    public class DischargeRepository : Repository<Discharge>
    {
        public DischargeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Discharge>> GetByEnterpriseAndDateAsync(int id, DateTime dateTime)
        {
            return await _context.Set<Discharge>()
                .Where(n => n.EnterpriseId == id && n.RegistrationAt <= dateTime)
                .Include(n => n.ControlPoint)
                .Include(n => n.Enterprise)
                .Include(n => n.TechnicalParameters)
                .ToListAsync();
        }

        public async Task<IEnumerable<Measurement>> GetSubstancesWithConcentrationsAsync(int dischargeId)
        {
            return await _context.Measurements
                .Where(m => m.DischargeId == dischargeId
                         && m.MeasurementType == MeasurementType.SubstanceConcentration)
                .Include(m => m.Substance)
                .OrderBy(m => m.Substance.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Measurement>> GetNoramtiveIndicatorsByDischargeAsync(int dischargeId)
        {
            var discharge = await _context.Discharges
                .Include(n => n.ControlPoint)
                .FirstOrDefaultAsync(n => n.Id == dischargeId);

            if(discharge?.ControlPoint == null) 
                return Enumerable.Empty<Measurement>();

            return await _context.Measurements
                .Where(n => n.ControlPointId == discharge.ControlPointId
                        && (n.MeasurementType == MeasurementType.PDK
                            || n.MeasurementType == MeasurementType.BackgroundConcentration
                            || n.MeasurementType == MeasurementType.KNK))
                .Include(n => n.Substance)
                .OrderBy(n => n.Substance.Name)
                .ThenBy(n => n.MeasurementType)
                .ToListAsync();
        }
    }
}
