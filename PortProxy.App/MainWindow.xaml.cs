using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
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

namespace PortProxy.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //// Check if Service is installed
            if (isServiceInstalled())
            {
                btn_InstallService.Visibility = Visibility.Hidden;
                btn_InstallService.IsEnabled = true;
                ///
                label_ServiceHeader.Text = "Local Service Options";
                serviceInstallerBox.Visibility = Visibility.Hidden;
                srv_OptionsBox.Visibility = Visibility.Visible;
            }
            else
            {
                btn_InstallService.Visibility = Visibility.Visible;
                btn_InstallService.IsEnabled = false;
                ///
                serviceInstallerBox.Visibility = Visibility.Visible;
                srv_OptionsBox.Visibility = Visibility.Visible;
            }
        }


        private void button_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        string ServiceUrl = "";

        private void TextBox_ServiceUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

            }
        }

        void VerifyUrl()
        {

        }

        private void btn_InstallService_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_configureService_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceConfigureBox.Visibility == Visibility.Visible)
                ServiceConfigureBox.Visibility = Visibility.Collapsed;
            else
                ServiceConfigureBox.Visibility = Visibility.Visible;
        }

        private void button_srv_AddPort_Click(object sender, RoutedEventArgs e)
        {

        }

        bool isServiceInstalled()
        {
            ServiceController[] services = ServiceController.GetServices();
            Dictionary<string, ServiceControllerStatus> servicesStatus = services.ToDictionary(s => s.ServiceName, s => s.Status);
            ServiceController sC = services.First(X => X.ServiceName == "DemoWorker");
            if (sC != null) return true;
            else return false;
        }
    }
}
