using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace MilestoneProject
{
    public partial class MainWindow : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";

        public MainWindow()
        {
            InitializeComponent();
        }

            private void CreateLogEntry(int userId, string actionType, string description)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionStringName"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                string query = "INSERT INTO logs (user_id, action_type, description) VALUES (@UserId, @ActionType, @Description)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ActionType", actionType);
                    cmd.Parameters.AddWithValue("@Description", description);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordBox.Password; // This should be a hashed password

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT user_id, hashed_password, role FROM users WHERE username = @username", conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // In production, compare hashed passwords here
                        int userId = Convert.ToInt32(reader["user_id"]);
                        string storedHashedPassword = reader["hashed_password"].ToString();
                        string role = reader["role"].ToString();

                        if (password == storedHashedPassword) // Placeholder for hash comparison
                        {
                            CreateLogEntry(userId, "Login", "User logged in successfully");

                          

                            // Open the relevant dashboard based on the user's role
                            switch (role)
                            {
                                case "admin":
                                    AdminDashboard adminDashboard = new AdminDashboard();
                                    adminDashboard.Show();
                                    break;
                                case "buyer":
                                    BuyerDashboard buyerDashboard = new BuyerDashboard();
                                    buyerDashboard.Show();
                                    break;
                                case "planner":
                                    PlannerDashboard plannerDashboard = new PlannerDashboard();
                                    plannerDashboard.Show();
                                    break;
                                default:
                                    MessageBox.Show("Unauthorized role.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                                    break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to login: {ex.Message}");
            }
        }

    }
}
