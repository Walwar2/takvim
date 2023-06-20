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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alarm_APP
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string username;
        public static string password;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            //open register window
            Window1 registerWindow = new Window1();
            registerWindow.Show();
            this.Close();
        }

        public void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            username = nameTxt.Text;
            password = passwordBox.Password;

            string connectionString = "Data Source=extronic\\sqlexpress;Initial Catalog=alarmapp;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM KullaniciVerileri WHERE KullaniciAdi = @kullaniciAdi AND Password = @password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@kullaniciAdi", username);
                    command.Parameters.AddWithValue("@password", password);

                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        Takvim takvimWindow = new Takvim();
                        takvimWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Geçersiz kullanıcı adı veya şifre!");
                    }
                }
            }
        }

       
    }
}
