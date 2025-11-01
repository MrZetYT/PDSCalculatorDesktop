using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IMeasurementRepository _measurementRepository;
        private readonly ISubstanceRepository _substanceRepository;
        private readonly IDischargeRepository _dischargeRepository;
        private readonly IControlPointRepository _controlPointRepository;

        public MeasurementService(IMeasurementRepository measurementRepository,
            ISubstanceRepository substanceRepository,
            IDischargeRepository dischargeRepository,
            IControlPointRepository controlPointRepository)
        {
            _measurementRepository = measurementRepository;
            _substanceRepository = substanceRepository;
            _dischargeRepository = dischargeRepository;
            _controlPointRepository = controlPointRepository;
        }

        public async Task<Measurement?> GetMeasurementByIdAsync(int id)
        {
            return await _measurementRepository.GetValueAsync(id);
        }
        public async Task<IEnumerable<Measurement>> GetAllMeasurementsAsync()
        {
            return await _measurementRepository.GetAllAsync();
        }

        public async Task<Measurement> CreateMeasurementAsync(
            int substanceId,
            int? dischargeId,
            int? controlPointId,
            MeasurementType measurementType,
            double value,
            DateTime date)
        {
            var substance = await _substanceRepository.GetValueAsync(substanceId);
            if (substance == null)
            {
                throw new ArgumentException("Не существует вещества с таким ID", nameof(substanceId));
            }
            bool isControlPoint = false, isDischarge = false;
            if (dischargeId == null && controlPointId == null)
            {
                throw new ArgumentException("Ни выпуск, ни створ не были выбраны");
            }

            Discharge? discharge = null;
            if (dischargeId != null)
            {
                discharge = await _dischargeRepository.GetValueAsync(dischargeId.Value);
                if (discharge == null)
                    throw new ArgumentException("Выпуска с таким ID не существует", nameof(dischargeId));
                isDischarge = true;
            }

            ControlPoint? controlPoint = null;
            if (controlPointId != null)
            {
                controlPoint = await _controlPointRepository.GetValueAsync(controlPointId.Value);
                if (controlPoint == null)
                    throw new ArgumentException("Створа с таким ID не существует", nameof(controlPointId));
                isControlPoint = true;
            }

            if (value <= 0)
            {
                throw new ArgumentException("Значение не может быть меньше нуля", nameof(value));
            }

            if (isControlPoint)
            {
                return await _measurementRepository.CreateAsync(new Measurement()
                {
                    SubstanceId = substanceId,
                    Substance = substance,
                    ControlPoint = controlPoint,
                    ControlPointId = controlPointId,
                    MeasurementType = measurementType,
                    Value = value,
                    Date = date
                });
            }
            else
            {
                return await _measurementRepository.CreateAsync(new Measurement()
                {
                    SubstanceId = substanceId,
                    Substance = substance,
                    Discharge = discharge,
                    DischargeId = dischargeId,
                    MeasurementType = measurementType,
                    Value = value,
                    Date = date
                });
            }
        }

        public async Task<Measurement> UpdateMeasurementAsync(
            int id,
            int substanceId,
            int? dischargeId,
            int? controlPointId,
            MeasurementType measurementType,
            double value,
            DateTime date)
        {
            var measurement = await _measurementRepository.GetValueAsync(id);
            if(measurement == null)
            {
                throw new ArgumentException("Не существует измерения с таким ID", nameof(id));
            }

            var substance = await _substanceRepository.GetValueAsync(substanceId);
            if (substance == null)
            {
                throw new ArgumentException("Не существует вещества с таким ID", nameof(substanceId));
            }
            bool isControlPoint = false, isDischarge = false;
            if (dischargeId == null && controlPointId == null)
            {
                throw new ArgumentException("Ни выпуск, ни створ не были выбраны");
            }

            Discharge? discharge = null;
            if (dischargeId != null)
            {
                discharge = await _dischargeRepository.GetValueAsync(dischargeId.Value);
                if (discharge == null)
                    throw new ArgumentException("Выпуска с таким ID не существует", nameof(dischargeId));
                isDischarge = true;
            }

            ControlPoint? controlPoint = null;
            if (controlPointId != null)
            {
                controlPoint = await _controlPointRepository.GetValueAsync(controlPointId.Value);
                if (controlPoint == null)
                    throw new ArgumentException("Створа с таким ID не существует", nameof(controlPointId));
                isControlPoint = true;
            }

            if (value <= 0)
            {
                throw new ArgumentException("Значение не может быть меньше нуля", nameof(value));
            }

            if (isDischarge)
            {
                measurement.DischargeId = dischargeId;
                measurement.Discharge = discharge;
            }
            else if (isControlPoint)
            {
                measurement.ControlPointId = controlPointId;
                measurement.ControlPoint = controlPoint;
            }
            measurement.SubstanceId = substanceId;
            measurement.Substance = substance;
            measurement.Value = value;
            measurement.Date = date;

            await _measurementRepository.SaveChangesAsync();
            return measurement;
        }

        public async Task<bool> DeleteMeasurementAsync(int id)
        {
            var measurement = await _measurementRepository.GetValueAsync(id);
            if(measurement == null)
            {
                throw new ArgumentException("Измерения с таким ID не существует", nameof(id));
            }
            return await _measurementRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Measurement>> GetAllMeasurementsWithRelatedDataAsync()
        {
            return await _measurementRepository.GetAllWithRelatedDataAsync();
        }
    }
}
