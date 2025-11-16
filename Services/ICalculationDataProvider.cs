using PDSCalculatorDesktop.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public interface ICalculationDataProvider
    {
        Task<CalculationInputData> GetCalculationDataAsync(
            int dischargeId,
            int substanceId,
            DateTime date);
    }
}
