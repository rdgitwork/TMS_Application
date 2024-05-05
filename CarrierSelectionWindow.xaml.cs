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
    public partial class CarrierSelectionWindow : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";

        public ObservableCollection<CarrierModel> Carriers { get; set; } = new ObservableCollection<CarrierModel>();
        public int SelectedCarrierId { get; private set; }

        public CarrierSelectionWindow()
        {
            InitializeComponent();
            LoadCarriers();
            carriersListBox.ItemsSource = Carriers;
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
                            contact_info = reader["contact_info"].ToString(),
                            capacity = Convert.ToInt32(reader["capacity"])
                        });
                    }
                }
            }
        }

        private void AssignCarrier_Click(object sender, RoutedEventArgs e)
        {
            var selectedCarrier = carriersListBox.SelectedItem as CarrierModel;
            if (selectedCarrier != null)
            {
                SelectedCarrierId = selectedCarrier.carrier_id;
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please select a carrier.");
            }
        }
    }
}
