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
    public partial class CarrierAssignmentPage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";
        public ObservableCollection<OrderModel> Orders { get; set; } = new ObservableCollection<OrderModel>();

        public CarrierAssignmentPage()
        {
            InitializeComponent();
            LoadOrders();
            ordersDataGrid.ItemsSource = Orders;
        }

        private void LoadOrders()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM orders WHERE status = 'received'", conn);
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

        private void AssignCarrier_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = ordersDataGrid.SelectedItem as OrderModel;
            if (selectedOrder != null)
            {
                var carrierSelectionWindow = new CarrierSelectionWindow();
                if (carrierSelectionWindow.ShowDialog() == true)
                {
                    int selectedCarrierId = carrierSelectionWindow.SelectedCarrierId;
                    // Update the order with the selected carrier
                    UpdateOrderCarrier(selectedOrder.OrderId, selectedCarrierId);
                }
            }
            else
            {
                MessageBox.Show("Please select an order.");
            }
        }

        private void UpdateOrderCarrier(int orderId, int carrierId)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE orders SET carrier_id=@carrier_id WHERE order_id=@order_id", conn);
                cmd.Parameters.AddWithValue("@order_id", orderId);
                cmd.Parameters.AddWithValue("@carrier_id", carrierId);
                cmd.ExecuteNonQuery();
            }
            LoadOrders(); // Refresh the orders list
        }
    }
}
