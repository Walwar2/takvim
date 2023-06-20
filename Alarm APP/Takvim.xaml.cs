using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// <summary>
    /// Takvim.xaml etkileşim mantığı
    /// </summary>
    public partial class Takvim : Window
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

        public Takvim()
        {
            InitializeComponent();
            cldSample.SelectedDate = DateTime.Today;
        }

        private void SetAlarm(DateTime alarmTime, string olaytipi)
        {
            DateTime currentTime = DateTime.Now;
            if (alarmTime > currentTime)
            {
                TimeSpan timeRemaining = alarmTime - currentTime;
                Timer timer = new Timer(AlarmCallback, null, timeRemaining, TimeSpan.Zero);
                MessageBox.Show($"Alarm {alarmTime} tarihine başarıyla kuruldu!");

                string connectionString = "Data Source=extronic\\sqlexpress;Initial Catalog=alarmapp;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string username = MainWindow.username;
                    string query = "INSERT INTO alarmDb (tarih, olaytipi, username) VALUES (@tarih, @olaytipi, @username)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@tarih", alarmTime);
                    command.Parameters.AddWithValue("@olaytipi", olaytipi);
                    command.Parameters.AddWithValue("@username", username);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                MessageBox.Show("Geçmiş bir zaman için alarm kurulamaz!");
            }
        }

        private void AlarmCallback(object state)
        {
            // Veritabanından ilgili alarmları kontrol et
            string connectionString = "Data Source=extronic\\sqlexpress;Initial Catalog=alarmapp;Integrated Security=True";
            string query = "SELECT * FROM alarmDb WHERE tarih <= @currentTime AND username = @username";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@currentTime", DateTime.Now);
                command.Parameters.AddWithValue("@username", MainWindow.username);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DateTime alarmTime = (DateTime)reader["tarih"];
                    string alarmOlaytipi = reader["olaytipi"].ToString();

                    string notificationMessage = $"Alarm {alarmTime} tarihine ulaştı! Olay Tipi: {alarmOlaytipi}";
                    MessageBox.Show(notificationMessage, "Alarm", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                reader.Close();
            }
        }


        private void SetAlarmButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedOlayTipi = string.Empty;

            if (radioButton1.IsChecked == true)
            {
                selectedOlayTipi = radioButton1.Content.ToString();
            }
            else if (radioButton2.IsChecked == true)
            {
                selectedOlayTipi = radioButton2.Content.ToString();
            }
            else if (radioButton3.IsChecked == true)
            {
                selectedOlayTipi = radioButton3.Content.ToString();
            }

            SelectedDate = cldSample.SelectedDate.GetValueOrDefault();
            SelectedOlaytipi = selectedOlayTipi;

            SetAlarm(SelectedDate, SelectedOlaytipi);
        }



        private void alarmEdit_Click(object sender, RoutedEventArgs e)
        {

            TakvimEdit takvimEdit = new TakvimEdit(SelectedDate, SelectedOlaytipi);
            takvimEdit.ShowDialog();

            if (takvimEdit.DialogResult == true)
            {
                DateTime editedDate = takvimEdit.SelectedDate;
                string editedOlaytipi = takvimEdit.SelectedOlaytipi;
                string connectionString = "Data Source=extronic\\sqlexpress;Initial Catalog=alarmapp;Integrated Security=True";
                string updateQuery = "UPDATE alarmDb SET tarih = @tarih, olaytipi = @olaytipi WHERE username = @username";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@tarih", editedDate);
                    command.Parameters.AddWithValue("@olaytipi", editedOlaytipi);
                    command.Parameters.AddWithValue("@username", MainWindow.username);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Kayıt başarıyla güncellendi.");
                    }
                    else
                    {
                        MessageBox.Show("Kayıt güncellenirken bir hata oluştu.");
                    }
                }
            }
        }

        private void testAlarmButton_Click(object sender, RoutedEventArgs e)
        {
            // Veritabanından en yakın alarmı al
            string connectionString = "Data Source=extronic\\sqlexpress;Initial Catalog=alarmapp;Integrated Security=True";
            string query = "SELECT TOP 1 * FROM alarmDb WHERE tarih >= @currentTime AND username = @username ORDER BY tarih ASC";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@currentTime", DateTime.Now);
                command.Parameters.AddWithValue("@username", MainWindow.username);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    DateTime alarmTime = (DateTime)reader["tarih"];
                    string alarmOlaytipi = reader["olaytipi"].ToString();

                    MessageBox.Show($"(TEST) Alarmınız {alarmTime} tarihinde çalışacak. Olay Tipi: {alarmOlaytipi}", "Test Alarmı", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Yaklaşan bir alarm bulunamadı.", "Test Alarmı", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                reader.Close();
            }
        }
    }
}
