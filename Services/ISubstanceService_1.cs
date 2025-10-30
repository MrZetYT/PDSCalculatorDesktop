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
        Task<IEnumerable<Enterprise>> GetAllEnterprisesAsync();
        Task<Enterprise> CreateEnterpriseAsync(string code, string name);
        Task<Enterprise> UpdateEnterpriseAsync(int id, string code, string name);
        Task<bool> DeleteEnterpriseAsync(int id);
    }
}