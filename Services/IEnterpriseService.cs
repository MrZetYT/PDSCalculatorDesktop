using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public interface IEnterpriseService
    {
        Task<Enterprise?> GetEnterpriseByIdAsync(int id);
    }
}
