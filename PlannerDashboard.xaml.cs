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

namespace MilestoneProject
{

    public partial class PlannerDashboard : Window
    {
        public PlannerDashboard()
        {
            InitializeComponent();
        }

        private void OpenOrderReceptionPage(object sender, RoutedEventArgs e)
        {
            var page = new OrderReceptionPage();
            page.Show();
        }

        private void OpenCarrierAssignmentPage(object sender, RoutedEventArgs e)
        {
            var page = new CarrierAssignmentPage();
            page.Show();
        }

        private void OpenCapacityManagementPage(object sender, RoutedEventArgs e)
        {
            var page = new CapacityManagementPage();
            page.Show();
        }

        private void OpenRoutePlanningPage(object sender, RoutedEventArgs e)
        {
            var page = new RoutePlanningPage();
            page.Show();
        }

        private void OpenOrderTrackingSimulationPage(object sender, RoutedEventArgs e)
        {
            var page = new OrderTrackingSimulationPage();
            page.Show();
        }

        private void OpenInvoiceReviewPage(object sender, RoutedEventArgs e)
        {
            var page = new InvoiceReviewPage();
            page.Show();
        }
    }

}
