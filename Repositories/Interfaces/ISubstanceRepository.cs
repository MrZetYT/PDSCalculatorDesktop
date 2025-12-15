using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Repositories.Interfaces
{
    public interface ISubstanceRepository : IRepository<Substance>
    {
        Task<IEnumerable<Substance>> GetAllWithCharacteristicsAsync();
        Task<Substance?> GetByIdWithCharacteristicsAsync(int id);
        Task<CanDeleteSubstanceResult> CanDeleteSubstanceAsync(int id);
    }

    public class CanDeleteSubstanceResult
    {
        public bool CanDelete { get; set; }
        public string Reason { get; set; } = string.Empty;
        public long RelatedDischarges { get; set; }
        public long RelatedBackgroundConc { get; set; }
    }
}
