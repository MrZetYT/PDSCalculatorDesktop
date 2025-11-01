using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public interface IMeasurementService
    {
        Task<Measurement?> GetMeasurementByIdAsync(int id);
        Task<IEnumerable<Measurement>> GetAllMeasurementsAsync();

        Task<Measurement> CreateMeasurementAsync(
            int substanceId,
            int? dischargeId,
            int? controlPointId,
            MeasurementType measurementType,
            double value,
            DateTime date);

        Task<Measurement> UpdateMeasurementAsync(
            int id,
            int substanceId,
            int? dischargeId,
            int? controlPointId,
            MeasurementType measurementType,
            double value,
            DateTime date);

        Task<bool> DeleteMeasurementAsync(int id);

        Task<IEnumerable<Measurement>> GetAllMeasurementsWithRelatedDataAsync();
    }
}
