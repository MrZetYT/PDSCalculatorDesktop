using PDSCalculatorDesktop.Models;

namespace PDSCalculatorDesktop.Services
{
    public interface ISubstanceService
    {
        Task<Substance?> GetSubstanceByIdAsync(int id);
        Task<IEnumerable<Substance>> GetAllSubstancesAsync();
        Task<IEnumerable<Substance>> GetAllWithCharacteristicsAsync();
        Task<Substance> CreateSubstanceAsync(string code, string name, double knk);
        Task<Substance> UpdateSubstanceAsync(int id, string code, string name, double knk);
        Task<bool> DeleteSubstanceAsync(int id);
    }
}