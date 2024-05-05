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
    public partial class RoutePlanningPage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";
        public ObservableCollection<RouteModel> Routes { get; set; } = new ObservableCollection<RouteModel>();

        public RoutePlanningPage()
        {
            InitializeComponent();
            LoadRoutes();
            routesDataGrid.ItemsSource = Routes;
        }

        private void LoadRoutes()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM routes", conn);
                using (var reader = cmd.ExecuteReader())
                {
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
        }

        private void AddRoute_Click(object sender, RoutedEventArgs e)
        {
            var addRouteWindow = new AddOrUpdateRouteWindow();
            if (addRouteWindow.ShowDialog() == true)
            {
                LoadRoutes(); // Reload routes after adding
            }
        }

        private void UpdateRoute_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoute = routesDataGrid.SelectedItem as RouteModel;
            if (selectedRoute != null)
            {
                var updateRouteWindow = new AddOrUpdateRouteWindow(selectedRoute);
                if (updateRouteWindow.ShowDialog() == true)
                {
                    LoadRoutes(); // Reload routes after updating
                }
            }
            else
            {
                MessageBox.Show("Please select a route to update.");
            }
        }

        private void DeleteRoute_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoute = routesDataGrid.SelectedItem as RouteModel;
            if (selectedRoute != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this route?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (var conn = new MySqlConnection(ConnectionString))
                    {
                        conn.Open();
                        var cmd = new MySqlCommand("DELETE FROM routes WHERE route_id = @routeId", conn);
                        cmd.Parameters.AddWithValue("@routeId", selectedRoute.route_id);
                        cmd.ExecuteNonQuery();
                    }
                    LoadRoutes(); // Reload routes after deleting
                }
            }
            else
            {
                MessageBox.Show("Please select a route to delete.");
            }
        }

    }
}
