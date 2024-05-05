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
    public partial class BuyerDashboard : Window
    {
        public BuyerDashboard()
        {
            InitializeComponent();
        }

        private void OpenContractMarketplacePage(object sender, RoutedEventArgs e)
        {
            var page = new ContractMarketplacePage();
            page.Show();
        }

        private void OpenCustomerReviewPage(object sender, RoutedEventArgs e)
        {
            var page = new CustomerReviewPage();
            page.Show();
        }

        private void OpenOrderManagementPage(object sender, RoutedEventArgs e)
        {
            var page = new OrderManagementPage();
            page.Show();
        }

        private void OpenInvoiceGenerationPage(object sender, RoutedEventArgs e)
        {
            var page = new InvoiceGenerationPage();
            page.Show();
        }
    }

}
