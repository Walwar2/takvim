using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Alarm_APP
{
    public partial class TakvimEditScreen : Window
    {
        private DataTable data;

        public DateTime SelectedDate
        {
            get { return (DateTime)calendar.SelectedDate; }
            set { calendar.SelectedDate = value; }
        }

        public TakvimEditScreen()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string connectionString = "Data Source=extronic\\sqlexpress;Initial Catalog=alarmapp;Integrated Security=True";
            string query = "SELECT * FROM alarmDb WHERE username = @username";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", MainWindow.username);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                data = new DataTable();
                adapter.Fill(data);
            }
            dataGrid.ItemsSource = data.DefaultView;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItem;
                DateTime editedDate = calendar.SelectedDate.GetValueOrDefault();
                string editedOlaytipi = row["olaytipi"].ToString();

                // Veritabanında kaydı güncelle
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
                        LoadData(); 
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


        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = calendar.SelectedDate.Value;
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
                if (selectedRow != null)
                {
                    selectedRow["tarih"] = selectedDate;
                }
            }
        }
    }
}
