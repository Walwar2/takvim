using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// <summary>
    /// Window1.xaml etkileşim mantığı
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //return to login window
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            string ad = adTxt.Text;
            string soyad = soyadTxt.Text;
            string kullaniciAdi = kullaniciAdiTxt.Text;
            string tcKimlik = tcKimlikTxt.Text;
            string telefon = telefonTxt.Text;
            string eposta = epostaTxt.Text;
            string adres = adresTxt.Text;
            string sifre = passwordTxt.Password;
            //bool isAdmin = adminCheckBox.IsChecked ?? false;
            //bool isUye = userCheckBox.IsChecked ?? false;

            
            string connectionString = "Data Source=extronic\\sqlexpress;Initial Catalog=alarmapp;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO KullaniciVerileri (Ad, Soyad, KullaniciAdi, password, tckimlik, telefon, eposta, Adres)" +
                               "VALUES (@ad, @soyad, @kullaniciAdi, @password, @tckimlik, @telefon, @eposta, @adres)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ad", ad);
                    command.Parameters.AddWithValue("@soyad", soyad);
                    command.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    command.Parameters.AddWithValue("@password", sifre);
                    command.Parameters.AddWithValue("@tckimlik", tcKimlik);
                    command.Parameters.AddWithValue("@telefon", telefon);
                    command.Parameters.AddWithValue("@eposta", eposta);
                    command.Parameters.AddWithValue("@adres", adres);
                    //command.Parameters.AddWithValue("@isAdmin", isAdmin);
                    //command.Parameters.AddWithValue("@isUye", isUye);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Üyelik kaydı başarıyla tamamlandı!");
                        MainWindow loginWindow = new MainWindow();
                        loginWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Üyelik kaydı sırasında bir hata oluştu!");
                    }
                }
            }
        }
    }
}
