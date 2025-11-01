﻿using PDSCalculatorDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Repositories.Interfaces
{
    public interface IMeasurementRepository : IRepository<Measurement>
    {
        Task<IEnumerable<Measurement>> GetByDischargeIdAsync(int dischargeId);
        Task<IEnumerable<Measurement>> GetByControlPointIdAsync(int controlPointId);
        Task<IEnumerable<Measurement>> GetBySubstanceIdAsync(int substanceId);
        Task<IEnumerable<Measurement>> GetByTypeAsync(MeasurementType type);

        Task<IEnumerable<Measurement>> GetByDischargeAndTypeAsync(int dischargeId, MeasurementType type);
        Task<IEnumerable<Measurement>> GetAllWithRelatedDataAsync();
    }
}
