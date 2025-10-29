using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSCalculatorDesktop.Services
{
    public class ControlPointService : IControlPointService
    {
        private readonly IControlPointRepository _controlPointRepository;

        public ControlPointService(IControlPointRepository controlPointRepository)
        {
            _controlPointRepository = controlPointRepository;
        }

        public async Task<ControlPoint?> GetControlPointByIdAsync(int id)
        {
            if (id <= 0) return null;
            return await _controlPointRepository.GetValueAsync(id);
        }

        public async Task<IEnumerable<ControlPoint>> GetAllControlPointsAsync()
        {
            return await _controlPointRepository.GetAllAsync();
        }

        public async Task<ControlPoint> CreateControlPointAsync(string number, string name, double distance)
        {
            if (string.IsNullOrEmpty(number))
            {
                throw new ArgumentException("Не введен номер створа.", nameof(number));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Не введено название створа", nameof(name));
            }
            if (distance <= 0)
            {
                throw new ArgumentException("Расстояние должно быть больше нуля", nameof(distance));
            }
            if (await _controlPointRepository.GetByNumberAsync(number) != null)
            {
                throw new ArgumentException("Контрольный створ с таким номером уже существует");
            }

            return await _controlPointRepository.CreateAsync(new ControlPoint() { Number = number, Name = name, Distance = distance });
        }

        public async Task<ControlPoint> UpdateControlPointAsync(int id, string number, string name, double distance)
        {
            var controlPoint = await _controlPointRepository.GetValueAsync(id);

            if (controlPoint == null)
            {
                throw new ArgumentException("Створа с таким Id не существует", nameof(id));
            }

            if (string.IsNullOrEmpty(number))
            {
                throw new ArgumentException("Не введен номер створа.", nameof(number));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Не введено название створа", nameof(name));
            }
            if (distance <= 0)
            {
                throw new ArgumentException("Расстояние должно быть больше нуля", nameof(distance));
            }

            var existingControlPoint = await _controlPointRepository.GetByNumberAsync(number);
            if (existingControlPoint != null && existingControlPoint.Id != id)
            {
                throw new ArgumentException("Этот номер створа занят", nameof(number));
            }


            controlPoint.Name = name;
            controlPoint.Number = number;
            controlPoint.Distance = distance;

            await _controlPointRepository.SaveChangesAsync();

            return controlPoint;
        }

        public async Task<bool> DeleteControlPointAsync(int id)
        {
            var controlPoint = await _controlPointRepository.GetValueAsync(id);
            if (controlPoint == null)
            {
                throw new ArgumentException("Створа с таким Id не существует", nameof(id));
            }

            if (await _controlPointRepository.HasDischargesAsync(id))
            {
                throw new InvalidOperationException("Невозможно удалить створ. У него есть свои выпуски");
            }

            return await _controlPointRepository.DeleteAsync(id);
        }
    }
}
