using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Repositories.Interfaces
{
    public interface ITechnicalParametersRepository : IRepository<TechnicalParameters>
    {
        Task<TechnicalParameters?> GetActualAsync(int dischargeId, DateTime date);
        Task UpdateTechnicalParametersAsync(
            int dischargeId,
            DateTime validFrom,
            double diameter,
            double flowRate,
            double waterFlowVelocity,
            double dischargeAngle,
            double distanceToWaterSurface,
            double distanceToShore,
            double distanceToControlPoint);
    }
}