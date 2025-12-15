using System.Windows;
using PDSCalculatorDesktop.ViewModels;

namespace PDSCalculatorDesktop.Views
{
    public partial class PDSCalculationView : Window
    {
        public PDSCalculationView(PDSCalculationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}