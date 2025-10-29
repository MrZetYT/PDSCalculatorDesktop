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
        public ICommand ExitCommand { get; }

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            OpenEnterprisesCommand = new RelayCommand(_ => OpenEnterprises());
            OpenDischargesCommand = new RelayCommand(_ => OpenDischarges());
            OpenControlPointsCommand = new RelayCommand(_ => OpenControlPoints());
            ExitCommand = new RelayCommand(_ => Exit());
        }

        private void OpenEnterprises()
        {
            var view = _serviceProvider.GetRequiredService<EnterpriseView>();
            view.Show();
        }

        private void OpenDischarges()
        {
            var view = _serviceProvider.GetRequiredService<DischargeView>();
            view.Show();
        }

        private void OpenControlPoints()
        {
            // TODO: Создадим позже
            MessageBox.Show("Окно управления контрольными створами еще не создано",
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}