using System;
using System.Windows;
using System.Windows.Input;
using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Views;
using Microsoft.Extensions.DependencyInjection;

namespace PDSCalculatorDesktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;

        public ICommand OpenEnterprisesCommand { get; }
        public ICommand OpenDischargesCommand { get; }
        public ICommand OpenControlPointsCommand { get; }
        public ICommand OpenSubstancesCommand { get; }
        public ICommand ExitCommand { get; }

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            OpenEnterprisesCommand = new RelayCommand(_ => OpenEnterprises());
            OpenDischargesCommand = new RelayCommand(_ => OpenDischarges());
            OpenControlPointsCommand = new RelayCommand(_ => OpenControlPoints());
            OpenSubstancesCommand = new RelayCommand(_ => OpenSubstances());
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

        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}