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
    public partial class SettingsPage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";

        public SettingsPage()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string ipAddress = ipAddressTextBox.Text;
            string port = portTextBox.Text;

            SaveSetting("ip_address", ipAddress);
            SaveSetting("port", port);

            MessageBox.Show("Settings saved successfully.", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadSettings()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT setting_name, setting_value FROM system_settings", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader["setting_name"].ToString();
                    string value = reader["setting_value"].ToString();

                    if (name == "ip_address") ipAddressTextBox.Text = value;
                    if (name == "port") portTextBox.Text = value;
                }
            }
        }

        private void SaveSetting(string name, string value)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO system_settings (setting_name, setting_value) VALUES (@name, @value) ON DUPLICATE KEY UPDATE setting_value=@value", conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
