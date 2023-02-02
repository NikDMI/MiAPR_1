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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MiAPR_1.Models;
using MiAPR_1.ViewModels;

namespace MiAPR_1.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ViewModel();
        }

        private void RenderNewData(object sender, RoutedEventArgs e)
        {
            AlgorithmInfo algorithmInfo = (AlgorithmInfo)FindResource("algorithmInfo");
            try
            {
                _viewModel.GenerateVectors(algorithmInfo, _vectorFieldBefore, _vectorFieldAfter);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private ViewModel _viewModel;
    }
}
