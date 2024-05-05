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
using Microsoft.Win32;
using System.IO;

namespace MilestoneProject
{
    public partial class LogViewerPage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";

        public LogViewerPage()
        {
            InitializeComponent();
            LoadLogs();
        }

        private void LoadLogs()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT log_id, user_id, action_type, description, log_timestamp FROM logs ORDER BY log_timestamp DESC", conn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var logEntry = $"Log ID: {reader["log_id"]}, User ID: {reader["user_id"]}, Action: {reader["action_type"]}, Description: {reader["description"]}, Timestamp: {reader["log_timestamp"]}";
                        logListBox.Items.Add(logEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load logs: {ex.Message}");
            }
        }

        private void DownloadLog_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                FileName = "logs.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    foreach (var item in logListBox.Items)
                    {
                        sw.WriteLine(item.ToString());
                    }
                }

                MessageBox.Show("Log downloaded successfully.");
            }
        }



    }
}
