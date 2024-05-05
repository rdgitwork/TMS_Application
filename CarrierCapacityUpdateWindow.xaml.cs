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
using MilestoneProject.Models;

namespace MilestoneProject
{
    public partial class CarrierCapacityUpdateWindow : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";
        public CarrierModel Carrier { get; set; }

        public CarrierCapacityUpdateWindow(CarrierModel carrier)
        {
            InitializeComponent();
            Carrier = carrier;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            int newCapacity;
            if (int.TryParse(newCapacityTextBox.Text, out newCapacity))
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand("UPDATE carriers SET capacity = @newCapacity WHERE carrier_id = @carrierId", conn);
                    cmd.Parameters.AddWithValue("@newCapacity", newCapacity);
                    cmd.Parameters.AddWithValue("@carrierId", Carrier.carrier_id);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Carrier capacity updated successfully.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid capacity.");
            }
        }
    }
}
