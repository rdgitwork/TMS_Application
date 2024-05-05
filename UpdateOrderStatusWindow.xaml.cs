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
using MySql.Data.MySqlClient;

namespace MilestoneProject
{
    public partial class UpdateOrderStatusWindow : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";
        public int OrderId { get; set; }

        public UpdateOrderStatusWindow(int orderId)
        {
            InitializeComponent();
            OrderId = orderId;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var newStatus = (statusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (!string.IsNullOrWhiteSpace(newStatus))
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand("UPDATE orders SET status = @status WHERE order_id = @orderId", conn);
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@orderId", OrderId);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Order status updated successfully.");
                Close();
            }
            else
            {
                MessageBox.Show("Please select a status.");
            }
        }
    }
}
