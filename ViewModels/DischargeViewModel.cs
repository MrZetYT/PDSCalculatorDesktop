using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Services;
using PDSCalculatorDesktop.Views;

namespace PDSCalculatorDesktop.ViewModels
{
    public class DischargeViewModel : ViewModelBase
    {
        private readonly IDischargeService _dischargeService;
        private readonly IEnterpriseService _enterpriseService;
        private readonly IControlPointService _controlPointService;
        private Discharge? _selectedDischarge;
        private string _searchText = string.Empty;
        private bool _isLoading = false;

        public ObservableCollection<Discharge> Discharges { get; }

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

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    LoadDischargesAsync();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        public DischargeViewModel(
            IDischargeService dischargeService,
            IEnterpriseService enterpriseService,
            IControlPointService controlPointService)
        {
            _dischargeService = dischargeService;
            _enterpriseService = enterpriseService;
            _controlPointService = controlPointService;
            Discharges = new ObservableCollection<Discharge>();

            AddCommand = new RelayCommand(_ => AddDischarge());

            EditCommand = new RelayCommand(
                execute: _ => EditDischarge(),
                canExecute: _ => SelectedDischarge != null
            );

            DeleteCommand = new RelayCommand(
                execute: _ => DeleteDischargeAsync(),
                canExecute: _ => SelectedDischarge != null
            );

            RefreshCommand = new RelayCommand(_ => LoadDischargesAsync());

            LoadDischargesAsync();
        }

        private async void LoadDischargesAsync()
        {
            IsLoading = true;
            try
            {
                var discharges = await _dischargeService.GetAllDischargesAsync();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    discharges = discharges.Where(d =>
                        d.Code.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        d.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        (d.Enterprise?.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (d.ControlPoint?.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
                    );
                }

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
            finally
            {
                IsLoading = false;
            }
        }

        private void AddDischarge()
        {
            var window = new DischargeEditView();
            var viewModel = new DischargeEditViewModel(
                _dischargeService,
                _enterpriseService,
                _controlPointService,
                window,
                null
            );

            window.DataContext = viewModel;

            if (window.ShowDialog() == true)
            {
                LoadDischargesAsync();
            }
        }

        private void EditDischarge()
        {
            if (SelectedDischarge == null) return;

            var window = new DischargeEditView();
            var viewModel = new DischargeEditViewModel(
                _dischargeService,
                _enterpriseService,
                _controlPointService,
                window,
                SelectedDischarge
            );

            window.DataContext = viewModel;

            if (window.ShowDialog() == true)
            {
                LoadDischargesAsync();
            }
        }

        private async void DeleteDischargeAsync()
        {
            if (SelectedDischarge == null) return;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить выпуск '{SelectedDischarge.Name}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result != MessageBoxResult.Yes) return;

            try
            {
                await _dischargeService.DeleteDischargeAsync(SelectedDischarge.Id);
                Discharges.Remove(SelectedDischarge);

                MessageBox.Show("Выпуск сточных вод успешно удален", "Успех",
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