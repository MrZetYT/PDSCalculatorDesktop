using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PDSCalculatorDesktop.Data;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;

namespace PDSCalculatorDesktop.Repositories
{
    public class DischargeRepository : Repository<Discharge>, IDischargeRepository
    {
        public DischargeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Discharge>> GetByEnterpriseAndDateAsync(int enterpriseId, DateTime dateTime)
        {
            return await _context.Set<Discharge>()
                .Where(d => d.EnterpriseId == enterpriseId && d.RegistrationDate <= dateTime)
                .Include(d => d.Enterprise)
                .Include(d => d.ControlPoint)
                .ThenInclude(cp => cp.WaterUseType)
                .Include(d => d.TechnicalParameters)
                .Include(d => d.DischargeConcentrations)
                .ThenInclude(dc => dc.Substance)
                .ToListAsync();
        }

        public async Task<IEnumerable<TechnicalParameters>> GetTechnicalParametersHistoryAsync(int dischargeId)
        {
            return await _context.TechnicalParameters
                .Where(tp => tp.DischargeId == dischargeId)
                .Include(tp => tp.Discharge)
                .OrderByDescending(tp => tp.ValidFrom)
                .ToListAsync();
        }

        public async Task<TechnicalParameters?> GetCurrentTechnicalParametersAsync(int dischargeId)
        {
            return await _context.TechnicalParameters
                .Where(tp => tp.DischargeId == dischargeId)
                .OrderByDescending(tp => tp.ValidFrom)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Discharge>> GetAllWithRelatedDataAsync()
        {
            return await _context.Set<Discharge>()
                .Include(d => d.Enterprise)
                .Include(d => d.ControlPoint)
                .ThenInclude(cp => cp.WaterUseType)
                .Include(d => d.TechnicalParameters)
                .ToListAsync();
        }

        public async Task<Discharge?> GetByIdWithRelatedDataAsync(int id)
        {
            return await _context.Set<Discharge>()
                .Include(d => d.Enterprise)
                .Include(d => d.ControlPoint)
                .ThenInclude(cp => cp.WaterUseType)
                .Include(d => d.TechnicalParameters)
                .Include(d => d.DischargeConcentrations)
                .ThenInclude(dc => dc.Substance)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
