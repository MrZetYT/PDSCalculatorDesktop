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
    public class TechnicalParametersRepository : Repository<TechnicalParameters>, ITechnicalParametersRepository
    {
        public TechnicalParametersRepository(ApplicationDbContext context) : base(context) { }

        public async Task<TechnicalParameters?> GetActualAsync(int dischargeId, DateTime date)
        {
            var utcDate = date.ToUniversalTime();

            return await _context.TechnicalParameters
                .FromSqlInterpolated($"SELECT * FROM get_actual_technical_parameters({dischargeId}, {utcDate})")
                .FirstOrDefaultAsync();
        }

        public async Task UpdateTechnicalParametersAsync(
            int dischargeId,
            DateTime validFrom,
            double diameter,
            double flowRate,
            double waterFlowVelocity,
            double dischargeAngle,
            double distanceToWaterSurface,
            double distanceToShore,
            double distanceToControlPoint)
        {
            var utcValidFrom = validFrom.ToUniversalTime();

            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                CALL update_technical_parameters(
                    {dischargeId},
                    {utcValidFrom},
                    {diameter},
                    {flowRate},
                    {waterFlowVelocity},
                    {dischargeAngle},
                    {distanceToWaterSurface},
                    {distanceToShore},
                    {distanceToControlPoint}
                )");
        }
    }
}