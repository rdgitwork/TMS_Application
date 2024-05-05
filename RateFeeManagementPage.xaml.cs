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
    public partial class RateFeeManagementPage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";

        // This ObservableCollection will be bound to the UI to display rates and fees.
        public ObservableCollection<RateFeeModel> RatesFees { get; set; } = new ObservableCollection<RateFeeModel>();

        public RateFeeManagementPage()
        {
            InitializeComponent();
            LoadRatesFees();
            rateFeeDataGrid.ItemsSource = RatesFees;
        }

        private void LoadRatesFees()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM rates_fees", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RatesFees.Add(new RateFeeModel
                        {
                            RateId = Convert.ToInt32(reader["rate_id"]),
                            Description = reader["description"].ToString(),
                            Amount = Convert.ToDecimal(reader["amount"])
                        });
                    }
                }
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                foreach (var rateFee in RatesFees)
                {
                    var cmd = new MySqlCommand("UPDATE rates_fees SET description=@description, amount=@amount WHERE rate_id=@rate_id", conn);
                    cmd.Parameters.AddWithValue("@rate_id", rateFee.RateId);
                    cmd.Parameters.AddWithValue("@description", rateFee.Description);
                    cmd.Parameters.AddWithValue("@amount", rateFee.Amount);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Changes saved successfully.");
        }
    }
}
