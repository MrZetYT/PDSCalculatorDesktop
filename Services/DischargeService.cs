using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories;
using PDSCalculatorDesktop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public class DischargeService : IDischargeService
    {
        private readonly IDischargeRepository _dischargeRepository;
        private readonly IEnterpriseRepository _enterpriseRepository;
        private readonly IControlPointRepository _controlPointRepository;
        private readonly ITechnicalParametersRepository _technicalParametersRepository;

        public DischargeService(IDischargeRepository dischargeRepository, IEnterpriseRepository enterpriseRepository,
                                IControlPointRepository controlPointRepository, ITechnicalParametersRepository technicalParametersRepository)
        {
            _dischargeRepository = dischargeRepository;
            _enterpriseRepository = enterpriseRepository;
            _controlPointRepository = controlPointRepository;
            _technicalParametersRepository = technicalParametersRepository;
        }

        public async Task<Discharge?> GetDischargeByIdAsync(int id)
        {
            if (id <= 0) return null;
            return await _dischargeRepository.GetValueAsync(id);
        }

        public async Task<IEnumerable<Discharge>> GetAllDischargesAsync()
        {
            return await _dischargeRepository.GetAllAsync();
        }

        public async Task<Discharge> CreateDischargeAsync(string code, string name, DateTime registrationAt,
            int enterpriseId, int controlPointId)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Не введен код выпуска", nameof(code));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Не введено название выпуска", nameof(name));
            }

            var enterprise = await _enterpriseRepository.GetValueAsync(enterpriseId);

            if (enterprise == null)
            {
                throw new ArgumentException("Предприятия с таким ID не найдено");
            }

            var controlPoint = await _controlPointRepository.GetValueAsync(controlPointId);

            if (controlPoint == null)
            {
                throw new ArgumentException("Контрольного створа с таким ID не найдено");
            }

            return await _dischargeRepository.CreateAsync(new Discharge() {
                Code = code, Name = name, RegistrationAt = registrationAt,
                EnterpriseId = enterpriseId, ControlPointId = controlPointId,
                Enterprise = enterprise!,
                ControlPoint = controlPoint!});
        }

        public async Task<Discharge> UpdateDischargeAsync(int id, string code, string name, DateTime registrationAt,
            int enterpriseId, int controlPointId)
        {
            var discharge = await _dischargeRepository.GetValueAsync(id);
            if (discharge == null)
            {
                throw new ArgumentException("Выпуска с таким Id не существует", nameof (id));
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Не введен код выпуска", nameof(code));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Не введено название выпуска", nameof(name));
            }

            var enterprise = await _enterpriseRepository.GetValueAsync(enterpriseId);

            if (enterprise == null)
            {
                throw new ArgumentException("Предприятия с таким ID не найдено");
            }

            var controlPoint = await _controlPointRepository.GetValueAsync(controlPointId);

            if (controlPoint == null)
            {
                throw new ArgumentException("Контрольного створа с таким ID не найдено");
            }

            discharge.Name = name;
            discharge.Code = code;
            discharge.RegistrationAt = registrationAt;
            discharge.Enterprise = enterprise;
            discharge.EnterpriseId = enterpriseId;
            discharge.ControlPoint = controlPoint;
            discharge.ControlPointId = controlPointId;

            await _dischargeRepository.SaveChangesAsync();

            return discharge;
        }

        public async Task<bool> DeleteDischargeAsync(int id)
        {
            var discharge = await _dischargeRepository.GetValueAsync(id);
            if (discharge == null)
            {
                throw new ArgumentException("Выпуска с таким Id не существует", nameof(id));
            }

            if (await _dischargeRepository.HasTechnicalParametersAsync(id))
            {
                throw new InvalidOperationException("Невозможно удалить выпуск. У него имеются технические параметры");
            }

            return await _dischargeRepository.DeleteAsync(id);
        }

        public async Task<TechnicalParameters> AddTechnicalParametersAsync(int dischargeId, DateTime validFrom, double diameter,
                                                              double flowRate, double waterFlowVelocity, double dischargeAngle,
                                                              double distanceToWaterSurface, double distanceToShore, double distanceToControlPoint)
        {
            var existingDischarge = await _dischargeRepository.GetValueAsync(dischargeId);
            if(existingDischarge == null)
                throw new ArgumentException("Выпуска с таким Id не существует", nameof(dischargeId));

            if (diameter <= 0)
                throw new ArgumentException("Диаметр должен быть больше нуля", nameof(diameter));

            if (flowRate <= 0)
                throw new ArgumentException("Скорость потока должна быть больше нуля", nameof(flowRate));

            if (waterFlowVelocity <= 0)
                throw new ArgumentException("Скорость потока воды должна быть больше нуля", nameof(waterFlowVelocity));

            if (dischargeAngle <= 0)
                throw new ArgumentException("Угол разгрузки должен быть больше нуля", nameof(dischargeAngle));

            if (distanceToWaterSurface <= 0)
                throw new ArgumentException("Расстояние до поверхности воды должно быть больше нуля", nameof(distanceToWaterSurface));

            if (distanceToShore <= 0)
                throw new ArgumentException("Расстояние до берега должно быть больше нуля", nameof(distanceToShore));

            if (distanceToControlPoint <= 0)
                throw new ArgumentException("Расстояние до контрольного створа должно быть больше нуля", nameof(distanceToControlPoint));

            var technicalParameters = new TechnicalParameters()
            {
                Diameter = diameter,
                FlowRate = flowRate,
                WaterFlowVelocity = waterFlowVelocity,
                DischargeAngle = dischargeAngle,
                DistanceToWaterSurface = distanceToWaterSurface,
                DistanceToShore = distanceToShore,
                DistanceToControlPoint = distanceToControlPoint,
                ValidFrom = validFrom,
                Discharge = existingDischarge,
                DischargeId = dischargeId
            };

            return await _technicalParametersRepository.CreateAsync(technicalParameters);
        }

        public async Task<IEnumerable<TechnicalParameters>> GetTechnicalParametersHistoryAsync(int dischargeId)
        {
            return await _dischargeRepository.GetTechnicalParametersAsync(dischargeId);
        }
    }
}
