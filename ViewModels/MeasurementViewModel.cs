using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Services;
using PDSCalculatorDesktop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace PDSCalculatorDesktop.ViewModels
{
    public class MeasurementViewModel : ViewModelBase
    {
        private readonly IMeasurementService _measurementService;
        private readonly IControlPointService _controlPointService;
        private readonly IDischargeService _dischargeService;
        private readonly ISubstanceService _substanceService;
        private Measurement? _selectedMeasurement;
        private string _searchText = string.Empty;

        public ObservableCollection<Measurement> Measurements { get; set; }

        public Measurement? SelectedMeasurement
        {
            get => _selectedMeasurement;
            set
            {
                if (SetProperty(ref _selectedMeasurement, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    LoadMeasurementsAsync();
                }
            }
        }

        public ICommand AddCommand { get; }

        public ICommand EditCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand RefreshCommand { get; }

        public MeasurementViewModel(IMeasurementService measurementService,
            IControlPointService controlPointService,
            IDischargeService dischargeService,
            ISubstanceService substanceService)
        {
            _measurementService = measurementService;
            _controlPointService = controlPointService;
            _dischargeService = dischargeService;
            _substanceService = substanceService;

            Measurements = new ObservableCollection<Measurement>();

            AddCommand = new RelayCommand(_ => AddMeasurement());

            EditCommand = new RelayCommand(
                execute: _ => EditMeasurement(),
                canExecute: _ => SelectedMeasurement != null
            );

            DeleteCommand = new RelayCommand(
                execute: _ => DeleteMeasurementAsync(),
                canExecute: _ => SelectedMeasurement != null
            );

            RefreshCommand = new RelayCommand(_ => LoadMeasurementsAsync());

            LoadMeasurementsAsync();
            _controlPointService = controlPointService;
            _dischargeService = dischargeService;
            _substanceService = substanceService;
        }

        private async void LoadMeasurementsAsync()
        {
            try
            {
                var measurements = await _measurementService.GetAllMeasurementsWithRelatedDataAsync();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    measurements = measurements.Where(e =>
                        e.MeasurementType.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        e.Value.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        e.Date.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        (e.Substance?.Name?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
                    );
                }

                Measurements.Clear();
                foreach (var measurement in measurements)
                {
                    Measurements.Add(measurement);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddMeasurement()
        {
            var window = new MeasurementEditView();

            var viewModel = new MeasurementEditViewModel(
                _measurementService,
                _substanceService,
                _controlPointService,
                _dischargeService,
                window,
                null
            );

            window.DataContext = viewModel;

            var result = window.ShowDialog();

            if (result == true)
            {
                LoadMeasurementsAsync();
            }
        }

        private void EditMeasurement()
        {
            if (SelectedMeasurement == null) return;

            var window = new MeasurementEditView();

            var viewModel = new MeasurementEditViewModel(
                _measurementService,
                _substanceService,
                _controlPointService,
                _dischargeService,
                window,
                SelectedMeasurement
            );

            window.DataContext = viewModel;

            var result = window.ShowDialog();

            if (result == true)
            {
                LoadMeasurementsAsync();
            }
        }

        private async void DeleteMeasurementAsync()
        {
            if (SelectedMeasurement == null) return;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить измерение '{SelectedMeasurement.Value}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result != MessageBoxResult.Yes) return;

            try
            {
                await _measurementService.DeleteMeasurementAsync(SelectedMeasurement.Id);

                Measurements.Remove(SelectedMeasurement);

                MessageBox.Show("Предприятие успешно удалено", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
