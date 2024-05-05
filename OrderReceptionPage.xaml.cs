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
using System.Data;
using MySql.Data.MySqlClient;
using MilestoneProject.Models;

namespace MilestoneProject
{
    public partial class OrderReceptionPage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";

        public ObservableCollection<OrderModel> Orders { get; set; } = new ObservableCollection<OrderModel>();

        public OrderReceptionPage()
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
                var cmd = new MySqlCommand("SELECT * FROM orders", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Orders.Add(new OrderModel
                        {
                            OrderId = Convert.ToInt32(reader["order_id"]),
                            BuyerId = Convert.ToInt32(reader["buyer_id"]),
                            CarrierId = reader.IsDBNull(reader.GetOrdinal("carrier_id")) ? (int?)null : Convert.ToInt32(reader["carrier_id"]),
                            Status = reader["status"].ToString(),
                            CreationDate = Convert.ToDateTime(reader["creation_date"]),
                            CompletionDate = reader.IsDBNull(reader.GetOrdinal("completion_date")) ? (DateTime?)null : Convert.ToDateTime(reader["completion_date"])
                        });
                    }
                }
            }
        }

        private void AssignCarrier_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = ordersDataGrid.SelectedItem as OrderModel;
            if (selectedOrder != null && selectedOrder.CarrierId == null)
            {
                AssignCarrierToOrder(selectedOrder.OrderId);
            }
            else
            {
                MessageBox.Show("Please select an unassigned order.");
            }
        }

        private void AssignCarrierToOrder(int orderId)
        {
            var carrierSelectionWindow = new CarrierSelectionWindow();
            if (carrierSelectionWindow.ShowDialog() == true)
            {
                int selectedCarrierId = carrierSelectionWindow.SelectedCarrierId;
                UpdateOrderCarrier(orderId, selectedCarrierId);
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
