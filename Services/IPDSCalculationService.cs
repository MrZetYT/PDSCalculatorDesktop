using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public interface IPDSCalculationService
    {
        Task<double> CalculateIndividualPDSAsync(
            int dischargeId,
            int substanceId,
            DateTime caculationDate);

        Task<IEnumerable<PDSCalculationResult>> CalculateFinalPDSAsync(int dischargeId, DateTime caculationDate);

        Task<PDSCalculationResult> SaveCalculationResultAsync();

        Task<IEnumerable<PDSCalculationResult>> GetCalculationHistoryAsync(int dischargeId);
    }
}
