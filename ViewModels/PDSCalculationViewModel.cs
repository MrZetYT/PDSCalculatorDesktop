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
    public class PDSCalculationViewModel : ViewModelBase
    {
        private readonly IDischargeService _dischargeService;
        private readonly IPDSCalculationService _calculationService;

        private Discharge? _selectedDischarge;
        private DateTime _calculationDate = DateTime.Today;
        private bool _isCalculating = false;
        private ObservableCollection<PDSStage1ResultItem> _stage1Results = new();
        private ObservableCollection<PDSStage2ResultItem> _stage2Results = new();

        public ObservableCollection<Discharge> Discharges { get; } = new();

        public ObservableCollection<PDSStage1ResultItem> Stage1Results
        {
            get => _stage1Results;
            set => SetProperty(ref _stage1Results, value);
        }

        public ObservableCollection<PDSStage2ResultItem> Stage2Results
        {
            get => _stage2Results;
            set => SetProperty(ref _stage2Results, value);
        }

        public Discharge? SelectedDischarge
        {
            get => _selectedDischarge;
            set
            {
                if (SetProperty(ref _selectedDischarge, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public DateTime CalculationDate
        {
            get => _calculationDate;
            set
            {
                if (SetProperty(ref _calculationDate, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public bool IsCalculating
        {
            get => _isCalculating;
            set => SetProperty(ref _isCalculating, value);
        }

        public ICommand CalculateCommand { get; }
        public ICommand RefreshCommand { get; }

        public PDSCalculationViewModel(
            IDischargeService dischargeService,
            IPDSCalculationService calculationService)
        {
            _dischargeService = dischargeService;
            _calculationService = calculationService;

            CalculateCommand = new RelayCommand(
                execute: _ => CalculateAsync(),
                canExecute: _ => CanCalculate()
            );

            RefreshCommand = new RelayCommand(_ => LoadDischargesAsync());

            LoadDischargesAsync();
        }

        private async void LoadDischargesAsync()
        {
            try
            {
                var discharges = await _dischargeService.GetAllWithRelatedDataAsync();
                Discharges.Clear();
                foreach (var discharge in discharges)
                {
                    Discharges.Add(discharge);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanCalculate()
        {
            return !IsCalculating && SelectedDischarge != null;
        }

        private async void CalculateAsync()
        {
            if (SelectedDischarge == null) return;

            IsCalculating = true;
            try
            {
                // ЭТАП 1: Индивидуальный ПДС (БЕЗ учета групп)
                var stage1Results = await _calculationService.CalculateStage1Async(
                    SelectedDischarge.Id,
                    CalculationDate);

                Stage1Results.Clear();
                foreach (var result in stage1Results)
                {
                    Stage1Results.Add(new PDSStage1ResultItem
                    {
                        SubstanceCode = result.SubstanceCode,
                        SubstanceName = result.SubstanceName,
                        IndividualPDS = result.IndividualPDS,
                        GroupLFV = result.GroupLFV,
                        HazardClass = result.HazardClass,
                        ActualConcentration = result.ActualConcentration,
                        BackgroundConcentration = result.BackgroundConcentration,
                        PDK = result.PDK,
                        DilutionRatio = result.DilutionRatio
                    });
                }

                // ЭТАП 2: Финальный ПДС (С учетом групп)
                var stage2Results = await _calculationService.CalculateFinalPDSAsync(
                    SelectedDischarge.Id,
                    CalculationDate);

                Stage2Results.Clear();
                foreach (var result in stage2Results)
                {
                    var notes = result.Notes ?? string.Empty;

                    Stage2Results.Add(new PDSStage2ResultItem
                    {
                        SubstanceCode = ExtractValue(notes, "Код:"),
                        SubstanceName = ExtractValue(notes, "Название:"),
                        IndividualPDS = ParseDouble(ExtractValue(notes, "Индив. ПДС:"), 0),
                        GroupLFV = ExtractValue(notes, "Группа ЛФВ:"),
                        HazardClass = ParseInt(ExtractValue(notes, "Класс опасности:"), 0),
                        GroupSum = ParseDouble(ExtractValue(notes, "Сумма группы:"), 0),
                        CorrectionFactor = ParseDouble(ExtractValue(notes, "Коэфф. коррекции:"), 0),
                        FinalPDS = result.FinalPDS,
                        MaxAllowedMassPerYear = ParseDouble(ExtractValue(notes, "Макс. масса (т/год):"), 0),
                        IsExceeded = notes.Contains("Превышение:") && !notes.Contains("Превышение: нет"),
                        ExcessPercent = ParseExcessPercent(notes)
                    });
                }

                MessageBox.Show(
                    $"Расчет выполнен успешно!\n\n" +
                    $"ЭТАП 1 - Индивидуальный ПДС:\n" +
                    $"Проанализировано веществ: {Stage1Results.Count}\n\n" +
                    $"ЭТАП 2 - Финальный ПДС:\n" +
                    $"Проанализировано веществ: {Stage2Results.Count}\n" +
                    $"Превышений ПДС: {Stage2Results.Count(r => r.IsExceeded)}",
                    "Расчет ПДС",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при расчете ПДС:\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            finally
            {
                IsCalculating = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string ExtractValue(string text, string key)
        {
            var parts = text.Split(new[] { ", " }, StringSplitOptions.None);
            var part = parts.FirstOrDefault(p => p.Trim().StartsWith(key));
            if (part != null)
            {
                var idx = part.IndexOf(':');
                if (idx >= 0)
                {
                    var value = part.Substring(idx + 1).Trim();
                    return value.Split(new[] { ' ', ',' })[0].Trim();
                }
            }
            return string.Empty;
        }

        private double ParseDouble(string value, double defaultValue)
        {
            if (double.TryParse(value, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            return defaultValue;
        }

        private int ParseInt(string value, int defaultValue)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        private double ParseExcessPercent(string text)
        {
            var match = System.Text.RegularExpressions.Regex.Match(text, @"Превышение: ([\d\.]+)%");
            if (match.Success && double.TryParse(match.Groups[1].Value,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double percent))
            {
                return percent;
            }
            return 0.0;
        }
    }

    public class PDSStage1ResultItem
    {
        public string SubstanceCode { get; set; } = string.Empty;
        public string SubstanceName { get; set; } = string.Empty;
        public double ActualConcentration { get; set; }
        public double BackgroundConcentration { get; set; }
        public double PDK { get; set; }
        public double DilutionRatio { get; set; }
        public double IndividualPDS { get; set; }
        public string GroupLFV { get; set; } = string.Empty;
        public int HazardClass { get; set; }
    }

    public class PDSStage2ResultItem
    {
        public string SubstanceCode { get; set; } = string.Empty;
        public string SubstanceName { get; set; } = string.Empty;
        public double IndividualPDS { get; set; }
        public string GroupLFV { get; set; } = string.Empty;
        public int HazardClass { get; set; }
        public double GroupSum { get; set; }
        public double CorrectionFactor { get; set; }
        public double FinalPDS { get; set; }
        public double MaxAllowedMassPerYear { get; set; }
        public bool IsExceeded { get; set; }
        public double ExcessPercent { get; set; }
    }
}