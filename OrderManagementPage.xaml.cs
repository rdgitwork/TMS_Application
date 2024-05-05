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


using MilestoneProject.Models;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;

namespace MilestoneProject
{
    public partial class OrderManagementPage : Window
    {
        private const string ConnectionString ="server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";
        public ObservableCollection<OrderModel> Orders { get; set; } = new ObservableCollection<OrderModel>();

        public OrderManagementPage()
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
                        var order = new OrderModel
                        {
                            OrderId = Convert.ToInt32(reader["order_id"]),
                            BuyerId = Convert.ToInt32(reader["buyer_id"]),
                            Status = reader["status"].ToString(),
                            CreationDate = Convert.ToDateTime(reader["creation_date"]),
                            CompletionDate = reader["completion_date"] != DBNull.Value ? Convert.ToDateTime(reader["completion_date"]) : (DateTime?)null
                        };
                        Orders.Add(order);
                    }
                }
            }
        }


    }
}
