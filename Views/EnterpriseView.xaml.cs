using System.Windows;
using PDSCalculatorDesktop.ViewModels;

namespace PDSCalculatorDesktop.Views
{
    public partial class EnterpriseView : Window
    {
        public EnterpriseView(EnterpriseViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}