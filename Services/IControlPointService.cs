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
        Task<ControlPoint> CreateControlPointAsync(string number, string name, double distance);
        Task<ControlPoint> UpdateControlPointAsync(int id, string number, string name, double distance);
        Task<bool> DeleteControlPointAsync(int id);
    }
}
