using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Xml.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Reflection;


namespace MyGameList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("fields", "*;"),
            };
            client.DefaultRequestHeaders.Add("Client-ID", "FAKE-ID");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer FAKE-TOKEN");
            var content = new StringContent("fields *;", Encoding.UTF8, "application/json");
            var response = client.PostAsync("https://api.igdb.com/v4/games", content).Result;
            if (response.IsSuccessStatusCode)
            {
                Trace.WriteLine("Success");
                Trace.WriteLine(response);
            } else
            {
                Trace.WriteLine("Fail");
            }
        }
    }
}
