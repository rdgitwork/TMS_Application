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

using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using MilestoneProject.Models;

namespace MilestoneProject
{
    public partial class CustomerManagementPage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=9877329037;database=db_milestone;";
        public ObservableCollection<UserModel> Customers { get; set; } = new ObservableCollection<UserModel>();

        public CustomerManagementPage()
        {
            InitializeComponent();
            LoadCustomers();
            customersDataGrid.ItemsSource = Customers;
        }

       
        private void LoadCustomers()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM users WHERE role = 'buyer'", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new UserModel
                        {
                            UserId = Convert.ToInt32(reader["user_id"]),
                            Username = reader["username"].ToString(),
                            FirstName = reader["first_name"].ToString(),
                            LastName = reader["last_name"].ToString(),
                            Email = reader["email"].ToString(),
                            Role = reader["role"].ToString(),
                            Status = reader["status"].ToString()
                        };
                        Customers.Add(user);
                    }
                }
            }
        }
        private void ReviewSelected_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomers = customersDataGrid.SelectedItems.Cast<UserModel>().ToList();
            foreach (var customer in selectedCustomers)
            {
                customer.Status = "reviewed";
                UpdateCustomerStatus(customer);
            }
        }

        private void UpdateCustomerStatus(UserModel customer)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE users SET status = @status WHERE user_id = @userId", conn);
                cmd.Parameters.AddWithValue("@status", customer.Status);
                cmd.Parameters.AddWithValue("@userId", customer.UserId);
                cmd.ExecuteNonQuery();
            }

        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            // Assuming the Customers collection is bound to the DataGrid and any changes are reflected in this collection.
            foreach (var customer in Customers)
            {
                // Update each customer in the database.
                UpdateCustomer(customer);
            }

            // Optionally, you can refresh the DataGrid to pull fresh data from the database.
            LoadCustomers();
        }

        private void UpdateCustomer(UserModel customer)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                // Prepare the SQL command for updating the customer.
                string query = "UPDATE users SET username = @username, first_name = @firstName, last_name = @lastName, " +
                               "email = @email, status = @status WHERE user_id = @userId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    // Fill in the parameters with the customer's information.
                    cmd.Parameters.AddWithValue("@username", customer.Username);
                    cmd.Parameters.AddWithValue("@firstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@email", customer.Email);
                    cmd.Parameters.AddWithValue("@status", customer.Status);
                    cmd.Parameters.AddWithValue("@userId", customer.UserId);

                    // Open the connection, execute the command, and close the connection.
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
