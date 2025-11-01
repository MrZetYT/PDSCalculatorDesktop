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
     public class MeasurementRepository : Repository<Measurement>, IMeasurementRepository
    {
        public MeasurementRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Measurement>> GetByDischargeIdAsync(int dischargeId)
        {
            return await _context.Set<Measurement>()
                .Where(n=> n.DischargeId == dischargeId)
                .Include(n=>n.Substance)
                .Include(n=>n.Discharge)
                .OrderByDescending(n=>n.Date)
                .ToListAsync();
        }
        public async Task<IEnumerable<Measurement>> GetByControlPointIdAsync(int controlPointId)
        {
            return await _context.Set<Measurement>()
                .Where(n=>n.ControlPointId == controlPointId)
                .Include(n => n.Substance)
                .Include(n => n.ControlPoint)
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }
        public async Task<IEnumerable<Measurement>> GetBySubstanceIdAsync(int substanceId)
        {
            return await _context.Set<Measurement>()
                .Where(n => n.SubstanceId == substanceId)
                .Include(n => n.Substance)
                .Include(n => n.Discharge)
                .Include(n=> n.ControlPoint)
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }
        public async Task<IEnumerable<Measurement>> GetByTypeAsync(MeasurementType type)
        {
            return await _context.Set<Measurement>()
                .Where(n=>n.MeasurementType == type)
                .Include(n => n.Substance)
                .Include(n => n.Discharge)
                .Include(n=>n.ControlPoint)
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Measurement>> GetByDischargeAndTypeAsync(int dischargeId, MeasurementType type)
        {
            return await _context.Set<Measurement>()
                .Where(n => n.DischargeId == dischargeId && n.MeasurementType == type)
                .Include(n => n.Substance)
                .Include(n => n.Discharge)
                .Include(n=>n.ControlPoint)
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Measurement>> GetAllWithRelatedDataAsync()
        {
            return await _context.Set<Measurement>().Include(n => n.Substance).Include(n => n.ControlPoint).Include(n => n.Discharge).ToListAsync();
        }
    }
}
