using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using PDSCalculatorDesktop.ViewModels;

namespace PDSCalculatorDesktop
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is MaterialDesignThemes.Wpf.Card card &&
                card.Tag is string commandName &&
                DataContext is MainViewModel viewModel)
            {
                var command = commandName switch
                {
                    "OpenEnterprisesCommand" => viewModel.OpenEnterprisesCommand,
                    "OpenDischargesCommand" => viewModel.OpenDischargesCommand,
                    "OpenControlPointsCommand" => viewModel.OpenControlPointsCommand,
                    "OpenSubstancesCommand" => viewModel.OpenSubstancesCommand,
                    "OpenMeasurementsCommand" => viewModel.OpenMeasurementsCommand,
                    _ => null
                };

                if (command?.CanExecute(null) == true)
                {
                    command.Execute(null);
                }
            }
        }
    }
}