using System;
using System.Windows;
using System.Windows.Input;
using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Views;
using Microsoft.Extensions.DependencyInjection;
using PDSCalculatorDesktop.Data;
using Microsoft.EntityFrameworkCore;

namespace PDSCalculatorDesktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ApplicationDbContext _dbContext;

        public ICommand OpenEnterprisesCommand { get; }
        public ICommand OpenDischargesCommand { get; }
        public ICommand OpenControlPointsCommand { get; }
        public ICommand OpenSubstancesCommand { get; }
        public ICommand OpenPDSCalculationCommand { get; }
        public ICommand ValidateDataIntegrityCommand { get; }
        public ICommand ExitCommand { get; }

        public MainViewModel(IServiceProvider serviceProvider, ApplicationDbContext dbContext)
        {
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;

            OpenEnterprisesCommand = new RelayCommand(_ => OpenEnterprises());
            OpenDischargesCommand = new RelayCommand(_ => OpenDischarges());
            OpenControlPointsCommand = new RelayCommand(_ => OpenControlPoints());
            OpenSubstancesCommand = new RelayCommand(_ => OpenSubstances());
            OpenPDSCalculationCommand = new RelayCommand(_ => OpenPDSCalculation());
            ValidateDataIntegrityCommand = new RelayCommand(_ => ValidateDataIntegrityAsync());
            ExitCommand = new RelayCommand(_ => Exit());
        }

        private void OpenEnterprises()
        {
            var view = _serviceProvider.GetRequiredService<EnterpriseView>();
            view.ShowDialog();
        }

        private void OpenDischarges()
        {
            var view = _serviceProvider.GetRequiredService<DischargeView>();
            view.ShowDialog();
        }

        private void OpenControlPoints()
        {
            var view = _serviceProvider.GetRequiredService<ControlPointView>();
            view.ShowDialog();
        }

        private void OpenSubstances()
        {
            var view = _serviceProvider.GetRequiredService<SubstanceView>();
            view.ShowDialog();
        }

        private void OpenPDSCalculation()
        {
            var view = _serviceProvider.GetRequiredService<PDSCalculationView>();
            view.ShowDialog();
        }

        private async void ValidateDataIntegrityAsync()
        {
            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync("CALL validate_data_integrity()");

                MessageBox.Show(
                    "Валидация целостности данных завершена успешно.",
                    "Валидация данных",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при валидации данных:\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}