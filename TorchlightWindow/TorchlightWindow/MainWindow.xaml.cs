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

namespace TorchlightWindow
{
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.Loaded += this.MainWindow_Loaded;
            this.Closed += this.MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this.viewModel.Dispose();
        }

        private ViewModel viewModel;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.viewModel = new ViewModel();
            this.DataContext = this.viewModel;
        }
    }
}
