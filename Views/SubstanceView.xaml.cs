using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PDSCalculatorDesktop.ViewModels;

namespace PDSCalculatorDesktop.Views
{
    public partial class SubstanceView : Window
    {
        public SubstanceView(SubstanceViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                var hitTestResult = VisualTreeHelper.HitTest(dataGrid, e.GetPosition(dataGrid));
                if (hitTestResult != null)
                {
                    var element = hitTestResult.VisualHit;

                    while (element != null && element is not DataGridRow && element != dataGrid)
                    {
                        element = VisualTreeHelper.GetParent(element);
                    }

                    if (element is not DataGridRow)
                    {
                        dataGrid.UnselectAll();
                    }
                }
            }
        }
    }
}