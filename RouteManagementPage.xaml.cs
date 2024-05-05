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
    public partial class RouteManagementPage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";
        public ObservableCollection<RouteModel> Routes { get; set; } = new ObservableCollection<RouteModel>();

        public RouteManagementPage()
        {
            InitializeComponent();
            LoadRoutes();
            routesDataGrid.ItemsSource = Routes;
        }

        private void LoadRoutes()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM routes", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Routes.Add(new RouteModel
                    {
                        route_id = Convert.ToInt32(reader["route_id"]),
                        start_point = reader["start_point"].ToString(),
                        end_point = reader["end_point"].ToString(),
                        distance = Convert.ToDecimal(reader["distance"]),
                        travel_time = Convert.ToDecimal(reader["travel_time"])
                    });
                }
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                foreach (var route in Routes)
                {
                    MySqlCommand cmd;
                    if (route.route_id == 0) // New route
                    {
                        cmd = new MySqlCommand("INSERT INTO routes (start_point, end_point, distance, travel_time) VALUES (@start_point, @end_point, @distance, @travel_time)", conn);
                    }
                    else // Existing route
                    {
                        cmd = new MySqlCommand("UPDATE routes SET start_point=@start_point, end_point=@end_point, distance=@distance, travel_time=@travel_time WHERE route_id=@route_id", conn);
                        cmd.Parameters.AddWithValue("@route_id", route.route_id);
                    }
                    cmd.Parameters.AddWithValue("@start_point", route.start_point);
                    cmd.Parameters.AddWithValue("@end_point", route.end_point);
                    cmd.Parameters.AddWithValue("@distance", route.distance);
                    cmd.Parameters.AddWithValue("@travel_time", route.travel_time);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Route data saved successfully.");
        }
    }
}
