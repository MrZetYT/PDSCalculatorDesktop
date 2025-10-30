using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace PDSCalculatorDesktop.ViewModels
{
    public class ControlPointEditViewModel : ViewModelBase
    {
        private readonly IControlPointService _controlPointService;
        private readonly ControlPoint? _originalControlPoint;
        private readonly Window _window;

        private string _number = string.Empty;
        private string _name = string.Empty;
        private double _distance = 0;

        public string Number
        {
            get => _number;
            set
            {
                if (SetProperty(ref _number, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public double Distance
        {
            get => _distance;
            set
            {
                if(SetProperty(ref _distance, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string WindowTitle => _originalControlPoint == null
            ? "Добавление створа"
            : "Редактирование створа";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ControlPointEditViewModel(
            IControlPointService controlPointService,
            Window window,
            ControlPoint? controlPoint = null)
        {
            _controlPointService = controlPointService;
            _window = window;
            _originalControlPoint = controlPoint;

            if (_originalControlPoint != null)
            {
                Number = _originalControlPoint.Number;
                Name = _originalControlPoint.Name;
                Distance = _originalControlPoint.Distance;
            }

            SaveCommand = new RelayCommand(
                execute: _ => SaveAsync(),
                canExecute: _ => CanSave()
            );

            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Number)
                && !string.IsNullOrWhiteSpace(Name)
                && Distance > 0;
        }

        private async void SaveAsync()
        {
            try
            {
                if (_originalControlPoint == null)
                {
                    await _controlPointService.CreateControlPointAsync(Number.Trim(), Name.Trim(), Distance);

                    MessageBox.Show(
                        "Створ успешно добавлен!",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                else
                {
                    await _controlPointService.UpdateControlPointAsync(
                        _originalControlPoint.Id,
                        Number.Trim(),
                        Name.Trim(),
                        Distance
                    );

                    MessageBox.Show(
                        "Створ успешно обновлен!",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }

                _window.DialogResult = true;
                _window.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка валидации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Произошла ошибка: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
        private void Cancel()
        {
            _window.DialogResult = false;
            _window.Close();
        }
    }
}
