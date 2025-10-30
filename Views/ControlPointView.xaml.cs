using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PDSCalculatorDesktop.ViewModels;

namespace PDSCalculatorDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для ControlPointView.xaml
    /// </summary>
    public partial class ControlPointView : Window
    {
        public ControlPointView(ControlPointViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
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
