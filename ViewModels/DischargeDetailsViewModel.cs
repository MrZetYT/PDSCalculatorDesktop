using System.Collections.ObjectModel;
using System.Windows.Input;
using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;
using PDSCalculatorDesktop.Services;
using PDSCalculatorDesktop.Views;

namespace PDSCalculatorDesktop.ViewModels
{
    public class DischargeDetailsViewModel : ViewModelBase
    {
        private readonly IDischargeService _dischargeService;
        private readonly IBackgroundConcentrationService _backgroundConcentrationService;
        private readonly IDischargeConcentrationService _dischargeConcentrationService;
        private readonly ISubstanceService _substanceService;
        private readonly ITechnicalParametersRepository _technicalParametersRepository;
        private readonly Discharge _discharge;

        private string _dischargeName = string.Empty;
        private string _dischargeCode = string.Empty;
        private string _enterpriseName = string.Empty;

        public string DischargeName
        {
            get => _dischargeName;
            set => SetProperty(ref _dischargeName, value);
        }

        public string DischargeCode
        {
            get => _dischargeCode;
            set => SetProperty(ref _dischargeCode, value);
        }

        public string EnterpriseName
        {
            get => _enterpriseName;
            set => SetProperty(ref _enterpriseName, value);
        }

        public ObservableCollection<TechnicalParameters> TechnicalParameters { get; } = new();
        public ObservableCollection<DischargeConcentration> DischargeConcentrations { get; } = new();
        public ObservableCollection<BackgroundConcentration> BackgroundConcentrations { get; } = new();

        public ICommand AddTechnicalParametersCommand { get; }
        public ICommand AddDischargeConcentrationCommand { get; }
        public ICommand AddBackgroundConcentrationCommand { get; }

        public DischargeDetailsViewModel(
            IDischargeService dischargeService,
            IBackgroundConcentrationService backgroundConcentrationService,
            IDischargeConcentrationService dischargeConcentrationService,
            ISubstanceService substanceService,
            ITechnicalParametersRepository technicalParametersRepository,
            Discharge discharge)
        {
            _dischargeService = dischargeService;
            _backgroundConcentrationService = backgroundConcentrationService;
            _dischargeConcentrationService = dischargeConcentrationService;
            _substanceService = substanceService;
            _technicalParametersRepository = technicalParametersRepository;
            _discharge = discharge;

            DischargeName = discharge.Name;
            DischargeCode = discharge.Code;
            EnterpriseName = discharge.Enterprise?.Name ?? "Не указано";

            AddTechnicalParametersCommand = new RelayCommand(_ => AddTechnicalParameters());
            AddDischargeConcentrationCommand = new RelayCommand(_ => AddDischargeConcentration());
            AddBackgroundConcentrationCommand = new RelayCommand(_ => AddBackgroundConcentration());

            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            try
            {
                // Загружаем технические параметры
                var techParams = await _dischargeService.GetTechnicalParametersHistoryAsync(_discharge.Id);
                TechnicalParameters.Clear();
                foreach (var param in techParams)
                {
                    TechnicalParameters.Add(param);
                }

                // Загружаем концентрации веществ в выпуске
                var dischargeDetail = await _dischargeService.GetDischargeByIdAsync(_discharge.Id);
                if (dischargeDetail != null)
                {
                    DischargeConcentrations.Clear();
                    foreach (var conc in dischargeDetail.DischargeConcentrations)
                    {
                        DischargeConcentrations.Add(conc);
                    }

                    // Загружаем фоновые концентрации для контрольного створа
                    if (dischargeDetail.ControlPointId > 0)
                    {
                        var bgConcentrations = await _backgroundConcentrationService
                            .GetByControlPointIdAsync(dischargeDetail.ControlPointId);

                        BackgroundConcentrations.Clear();
                        foreach (var bg in bgConcentrations)
                        {
                            BackgroundConcentrations.Add(bg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void AddTechnicalParameters()
        {
            var window = new TechnicalParametersEditView();
            var viewModel = new TechnicalParametersEditViewModel(
                _technicalParametersRepository,
                _discharge.Id,
                window
            );

            window.DataContext = viewModel;

            if (window.ShowDialog() == true)
            {
                LoadDataAsync();
            }
        }

        private void AddDischargeConcentration()
        {
            var window = new ConcentrationEditView();
            var viewModel = new ConcentrationEditViewModel(
                _dischargeConcentrationService,
                _substanceService,
                _discharge.Id,
                window
            );

            window.DataContext = viewModel;

            if (window.ShowDialog() == true)
            {
                LoadDataAsync();
            }
        }

        private void AddBackgroundConcentration()
        {
            var window = new ConcentrationEditView();
            var viewModel = new ConcentrationEditViewModel(
                _backgroundConcentrationService,
                _substanceService,
                _discharge.ControlPointId,
                window
            );

            window.DataContext = viewModel;

            if (window.ShowDialog() == true)
            {
                LoadDataAsync();
            }
        }
    }
}