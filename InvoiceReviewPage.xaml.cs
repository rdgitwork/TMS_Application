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

using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using MilestoneProject.Models;

namespace MilestoneProject
{
    public partial class InvoiceReviewPage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";
        public ObservableCollection<InvoiceModel> Invoices { get; set; } = new ObservableCollection<InvoiceModel>();

        public InvoiceReviewPage()
        {
            InitializeComponent();
            LoadInvoices();
            invoicesDataGrid.ItemsSource = Invoices;
        }

        private void LoadInvoices()
        {
            Invoices.Clear();
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM invoices", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Invoices.Add(new InvoiceModel
                        {
                            InvoiceId = Convert.ToInt32(reader["invoice_id"]),
                            OrderId = Convert.ToInt32(reader["order_id"]),
                            Amount = Convert.ToDecimal(reader["amount"]),
                            IssueDate = Convert.ToDateTime(reader["issue_date"]),
                            Status = reader["status"].ToString(),
                            
                        });
                    }
                }
            }
        }

        private void ConfirmCompletion_Click(object sender, RoutedEventArgs e)
        {
            var selectedInvoice = invoicesDataGrid.SelectedItem as InvoiceModel;
            if (selectedInvoice != null)
            {
                // Confirmation logic
                var result = MessageBox.Show($"Are you sure you want to confirm completion for Order ID: {selectedInvoice.OrderId}?",
                                             "Confirm Completion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var conn = new MySqlConnection(ConnectionString))
                    {
                        conn.Open();
                        var cmd = new MySqlCommand("UPDATE orders SET status = 'completed' WHERE order_id = @orderId", conn);
                        cmd.Parameters.AddWithValue("@orderId", selectedInvoice.OrderId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Order completion confirmed.");
                        }
                        else
                        {
                            MessageBox.Show("Order not found or already completed.");
                        }
                    }
                    LoadInvoices(); // Reload invoices to reflect the changes
                }
            }
            else
            {
                MessageBox.Show("Please select an invoice to confirm order completion.");
            }
        }

    }
}
