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

namespace MilestoneProject
{
    public partial class ContractMarketplacePage : Window
    {
        private const string ConnectionString = "server=localhost;port=3306;user id=root;password=Ricky@9877329037;database=db_milestone;";

        public ContractMarketplacePage()
        {
            InitializeComponent();
            LoadContracts();
        }

        private void LoadContracts()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM marketplace_contracts WHERE status = 'available'", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                contractsDataGrid.DataContext = dt;
            }
        }

        private void ImportSelected_Click(object sender, RoutedEventArgs e)
        {
            var selectedContracts = contractsDataGrid.SelectedItems;
            if (selectedContracts.Count == 0)
            {
                MessageBox.Show("Please select at least one contract to import.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRowView contract in selectedContracts)
                        {
                            int contractId = Convert.ToInt32(contract["contract_id"]);

                            // Update the status of the contract to 'imported'
                            MySqlCommand updateCmd = new MySqlCommand("UPDATE marketplace_contracts SET status = 'imported' WHERE contract_id = @contractId", conn, transaction);
                            updateCmd.Parameters.AddWithValue("@contractId", contractId);
                            updateCmd.ExecuteNonQuery();

                            
                        }

                        transaction.Commit();
                        MessageBox.Show("Selected contracts have been imported.");
                        LoadContracts(); // Refresh the DataGrid to show updated contract statuses
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error importing contracts: " + ex.Message);
                    }
                }
            }
        }

    }
}
