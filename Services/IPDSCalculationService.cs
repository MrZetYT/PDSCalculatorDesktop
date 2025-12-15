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

        Task<IEnumerable<Stage1Result>> CalculateStage1Async(
            int dischargeId,
            DateTime calculationDate);

        Task<IEnumerable<PDSCalculationResult>> CalculateFinalPDSAsync(
            int dischargeId,
            DateTime calculationDate);

        Task<IEnumerable<PDSCalculationResult>> GetCalculationHistoryAsync(int dischargeId);
    }
}