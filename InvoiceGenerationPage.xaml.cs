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

using System;
using System.Collections.ObjectModel;
using System.Windows;
using MySql.Data.MySqlClient;
using MilestoneProject.Models;

namespace MilestoneProject
{
    public partial class InvoiceGenerationPage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";

        public ObservableCollection<OrderModel> CompletedOrders { get; set; } = new ObservableCollection<OrderModel>();

        public InvoiceGenerationPage()
        {
            InitializeComponent();
            LoadCompletedOrders();
            ordersDataGrid.ItemsSource = CompletedOrders;
        }

        private void LoadCompletedOrders()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM orders WHERE status='completed'", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CompletedOrders.Add(new OrderModel
                        {
                            OrderId = Convert.ToInt32(reader["order_id"]),
                            BuyerId = Convert.ToInt32(reader["buyer_id"]),
                            Status = reader["status"].ToString(),
                            CreationDate = Convert.ToDateTime(reader["creation_date"]),
                            CompletionDate = reader["completion_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["completion_date"])
                        });
                    }
                }
            }
        }

        private void GenerateInvoice_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = ordersDataGrid.SelectedItem as OrderModel;
            if (selectedOrder != null)
            {
                GenerateInvoiceForOrder(selectedOrder);
            }
            else
            {
                MessageBox.Show("Please select an order to generate an invoice.");
            }
        }

        private void GenerateInvoiceForOrder(OrderModel order)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO invoices (order_id, amount, issue_date, due_date, status) VALUES (@order_id, @amount, @issue_date, @due_date, 'unpaid')", conn);
                cmd.Parameters.AddWithValue("@order_id", order.OrderId);
                cmd.Parameters.AddWithValue("@amount", CalculateInvoiceAmount(order));
                cmd.Parameters.AddWithValue("@issue_date", DateTime.Now);
                cmd.Parameters.AddWithValue("@due_date", DateTime.Now.AddDays(30)); 

                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Invoice generated successfully.");
        }

        private decimal CalculateInvoiceAmount(OrderModel order)
        {
            
            decimal rate = 0m; // You would get this from your database or some service
            int quantity = 1; // For simplicity, assuming 1 quantity per order, adjust as necessary
            decimal taxRate = 0.13m; // Example tax rate of 13%
            decimal discount = 0m; // Assume no discount for this example

            // Calculate the base amount
            decimal baseAmount = rate * quantity;

            // Apply discount
            decimal discountAmount = baseAmount * discount;

            // Calculate total amount after discount
            decimal totalAfterDiscount = baseAmount - discountAmount;

            // Apply taxes
            decimal taxAmount = totalAfterDiscount * taxRate;

            // Calculate final amount
            decimal finalAmount = totalAfterDiscount + taxAmount;

            return finalAmount;
        }

      


      
    }
}
