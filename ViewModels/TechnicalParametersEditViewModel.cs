using System;
using System.Windows;
using System.Windows.Input;
using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Repositories.Interfaces;

namespace PDSCalculatorDesktop.ViewModels
{
    public class TechnicalParametersEditViewModel : ViewModelBase
    {
        private readonly ITechnicalParametersRepository _repository;
        private readonly int _dischargeId;
        private readonly Window _window;

        private DateTime _validFrom = DateTime.Today;
        private double _diameter;
        private double _flowRate;
        private double _waterFlowVelocity;
        private double _dischargeAngle;
        private double _distanceToWaterSurface;
        private double _distanceToShore;
        private double _distanceToControlPoint;
        private bool _isSaving;

        public DateTime ValidFrom
        {
            get => _validFrom;
            set => SetProperty(ref _validFrom, value);
        }

        public double Diameter
        {
            get => _diameter;
            set => SetProperty(ref _diameter, value);
        }

        public double FlowRate
        {
            get => _flowRate;
            set => SetProperty(ref _flowRate, value);
        }

        public double WaterFlowVelocity
        {
            get => _waterFlowVelocity;
            set => SetProperty(ref _waterFlowVelocity, value);
        }

        public double DischargeAngle
        {
            get => _dischargeAngle;
            set => SetProperty(ref _dischargeAngle, value);
        }

        public double DistanceToWaterSurface
        {
            get => _distanceToWaterSurface;
            set => SetProperty(ref _distanceToWaterSurface, value);
        }

        public double DistanceToShore
        {
            get => _distanceToShore;
            set => SetProperty(ref _distanceToShore, value);
        }

        public double DistanceToControlPoint
        {
            get => _distanceToControlPoint;
            set => SetProperty(ref _distanceToControlPoint, value);
        }

        public bool IsSaving
        {
            get => _isSaving;
            set => SetProperty(ref _isSaving, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public TechnicalParametersEditViewModel(
            ITechnicalParametersRepository repository,
            int dischargeId,
            Window window)
        {
            _repository = repository;
            _dischargeId = dischargeId;
            _window = window;

            SaveCommand = new RelayCommand(
                execute: _ => SaveAsync(),
                canExecute: _ => CanSave()
            );

            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private bool CanSave()
        {
            return !IsSaving
                && Diameter > 0
                && FlowRate > 0
                && WaterFlowVelocity > 0
                && DischargeAngle >= 0 && DischargeAngle <= 360
                && DistanceToWaterSurface > 0
                && DistanceToShore > 0
                && DistanceToControlPoint > 0;
        }

        private async void SaveAsync()
        {
            IsSaving = true;
            try
            {
                await _repository.UpdateTechnicalParametersAsync(
                    _dischargeId,
                    ValidFrom,
                    Diameter,
                    FlowRate,
                    WaterFlowVelocity,
                    DischargeAngle,
                    DistanceToWaterSurface,
                    DistanceToShore,
                    DistanceToControlPoint
                );

                MessageBox.Show(
                    "Технические параметры успешно добавлены!",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                _window.DialogResult = true;
                _window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            finally
            {
                IsSaving = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void Cancel()
        {
            _window.DialogResult = false;
            _window.Close();
        }
    }
}