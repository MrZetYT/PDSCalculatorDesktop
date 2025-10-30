using PDSCalculatorDesktop.ViewModels;
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

namespace PDSCalculatorDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для SubstanceEditView.xaml
    /// </summary>
    public partial class SubstanceEditView : Window
    {
        public SubstanceEditView()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                if (DataContext is SubstanceEditViewModel vm)
                {
                    // Устанавливаем ItemsSource программно
                    HazardClassComboBox.ItemsSource = vm.HazardClasses;
                    HazardClassComboBox.SelectedItem = vm.HazardClass;
                }
            };
        }
    }
}
