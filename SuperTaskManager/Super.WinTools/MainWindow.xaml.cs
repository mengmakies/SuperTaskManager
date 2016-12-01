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
using MLS.Service.Tasks;
using Super.Service.Tasks;
using Super.Service.Tasks.Jobs;

namespace Super.WinTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            TaskManager.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            TaskManager.Stop();
        }

        // 花猫奶粉
        private void btnGetHuamao_Click(object sender, RoutedEventArgs e)
        {
            var job = new GetProductPriceJob();
            job.ExcuteTask();
        }
    }
}
