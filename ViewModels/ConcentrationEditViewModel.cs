using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Services;

namespace PDSCalculatorDesktop.ViewModels
{
    public class ConcentrationEditViewModel : ViewModelBase
    {
        private readonly IDischargeConcentrationService? _dischargeConcentrationService;
        private readonly IBackgroundConcentrationService? _backgroundConcentrationService;
        private readonly ISubstanceService _substanceService;
        private readonly int _targetId; // DischargeId или ControlPointId
        private readonly bool _isDischargeConcentration;
        private readonly Window _window;

        private Substance? _selectedSubstance;
        private double _concentration;
        private DateTime _measurementDate = DateTime.Today;
        private bool _isSaving;
        private ObservableCollection<Substance> _substances = new();

        public string WindowTitle => _isDischargeConcentration
            ? "Добавление концентрации в выпуске"
            : "Добавление фоновой концентрации";

        public ObservableCollection<Substance> Substances
        {
            get => _substances;
            set => SetProperty(ref _substances, value);
        }

        public Substance? SelectedSubstance
        {
            get => _selectedSubstance;
            set
            {
                if (SetProperty(ref _selectedSubstance, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public double Concentration
        {
            get => _concentration;
            set
            {
                if (SetProperty(ref _concentration, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public DateTime MeasurementDate
        {
            get => _measurementDate;
            set => SetProperty(ref _measurementDate, value);
        }

        public bool IsSaving
        {
            get => _isSaving;
            set => SetProperty(ref _isSaving, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ConcentrationEditViewModel(
            IDischargeConcentrationService dischargeConcentrationService,
            ISubstanceService substanceService,
            int dischargeId,
            Window window)
        {
            _dischargeConcentrationService = dischargeConcentrationService;
            _substanceService = substanceService;
            _targetId = dischargeId;
            _isDischargeConcentration = true;
            _window = window;

            SaveCommand = new RelayCommand(
                execute: _ => SaveAsync(),
                canExecute: _ => CanSave()
            );

            CancelCommand = new RelayCommand(_ => Cancel());

            LoadDataAsync();
        }

        public ConcentrationEditViewModel(
            IBackgroundConcentrationService backgroundConcentrationService,
            ISubstanceService substanceService,
            int controlPointId,
            Window window)
        {
            _backgroundConcentrationService = backgroundConcentrationService;
            _substanceService = substanceService;
            _targetId = controlPointId;
            _isDischargeConcentration = false;
            _window = window;

            SaveCommand = new RelayCommand(
                execute: _ => SaveAsync(),
                canExecute: _ => CanSave()
            );

            CancelCommand = new RelayCommand(_ => Cancel());

            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            try
            {
                var substances = await _substanceService.GetAllSubstancesAsync();
                Substances.Clear();
                foreach (var substance in substances)
                {
                    Substances.Add(substance);
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
            return !IsSaving
                && SelectedSubstance != null
                && Concentration >= 0;
        }

        private async void SaveAsync()
        {
            IsSaving = true;
            try
            {
                var measurementDateUtc = DateTime.SpecifyKind(MeasurementDate, DateTimeKind.Utc);

                if (_isDischargeConcentration && _dischargeConcentrationService != null)
                {
                    await _dischargeConcentrationService.CreateAsync(
                        _targetId,
                        SelectedSubstance!.Id,
                        Concentration,
                        measurementDateUtc
                    );

                    MessageBox.Show(
                        "Концентрация в выпуске успешно добавлена!",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                else if (!_isDischargeConcentration && _backgroundConcentrationService != null)
                {
                    await _backgroundConcentrationService.CreateAsync(
                        _targetId,
                        SelectedSubstance!.Id,
                        Concentration,
                        measurementDateUtc
                    );

                    MessageBox.Show(
                        "Фоновая концентрация успешно добавлена!",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }

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