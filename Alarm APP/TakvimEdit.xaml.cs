using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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

namespace Alarm_APP
{
    
    public partial class TakvimEdit : Window
    {
        private DateTime selectedDate;
        private string selectedOlaytipi;

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; }
        }

        public string SelectedOlaytipi
        {
            get { return selectedOlaytipi; }
            set { selectedOlaytipi = value; }
        }
        public TakvimEdit(DateTime selectedDate, string selectedOlaytipi)
        {
            InitializeComponent();
            this.selectedDate = selectedDate;
            this.selectedOlaytipi = selectedOlaytipi;
            LoadEvents();
        }

        private void LoadEvents()
        {
            string connectionString = "Data Source=extronic\\sqlexpress;Initial Catalog=alarmapp;Integrated Security=True";
            string query = "SELECT * FROM alarmDb WHERE username = @username";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", MainWindow.username);
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView.ItemsSource = dataTable.DefaultView;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)dataGridView.SelectedItems[0];
                TakvimEditScreen takvimEditScreen = new TakvimEditScreen();
                takvimEditScreen.SelectedDate = Convert.ToDateTime(row["tarih"]);
                takvimEditScreen.ShowDialog();

                if (takvimEditScreen.DialogResult == true)
                {
                    DateTime editedDate = takvimEditScreen.SelectedDate;

                    
                    string connectionString = "Data Source=extronic\\sqlexpress;Initial Catalog=alarmapp;Integrated Security=True";
                    string updateQuery = "UPDATE alarmDb SET tarih = @tarih WHERE username = @username";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(updateQuery, connection);
                        command.Parameters.AddWithValue("@tarih", editedDate);
                        command.Parameters.AddWithValue("@username", row["username"]);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Kayıt başarıyla güncellendi.");
                            LoadEvents(); 
                        }
                        else
                        {
                            MessageBox.Show("Kayıt güncellenirken bir hata oluştu.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen bir satır seçiniz.");
                }
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
            var selectedItems = dataGridView.SelectedItems;
            if (selectedItems.Count > 0)
            {
                
                MessageBoxResult result = MessageBox.Show("Seçili öğeleri silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    string connectionString = "Data Source=extronic\\sqlexpress;Initial Catalog=alarmapp;Integrated Security=True";
                    string deleteQuery = "DELETE FROM alarmDb WHERE username = @username AND tarih = @tarih";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        foreach (var selectedItem in selectedItems)
                        {
                            DataRowView row = (DataRowView)selectedItem;
                            SqlCommand command = new SqlCommand(deleteQuery, connection);
                            command.Parameters.AddWithValue("@username", row["username"]);
                            command.Parameters.AddWithValue("@tarih", row["tarih"]);
                            command.ExecuteNonQuery();
                        }
                    }

                    LoadEvents();

                    MessageBox.Show("Seçili öğeler başarıyla silindi.");
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için öğe seçiniz.");
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadEvents();
        }

        private void RefreshData()
        {
            LoadEvents();
        }

        
    } 
}
