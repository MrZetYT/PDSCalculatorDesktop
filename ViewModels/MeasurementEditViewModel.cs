using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Configuration;

namespace PDSCalculatorDesktop.ViewModels
{
    public class MeasurementEditViewModel : ViewModelBase
    {
        private readonly IMeasurementService _measurementService;
        private readonly ISubstanceService _substanceService;
        private readonly IControlPointService _controlPointService;
        private readonly IDischargeService _dischargeService;
        private readonly Measurement? _originalMeasurement;
        private readonly Window _window;

        private MeasurementType _measurementType;
        private double _value;
        private int _substanceId=0;
        private DateTime _date = DateTime.MinValue;
        private int? _dischargeId=0;
        private int? _controlPointId = 0;
        private ObservableCollection<Discharge> _discharges = new();
        private ObservableCollection<ControlPoint> _controlPoints = new();
        private ObservableCollection<Substance> _substances = new();
        private Substance _selectedSubstance;
        private ControlPoint? _selectedControlPoint;
        private Discharge? _selectedDischarge;

        public MeasurementType MeasurementType
        {
            get => _measurementType;
            set => SetProperty(ref _measurementType, value);
        }

        public double Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public ObservableCollection<Discharge> Discharges
        {
            get => _discharges;
            set => SetProperty(ref _discharges, value);
        }

        public ObservableCollection<ControlPoint> ControlPoints
        {
            get => _controlPoints;
            set => SetProperty(ref _controlPoints, value);
        }

        public ObservableCollection<Substance> Substances
        {
            get => _substances;
            set => SetProperty(ref _substances, value);
        }

        public Discharge? SelectedDischarge
        {
            get => _selectedDischarge;
            set
            {
                if (SetProperty(ref _selectedDischarge, value))
                {
                    DischargeId = value?.Id ?? 0;
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public ControlPoint? SelectedControlPoint
        {
            get => _selectedControlPoint;
            set
            {
                if (SetProperty(ref _selectedControlPoint, value))
                {
                    ControlPointId = value?.Id ?? 0;
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public Substance SelectedSubstance
        {
            get => _selectedSubstance;
            set
            {
                if (SetProperty(ref _selectedSubstance, value))
                {
                    SubstanceId = value?.Id ?? 0;
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public int SubstanceId
        {
            get => _substanceId;
            set
            {
                if (SetProperty(ref _substanceId, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                if (SetProperty(ref _date, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public int? DischargeId
        {
            get => _dischargeId;
            set
            {
                if (SetProperty(ref _dischargeId, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public int? ControlPointId
        {
            get => _controlPointId;
            set
            {
                if (SetProperty(ref _controlPointId, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string WindowTitle => _originalMeasurement == null
            ? "Добавление измерения"
            : "Редактирование измерения";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public MeasurementEditViewModel(
                        IMeasurementService measurementService,
                        ISubstanceService substanceService,
                        IControlPointService controlPointService,
                        IDischargeService dischargeService,
                        Window window,
                        Measurement? measurement = null)
        {
            _measurementService = measurementService;
            _substanceService = substanceService;
            _controlPointService = controlPointService;
            _dischargeService = dischargeService;
            _window = window;
            _originalMeasurement = measurement;

            LoadDataAsync();

            if (_originalMeasurement != null)
            {
                DischargeId = _originalMeasurement.DischargeId;
                ControlPointId = _originalMeasurement.ControlPointId;
                SubstanceId = _originalMeasurement.SubstanceId;
                Date = _originalMeasurement.Date;
                MeasurementType = _originalMeasurement.MeasurementType;
                Value = _originalMeasurement.Value;
            }

            SaveCommand = new RelayCommand(
                execute: _ => SaveAsync(),
                canExecute: _ => CanSave()
            );

            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private async void LoadDataAsync()
        {
            try
            {
                var discharges = await _dischargeService.GetAllDischargesAsync();
                var substances = await _substanceService.GetAllSubstancesAsync();
                var controlPoints = await _controlPointService.GetAllControlPointsAsync();

                Discharges.Clear();
                foreach (var discharge in discharges)
                {
                    Discharges.Add(discharge);
                }

                Substances.Clear();
                foreach (var substance in substances)
                {
                    Substances.Add(substance);
                }

                ControlPoints.Clear();
                foreach (var controlPoint in controlPoints)
                {
                    ControlPoints.Add(controlPoint);
                }

                if (_originalMeasurement != null)
                {
                    SelectedDischarge = Discharges.FirstOrDefault(e => e.Id == _originalMeasurement.DischargeId);
                    SelectedSubstance = Substances.FirstOrDefault(n => n.Id == _originalMeasurement.SubstanceId);
                    SelectedControlPoint = ControlPoints.FirstOrDefault(c => c.Id == _originalMeasurement.ControlPointId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanSave()
        {
            return
                (SelectedDischarge != null ^ SelectedControlPoint != null)
                && SelectedSubstance != null
                && Value > 0
                && Date != DateTime.MinValue;
        }

        private async void SaveAsync()
        {
            try
            {
                if (_originalMeasurement == null)
                {
                    await _measurementService.CreateMeasurementAsync(
                        SubstanceId,
                        DischargeId,
                        ControlPointId,
                        MeasurementType,
                        Value,
                        DateTime.SpecifyKind(Date, DateTimeKind.Utc));

                    MessageBox.Show(
                        "Измерение успешно добавлено!",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                else
                {
                    await _measurementService.UpdateMeasurementAsync(
                        _originalMeasurement.Id,
                        SubstanceId,
                        DischargeId,
                        ControlPointId,
                        MeasurementType,
                        Value,
                        DateTime.SpecifyKind(Date, DateTimeKind.Utc));

                    MessageBox.Show(
                        "Измерение успешно обновлен!",
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
                var innerMessage = ex.InnerException != null
                    ? $"\n\nВнутренняя ошибка: {ex.InnerException.Message}"
                    : "";

                MessageBox.Show(
                    $"Произошла ошибка: {ex.Message}{innerMessage}",
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