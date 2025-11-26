using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public interface IControlPointService
    {
        Task<ControlPoint?> GetControlPointByIdAsync(int id);
        Task<IEnumerable<ControlPoint>> GetAllControlPointsAsync();
        Task<IEnumerable<ControlPoint>> GetAllWithWaterUseTypeAsync();
        Task<ControlPoint> CreateControlPointAsync(string number, string name, int waterUseTypeId);
        Task<ControlPoint> UpdateControlPointAsync(int id, string number, string name, int waterUseTypeId);
        Task<bool> DeleteControlPointAsync(int id);
    }
}
