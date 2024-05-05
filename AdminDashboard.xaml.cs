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
using System.Configuration;


namespace MilestoneProject
{
    public partial class AdminDashboard : Window
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void OpenSettingsPage(object sender, RoutedEventArgs e)
        {
            var page = new SettingsPage();
            page.Show();
        }

        private void OpenLogViewerPage(object sender, RoutedEventArgs e)
        {
            var page = new LogViewerPage();
            page.Show();
        }

        private void OpenRateFeeManagementPage(object sender, RoutedEventArgs e)
        {
            var page = new RateFeeManagementPage();
            page.Show();
        }

        private void OpenCarrierManagementPage(object sender, RoutedEventArgs e)
        {
            var page = new CarrierManagementPage();
            page.Show();
        }

        private void OpenRouteManagementPage(object sender, RoutedEventArgs e)
        {
            var page = new RouteManagementPage();
            page.Show();
        }

       
    }

}
