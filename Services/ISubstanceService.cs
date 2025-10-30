using PDSCalculatorDesktop.Models;

namespace PDSCalculatorDesktop.Services
{
    public interface ISubstanceService
    {
        Task<Substance?> GetSubstanceByIdAsync(int id);
        Task<IEnumerable<Substance>> GetAllSubstancesAsync();
        Task<Substance> CreateSubstanceAsync(string code, string name, string groupLFV, HazardClass hazardClass);
        Task<Substance> UpdateSubstanceAsync(int id, string code, string name, string groupLFV, HazardClass hazardClass);
        Task<bool> DeleteSubstanceAsync(int id);
    }
}