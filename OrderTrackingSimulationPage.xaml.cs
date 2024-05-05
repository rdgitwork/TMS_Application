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
    public partial class OrderTrackingSimulationPage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";
        public ObservableCollection<OrderModel> Orders { get; set; } = new ObservableCollection<OrderModel>();

        public OrderTrackingSimulationPage()
        {
            InitializeComponent();
            LoadOrders();
            ordersDataGrid.ItemsSource = Orders;
        }

        private void LoadOrders()
        {
            Orders.Clear();
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM orders", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Orders.Add(new OrderModel
                        {
                            OrderId = Convert.ToInt32(reader["order_id"]),
                            Status = reader["status"].ToString(),
                            
                        });
                    }
                }
            }
        }

        private void UpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = ordersDataGrid.SelectedItem as OrderModel;
            if (selectedOrder != null)
            {
                var updateStatusWindow = new UpdateOrderStatusWindow(selectedOrder.OrderId);
                updateStatusWindow.ShowDialog();
                LoadOrders(); // Reload orders to reflect any changes
            }
            else
            {
                MessageBox.Show("Please select an order to update its status.");
            }
        }


        private void SimulateDay_Click(object sender, RoutedEventArgs e)
        {
            // Example logic: Advance all 'in_transit' orders to 'completed'
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE orders SET status = 'completed' WHERE status = 'in_transit'", conn);
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Day simulated.");
            LoadOrders(); // Reload orders to reflect any changes
        }

    }
}
