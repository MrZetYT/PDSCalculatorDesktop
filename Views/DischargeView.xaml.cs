using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PDSCalculatorDesktop.ViewModels;

namespace PDSCalculatorDesktop.Views
{
    public partial class DischargeView : Window
    {
        public DischargeView(DischargeViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        // Обработчик для снятия выделения при клике на пустое место
        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                // Проверяем, кликнули ли мы НЕ на строку
                var hitTestResult = VisualTreeHelper.HitTest(dataGrid, e.GetPosition(dataGrid));
                if (hitTestResult != null)
                {
                    var element = hitTestResult.VisualHit;

                    // Ищем родительский DataGridRow
                    while (element != null && element is not DataGridRow && element != dataGrid)
                    {
                        element = VisualTreeHelper.GetParent(element);
                    }

                    // Если не нашли DataGridRow (т.е. кликнули в пустое место)
                    if (element is not DataGridRow)
                    {
                        dataGrid.UnselectAll();
                    }
                }
            }
        }
    }
}