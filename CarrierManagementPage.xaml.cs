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
    public partial class CarrierManagementPage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";
        public ObservableCollection<CarrierModel> Carriers { get; set; } = new ObservableCollection<CarrierModel>();

        public CarrierManagementPage()
        {
            InitializeComponent();
            LoadCarriers();
            carriersDataGrid.ItemsSource = Carriers;
        }

        private void LoadCarriers()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM carriers", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Carriers.Add(new CarrierModel
                    {
                        carrier_id = Convert.ToInt32(reader["carrier_id"]),
                        name = reader["name"].ToString(),
                        contact_info = reader["contact_info"].ToString(),
                        capacity = Convert.ToInt32(reader["capacity"])
                    });
                }
            }
        }

        private void SaveCarrierData_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                foreach (var carrier in Carriers)
                {
                    MySqlCommand cmd;
                    if (carrier.carrier_id == 0) // New carrier
                    {
                        cmd = new MySqlCommand("INSERT INTO carriers (name, contact_info, capacity) VALUES (@name, @contact_info, @capacity)", conn);
                    }
                    else // Existing carrier
                    {
                        cmd = new MySqlCommand("UPDATE carriers SET name=@name, contact_info=@contact_info, capacity=@capacity WHERE carrier_id=@carrier_id", conn);
                        cmd.Parameters.AddWithValue("@carrier_id", carrier.carrier_id);
                    }
                    cmd.Parameters.AddWithValue("@name", carrier.name);
                    cmd.Parameters.AddWithValue("@contact_info", carrier.contact_info);
                    cmd.Parameters.AddWithValue("@capacity", carrier.capacity);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Carrier data saved successfully.");
        }
    }
}
