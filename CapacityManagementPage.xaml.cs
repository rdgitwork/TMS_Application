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
    public partial class CapacityManagementPage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";
        public ObservableCollection<CarrierModel> Carriers { get; set; } = new ObservableCollection<CarrierModel>();

        public CapacityManagementPage()
        {
            InitializeComponent();
            LoadCarriers();
            carriersDataGrid.ItemsSource = Carriers;
        }

        private void LoadCarriers()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM carriers", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Carriers.Add(new CarrierModel
                        {
                            carrier_id = Convert.ToInt32(reader["carrier_id"]),
                            name = reader["name"].ToString(),
                            capacity = Convert.ToInt32(reader["capacity"])
                        });
                    }
                }
            }
        }

        private void UpdateCapacity_Click(object sender, RoutedEventArgs e)
        {
            var selectedCarrier = carriersDataGrid.SelectedItem as CarrierModel;
            if (selectedCarrier != null)
            {
                var updateWindow = new CarrierCapacityUpdateWindow(selectedCarrier);
                updateWindow.ShowDialog();
                LoadCarriers(); // Refresh carriers list after updating
            }
            else
            {
                MessageBox.Show("Please select a carrier to update its capacity.");
            }
        }


        
    }
}
